use std::process::exit;

use crate::git::stash_pop;

pub fn success() -> ! {
	stop(0)
}

pub fn throw(code: i32, text: &str) -> ! {
	eprintln!("{}", text);
	stop(code)
}

pub fn throw_format(code: i32, text: String) -> ! {
	throw(code, &text)
}

pub fn throw_multiple(code: i32, texts: Vec<&str>) -> ! {
	for text in texts {
		eprintln!("{}", text);
	}
	stop(code)
}

fn stop(code: i32) -> ! {
	stash_pop();
	exit(code)
}
