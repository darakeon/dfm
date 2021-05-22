use std::io::{stdin,stdout,Write};

use crate::file::{get_path, get_content, set_content};
use crate::version::Version;

fn path() -> String { get_path(vec!["..", "core", "Language", "Language", "Version"]) }
static EXT: &str = r".json";

pub fn update_notes(version: &Version) {
	update_notes_for_language(version, "en-US");
	update_notes_for_language(version, "pt-BR");
}

pub fn update_notes_for_language(version: &Version, language: &str) {
	print!("\n\n{} file:\n", language);

	let path = get_path(vec![&path(), &format!("{}{}", language, EXT)]);
	let mut content = get_content(path.clone());

	if content.contains(&version.code) {
		return;
	}

	let mut tasks_json = "".to_string();
	let mut tasks = version.tasks.clone();

	if let Some(first_task) = tasks.pop_front() {
		tasks_json = translate_and_format(&first_task).unwrap();

		while let Some(task) = tasks.pop_front() {
			if let Some(translated) = translate_and_format(&task) {
				tasks_json = format!(
					"{},\n{}",
					tasks_json,
					translated
				);
			}
		}
	}

	let new_release = format!(
		"{{\n\t\"{}\": [\n{}\n\t],",
		&version.code,
		tasks_json
	);

	content.remove(0);
	let new_content = format!("{}{}", new_release, content);

	set_content(path, new_content);
}

fn translate_and_format(task: &str) -> Option<String> {
	print!("\"{}\": ", task);
	stdout().flush().unwrap();

	let mut translation = String::new();
	stdin().read_line(&mut translation).unwrap();
	translation = translation.trim().to_string();

	if translation == "-" {
		return None;
	}

	if translation == "=" {
		translation = task.to_string()
	}

	return Some(format!("\t\t\"{}\"", translation));
}
