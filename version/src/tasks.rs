use crate::file::{get_path, get_content, set_content};
use crate::version::Version;

fn path() -> String { get_path(vec!["..", "docs", "RELEASES.md"]) }

pub fn update_task_list(version: &Version) {
	let old_published = published(&version.prev);
	let old_development = development(&version.code);
	let old_state = state(&version.code);

	let new_published = published(&version.code);
	let new_development = development(&version.next);
	let new_state = state(&version.next);

	let content = get_content(path())
		.replace(&old_development, &new_development)
		.replace(&old_published, &new_published)
		.replace(&old_state, &new_state);

	set_content(path(), content);
}

fn published(version: &str) -> String {
	format!("[go to published version](#{})", version)
}

fn development(version: &str) -> String {
	format!("[go to version in development](#{})", version)
}

fn state(version: &str) -> String {
	format!("{}/docs/RELEASES.md#{}", version, version)
}
