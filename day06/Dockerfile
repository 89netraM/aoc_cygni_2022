FROM rust:1.64 as builder
WORKDIR /aoc
RUN rustup override set nightly
COPY . .
RUN cargo install --path .

FROM debian:bullseye
WORKDIR /
COPY --from=builder /usr/local/cargo/bin/day06 .
ENTRYPOINT ./day06
