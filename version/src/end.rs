use std::process::exit;

pub fn success<T>() -> Option<T> {
	exit(0);
}

pub fn throw<T>(code: i32, text: &str) -> Option<T> {
	eprintln!("{}", text);
	exit(code);
}

pub fn throw_format<T>(code: i32, text: String) -> Option<T> {
	eprintln!("{}", text);
	exit(code);
}

pub fn throw_multiple<T>(code: i32, texts: Vec<&str>) -> Option<T> {
	for text in texts {
		eprintln!("{}", text);
	}
	exit(code);
}
