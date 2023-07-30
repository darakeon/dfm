use regex::Regex;

use crate::file::{get_path, get_content, set_content};
use crate::version::Version;

fn path() -> String { get_path(vec!["..", ".github", "dependabot.yml"]) }

pub fn update_node(version: &Version) {
	let old_version = config_version(&version.prev);
	let new_version = config_version(&version.code);

	let content = get_content(path())
		.replace(&old_version, &new_version);

	set_content(path(), content);
}

fn config_version(version: &str) -> String {
	format!(r#"    target-branch: "{}""#, version)
}
