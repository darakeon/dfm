use regex::Regex;
use std::fs;

use crate::version::Version;

static PATH: &str = r"..\site\Tests\Browser\package.json";

pub fn update_node(version: &Version) {
	let old_version = config_version(&version.prev);
	let new_version = config_version(&version.code);

	let content = fs::read_to_string(PATH)
		.unwrap()
		.replace(&old_version, &new_version);

	fs::write(PATH, content).expect("error on node recording");
}

fn config_version(version: &str) -> String {
	let regex = Regex::new(r"(\d+)\.(\d+\.\d+\.\d+)").unwrap();
	let node_version = regex.replace(version, "${1}0$2");

	format!(r#""version": "{}""#, node_version)
}
