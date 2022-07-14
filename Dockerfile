#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["IdentityWebApplication.csproj", "."]
COPY ["ssl-iam-identity-docker.pfx","."]
COPY ["HMIRootCA.pem","."]
COPY ["./ssl-iam-identity-docker.cer","/usr/local/share/ca-certificates/"]
COPY ["./ssl-iam-identity-docker.pfx","/usr/local/share/ca-certificates/"]
COPY ["./HMIRootCA.pem","/usr/local/share/ca-certificates/"]
RUN dotnet restore "./IdentityWebApplication.csproj"
COPY . .
WORKDIR "/src/."
RUN dotnet build "IdentityWebApplication.csproj" -c Release -o /app/build
RUN update-ca-certificates

FROM build AS publish
RUN dotnet publish "IdentityWebApplication.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "IdentityWebApplication.dll"]