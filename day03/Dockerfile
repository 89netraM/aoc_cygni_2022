FROM debian:bullseye as builder
WORKDIR /
RUN apt update && apt upgrade -y && apt install curl rsync libncurses5 clang -y
RUN curl -o emojicode.tar.gz -L https://github.com/emojicode/emojicode/releases/download/v1.0-beta.2/Emojicode-1.0-beta.2-Linux-x86_64.tar.gz \
	&& tar -xzf emojicode.tar.gz && rm emojicode.tar.gz \
	&& cd Emojicode-1.0-beta.2-Linux-x86_64 && yes | ./install.sh \
	&& cd .. && rm -r Emojicode-1.0-beta.2-Linux-x86_64
COPY ./day03.emojic .
RUN emojicodec ./day03.emojic

FROM debian:bullseye
WORKDIR /
COPY ./input.txt .
COPY --from=builder /day03 .
ENTRYPOINT ./day03
