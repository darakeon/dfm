use regex::Regex;
use std::env;

use crate::file::{get_path, get_content, set_content};
use crate::version::Version;

fn path_main() -> String { get_path(vec!["..", "android", "build.gradle"]) }
fn path_app() -> String { get_path(vec!["..", "android", "App", "build.gradle"]) }
fn path_error_logs() -> String { get_path(vec!["..", "android", "ErrorLogs", "build.gradle"]) }

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

	change_code(path_app(), "(2011\\d{6})");
	change_code(path_error_logs(), "(\\d+)");
}

fn update_main(version: &Version) -> bool {
	let mut content = get_content(path_main());

	if !content.contains(&version.prev) {
		return false;
	}

	let old_name = version_name(&version.prev);
	let new_name = version_name(&version.code);

	content = content.replace(&old_name, &new_name);

	set_content(path_main(), content);

	return true
}

fn version_name(version: &str) -> String {
	format!(r#"ext.dfm_version = "{}""#, version)
}

fn change_code(path: String, pattern: &str) {
	let mut content = get_content(path.clone());

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

	set_content(path, content);
}

fn version_code(version: &str) -> String {
	format!("versionCode {}", version)
}
