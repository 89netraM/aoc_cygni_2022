use std::collections::HashSet;

const INPUT: &str = include_str!("../input.txt");

fn main() {
	match std::env::var("part") {
		Ok(part) if part == "part2" => println!("{}", part2()),
		_ => println!("{}", part1()),
	};
}

fn parse_map() -> Vec<Vec<usize>> {
	INPUT.lines().map(|l| l.chars().map(|c| c as usize - '0' as usize).collect()).collect()
}

fn part1() -> usize {
	let map = parse_map();
	let mut seen = HashSet::new();
	count_visible(&mut seen, &map, 0..map[0].len(), |c| (0..map.len()).map(move |r| (r, c)));
	count_visible(&mut seen, &map, 0..map[0].len(), |c| (0..map.len()).rev().map(move |r| (r, c)));
	count_visible(&mut seen, &map, 0..map.len(), |r| (0..map[0].len()).map(move |c| (r, c)));
	count_visible(&mut seen, &map, 0..map.len(), |r| (0..map[0].len()).rev().map(move |c| (r, c)));
	seen.len()
}

fn count_visible<I: Iterator<Item = (usize, usize)>, F: Fn(usize) -> I>(seen: &mut HashSet<(usize, usize)>, map: &[Vec<usize>], across: impl Iterator<Item = usize>, into: F) {
	for a in across {
		let mut height = None;
		for (r, c) in into(a) {
			if height.is_none() || height.unwrap() < map[r][c] {
				height = Some(map[r][c]);
				seen.insert((r, c));
			}
		}
	}
}

fn part2() -> usize {
	let map = parse_map();
	let mut maximum = (0, (0, 0));

	for r in 0..map.len() {
		for c in 0..map[r].len() {
			let mut score = 1;
			let pos = (r, c);

			score *= calc_score(&map, pos, (0, -1));
			score *= calc_score(&map, pos, (1, 0));
			score *= calc_score(&map, pos, (-1, 0));
			score *= calc_score(&map, pos, (0, 1));

			if score > maximum.0 {
				maximum = (score, pos);
			}
		}
	}

	maximum.0
}

fn calc_score(map: &[Vec<usize>], mut pos: (usize, usize), dir: (isize, isize)) -> usize {
	let height = map[pos.0][pos.1];
	let mut i = 1;
	pos = add(pos, dir);
	loop {
		if pos.0 < map.len() && pos.1 < map[pos.0].len() {
			if map[pos.0][pos.1] >= height {
				return i;
			}
		} else {
			return i - 1;
		}
		pos = add(pos, dir);
		i += 1;
	}
}

fn add(a: (usize, usize), b: (isize, isize)) -> (usize, usize) {
	let r = a.0 as isize + b.0;
	let c = a.1 as isize + b.1;
	if r < 0 || c < 0 {
		(usize::MAX, usize::MAX)
	} else {
		(r as usize, c as usize)
	}
}
