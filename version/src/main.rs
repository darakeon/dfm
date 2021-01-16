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
use arguments::{parse_arguments,ProgramOption};
use browser::update_node;
use csharp::update_csharp;
use end::success;
use git::{update_local,go_to_main,commit,connect_local_and_remote_branch,create_tag,create_branch,remove_branch,update_remote,stash,stash_pop};
use notes::update_notes;
use rust::update_rust;
use tasks::update_task_list;
use version::{create_version,Version};

fn main() {
	stash("running version");

	if let Some((option, numbers)) = parse_arguments() {
		if let Some(version) = create_version(&option, numbers) {
			match option {
				ProgramOption::Check =>
					success().unwrap(),
				ProgramOption::Git =>
					update_git(version),
				_ =>
					update_version(version),
			}
		}
	}

	stash_pop();
}

fn update_git(version: Version) {
	if !version.done {
		return;
	}

	let tag = version.prev.clone();
	let old_branch = version.prev;
	let new_branch = version.code;
	let mut tasks = "".to_string();

	for task in version.tasks.iter() {
		tasks += task;
		tasks += "\n";
	}

	update_local(&old_branch);
	go_to_main();
	create_tag(&tag, &tasks);
	create_branch(&new_branch);
	remove_branch(&old_branch);
	update_remote(&tag, &new_branch);
	remove_branch("main");
	connect_local_and_remote_branch(&new_branch);
}

fn update_version(version: Version) {
	update_task_list(&version);
	update_android(&version);
	update_csharp(&version);
	update_rust(&version);
	update_node(&version);
	update_notes(&version);

	commit(&format!("version: update to {}", &version.code));
}
