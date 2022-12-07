FROM haskell:9 as builder
WORKDIR /day07
RUN cabal update
COPY ./day07.cabal .
RUN cabal new-build --only-dependencies
COPY . .
RUN cabal new-install --installdir .

FROM debian:bullseye
WORKDIR /
COPY ./input.txt .
COPY --from=builder /day07/day07 .
ENTRYPOINT ./day07
