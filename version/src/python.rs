use crate::file::{get_path, get_content, set_content};
use crate::version::Version;

fn path_settings() -> String { get_path(vec!["..", "midna", "src", "midna", "settings.py"]) }

pub fn update_python(version: &Version) {
	let old_version = config_version(&version.prod);
	let new_version = config_version(&version.dev);

	let content_settings = get_content(path_settings())
		.replace(&old_version, &new_version);

	set_content(path_settings(), content_settings);
}

fn config_version(version: &str) -> String {
	format!(r#"APP_VERSION = "{}""#, version)
}
