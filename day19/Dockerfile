FROM mcr.microsoft.com/dotnet/sdk:7.0-nanoserver-1809
COPY ./Day19.csproj .
RUN dotnet restore
COPY ./Program.cs .
RUN dotnet publish --no-restore -c Release -o .
COPY ./input.txt .
ENTRYPOINT ["dotnet" ".\\Day19.dll"]
