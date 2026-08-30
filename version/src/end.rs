use std::process::exit;

use crate::git::{stash_pop, reset_all};

pub fn success() -> ! {
	stop(0)
}

pub fn throw(code: i32, text: &str) -> ! {
	eprintln!("{}", text);
	stop(code)
}

pub fn throw_multiple(code: i32, texts: Vec<&str>) -> ! {
	for text in texts {
		eprintln!("{}", text);
	}
	stop(code)
}

fn stop(code: i32) -> ! {
	reset_all();
	stash_pop();
	exit(code)
}
