# Etapa 1: Build da aplicação
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /source

# Copiar o arquivo de solução e todos os projetos
COPY . .

# Restaurar as dependências (usando todos os .csproj)
RUN dotnet restore "AJTarefasApp/AJTarefasApp.csproj"

# Build da aplicação (modo Release)
RUN dotnet build "AJTarefasApp/AJTarefasApp.csproj" -c Release -o /app/build

# Publicar a aplicação (Release)
RUN dotnet publish "AJTarefasApp/AJTarefasApp.csproj" -c Release -o /app/publish

# Etapa 2: Configurar a imagem de runtime
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime
WORKDIR /app
COPY --from=build /app/publish .

# Expor a porta padrão da Web API
EXPOSE 80

# Definir o ponto de entrada da aplicação
ENTRYPOINT ["dotnet", "AJTarefasApp.dll"]