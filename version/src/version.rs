use std::collections::LinkedList;

use crate::arguments::ProgramOption;
use crate::end::{success,throw,throw_format};
use crate::file::{get_path, get_lines};
use crate::git::{current_branch,has_pull_request};
use crate::regex::{extract, extract_line, is_match};
use crate::todos::add_release;

fn path() -> String { get_path(vec!["..", "docs", "RELEASES.md"]) }

pub fn create_version(option: &ProgramOption, numbers: Vec<usize>) -> Option<Version> {
	let task_list = get_lines(path());
	let pattern = r"^\- \[.+\]\(\#(\d+\.\d+\.\d+\.\d+)\)$";

	let prod = extract_line(&task_list, 6, pattern);
	let dev = extract_line(&task_list, 7, pattern);

	let branch = current_branch().unwrap();

	let version = mount_version(
		prod.clone(),
		dev.clone(),
		branch.clone(),
		&task_list,
		numbers,
	);

	if option == &ProgramOption::Git {
		return Some(version);
	}

	if !version.done {
		return end_not_done(version.code);
	}

	let just_check = option == &ProgramOption::Check;

	let compare = if just_check { prod } else { dev };

	let should_check = is_match(&branch, pattern);

	if should_check {
		if branch != compare {
			throw_format(12, format!("Branch is '{}', but release is of '{}'", branch, compare));
		}

		if branch != "main" && version.tasks.len() == 0 {
			throw(13, "Version without tasks");
		}
	}

	Some(version)
}

const START_OF_VERSIONS: usize = 16;

fn mount_version(
	prod: String,
	dev: String,
	branch: String,
	task_list: &Vec<String>,
	numbers: Vec<usize>,
) -> Version {
	let mut version = Version::new(dev, prod);

	let start = start_of_current_tasks(task_list, branch);

	if start != START_OF_VERSIONS {
		let pattern = r"(\d+\.\d+\.\d+\.\d+)";
		version.next = extract_line(task_list, START_OF_VERSIONS, pattern);
	} else if numbers.len() > 0 {
		let code = version.code.clone();

		if let Some(next) = add_release(code, numbers) {
			version.next = next;
		}
	}

	let done_pattern = r"^\- \[([ x])\] ";
	let task_pattern = r"^\- \[[ x]\](?: `.{6}>.{6}`)? (.+)";

	for l in (start+1)..task_list.len() {
		let line = task_list.get(l).unwrap();

		if let Some(done) = extract(&line, done_pattern) {
			version.done &= done == "x";

			if let Some(task) = extract(&line, task_pattern) {
				version.tasks.push_back(task);
			}
		} else {
			break;
		}
	}

	return version;
}

fn start_of_current_tasks(task_list: &Vec<String>, branch: String) -> usize {
	let mut start = START_OF_VERSIONS;

	let header = format!("## <a name=\"{}\">", branch);

	while start < task_list.len() && !task_list.get(start).unwrap().starts_with(&header) {
		start += 1;
	}

	if start == task_list.len() {
		return START_OF_VERSIONS;
	}

	return start;
}

fn end_not_done(version: String) -> Option<Version> {
	if has_pull_request(version) {
		throw(11, "There is an opened pull request and the version is not done");
	}

	success();
}

pub struct Version {
	pub code: String,
	pub done: bool,
	pub prev: String,
	pub next: String,
	pub tasks: LinkedList<String>,
}

impl Version {
	pub fn new(code: String, prev: String) -> Self {
		Version {
			code,
			done: true,
			prev,
			next: String::new(),
			tasks: LinkedList::new(),
		}
	}
}

impl ToString for Version {
	fn to_string(&self) -> String {
		format!(
			"{} [{}] .. > {} .. < {}",
			self.code, self.done, self.prev, self.next
		)
	}
}
