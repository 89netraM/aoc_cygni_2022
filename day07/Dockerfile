FROM rust:1.65 as builder
WORKDIR /aoc
RUN rustup override set nightly
COPY . .
RUN cargo install --path .

FROM debian:bullseye
WORKDIR /
COPY --from=builder /usr/local/cargo/bin/day07 .
ENTRYPOINT ./day07
