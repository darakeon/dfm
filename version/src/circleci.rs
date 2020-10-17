use std::fs;

use crate::version::Version;

static PATH_MAIN: &str = r"..\.circleci\config.yml";

pub fn update_config(version: &Version) {
	update_main(version);
}

fn update_main(version: &Version) {
	let mut content = fs::read_to_string(PATH_MAIN).unwrap();

	if !content.contains(&version.prev) {
		return;
	}
	
	let old_name = version_name(&version.prev);
	let new_name = version_name(&version.code);

	content = content.replace(&old_name, &new_name);

	fs::write(PATH_MAIN, content).expect("error on circle ci recording");
}

fn version_name(version: &str) -> String {
	format!(r#"                - {} #version"#, version)
}
