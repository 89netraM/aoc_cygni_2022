FROM mcr.microsoft.com/dotnet/sdk:7.0-alpine as builder
WORKDIR /aoc
RUN apk add clang build-base zlib-dev
COPY ./Day24.csproj .
RUN dotnet restore
COPY ./input.txt .
COPY ./src .
RUN dotnet publish --no-restore -c Release -r linux-musl-x64 -o .

FROM alpine:latest
WORKDIR /
RUN apk add libstdc++
COPY --from=builder /aoc/Day24 .
ENTRYPOINT ./Day24
