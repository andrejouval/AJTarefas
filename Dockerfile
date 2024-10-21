# Etapa 1: Build da aplica��o
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /source

# Copiar o arquivo de solu��o e todos os projetos
COPY . .

# Restaurar as depend�ncias (usando todos os .csproj)
RUN dotnet restore "AJTarefasApp/AJTarefasApp.csproj"

# Build da aplica��o (modo Release)
RUN dotnet build "AJTarefasApp/AJTarefasApp.csproj" -c Release -o /app/build

# Publicar a aplica��o (Release)
RUN dotnet publish "AJTarefasApp/AJTarefasApp.csproj" -c Release -o /app/publish

# Etapa 2: Configurar a imagem de runtime
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime
WORKDIR /app
COPY --from=build /app/publish .

# Expor a porta padr�o da Web API
EXPOSE 80

# Definir o ponto de entrada da aplica��o
ENTRYPOINT ["dotnet", "AJTarefasApp.dll"]