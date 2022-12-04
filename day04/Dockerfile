FROM debian:bullseye
WORKDIR /
RUN apt update && apt upgrade -y && apt install curl clang -y
RUN curl -sSL https://get.haskellstack.org/ | sh
RUN git clone https://github.com/89netraM/UwUpp.git \
	&& cd UwUpp && stack install \
	&& cd .. && rm -r UwUpp
COPY ./input.txt .
COPY ./day04.uwupp .
CMD /root/.local/bin/UwUpp-exe ./day04.uwupp
