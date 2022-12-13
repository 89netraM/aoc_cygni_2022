FROM mcr.microsoft.com/dotnet/sdk:7.0-alpine
WORKDIR /aoc
COPY ./Day13.csproj .
RUN dotnet restore
COPY ./Program.cs .
RUN dotnet publish --no-restore -c Release -o .
COPY ./input.txt .
ENTRYPOINT dotnet ./Day13.dll
