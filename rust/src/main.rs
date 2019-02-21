use std::io;
use std::cmp::Ordering;
use rand::Rng;

fn main() {
	let number =
		rand::thread_rng()
			.gen_range(1, 101);
			
	println!("{}", five());
	println!("{}", plus_one(2));
	
	loop {
		let mut guess = String::new();

		println!("try:	");
		
		println!("{}", guess);

		io::stdin().read_line(&mut guess)
			.expect("read line");

		let guess: u32 = match guess.trim().parse() {
			Ok(num) => num,
			Err(_) => continue,
		};

		match guess.cmp(&number) {
			Ordering::Less =>
				println!("{} is less than {}", guess, number),

			Ordering::Greater =>
				println!("{} is greater than {}", guess, number),

			Ordering::Equal => {
				println!("{} is equal to {}", guess, number);
				break;
			},
		}
	}
	
	let a = ["M", "E", "A", "K"];

    for element in a.iter() {
        println!("the value is: {}", element);
    }
	
	
}

fn five() -> i32 {
	5
}

fn plus_one(x: i32) -> i32 {
    x + 1
}
