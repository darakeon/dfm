use regex::Regex;
use std::env;
use std::fs;

use crate::version::Version;

static PATH_MAIN: &str = r"..\android\build.gradle";
static PATH_APP: &str = r"..\android\App\build.gradle";
static PATH_ERROR_LOGS: &str = r"..\android\ErrorLogs\build.gradle";

pub fn update_android(version: &Version) {
	let changed = update_main(version);

	if !changed {
		return
	}

	let args: Vec<String> = env::args().collect();
	let change_android = args.len() < 2 || args[1] != "--no-android";

	if !change_android {
		return
	}

	change_code(PATH_APP, "(2011\\d{6})");
	change_code(PATH_ERROR_LOGS, "(\\d+)");
}

fn update_main(version: &Version) -> bool {
	let mut content = fs::read_to_string(PATH_MAIN).unwrap();

	if !content.contains(&version.prev) {
		return false;
	}

	let old_name = version_name(&version.prev);
	let new_name = version_name(&version.code);

	content = content.replace(&old_name, &new_name);

	fs::write(PATH_MAIN, content).expect("error on android recording");

	return true
}

fn version_name(version: &str) -> String {
	format!(r#"ext.dfm_version = "{}""#, version)
}

fn change_code(path: &str, pattern: &str) {
	let mut content = fs::read_to_string(path).unwrap();

	let pattern = version_code(&pattern);
	let regex = Regex::new(&pattern).unwrap();

	let old_number = regex
		.captures(&content)
		.unwrap()
		.get(1)
		.unwrap()
		.as_str()
		.parse::<i32>()
		.unwrap();

	let new_number = old_number + 1;

	let old_code = version_code(&old_number.to_string());
	let new_code = version_code(&new_number.to_string());

	content = content.replace(&old_code, &new_code);

	fs::write(path, content).expect("error on android recording");
}

fn version_code(version: &str) -> String {
	format!("versionCode {}", version)
}
