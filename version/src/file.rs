use std::env;
use std::fs;

pub fn get_lines(path: String) -> Vec<String> {
	fs::read_to_string(path)
		.unwrap()
		.split("\n")
		.map(|s| s.to_string())
		.collect()
}

pub fn set_lines(path: String, lines: Vec<String>) {
	fs::write(path, lines.join("\n")).unwrap()
}

pub fn get_content(path: String) -> String {
	fs::read_to_string(path).unwrap()
}

pub fn set_content(path: String, content: String) {
	fs::write(path, content).unwrap()
}

pub fn get_path(paths: Vec<&str>) -> String {
	let mut base = env::current_dir().unwrap();

	for path in paths {
		base = base.join(path);
	}

	return base.to_str().unwrap().to_string();
}
