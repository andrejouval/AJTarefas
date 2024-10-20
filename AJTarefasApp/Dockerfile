# STAGE 1 - Build .NET Framework libraries on Windows
# Nomeie o estágio como 'build-api'
FROM mcr.microsoft.com/dotnet/framework/sdk:4.8-windowsservercore-ltsc2019 AS build
FROM mcr.microsoft.com/dotnet/sdk:8.0 
WORKDIR /src

# Copiar o projeto WebAPI
COPY ["./AJTarefasNegocio/AJTarefasNegocio.csproj", "."]
COPY ["./AJTarefasDomain/AJTarefasDomain.csproj", "."]
COPY ["./AJTarefasRecursos/AJTarefasRecursos.csproj", "."]
COPY ["./AJTarefasApp/AJTarefasApp.csproj", "."]

# Restaurar pacotes do projeto 
RUN dotnet restore "./AJTarefasNegocio/AJTarefasNegocio.csproj"
RUN dotnet restore "./AJTarefasDomain/AJTarefasDomain.csproj"
RUN dotnet restore "./AJTarefasRecursos/AJTarefasRecursos.csproj"
RUN dotnet restore "./AJTarefasApp.csproj"

# Copiar o restante do código da aplicação
COPY . .

# Construir o projeto WebAPI
RUN dotnet build "./AJTarefasDomain/AJTarefasDomain.csproj" /p:Configuration=Release
RUN dotnet build "./AJTarefasNegocio/AJTarefasNegocio.csproj" /p:Configuration=Release
RUN dotnet build "./AJTarefasRecursos/AJTarefasRecursos.csproj" /p:Configuration=Release
RUN dotnet build "./AJTarefasApp.csproj" -c Release -o /app/build

#STAGE 2 - Publish
# Publicar o projeto WebAPI
RUN dotnet publish "./AJTarefasApp.csproj" -c Release -o /app/publish

# STAGE 3 - Configurar o ambiente de execução no Linux para a API
# Nomeie o estágio como 'runtime'
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime
WORKDIR /app

# Copiar as bibliotecas compiladas do .NET Framework
COPY --from=build /src/AJTarefasNegocio/bin/Release /app/AJTarefasNegocio
COPY --from=build /src/AJTarefasDomain/bin/Release /app/AJTarefasDomain
COPY --from=build /src/AJTarefasRecursos/bin/Release /app/AJTarefasRecursos

# Copiar a API publicada
COPY --from=build /app/publish .

# Configurar o ponto de entrada da API
ENTRYPOINT ["dotnet", "AJTarefasApp.dll"]
