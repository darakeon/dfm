use regex::Regex;

use crate::end::throw;
use crate::file::{get_path,get_lines,set_lines};

fn path_todo() -> String { get_path(vec!["..", "docs", "TODO.md"]) }
fn path_release() -> String { get_path(vec!["..", "docs", "RELEASES.md"]) }

pub fn add_release(code: String, numbers: Vec<usize>) {
	let (new_tasks, sizes) = process_tasks(numbers);

	let icon = get_new_version_icon(sizes);

	write_release(code.clone(), new_tasks, icon);
}

fn process_tasks(mut numbers: Vec<usize>) -> (Vec<String>, Vec<String>) {
	let mut count: usize = 0;

	let mut new_tasks: Vec<String> = Vec::new();
	let mut sizes: Vec<String> = Vec::new();

	let mut todo_list = get_lines(path_todo());
	let mut line_number = 16;

	let old_size = todo_list.len();

	while numbers.len() > 0 && line_number < todo_list.len() {
		let line = todo_list.get(line_number).unwrap();
		line_number += 1;

		if let Some((todo, size)) = extract_task(&line) {
			count += 1;

			if let Some(n) = numbers.iter().position(|&x| x == count) {
				let task = format!("- [ ] {}", todo.trim());
				new_tasks.push(task);

				sizes.push(size);

				numbers.remove(n);

				line_number -= 1;
				todo_list.remove(line_number);
			}
		}
	}

	let new_size = todo_list.len();

	if let Some(title) = todo_list.get_mut(16) {
		let old_count = extract_count(title);
		let new_count = old_count - (old_size - new_size);

		*title = (
			*title.replace(
				&old_count.to_string(),
				&new_count.to_string()
			)
		).to_string();
	}

	set_lines(path_todo(), todo_list);

	return (new_tasks, sizes);
}

fn extract_task(text: &str) -> Option<(String, String)> {
	let pattern =
		r#"^\| ([^|]+) +\|  \w  \| (.) +\|  \d  \|  \d  \|  \d  \|$"#;

	let regex = Regex::new(pattern).unwrap();

	if !regex.is_match(text) {
		return None;
	}

	let captures = regex.captures(text).unwrap();

	let task = captures.get(1).unwrap().as_str().to_string();
	let size = captures.get(2).unwrap().as_str().to_string();

	Some((task, size))
}

fn extract_count(text: &str) -> usize {
	let pattern = r#"^\| Task \((\d+)\)"#;
	let regex = Regex::new(pattern).unwrap();

	if !regex.is_match(text) {
		return 0;
	}

	let captures = regex.captures(text).unwrap();

	return captures.get(1).unwrap().as_str().parse::<usize>().unwrap();
}

fn get_new_version_icon(sizes: Vec<String>) -> String {
	let dragon = "🐉".to_string();
	if sizes.contains(&dragon) {
		return dragon;
	}

	let whale = "🐳".to_string();
	if sizes.contains(&whale) {
		return whale;
	}

	let sheep = "🐑".to_string();
	if sizes.contains(&sheep) {
		return sheep;
	}

	let ant = "🐜".to_string();
	if sizes.contains(&ant) {
		return ant;
	}

	throw(21, "Unknown version size");
}

fn write_release(prod: String, new_tasks: Vec<String>, icon: String) {
	let mut new_version: Vec<String> = Vec::new();

	let count = new_tasks.len();

	new_version.push(format!(
		"## <a name=\"development\"></a>Development :{}: <sup>`{}`</sup>",
		icon, count
	));

	for t in (0..count).rev() {
		let task = new_tasks.get(t).unwrap();
		new_version.push(task.to_string());
	}

	new_version.push("".to_string());

	let mut release_list = get_lines(path_release());

	let mut prod_version = release_list.get(16).unwrap().to_string();
	prod_version = prod_version.replace("Development", &prod);
	prod_version = prod_version.replace("development", &prod);
	new_version.push(prod_version);

	release_list.splice(16..17, new_version);

	set_lines(path_release(), release_list);
}
