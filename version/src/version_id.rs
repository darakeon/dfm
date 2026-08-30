use crate::file::{get_path, set_content};
use crate::version::Version;

fn path() -> String { get_path(vec!["..", "docs", "current-version"]) }

pub fn update_version_id(version: &Version) {
	set_content(path(), version.dev.clone());
}
