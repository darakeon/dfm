use crate::version::Version;
use std::fs;

static PATH: &str = r"..\docs\TASKS.MD";

pub fn task_list() -> Vec<String> {
	fs::read_to_string(PATH)
		.unwrap()
		.replace("\r", "")
		.split("\n")
		.map(|s| s.to_string())
		.collect()
}

pub fn update_task_list(version: &Version) {
	let old_link = link(&version.code);
	let new_link = link(&version.next);

	let old_prev = prod_anchor(&version.prev);
	let new_prev = no_anchor(&version.prev);

	let old_curr = dev_anchor(&version.code);
	let new_curr = prod_anchor(&version.code);

	let old_next = no_anchor(&version.next);
	let new_next = dev_anchor(&version.next);

	let content = fs::read_to_string(PATH)
		.unwrap()
		.replace(&old_link, &new_link)
		.replace(&old_prev, &new_prev)
		.replace(&old_curr, &new_curr)
		.replace(&old_next, &new_next);

	fs::write(PATH, content).expect("error on tasks recording");
}

fn link(version: &str) -> String {
	format!("{}/docs/TASKS.md", version)
}

fn dev_anchor(version: &str) -> String {
	format!(r#"## <a name="dev"></a>{}"#, version)
}

fn prod_anchor(version: &str) -> String {
	format!(r#"## <a name="prod"></a>{}"#, version)
}

fn no_anchor(version: &str) -> String {
	format!(r#"## {}"#, version)
}
