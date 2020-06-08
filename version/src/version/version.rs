use std::collections::LinkedList;

pub struct Version {
	pub code: String,
	pub done: bool,
	pub prev: String,
	pub next: String,
	pub tasks: LinkedList<String>,
}

impl Version {
	pub fn empty() -> Self {
		Version {
			code: "".to_string(),
			done: false,
			prev: "".to_string(),
			next: "".to_string(),
			tasks: LinkedList::new(),
		}
	}
	pub fn new(code: String, next: String) -> Self {
		Version {
			code,
			done: true,
			prev: String::new(),
			next,
			tasks: LinkedList::new(),
		}
	}
}

impl ToString for Version {
	fn to_string(&self) -> String {
		format!(
			"{} [{}] .. > {} .. < {}",
			self.code, self.done, self.prev, self.next
		)
	}
}
