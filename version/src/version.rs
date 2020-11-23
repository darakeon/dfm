use regex::Regex;
use std::collections::LinkedList;

use crate::file::get_lines;
use crate::git::current_branch;

pub fn create_version() -> Option<Version> {
	let task_list = get_lines(r"..\docs\RELEASES.md");

	let version_pattern = r"^\- \[.+\]\(\#(\d+\.\d+\.\d+\.\d+)\)$";

	let published = extract_line(&task_list, 6, version_pattern);
	let development = extract_line(&task_list, 7, version_pattern);

	let branch = current_branch().unwrap();

	if development != branch {		
		println!("Branch is '{}', but release is of '{}'", branch, development);
		return None;
	}

	let version = mount_version(published, development, &task_list);

	if !version.done {
		eprintln!("Version is not done");
		return None;
	}

	if version.tasks.len() == 0 {
		eprintln!("Version without tasks");
		return None;
	}

	Some(version)
}

fn extract_line(lines: &Vec<String>, position: usize, pattern: &str) -> String {
	let line = lines.get(position).unwrap();
	return extract(line, pattern).unwrap();
}

fn extract(text: &str, pattern: &str) -> Option<String> {
	let regex = Regex::new(pattern).unwrap();

	if regex.is_match(text) {
		let captures = regex.captures(text).unwrap();
		Some(captures.get(1).unwrap().as_str().to_string())
	} else {
		None
	}
}

fn mount_version(published: String, development: String, task_list: &Vec<String>) -> Version {
	let mut version = Version::new(development, published);

	let done_pattern = r"^\- \[([ x])\] ";
	let task_pattern = r"^\- \[[ x]\](?: `.{6}>.{6}`)? (.+)";

	for l in 17..task_list.len() {
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
