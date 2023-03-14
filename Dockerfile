FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 8079
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["Lenceas.Core.Api/Lenceas.Core.Api.csproj", "Lenceas.Core.Api/"]
COPY ["Lenceas.Core.Common/Lenceas.Core.Common.csproj", "Lenceas.Core.Common/"]
COPY ["Lenceas.Core.Extensions/Lenceas.Core.Extensions.csproj", "Lenceas.Core.Extensions/"]
COPY ["Lenceas.Core.Services/Lenceas.Core.Services.csproj", "Lenceas.Core.Services/"]
COPY ["Lenceas.Core.IServices/Lenceas.Core.IServices.csproj", "Lenceas.Core.IServices/"]
COPY ["Lenceas.Core.Model/Lenceas.Core.Model.csproj", "Lenceas.Core.Model/"]
COPY ["Lenceas.Core.Repository/Lenceas.Core.Repository.csproj", "Lenceas.Core.Repository/"]
RUN dotnet restore "Lenceas.Core.Api/Lenceas.Core.Api.csproj"
COPY . .
WORKDIR "/src/Lenceas.Core.Api"
RUN dotnet build "Lenceas.Core.Api.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Lenceas.Core.Api.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Lenceas.Core.Api.dll"]