pub struct Version {
	pub code: String,
	pub done: bool,
	pub prev: String,
	pub next: String,
}

impl Version {
	pub fn empty() -> Self {
		Version {
			code: "".to_string(),
			done: false,
			prev: "".to_string(),
			next: "".to_string(),
		}
	}
	pub fn new(code: String, next: String) -> Self {
		Version {
			code,
			done: true,
			prev: String::new(),
			next,
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
