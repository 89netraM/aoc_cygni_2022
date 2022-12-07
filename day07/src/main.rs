const INPUT: &'static [u8] = include_bytes!("../input.txt");

fn main() {
	match std::env::var("part") {
		Ok(part) if part == "part2" => println!("{}", unsafe { part2() }),
		_ => println!("{}", unsafe { part1() }),
	};
}

static mut STACK: [usize; 200] = [0; 200];
static mut STACK_LEN: usize = 0;

unsafe fn part1() -> usize {
	STACK_LEN = 0;
	let mut sum = 0;

	let mut i = 2;
	while i < INPUT.len() {
		if *INPUT.get_unchecked(i) == b'c' {
			i += 3;
			if *INPUT.get_unchecked(i) == b'.' {
				let size = *STACK.get_unchecked(STACK_LEN - 1);
				STACK_LEN -= 1;
				if size <= 100000 {
					sum += size;
				}
				*STACK.get_unchecked_mut(STACK_LEN - 1) += size;
				i += 5;
			} else {
				*STACK.get_unchecked_mut(STACK_LEN) = 0;
				STACK_LEN += 1;
				while *INPUT.get_unchecked(i) != b'\n' {
					i += 1;
				}
				i += 3;
			}
		} else {
			i += 3;
			while i < INPUT.len() && *INPUT.get_unchecked(i) != b'$' {
				if INPUT.get_unchecked(i).is_ascii_digit() {
					let mut size = 0;
					while *INPUT.get_unchecked(i) != b' ' {
						size = size * 10 + (INPUT.get_unchecked(i) - b'0') as usize;
						i += 1;
					}
					*STACK.get_unchecked_mut(STACK_LEN - 1) += size;
				}
				while *INPUT.get_unchecked(i) != b'\n' {
					i += 1;
				}
				i += 1;
			}
			i += 2;
		}
	}
	while STACK_LEN > 0 {
		let size = *STACK.get_unchecked(STACK_LEN - 1);
		if size > 100000 {
			break;
		}
		sum += size;
		STACK_LEN -= 1;
		if STACK_LEN > 0 {
			*STACK.get_unchecked_mut(STACK_LEN - 1) += size;
		}
	}

	sum
}

static mut SIZES: [usize; 200] = [0; 200];
static mut SIZES_LEN: usize = 0;

unsafe fn part2() -> usize {
	STACK_LEN = 0;
	SIZES_LEN = 0;

	let mut total_size = 0;

	let mut i = 2;
	while i < INPUT.len() {
		if *INPUT.get_unchecked(i) == b'c' {
			i += 3;
			if *INPUT.get_unchecked(i) == b'.' {
				let size = *STACK.get_unchecked(STACK_LEN - 1);
				STACK_LEN -= 1;
				total_size = size;
				*SIZES.get_unchecked_mut(SIZES_LEN) = size;
				SIZES_LEN += 1;
				*STACK.get_unchecked_mut(STACK_LEN - 1) += size;
				i += 5;
			} else {
				*STACK.get_unchecked_mut(STACK_LEN) = 0;
				STACK_LEN += 1;
				while *INPUT.get_unchecked(i) != b'\n' {
					i += 1;
				}
				i += 3;
			}
		} else {
			i += 3;
			while i < INPUT.len() && *INPUT.get_unchecked(i) != b'$' {
				if INPUT.get_unchecked(i).is_ascii_digit() {
					let mut size = 0;
					while *INPUT.get_unchecked(i) != b' ' {
						size = size * 10 + (INPUT.get_unchecked(i) - b'0') as usize;
						i += 1;
					}
					*STACK.get_unchecked_mut(STACK_LEN - 1) += size;
				}
				while *INPUT.get_unchecked(i) != b'\n' {
					i += 1;
				}
				i += 1;
			}
			i += 2;
		}
	}
	while STACK_LEN > 0 {
		let size = *STACK.get_unchecked(STACK_LEN - 1);
		total_size = size;
		*SIZES.get_unchecked_mut(SIZES_LEN) = size;
		SIZES_LEN += 1;
		STACK_LEN -= 1;
		if STACK_LEN > 0 {
			*STACK.get_unchecked_mut(STACK_LEN - 1) += size;
		}
	}

	let missing = 30000000 - (70000000 - total_size);
	let mut minimum = usize::MAX;
	for i in 0..SIZES_LEN {
		let size = *SIZES.get_unchecked(i);
		if size >= missing && size < minimum {
			minimum = size;
		}
	}
	minimum
}
