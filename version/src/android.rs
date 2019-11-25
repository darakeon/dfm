use regex::Regex;
use std::env;
use std::fs;

use crate::version::Version;

static PATH: &str = r"..\android\DFM\build.gradle";

pub fn update_android(version: &Version) {
	let mut content = fs::read_to_string(PATH).unwrap();

	if !content.contains(&version.prev) {
		return;
	}

	let old_name = version_name(&version.prev);
	let new_name = version_name(&version.code);

	content = content.replace(&old_name, &new_name);

	content = change_code(content);

	fs::write(PATH, content).expect("error on android recording");
}

fn change_code(content: String) -> String {
	let args: Vec<String> = env::args().collect();
	let change_android = args.len() < 2 || args[1] != "--no-android";

	if !change_android {
		return content;
	}

	let pattern = version_code("(2011\\d{6})");
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

	content.replace(&old_code, &new_code)
}

fn version_name(version: &str) -> String {
	format!(r#"versionName "{}""#, version)
}

fn version_code(version: &str) -> String {
	format!("versionCode {}", version)
}
