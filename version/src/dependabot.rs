use crate::file::{get_path, get_content, set_content};
use crate::version::Version;

fn path() -> String { get_path(vec!["..", ".github", "dependabot.yml"]) }

pub fn update_dependabot(version: &Version) {
	let old_version = config_version(&version.dev);
	let new_version = config_version("main");

	let content = get_content(path())
		.replace(&old_version, &new_version);

	set_content(path(), content);
}

fn config_version(version: &str) -> String {
	format!(r#"    target-branch: "{}""#, version)
}
