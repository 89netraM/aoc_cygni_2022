FROM rust:1.64 as builder
WORKDIR /aoc
COPY . .
RUN cargo install --path .

FROM debian:bullseye
WORKDIR /
COPY --from=builder /usr/local/cargo/bin/day02 .
ENTRYPOINT ./day02
