use git2::{BranchType,DiffOptions,Repository,DiffFile};

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
