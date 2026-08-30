use crate::file::{get_path, get_content, set_content};
use crate::version::Version;

fn path() -> String { get_path(vec!["..", "docs", "RELEASES.md"]) }

pub fn update_task_list(version: &Version) {
	let old_published = published(&version.prod);
	let new_published = published(&version.dev);

	let content = get_content(path())
		.replace(&old_published, &new_published);

	set_content(path(), content);
}

fn published(version: &str) -> String {
	format!("[go to published version](#{})", version)
}
