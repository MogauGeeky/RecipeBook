FROM microsoft/dotnet:2.1-aspnetcore-runtime-nanoserver-1709 AS base
WORKDIR /app
EXPOSE 2558
EXPOSE 44309

FROM microsoft/dotnet:2.1-sdk-nanoserver-1709 AS build
WORKDIR /src
COPY src/RecipeBook.API/RecipeBook.API.csproj src/RecipeBook.API/
RUN dotnet restore src/RecipeBook.API/RecipeBook.API.csproj
COPY . .
WORKDIR /src/src/RecipeBook.API
RUN dotnet build RecipeBook.API.csproj -c Release -o /app

FROM build AS publish
RUN dotnet publish RecipeBook.API.csproj -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "RecipeBook.API.dll"]
