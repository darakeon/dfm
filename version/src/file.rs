use std::fs;
use std::io::Error;

pub fn get_lines(path: &str) -> Vec<String> {
	fs::read_to_string(path)
		.unwrap()
		.split("\n")
		.map(|s| s.to_string())
		.collect()
}

pub fn set_lines(path: &str, lines: Vec<String>) -> Result<(), Error> {
	fs::write(path, lines.join("\n"))
}
