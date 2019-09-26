mod version;
use version::current_version;

mod task_list;
use task_list::{task_list, update_task_list};

mod android;
use android::update_android;

mod csharp;
use csharp::update_csharp;

mod rust;
use rust::update_rust;

fn main() {
	let task_list = task_list();

	let version =
		current_version(task_list).expect("not version found");

	update_task_list(&version);

	update_android(&version);

	update_csharp(&version);

	update_rust(&version);
}
