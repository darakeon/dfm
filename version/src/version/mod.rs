use regex::Regex;

mod version;
pub use version::Version;

pub fn current_version(task_list: Vec<String>) -> Option<Version> {
	let version_pattern =
		r#"^\#\# (?:<a name="\d+\.\d+\.\d+\.\d+"></a>)?(\d+\.\d+\.\d+\.\d+)"#;
	let done_pattern = r"^\- \[([ x])\] ";
	let task_pattern = r"^\- \[x\] `\d{6}>\d{6}` (.+)";

	let mut current_version: Version = Version::empty();

	for line in task_list {
		if let Some(version_code) =
			extract(&line, version_pattern)
		{
			if current_version.done {
				current_version.prev = version_code;
				break;
			}

			current_version =
				Version::new(version_code, current_version.code);
		} else if let Some(done) = extract(&line, done_pattern) {
			current_version.done &= done == "x";
			
			if let Some(task) = extract(&line, task_pattern) {
				current_version.tasks.push_back(task);
			}
		}
	}

	if current_version.code.is_empty() {
		None
	} else {
		Some(current_version)
	}
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
