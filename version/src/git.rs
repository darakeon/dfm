use dirs::home_dir;

use git2::{
	BranchType,
	CredentialType,
	Cred,
	Delta,
	DiffFile,
	DiffOptions,
	Error,
	ErrorCode,
	FetchOptions,
	FetchPrune,
	IndexAddOption,
	ObjectType,
	PushOptions,
	Reference,
	RemoteCallbacks,
	Repository,
	Signature,
};

use reqwest::blocking::Client;

use std::env;

use crate::regex::replace;

pub fn current_branch() -> Option<String> {
	let repo = repo();
	let branches = repo.branches(None).unwrap();

	for item in branches {
		let (branch, typ) = item.unwrap();

		if branch.is_head() && typ == BranchType::Local {
			let name = branch.name().unwrap().unwrap();
			return Some(name.to_string());
		}
	}

	return None;
}

pub fn list_changes_main() -> Vec<String> {
	list_changes(
		repo().find_branch("origin/main", BranchType::Remote)
			.unwrap()
			.get(),
		false
	)
}

fn list_stashable_changes_head() -> Vec<String> {
	list_changes(&repo().head().unwrap(), true)
}

fn list_changes(reference: &Reference, stashable_only: bool) -> Vec<String> {
	let tree = reference.peel_to_tree().unwrap();

	let repo = repo();

	let mut options = DiffOptions::new();
	options.include_untracked(true);

	let diff = repo.diff_tree_to_workdir(
		Some(&tree), Some(&mut options)
	).unwrap();

	let deltas = diff.deltas();

	let mut result: Vec<String> = Vec::new();

	for delta in deltas {
		if stashable_only && delta.status() == Delta::Untracked {
			continue;
		}

		let old = get_path(delta.old_file());
		if !result.contains(&old) {
			result.push(old);
		}

		let new = get_path(delta.new_file());
		if !result.contains(&new) {
			result.push(new);
		}
	}

	return result;
}

fn get_path(file: DiffFile) -> String {
	return file.path().unwrap().to_str().unwrap().to_string();
}

pub fn has_pull_request() -> bool {
	let url_repo = url_repo();
	let url_api = url_api(&url_repo);

	let client = Client::new().get(&url_api).header("User-Agent", "");
	let response = client.send().unwrap();
	let body = response.text().unwrap();

	return body != "[]";
}

fn url_repo() -> String {
	return repo().find_remote("origin").unwrap()
		.url().unwrap().to_string();
}

fn url_api(url_repo: &str) -> String {
	let pattern = r"git@github.com:(\w+)/(\w+).git";
	let replacer = r"https://api.github.com/repos/$1/$2/pulls?state=open&head=$1:";
	let replaced = replace(url_repo, pattern, replacer).unwrap();

	let branch = current_branch().unwrap();

	return format!("{}{}", replaced, branch);
}

pub fn update_local() {
	let repo = repo();
	let mut remote = repo.find_remote("origin").unwrap();

	let mut callbacks = RemoteCallbacks::new();
	callbacks.credentials(&git_credentials_callback);

	let mut options = FetchOptions::new();
	options.remote_callbacks(callbacks);
	options.prune(FetchPrune::On);

	remote.fetch(
		&[] as &[&str],
		Some(&mut options),
		Some("updating local")
	).unwrap();
}

fn git_credentials_callback(
	_url: &str,
	username: Option<&str>,
	_cred_type: CredentialType,
) -> Result<Cred, Error> {
	let mut public = home_dir().unwrap();
	public.push(".ssh");
	public.push("dk");
	public.set_extension("pub");

	let mut private = home_dir().unwrap();
	private.push(".ssh");
	private.push("dk");

	let password = env::var("GIT_PASSWORD")
		.expect("No GIT_PASSWORD environment variable found");

	return Cred::ssh_key(
		username.unwrap(),
		Some(public.as_path()),
		private.as_path(),
		Some(&password),
	);
}

pub fn go_to_main() {
	let repo = repo();

	let commit = repo.find_commit(
		repo.find_branch(
			"origin/main", BranchType::Remote
		).unwrap()
		.get().target().unwrap()
	).unwrap();

	repo.branch("main", &commit, false).unwrap();

	let obj = repo.revparse_single("refs/heads/main").unwrap();
	repo.checkout_tree(&obj, None).unwrap();

	repo.set_head("refs/heads/main").unwrap();
}

pub fn create_tag(name: &str, annotation: &str) {
	let repo = repo();
	let head = repo.head().unwrap().peel(ObjectType::Commit).unwrap();
	repo.tag(&name, &head, &signature(), &annotation, false).unwrap();
}

pub fn create_branch(branch_name: &str) {
	let repo = repo();

	create_branch_if_not_exists(&repo, branch_name);

	let obj = repo.revparse_single(
		&("refs/heads/".to_owned() + &branch_name)
	).unwrap();

	repo.checkout_tree(&obj, None).unwrap();

	repo.set_head(&("refs/heads/".to_owned() + &branch_name)).unwrap();
}

fn create_branch_if_not_exists(repo: &Repository, branch_name: &str) {
	if let Err(err) = repo.find_branch(&branch_name, BranchType::Local) {
		if err.code() == ErrorCode::NotFound {
			let head = repo.head().unwrap();
			let oid = head.target().unwrap();
			let commit = repo.find_commit(oid).unwrap();

			repo.branch(&branch_name, &commit, false).unwrap();

			return;
		}
	}

	println!("... Branch {} already exists ...", &branch_name);
}

pub fn remove_branch(name: &str) {
	repo().find_branch(&name, BranchType::Local).unwrap()
		.delete().unwrap();
}

pub fn update_remote(tag: &str, branch: &str) {
	let repo = repo();
	let mut remote = repo.find_remote("origin").unwrap();

	let mut callbacks = RemoteCallbacks::new();
	callbacks.credentials(&git_credentials_callback);

	let mut options = PushOptions::new();
	options.remote_callbacks(callbacks);

	let refs = [
		format!("refs/tags/{}", tag),
		format!("refs/heads/{}", branch),
	];

	remote.push(
		&refs,
		Some(&mut options)
	).unwrap();
}

pub fn connect_local_and_remote_branch(branch_name: &str) {
	let repo = repo();
	let mut branch = repo.find_branch(&branch_name, BranchType::Local).unwrap();

	let remote = format!("origin/{}", &branch_name);

	branch.set_upstream(Some(&remote)).unwrap();
}

static mut STASHED: bool = false;

pub fn stash(message: &str) {
	unsafe {
		STASHED = list_stashable_changes_head().len() != 0;

		if STASHED {
			repo().stash_save(&signature(), message, None).unwrap();
		}
	}
}

pub fn commit(message: &str) {
	let repo = repo();

	let mut index = repo.index().unwrap();
	index.add_all(["*"].iter(), IndexAddOption::CHECK_PATHSPEC, None).unwrap();
	index.write().unwrap();

	let sign = signature();
	let tree = repo.find_tree(index.write_tree().unwrap()).unwrap();
	let parent = repo.head().unwrap().peel_to_commit().unwrap();
	repo.commit(Some("HEAD"), &sign, &sign, message, &tree, &[&parent]).unwrap();
}

pub fn stash_pop() {
	unsafe {
		if STASHED {
			repo().stash_pop(0, None).unwrap();
		}
	}
}

fn signature() -> Signature<'static> {
	let name = env::var("GIT_NAME")
		.expect("No GIT_NAME environment variable found");

	let email = env::var("GIT_EMAIL")
		.expect("No GIT_EMAIL environment variable found");

	return Signature::now(&name, &email).unwrap();
}

fn repo() -> Repository {
	Repository::open("..").unwrap()
}
