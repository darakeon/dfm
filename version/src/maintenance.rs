use crate::file::{get_path, get_content, set_content};
use crate::version::Version;

fn path_api() -> String { get_path(vec!["..", "architecture", "maintenance", "api", "index.json"]) }

pub fn update_maintenance_api_json(version: &Version) {
	let content_api = get_content(path_api())
		.replace(&version.prev, &version.code);

	set_content(path_api(), content_api);
}
