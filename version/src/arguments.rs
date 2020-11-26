use std::env;

pub fn parse_arguments() -> Option<(bool, Vec<usize>)> {
    let args: Vec<String> = env::args().collect();

	if args.len() < 2 {
		return stop_program();
	}

	let cmd = &args[1];

    match &cmd[..] {
		"-q" => { return parse_quantity(args); },
		"-n" => { return parse_numbers(args); },
		"-c" => { return check(); },
		_ => { return stop_program(); }
	}
}

fn stop_program() -> Option<(bool, Vec<usize>)> {
	eprintln!("");
	eprintln!("    |-----------------------------------------------------------------------------|");
	eprintln!("    |                                                                             |");
	eprintln!("    |  Need parameters to get tasks from TODO and create new version at RELEASES  |");
	eprintln!("    |                                                                             |");
	eprintln!("    |    -q {{N}}                                                                   |");
	eprintln!("    |    >>> get first N tasks;                                                   |");
	eprintln!("    |                                                                             |");
	eprintln!("    |    -n {{p1}} {{p2}} {{p3}} ...                                                    |");
	eprintln!("    |    >>> get tasks by position;                                               |");
	eprintln!("    |                                                                             |");
	eprintln!("    |    -c                                                                       |");
	eprintln!("    |    >>> just check if the version is right.                                  |");
	eprintln!("    |                                                                             |");
	eprintln!("    |                             use just numbers at parameters, not the braces  |");
	eprintln!("    |                                                                             |");
	eprintln!("    |-----------------------------------------------------------------------------|");
	eprintln!("");

	return None;
}

fn parse_quantity(args: Vec<String>) -> Option<(bool, Vec<usize>)> {
	if args.len() != 3 {
		return quantity_error();
	}

	let quantity = args[2].parse::<usize>();

	match quantity {
		Ok(0) | Err(_) => {
			return quantity_error();
		}
		Ok(q) => {
			let mut numbers: Vec<usize> = Vec::new();

			for number in 1..=q {
				numbers.push(number);
			}

			return Some((false, numbers));
		}
	}
}

fn quantity_error() -> Option<(bool, Vec<usize>)> {
	eprintln!("-q must have one argument and it must be a number greater than zero");
	return None;
}

fn parse_numbers(args: Vec<String>) -> Option<(bool, Vec<usize>)> {
	if args.len() < 3 {
		return numbers_error();
	}

	let mut numbers: Vec<usize> = Vec::new();

	for position in 2..args.len() {
		let number = args[position].parse::<usize>();

		match number {
			Ok(0) | Err(_) => {
				return numbers_error();
			}
			Ok(n) => {
				numbers.push(n);
			}
		}
	}

	return Some((false, numbers));
}

fn numbers_error() -> Option<(bool, Vec<usize>)> {
	eprintln!("-n must have at least one argument and they all must be numbers greater than zero");
	return None;
}

fn check() -> Option<(bool, Vec<usize>)> {
	Some((true, Vec::new()))
}
