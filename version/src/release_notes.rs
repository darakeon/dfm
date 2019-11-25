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

	content.remove(0);
	let new_release = format!("{{\n\t\"{}\": [\n\t],", &version.code);
	let new_content = format!("{}{}", new_release, content);

	fs::write(path, new_content)
		.expect("error on tasks recording");
}
