use crate::version::Version;
use std::fs;

static PATH: &str = r"..\site\Language\Language\Version\";
static EXT: &str = r".json";

pub fn update_notes(version: &Version) {
	update_notes_for_language(version, "en-US");
	update_notes_for_language(version, "pt-BR");
}

pub fn update_notes_for_language(version: &Version, language: &str) {
	let path = format!("{}{}{}", PATH, language, EXT);
	let mut content = fs::read_to_string(&path).unwrap();

	if content.contains(&version.code) {
		return;
	}

	let mut tasks_json = "".to_string();
	let mut tasks = version.tasks.clone();
	
	if let Some(first_task) = tasks.pop_front() {
		tasks_json = make_json(&first_task);

		while let Some(task) = tasks.pop_front() {
			tasks_json = format!(
				"{},\n{}",
				tasks_json,
				make_json(&task)
			);
		}
	}

	let new_release = format!(
		"{{\n\t\"{}\": [\n{}\n\t],",
		&version.code,
		tasks_json
	);

	content.remove(0);
	let new_content = format!("{}{}", new_release, content);

	fs::write(path, new_content)
		.expect("error on tasks recording");
}

fn make_json(task: &str) -> String {
	format!("\t\t\"{}\"", task)
}
