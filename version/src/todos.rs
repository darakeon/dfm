use regex::Regex;

use crate::end::throw;
use crate::file::{get_path,get_lines,set_lines};

fn path_todo() -> String { get_path(vec!["..", "docs", "TODO.md"]) }
fn path_release() -> String { get_path(vec!["..", "docs", "RELEASES.md"]) }

pub fn add_release(code: String, numbers: Vec<usize>) -> Option<String> {
	let (new_tasks, sizes) = process_tasks(numbers);

	let try_next = get_next(sizes, code);
	if try_next.is_none() {
		return None;
	}
	let (next, icon) = try_next.unwrap();

	write_release(next.clone(), new_tasks, icon);

	return Some(next);
}

fn process_tasks(mut numbers: Vec<usize>) -> (Vec<String>, Vec<String>) {
	let mut count: usize = 0;

	let mut new_tasks: Vec<String> = Vec::new();
	let mut sizes: Vec<String> = Vec::new();

	let mut todo_list = get_lines(path_todo());
	let mut line_number = 15;

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

	new_tasks.push(">>>>> ADD A DEPLOY AUTOMATION TASK".to_string());

	let new_size = todo_list.len();

	if let Some(title) = todo_list.get_mut(15) {
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
		r#"^\| ([^|]+) +\| :(ant|sheep|whale|dragon): +\|  \d  \|  \d  \|  \d  \|$"#;

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

fn get_next(sizes: Vec<String>, current: String) -> Option<(String, String)> {
	let new_version = get_new_version(sizes);

	if let Some((size_pattern, end, icon)) = new_version {
		let regex = Regex::new(&size_pattern).unwrap();
		let captures = regex.captures(&current).unwrap();

		let start = captures.get(1).unwrap().as_str().to_string();
		let change: i32 = captures.get(2).unwrap().as_str().parse().unwrap();

		return Some((format!("{}{}{}", start, change + 1, end), icon));
	}

	throw(21, "Unknown version size");
}

fn get_new_version(sizes: Vec<String>) -> Option<(String, String, String)> {
	let dragon = "dragon".to_string();

	if sizes.contains(&dragon) {
		return Some((r"()(\d+)\.\d+\.\d+\.\d+".to_string(), r".0.0.0".to_string(), dragon));
	}

	let whale = "whale".to_string();
	if sizes.contains(&whale) {
		return Some((r"(\d+\.)(\d+)\.\d+\.\d+".to_string(), r".0.0".to_string(), whale));
	}

	let sheep = "sheep".to_string();
	if sizes.contains(&sheep) {
		return Some((r"(\d+\.\d+\.)(\d+)\.\d+".to_string(), r".0".to_string(), sheep));
	}

	let ant = "ant".to_string();
	if sizes.contains(&ant) {
		return Some((r"(\d+\.\d+\.\d+\.)(\d+)".to_string(), r"".to_string(), ant));
	}

	return None;
}

fn write_release(next: String, new_tasks: Vec<String>, icon: String) {
	let mut new_version: Vec<String> = Vec::new();

	let count = new_tasks.len();

	new_version.push(format!(
		"## <a name=\"{}\"></a>{} :{}: <sup>`{}`</sup>",
		next, next, icon, count
	));

	for t in (0..count).rev() {
		let task = new_tasks.get(t).unwrap();
		new_version.push(task.to_string());
	}

	new_version.push("".to_string());

	let mut release_list = get_lines(path_release());
	release_list.splice(16..16, new_version);

	set_lines(path_release(), release_list);
}
