use json;
use json::JsonValue;

use crate::file::{get_path, get_content, set_content};
use crate::version::Version;

fn path() -> String { get_path(vec!["..", "core", "Language", "Language", "Version"]) }
static EXT: &str = r".json";

pub fn update_notes(version: &Version) {
	update_notes_for_language(version, "en-us");
	update_notes_for_language(version, "pt-br");
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

	let translations_path = get_path(vec!["..", "docs", "translations.json"]);
	let translations_file = get_content(translations_path);
	let translations = json::parse(&translations_file).unwrap();

	while let Some(task) = tasks.pop_front() {
		let translated = &translations[language][&task];
		if let Some(formatted) = format(&task, translated) {
			if tasks_json.len() > 0 {
				tasks_json = format!(
					"{},\n",
					tasks_json,
				);
			}

			tasks_json = format!(
				"{}{}",
				tasks_json,
				formatted,
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

	set_content(path, new_content);
}

fn format(task: &str, translation: &JsonValue) -> Option<String> {
	let mut result = translation.to_string().trim().to_string();
	print!("\"{}\": {}", task, result);

	if result == "-" {
		return None;
	}

	if result == "=" {
		result = task.to_string()
	}

	return Some(format!("\t\t\"{}\"", result));
}
