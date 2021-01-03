use std::process::exit;

use crate::git::stash_pop;

pub fn success<T>() -> Option<T> {
	stop(0)
}

pub fn throw<T>(code: i32, text: &str) -> Option<T> {
	eprintln!("{}", text);
	stop(code)
}

pub fn throw_format<T>(code: i32, text: String) -> Option<T> {
	throw(code, &text)
}

pub fn throw_multiple<T>(code: i32, texts: Vec<&str>) -> Option<T> {
	for text in texts {
		eprintln!("{}", text);
	}
	stop(code)
}

fn stop<T>(code: i32) -> Option<T> {
	stash_pop();
	exit(code)
}
