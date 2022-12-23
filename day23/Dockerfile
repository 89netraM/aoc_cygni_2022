FROM mcr.microsoft.com/dotnet/sdk:7.0-alpine as builder
WORKDIR /aoc
RUN apk add clang build-base zlib-dev
COPY ./Day23.csproj .
RUN dotnet restore
COPY ./input.txt .
COPY ./Program.cs .
RUN dotnet publish --no-restore -c Release -r linux-musl-x64 -o .

FROM alpine:latest
WORKDIR /
RUN apk add libstdc++
COPY --from=builder /aoc/Day23 .
ENTRYPOINT ./Day23
