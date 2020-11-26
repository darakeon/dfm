use std::env;

pub fn parse_arguments() -> Option<Vec<usize>> {
    let args: Vec<String> = env::args().collect();

	if args.len() < 3 {
		return stop_program();
	}

	let cmd = &args[1];

    match &cmd[..] {
		"-q" => { return parse_quantity(args); },
		"-n" => { return parse_numbers(args); },
		_ => { return stop_program(); }
	}
}

fn stop_program() -> Option<Vec<usize>> {
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
	eprintln!("    |                             use just numbers at parameters, not the braces  |");
	eprintln!("    |                                                                             |");
	eprintln!("    |-----------------------------------------------------------------------------|");
	eprintln!("");

	return None;
}

fn parse_quantity(args: Vec<String>) -> Option<Vec<usize>> {
	let quantity = args[2].parse::<usize>();

	match quantity {
		Ok(q) => {
			let mut numbers: Vec<usize> = Vec::new();

			for number in 1..=q {
				numbers.push(number);
			}

			return Some(numbers);
		}
		Err(_) => {
			eprintln!("-q must have one argument and it must be a number");
			return None;
		}
	}
}

fn parse_numbers(args: Vec<String>) -> Option<Vec<usize>> {
	if args.len() < 2 {
		eprintln!("-n must have at least one argument and they must be all numbers");
		return None;
	}

	let mut numbers: Vec<usize> = Vec::new();

	for position in 2..args.len() {
		let number = args[position].parse::<usize>();

		match number {
			Ok(n) => {
				numbers.push(n);
			}
			Err(_) => {
				eprintln!("At least of of the arguments of -n is not a number");
				return None;
			}
		}
	}

	return Some(numbers);
}
