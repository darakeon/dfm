use regex::Regex;
use std::fs;

use crate::version::Version;

static PATH: &str = r"..\version\Cargo.toml";

pub fn update_rust(version: &Version) {
	let old_version = config_version(&version.prev);
	let new_version = config_version(&version.code);

	let content = fs::read_to_string(PATH)
		.unwrap()
		.replace(&old_version, &new_version);

	fs::write(PATH, content).expect("error on rust recording");
}

fn config_version(version: &str) -> String {
	let regex = Regex::new(r"(\d+)\.(\d+\.\d+\.\d+)").unwrap();
	let rust_version = regex.replace(version, "${1}0$2");

	format!(r#"version = "{}""#, rust_version)
}
