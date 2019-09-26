use regex::Regex;

mod version;
pub use version::Version;

pub fn current_version(task_list: Vec<String>) -> Option<Version> {
	let version_pattern =
		r#"^\#\# (?:<a name="[a-z]+"></a>)?(\d+\.\d+\.\d+\.\d+)"#;
	let task_pattern = r"^\- \[([ x])\] ";

	let mut current_version: Version = Version::empty();

	for line in task_list {
		if let Some(version_code) =
			get_by_pattern(&line, version_pattern)
		{
			if current_version.done {
				current_version.prev = version_code;
				break;
			}

			current_version =
				Version::new(version_code, current_version.code);
		} else if let Some(task) = get_by_pattern(&line, task_pattern) {
			current_version.done &= task == "x";
		}
	}

	if current_version.code.is_empty() {
		None
	} else {
		Some(current_version)
	}
}

fn get_by_pattern(text: &str, pattern: &str) -> Option<String> {
	let regex = Regex::new(pattern).unwrap();

	if regex.is_match(text) {
		let captures = regex.captures(text).unwrap();
		Some(captures.get(1).unwrap().as_str().to_string())
	} else {
		None
	}
}
