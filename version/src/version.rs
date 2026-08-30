use regex::Regex;
use std::collections::LinkedList;

use crate::arguments::ProgramOption;
use crate::end::{success,throw};
use crate::file::{get_path, get_lines};
use crate::regex::{extract, extract_line};


const START_OF_VERSIONS: usize = 16;

fn current_version_path() -> String { get_path(vec!["..", "docs", "current-version"]) }
fn releases_path() -> String { get_path(vec!["..", "docs", "RELEASES.md"]) }


pub fn create_version(option: &ProgramOption) -> Option<Version> {
	let prod = get_lines(current_version_path())[0].clone();

	let task_list = get_lines(releases_path());
	let pattern = r"Development :(.):";

	let dev_augmentor = extract_line(&task_list, START_OF_VERSIONS, pattern);

	let version = mount_version(
		prod.clone(),
		dev_augmentor.clone(),
		&task_list,
	);

	if option == &ProgramOption::Git {
		return Some(version);
	}

	if !version.done {
		success("Version is not done yet");
	}

	if version.tasks.len() == 0 {
		throw(12, "Version without tasks");
	}

	Some(version)
}

fn mount_version(
	prod: String,
	dev_augmentor: String,
	task_list: &Vec<String>,
) -> Version {
	let dev = get_next(dev_augmentor, prod.clone());

	let mut version = Version::new(dev, prod);

	let done_pattern = r"^\- \[([ x])\] ";
	let task_pattern = r"^\- \[[ x]\](?: `.{6}>.{6}`)? (.+)";

	let mut count_all = 0;
	let mut count_done = 0;

	for l in (START_OF_VERSIONS+1)..task_list.len() {
		let line = task_list.get(l).unwrap();

		if let Some(done) = extract(&line, done_pattern) {
			count_all += 1;
			count_done += if done == "x" { 1 } else { 0 };

			if let Some(task) = extract(&line, task_pattern) {
				version.tasks.push_back(task);
			}
		} else {
			break;
		}
	}

	version.done = count_all == count_done;

	return version;
}

fn get_next(size: String, current: String) -> String {
	let new_version = get_new_version(size);

	if let Some((size_pattern, end)) = new_version {
		let regex = Regex::new(&size_pattern).unwrap();
		let captures = regex.captures(&current).unwrap();

		let start = captures.get(1).unwrap().as_str().to_string();
		let change: i32 = captures.get(2).unwrap().as_str().parse().unwrap();

		return format!("{}{}{}", start, change + 1, end);
	}

	throw(11, "Unknown next version");
}

fn get_new_version(size: String) -> Option<(String, String)> {
	let dragon = "🐉".to_string();
	if size == dragon {
		return Some((r"()(\d+)\.\d+\.\d+\.\d+".to_string(), r".0.0.0".to_string()));
	}

	let whale = "🐳".to_string();
	if size == whale {
		return Some((r"(\d+\.)(\d+)\.\d+\.\d+".to_string(), r".0.0".to_string()));
	}

	let sheep = "🐑".to_string();
	if size == sheep {
		return Some((r"(\d+\.\d+\.)(\d+)\.\d+".to_string(), r".0".to_string()));
	}

	let ant = "🐜".to_string();
	if size == ant {
		return Some((r"(\d+\.\d+\.\d+\.)(\d+)".to_string(), r"".to_string()));
	}

	return None;
}

#[derive(Debug)]
pub struct Version {
	pub dev: String,
	pub prod: String,
	pub done: bool,
	pub tasks: LinkedList<String>,
}

impl Version {
	pub fn new(dev: String, prod: String) -> Self {
		Version {
			dev: dev,
			prod: prod,
			done: false,
			tasks: LinkedList::new(),
		}
	}
}

impl ToString for Version {
	fn to_string(&self) -> String {
		format!(
			"{} > {} [{}]",
			self.prod, self.dev, self.done
		)
	}
}
