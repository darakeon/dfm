use git2::Repository;
use git2::BranchType;

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
