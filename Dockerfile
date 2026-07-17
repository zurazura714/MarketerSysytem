FROM mcr.microsoft.com/dotnet/sdk:10.0
WORKDIR /src
COPY . .
RUN dotnet publish MarketerSysytem.Web -c Release -o /app
WORKDIR /app
ENTRYPOINT ["dotnet", "MarketerSysytem.Web.dll"]