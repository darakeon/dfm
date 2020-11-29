use git2::{BranchType,DiffFile,DiffOptions,Repository};
use reqwest::blocking::Client;

use crate::regex::replace;

pub fn current_branch() -> Option<String> {
	let repo = Repository::open("../").unwrap();
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

pub fn list_changed() -> Vec<String> {
	let repo = Repository::open("../").unwrap();

	let branch = repo.find_branch(
		"origin/main", BranchType::Remote
	).unwrap();

	let tree = branch.get()
		.peel_to_tree().unwrap();

	let mut options = DiffOptions::new();
	options.include_untracked(true);

	let diff = repo.diff_tree_to_workdir(
		Some(&tree), Some(&mut options)
	).unwrap();

	let deltas = diff.deltas();

	let mut result: Vec<String> = Vec::new();

	for delta in deltas {
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
	let repo = Repository::open("../").unwrap();
	let remote = repo.find_remote("origin").unwrap();
	return remote.url().unwrap().to_string();
}

fn url_api(url_repo: &str) -> String {
	let pattern = r"git@github.com:(\w+)/(\w+).git";
	let replacer = r"https://api.github.com/repos/$1/$2/pulls?state=open&head=$1:";
	let replaced = replace(url_repo, pattern, replacer).unwrap();

	let branch = current_branch().unwrap();

	return format!("{}{}", replaced, branch);
}
