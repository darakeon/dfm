use regex::Regex;

use crate::file::{get_lines,set_lines};
use crate::version::Version;

static PATH_TODO: &str = r"..\docs\TODO.md";
static PATH_RELEASE: &str = r"..\docs\RELEASES.md";

pub fn add_release(mut version: Version, numbers: Vec<usize>) -> Option<Version> {
	let processed = process_tasks(numbers);
	if processed.is_none() {
		return None;
	}
	let (new_tasks, sizes) = processed.unwrap();

	let try_next = get_next(sizes, version.code.clone());
	if try_next.is_none() {
		return None;
	}
	let next = try_next.unwrap();

	let written = write_release(next.clone(), new_tasks);

	if !written {
		return None;
	}

	version.next = next;
	return Some(version);
}

fn process_tasks(mut numbers: Vec<usize>) -> Option<(Vec<String>, Vec<String>)> {
	let mut count: usize = 0;

	let mut new_tasks: Vec<String> = Vec::new();
	let mut sizes: Vec<String> = Vec::new();

	let mut todo_list = get_lines(PATH_TODO);
	let mut l = 15;

	let old_size = todo_list.len();

	while numbers.len() > 0 && l <= todo_list.len() {
		let line = todo_list.get(l).unwrap();
		l += 1;

		if let Some((todo, size, issue)) = extract_task(&line) {
			count += 1;

			if let Some(n) = numbers.iter().position(|&x| x == count) {
				let task = format!("- [ ] {}{}", todo.trim(), issue);
				new_tasks.push(task);

				sizes.push(size);

				numbers.remove(n);

				l -= 1;
				todo_list.remove(l);
			}
		}
	}

	let new_size = todo_list.len();

	if let Some(title) = todo_list.get_mut(13) {
		let old_count = extract_count(title);
		let new_count = old_count - (old_size - new_size);

		*title = (
			*title.replace(
				&old_count.to_string(),
				&new_count.to_string()
			)
		).to_string();
	}

	let error_on_write = set_lines(PATH_TODO, todo_list).is_err();

	if error_on_write {
		eprintln!("Error while writing TODO.md");
		return None;
	}

	return Some((new_tasks, sizes));
}

fn extract_task(text: &str) -> Option<(String, String, String)> {
	let pattern =
		r#"^\| ([^|]+) +\| :(ant|sheep|whale|dragon): +\|  \d  \|  \d  \|  \d  \| .+ \|(?: +|( \[\#\d+\])\(.+\)) \|$"#;

	let regex = Regex::new(pattern).unwrap();

	if !regex.is_match(text) {
		return None;
	}

	let captures = regex.captures(text).unwrap();

	let task = captures.get(1).unwrap().as_str().to_string();
	let size = captures.get(2).unwrap().as_str().to_string();
	let mut issue: String = "".to_string();

	if let Some(i) = captures.get(3) {
		issue = i.as_str().to_string();
	}

	Some((task, size, issue))
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

fn get_next(sizes: Vec<String>, current: String) -> Option<String> {
	let new_version = get_new_version(sizes);
	if new_version.is_none() {
		eprintln!("Unknown size");
		return None;
	}

	let (size_pattern, end) = new_version.unwrap();

	let regex = Regex::new(&size_pattern).unwrap();
	let captures = regex.captures(&current).unwrap();

	let start = captures.get(1).unwrap().as_str().to_string();
	let change: i32 = captures.get(2).unwrap().as_str().parse().unwrap();

	return Some(format!("{}{}{}", start, change + 1, end));
}

fn get_new_version(sizes: Vec<String>) -> Option<(String, String)> {
	if sizes.contains(&"dragon".to_string()) {
		return Some((r"()(\d+)\.\d+\.\d+\.\d+".to_string(), r".0.0.0".to_string()));
	} else if sizes.contains(&"whale".to_string()) {
		return Some((r"(\d+\.)(\d+)\.\d+\.\d+".to_string(), r".0.0".to_string()));
	} else if sizes.contains(&"sheep".to_string()) {
		return Some((r"(\d+\.\d+\.)(\d+)\.\d+".to_string(), r".0".to_string()));
	} else if sizes.contains(&"ant".to_string()) {
		return Some((r"(\d+\.\d+\.\d+\.)(\d+)".to_string(), r"".to_string()));
	} else {
		return None;
	}
}

fn write_release(next: String, new_tasks: Vec<String>) -> bool {
	let mut new_version: Vec<String> = Vec::new();

	new_version.push(format!(
		"## <a name=\"{}\"></a>{} :ant: <sup>`{}`</sup>",
		next, next, new_tasks.len()
	));

	new_version.splice(1..1, new_tasks);

	new_version.push("".to_string());

	let mut release_list = get_lines(PATH_RELEASE);
	release_list.splice(16..16, new_version);

	let written = !set_lines(PATH_RELEASE, release_list).is_err();
	if !written {
		eprintln!("Error while writing RELEASES.md");
	}

	return written;
}
