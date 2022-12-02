fn main() {
	match std::env::var("part") {
		Ok(part) if part == "part2" => println!("{}", sum_games(|e, w| 1 + (e + w + 2) % 3 + w * 3)),
		_ => println!("{}", sum_games(|e, m| 1 + m + ((m - e + 4) % 3) * 3)),
	};
}

fn sum_games(calc: impl Fn(i64, i64) -> i64) -> i64 {
	let input = include_bytes!("../input.txt");
	let mut sum = 0;
	let mut i = 0;
	while i < input.len() {
		sum += calc((input[i] - b'A') as i64, (input[i + 2] - b'X') as i64);
		i += 4;
	}
	sum
}
