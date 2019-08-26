use std::fs;

pub fn task_list() -> Vec<String> {
	fs::read_to_string(r"..\docs\TASKS.MD")
		.unwrap()
		.replace("\r", "")
		.split("\n")
		.map(|s| s.to_string())
		.collect()
}
