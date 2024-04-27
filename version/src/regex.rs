use regex::Regex;

pub fn extract_line(lines: &Vec<String>, position: usize, pattern: &str) -> String {
	let line = lines.get(position).unwrap();
	return extract(line, pattern).unwrap();
}

pub fn extract(text: &str, pattern: &str) -> Option<String> {
	let regex = Regex::new(pattern).unwrap();

	if regex.is_match(text) {
		let captures = regex.captures(text).unwrap();
		Some(captures.get(1).unwrap().as_str().to_string())
	} else {
		None
	}
}

pub fn replace(text: &str, pattern: &str, replacer: &str) -> Option<String> {
	let regex = Regex::new(pattern).unwrap();

	if regex.is_match(text) {
		Some(regex.replace(text, replacer).into_owned())
	} else {
		None
	}
}

pub fn is_match(text: &str, pattern: &str) -> bool {
	let regex = Regex::new(pattern).unwrap();
	return regex.is_match(text)
}