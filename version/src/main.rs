mod android;
mod arguments;
mod browser;
mod csharp;
mod end;
mod file;
mod git;
mod notes;
mod regex;
mod rust;
mod tasks;
mod todos;
mod version;

use android::update_android;
use arguments::parse_arguments;
use browser::update_node;
use csharp::update_csharp;
use end::success;
use notes::update_notes;
use rust::update_rust;
use tasks::update_task_list;
use version::{create_version,Version};

fn main() {
	if let Some((just_check, numbers)) = parse_arguments() {
		if let Some(version) = create_version(just_check, numbers) {
			if just_check {
				return success().unwrap();
			}

			update_version(version);
		}
	}
}

fn update_version(version: Version) {
	update_task_list(&version);
	update_android(&version);
	update_csharp(&version);
	update_rust(&version);
	update_node(&version);
	update_notes(&version);
}
