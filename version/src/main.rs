mod android;
mod arguments;
mod browser;
mod csharp;
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
mod version_id;

use crate::end::throw;
use android::update_android;
use arguments::{parse_arguments,ProgramOption};
use todos::add_release;
use browser::update_node;
use csharp::update_csharp;
use end::success;
use git::{update_local,go_to_main,commit,connect_local_and_remote_branch,create_tag,create_branch,remove_branch,update_remote,stash};
use maintenance::update_maintenance_api_json;
use notes::update_notes;
use rust::update_rust;
use tasks::update_task_list;
use version::{create_version,Version};
use version_id::update_version_id;

fn main() {
	stash("running version");

	let execution = || -> Result<(), ()> {
		if let Some((option, numbers)) = parse_arguments() {
			if let Some(version) = create_version(&option) {
				match option {
					ProgramOption::Check =>
						success("Version is alright!"),
					ProgramOption::Git =>
						update_git(version),
					_ =>
						update_version(version, option, numbers),
				}
			}
		}

		Ok(())
	};

	let result = execution();
	result.unwrap();
}

fn update_git(version: Version) {
	if !version.done {
		return;
	}

	update_local();

	go_to_main();

	let tag = version.prod.clone();
	let mut tasks = "".to_string();

	for task in version.tasks.iter() {
		tasks += task;
		tasks += "\n";
	}

	create_tag(&tag, &tasks);

	let old_branch = version.prod;
	let new_branch = version.dev;

	create_branch(&new_branch);
	remove_branch(&old_branch);

	update_remote(&tag, &new_branch);
	remove_branch("main");

	connect_local_and_remote_branch(&new_branch);
}

fn update_version(version: Version, option: ProgramOption, numbers: Vec<usize>) {
	let update_notes_result = update_notes(&version);

	if update_notes_result.is_err() {
		throw(31, "errors while translating release");
	}

	if option != ProgramOption::Empty {
		add_release(version.dev.clone(), numbers);
	}

	update_task_list(&version);
	update_android(&version);
	update_csharp(&version);
	update_rust(&version);
	update_node(&version);
	update_maintenance_api_json(&version);

	update_version_id(&version);

	commit(&format!("version: update to {}", &version.dev));
}
