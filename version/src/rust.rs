use regex::Regex;

use crate::file::{get_path, get_content, set_content};
use crate::version::Version;

fn path_toml() -> String { get_path(vec!["..", "version", "Cargo.toml"]) }
fn path_lock() -> String { get_path(vec!["..", "version", "Cargo.lock"]) }

pub fn update_rust(version: &Version) {
	let old_version = config_version(&version.prev);
	let new_version = config_version(&version.code);

	let content_toml = get_content(path_toml())
		.replace(&old_version, &new_version);

	set_content(path_toml(), content_toml);

	let content_lock = get_content(path_lock())
		.replace(&old_version, &new_version);

	set_content(path_lock(), content_lock);
}

fn config_version(version: &str) -> String {
	let regex = Regex::new(r"(\d+)\.(\d+\.\d+\.\d+)").unwrap();
	let rust_version = regex.replace(version, "${1}0$2");

	format!(r#"version = "{}""#, rust_version)
}
