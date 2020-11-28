use std::collections::LinkedList;

use crate::end::{throw,throw_format};
use crate::file::{get_path, get_lines};
use crate::git::current_branch;
use crate::regex::{extract, extract_line};
use crate::todos::add_release;

fn path() -> String { get_path(vec!["..", "docs", "RELEASES.md"]) }

pub fn create_version(just_check: bool, numbers: Vec<usize>) -> Option<Version> {
	let task_list = get_lines(path());
	let pattern = r"^\- \[.+\]\(\#(\d+\.\d+\.\d+\.\d+)\)$";

	let prod = extract_line(&task_list, 6, pattern);
	let dev = extract_line(&task_list, 7, pattern);

	let version = mount_version(
		prod.clone(),
		dev.clone(),
		&task_list,
		numbers,
	);

	let branch = current_branch().unwrap();

	if just_check {
		if branch != "main" && branch != prod {
			return throw_format(11, format!("Branch is '{}', but release is of '{}'", branch, prod));
		}
	} else {
		if !version.done {
			return throw(12, "Version is not done");
		}

		if branch != dev {
			return throw_format(13, format!("Branch is '{}', but release is of '{}'", branch, dev));
		}
	}

	if version.tasks.len() == 0 {
		return throw(14, "Version without tasks");
	}

	Some(version)
}

fn mount_version(published: String, development: String, task_list: &Vec<String>, numbers: Vec<usize>) -> Version {
	let mut version = Version::new(development.clone(), published);

	let done_pattern = r"^\- \[([ x])\] ";
	let task_pattern = r"^\- \[[ x]\](?: `.{6}>.{6}`)? (.+)";

	let mut start = 16;

	let header = format!("## <a name=\"{}\">", development);

	while !task_list.get(start).unwrap().starts_with(&header) {
		start += 1;
	}

	if start != 16 {
		let pattern = r"(\d+\.\d+\.\d+\.\d+)";
		version.next = extract_line(task_list, 16, pattern);
	} else if numbers.len() > 0 {
		version = add_release(version, numbers).unwrap();
	}

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
