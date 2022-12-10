FROM rust:1.65 as builder
WORKDIR /aoc
COPY . .
RUN cargo install --path .

FROM debian:bullseye
WORKDIR /
COPY --from=builder /usr/local/cargo/bin/day08 .
ENTRYPOINT ./day08
