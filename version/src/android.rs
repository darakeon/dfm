use regex::Regex;

use crate::file::{get_path, get_content, set_content};
use crate::git::list_changes_main;
use crate::version::Version;

fn path_main() -> String { get_path(vec!["..", "android", "build.gradle"]) }
fn path_app() -> String { get_path(vec!["..", "android", "App", "build.gradle"]) }
fn path_error_logs() -> String { get_path(vec!["..", "android", "ErrorLogs", "build.gradle"]) }

pub fn update_android(version: &Version) {
	let changes = list_changes_main();
	let changed_general = general_changed(&changes);

	let updated_main = update_main(version);

	if !updated_main { return }

	if changed_general || changed("App", &changes) {
		change_code(path_app(), "(2011\\d{6})");
	}

	if changed_general || changed("ErrorLogs", &changes) {
		change_code(path_error_logs(), "(\\d+)");
	}
}

fn general_changed(list: &Vec<String>) -> bool {
	let checks = vec![
		"Lib",
		"TestUtils",
		"build.gradle",
		"gradle.properties",
		"settings.gradle"
	];

	for check in checks {
		if changed(check, list) {
			return true;
		}
	}

	return false;
}

fn changed(name: &str, list: &Vec<String>) -> bool {
	let path = format!("android/{}", name);

	for item in list {
		if item.starts_with(&path) {
			return true;
		}
	}

	return false;
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
