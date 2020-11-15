use crate::version::Version;
use std::fs;

static PATH: &str = r"..\docs\RELEASES.md";

pub fn update_task_list(version: &Version) {
	let old_published = published(&version.prev);
	let old_development = development(&version.code);
	let old_state = state(&version.code);

	let new_published = published(&version.code);
	let new_development = development(&version.next);
	let new_state = state(&version.next);

	let content = fs::read_to_string(PATH)
		.unwrap()
		.replace(&old_development, &new_development)
		.replace(&old_published, &new_published)
		.replace(&old_state, &new_state);

	fs::write(PATH, content).expect("error on tasks recording");
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
