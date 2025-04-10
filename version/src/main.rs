mod android;
mod arguments;
mod browser;
mod csharp;
mod dependabot;
mod commit_checker;
mod end;
mod file;
mod git;
mod maintenance;
mod notes;
mod regex;
mod rust;
mod tasks;
mod todos;
mod version;

use crate::end::throw;
use android::update_android;
use arguments::{parse_arguments,ProgramOption};
use browser::update_node;
use csharp::update_csharp;
use dependabot::update_dependabot;
use commit_checker::update_commit_checker;
use end::success;
use git::{update_local,go_to_main,commit,connect_local_and_remote_branch,create_tag,create_branch,remove_branch,update_remote,stash,stash_pop};
use maintenance::update_maintenance_api_json;
use notes::update_notes;
use rust::update_rust;
use tasks::update_task_list;
use version::{create_version,Version};

fn main() {
	stash("running version");

	let execution = || -> Result<(), ()> {
		if let Some((option, numbers)) = parse_arguments() {
			if let Some(version) = create_version(&option, numbers) {
				match option {
					ProgramOption::Check =>
						success(),
					ProgramOption::Git =>
						update_git(version),
					_ =>
						update_version(version),
				}
			}
		}

		Ok(())
	};

	let result = execution();

	stash_pop();

	result.unwrap();
}

fn update_git(version: Version) {
	if !version.done {
		return;
	}

	update_local();

	go_to_main();

	let tag = version.prev.clone();
	let mut tasks = "".to_string();

	for task in version.tasks.iter() {
		tasks += task;
		tasks += "\n";
	}

	create_tag(&tag, &tasks);

	let old_branch = version.prev;
	let new_branch = version.code;

	create_branch(&new_branch);
	remove_branch(&old_branch);

	update_remote(&tag, &new_branch);
	remove_branch("main");

	connect_local_and_remote_branch(&new_branch);
}

fn update_version(version: Version) {
	let update_notes_result = update_notes(&version);

	if update_notes_result.is_err() {
		throw(31, "errors while translating release");
	}

	update_task_list(&version);
	update_android(&version);
	update_csharp(&version);
	update_dependabot(&version);
	update_commit_checker(&version);
	update_rust(&version);
	update_node(&version);
	update_maintenance_api_json(&version);

	commit(&format!("version: update to {}", &version.code));
}
