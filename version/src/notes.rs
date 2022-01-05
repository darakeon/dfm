use json;
use json::JsonValue;

use crate::file::{get_path, get_content, set_content};
use crate::version::Version;

fn path() -> String { get_path(vec!["..", "core", "Language", "Language", "Version"]) }
static EXT: &str = r".json";

pub fn update_notes(version: &Version) -> Result<(), ()> {
	let errors =
		update_notes_for_language(version, "pt-br")
		+
		update_notes_for_language(version, "en-us");

	if errors == 0 {
		return Ok(());
	}

	return Err(());
}

pub fn update_notes_for_language(version: &Version, language: &str) -> i32 {
	println!("\n{} file:", language);

	let path = get_path(vec![&path(), &format!("{}{}", language, EXT)]);
	let mut content = get_content(path.clone());

	let mut errors = 0;

	if content.contains(&version.code) {
		return errors;
	}

	let mut tasks_json = "".to_string();
	let mut tasks = version.tasks.clone();

	let translations_path = get_path(vec!["..", "docs", "translations.json"]);
	let translations_file = get_content(translations_path);
	let translations = json::parse(&translations_file).unwrap();

	while let Some(task) = tasks.pop_front() {
		let translated = &translations[language][&task];

		match format(language, &task, translated) {
			Ok(format_ok) => {
				if let Some(formatted) = format_ok {
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
			},
			Err(()) => {
				errors += 1;
			},
		}
	}

	if errors == 0 {
		let new_release = format!(
			"{{\n\t\"{}\": [\n{}\n\t],",
			&version.code,
			tasks_json
		);
	
		content.remove(0);
		let new_content = format!("{}{}", new_release, content);
	
		set_content(path, new_content);
	}

	println!();

	return errors;
}

fn format(language: &str, task: &str, translation: &JsonValue) -> Result<Option<String>, ()> {
	if translation.is_null() {
		eprintln!(
			"task \"{}\" not translated to lang {}",
			task, language
		);

		return Err(());
	}

	let mut result = translation.to_string().trim().to_string();

	println!("\"{}\": {}", task, result);

	if result == "-" {
		return Ok(None);
	}

	if result == "=" {
		result = task.to_string()
	}

	return Ok(Some(format!("\t\t\"{}\"", result)));
}
