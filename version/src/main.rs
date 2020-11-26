use std::process::exit;

mod android;
mod arguments;
mod browser;
mod csharp;
mod file;
mod git;
mod notes;
mod rust;
mod tasks;
mod todos;
mod version;

use android::update_android;
use arguments::parse_arguments;
use browser::update_node;
use csharp::update_csharp;
use notes::update_notes;
use rust::update_rust;
use tasks::update_task_list;
use todos::add_release;
use version::{create_version,Version};

fn main() {
	if let Some((just_check, numbers)) = parse_arguments() {
		if let Some(version) = create_version(just_check) {
			if just_check {
				exit(0);
			}

			if let Some(new_version) = add_release(version, numbers) {
				update_version(new_version);
			}
		}
	}
	exit(1);
}

fn update_version(version: Version) {
	update_task_list(&version);
	update_android(&version);
	update_csharp(&version);
	update_rust(&version);
	update_node(&version);
	update_notes(&version);
	exit(0);
}
