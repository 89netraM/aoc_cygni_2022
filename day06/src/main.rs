fn main() {
	match std::env::var("part") {
		Ok(part) if part == "part2" => println!("{}", find_first(14)),
		_ => println!("{}", find_first(4)),
	};
}

fn find_first(length: usize) -> usize {
	const INPUT: &'static [u8] = include_bytes!("../input.txt");
	let mut i = length;
	while i < INPUT.len() {
		let Some(j) = unique(&INPUT[i - length..i]) else { return i };
		i += j + 1;
	}
	unreachable!()
}

#[inline(always)]
fn unique(section: &[u8]) -> Option<usize> {
	for i in 0..section.len() {
		for j in i + 1..section.len() {
			if section[i] == section[j] {
				return Some(i);
			}
		}
	}
	None
}
