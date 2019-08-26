mod version;
use version::current_version;

mod task_list;
use task_list::task_list;

fn main() {
	let task_list = task_list();

	if let Some(version) = current_version(task_list) {
		print!("{}", version.to_string());
	}
}
