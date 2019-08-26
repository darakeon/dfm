mod version;
use version::current_version;

mod task_list;
use task_list::{task_list, update_task_list};

fn main() {
	let task_list = task_list();

	let version =
		current_version(task_list).expect("not version found");

	update_task_list(&version);
}
