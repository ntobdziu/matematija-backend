# Build faza
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build-env
WORKDIR /app

# Kopiraj i restore-uj pakete
COPY *.csproj ./
RUN dotnet restore

# Kopiraj sve ostalo i napravi build
COPY . ./
RUN dotnet publish -c Release -o out

# Finalna faza (Runtime)
FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app
COPY --from=build-env /app/out .

# Zameni "MatematijaAPI.dll" tačnim nazivom tvog izlaznog fajla ako je drugačiji
ENTRYPOINT ["dotnet", "MatematijaAPI.dll"]