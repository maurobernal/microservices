#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

#Imagen de Producion
#############################################################
FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base  
WORKDIR /app
EXPOSE 80
EXPOSE 443









#Imagen Build
###########################################################
FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build  
WORKDIR /src
COPY ["api.csproj", "."]
RUN dotnet restore "./api.csproj"
COPY . .
WORKDIR "/src/."
RUN dotnet build "api.csproj" -c Release -o /app/build

#Imagen Publish
############################################################
FROM build AS publish
RUN dotnet publish "api.csproj" -c Release -o /app/publish

#Mover Publish a Final
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "api.dll"]