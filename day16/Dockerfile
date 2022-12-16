FROM mcr.microsoft.com/dotnet/sdk:7.0-alpine
WORKDIR /aoc
COPY ./Day16.csproj .
RUN dotnet restore
COPY ./src .
RUN dotnet publish --no-restore -c Release -o .
COPY ./input.txt .
ENTRYPOINT dotnet ./Day16.dll
