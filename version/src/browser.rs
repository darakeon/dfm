use regex::Regex;

use crate::file::{get_path, get_content, set_content};
use crate::version::Version;

fn path_main() -> String { get_path(vec!["..", "site", "Tests", "Browser", "package.json"]) }
fn path_lock() -> String { get_path(vec!["..", "site", "Tests", "Browser", "package-lock.json"]) }

pub fn update_node(version: &Version) {
	let old_version = config_version(&version.prev);
	let new_version = config_version(&version.code);

	let content_main = get_content(path_main())
		.replace(&old_version, &new_version);

	set_content(path_main(), content_main);

	let content_lock = get_content(path_lock())
		.replace(&old_version, &new_version);

	set_content(path_lock(), content_lock);
}

fn config_version(version: &str) -> String {
	let regex = Regex::new(r"(\d+)\.(\d+\.\d+\.\d+)").unwrap();
	let node_version = regex.replace(version, "${1}0$2");

	format!(r#""version": "{}""#, node_version)
}
