# AniList API Integration in C#

## (EN-US) English Documentation

### Project Overview

This project implements a REST API that integrates with the AniList GraphQL API, providing a seamless bridge between external systems and anime/manga data. The application is built using .NET 9.0 with a clean architecture pattern, ensuring maintainability and scalability.

#### Key Features
- **GraphQL to REST Integration**: Converts GraphQL queries to the AniList API into REST endpoints
- **Database Persistence**: Stores anime and character data using SQL Server with Entity Framework Core
- **Clean Architecture**: Implements a 3-layer architecture (API, Core, Infrastructure)
- **Containerized Database**: Uses Docker for SQL Server deployment

#### Third-Party APIs
This project integrates with the [AniList GraphQL API](https://graphql.anilist.co) to fetch anime and character information. For detailed API documentation and available queries, visit the official [AniList API Documentation](https://docs.anilist.co/guide/graphql).

## Prerequisites

Before running this project, ensure you have the following installed:
- **.NET 8.0 SDK**
- **Docker** (for SQL Server container)
- **Git**

## Quick Start

Clone the repository and navigate to the project directory:
```bash
git clone https://github.com/luc-llb/api-integration.git
cd api-integration/
```

## Setup Instructions

### 1. Environment Configuration

First, create your environment configuration file by copying the example:

```bash
cp exemple.env .env
```

Edit the `.env` file to match your preferences. The default configuration should work for most setups.

### 2. Install SQL Command Line Tools (Linux only)

If you're on Linux and don't have `sqlcmd` installed, use the provided script:

```bash
cd db/
chmod +x install_db.sh
./install_db.sh
```

**Note:** After installing sqlcmd via the script, you'll need to open a new terminal session.

### 3. Start SQL Server Container

Launch the SQL Server container using Docker:

```bash
docker run -e "ACCEPT_EULA=Y" -e "MSSQL_SA_PASSWORD=Password123!" -p 1433:1433 -d --name sqlserver mcr.microsoft.com/mssql/server:2022-latest
```

> **IMPORTANT:** Replace "Password123!" with the value of the `DB_SA_PASSWORD` variable from your `.env` file.

### 4. Create Database and User

Execute the script to create the database and user:

```bash
cd db/
sleep 60 && ./create_database.sh
```

> **Note:** The 60-second wait ensures SQL Server is fully initialized. If the script fails, wait an additional 2 minutes before retrying.

This script will:
- Connect to SQL Server using the SA user
- Create the database specified in the `DB_NAME` variable from your `.env` file
- Create a login with the username specified in `DB_USER` and password in `DB_PASSWORD`
- Grant appropriate permissions to the user

### 5. Install Entity Framework Tools

Install the `dotnet-ef` tool if you don't have it:

```bash
dotnet tool install --global dotnet-ef
```

### 6. Run Entity Framework Migrations

Navigate back to the project root and apply database migrations:

```bash
cd ..
dotnet ef database update --project IntegrationApi/src/IntegrationApi.Infrastructure --startup-project IntegrationApi/src/IntegrationApi.Api --context AppDbContext
```

### 7. Start the API

Run the API application:

```bash
dotnet run --project IntegrationApi/src/IntegrationApi.Api
```

The API will be available at `https://localhost:7008` (HTTPS) or `http://localhost:5084` (HTTP).

## Database Management Commands

### Reset Database (Development Only)

To completely reset the database (drop and recreate):

```bash
cd db/
./reset_database.sh
```

**Warning:** This will permanently delete all data in the database.

## Project Architecture

This project follows a clean architecture pattern with 3 distinct layers:

### API Layer (`IntegrationApi.Api`)
The presentation layer responsible for:
- Handling HTTP requests and responses
- API controllers and routing
- Authentication and authorization
- Input validation and data transfer objects (DTOs)
- Global exception handling middleware

### Core Layer (`IntegrationApi.Core`)
The business logic layer that contains:
- Domain entities and business rules
- Service interfaces and contracts
- Custom exceptions and validation logic
- Application services implementation
- Domain models and value objects

### Infrastructure Layer (`IntegrationApi.Infrastructure`)
The data access and external integration layer responsible for:
- Database context and Entity Framework configuration
- Repository implementations
- External API integrations (AniList GraphQL API)
- Data persistence and migration management
- Connection string management and database factory

This architecture ensures:
- **Separation of Concerns**: Each layer has a specific responsibility
- **Dependency Inversion**: Higher layers depend on abstractions, not implementations
- **Testability**: Business logic is isolated and can be unit tested
- **Maintainability**: Changes in one layer don't affect others
- **Scalability**: Easy to extend and modify individual components

## API Endpoints

The API provides the following endpoints:

### Animations
- `GET /api/animations` - Retrieve all animations from database
- `GET /api/animations/{id}` - Retrieve a specific animation by ID from database
- `GET /api/animations/search?searchTerm={term}` - Search animations from AniList API
- `POST /api/animations/search-and-save?searchTerm={term}` - Search animation from AniList API and save the first result to database

### Characters
- `GET /api/characters` - Retrieve all characters from database
- `GET /api/characters/{id}` - Retrieve a specific character by ID from database
- `GET /api/characters/search?searchTerm={term}` - Search characters from AniList API
- `POST /api/characters/search-and-save?searchTerm={term}` - Search character from AniList API and save the first result to database

## Development Guidelines

### Adding New Migrations
When you modify entities or add new ones, create a new migration:

```bash
dotnet ef migrations add MigrationName --project IntegrationApi/src/IntegrationApi.Infrastructure --startup-project IntegrationApi/src/IntegrationApi.Api --context AppDbContext
```

### Code Style
- Follow C# naming conventions
- Use async/await for asynchronous operations
- Implement proper error handling and logging
- Write unit tests for business logic

## Troubleshooting

### Common Issues

**SQL Server Connection Failed**
- Ensure Docker container is running: `docker ps`
- Check if port 1433 is available: `netstat -an | grep 1433`
- Verify credentials in `.env` file match SQL Server configuration

**Entity Framework Migration Errors**
- Ensure database exists and user has proper permissions
- Check connection string configuration
- Verify Entity Framework tools are installed globally

**API Startup Issues**
- Check if all dependencies are properly installed: `dotnet restore`
- Verify database connection and migrations are applied
- Review application logs for specific error messages

## License

This project is licensed under the Unlicense - see the [LICENSE](LICENSE) file for details.

## Third-Party APIs

This project integrates with external APIs to provide comprehensive anime and character data:

- **AniList GraphQL API**: Primary data source for anime and character information
  - Documentation: [https://docs.anilist.co/guide/graphql](https://docs.anilist.co/guide/graphql)
  - GraphQL Playground: [https://anilist.co/graphiql](https://anilist.co/graphiql)
  - Rate Limits: 90 requests per minute
  - No authentication required for public data

---

## (PT-BR) Documentação em Português

### Visão Geral do Projeto

Este projeto implementa uma API REST que se integra com a API GraphQL do AniList, fornecendo uma ponte perfeita entre sistemas externos e dados de anime/mangá. A aplicação é construída usando .NET 9.0 com um padrão de arquitetura limpa, garantindo manutenibilidade e escalabilidade.

#### Principais Recursos
- **Integração GraphQL para REST**: Converte consultas GraphQL para a API AniList em endpoints REST
- **Persistência de Banco de Dados**: Armazena dados de anime e personagens usando SQL Server com Entity Framework Core
- **Arquitetura Limpa**: Implementa uma arquitetura de 3 camadas (API, Core, Infrastructure)
- **Banco de Dados Containerizado**: Usa Docker para implantação do SQL Server

#### APIs de Terceiros
Este projeto se integra com a [API GraphQL do AniList](https://graphql.anilist.co) para buscar informações de anime e personagens. Para documentação detalhada da API e consultas disponíveis, visite a [Documentação Oficial da API AniList](https://docs.anilist.co/guide/graphql).

### Pré-requisitos

Antes de executar este projeto, certifique-se de ter os seguintes itens instalados:
- **.NET 8.0 SDK**
- **Docker** (para o container do SQL Server)
- **Git**

### Início Rápido

Clone o repositório e navegue até o diretório do projeto:
```bash
git clone https://github.com/luc-llb/api-integration.git
cd api-integration/
```

### Instruções de Configuração

#### 1. Configuração do Ambiente

Primeiro, crie seu arquivo de configuração de ambiente copiando o exemplo:

```bash
cp exemple.env .env
```

Edite o arquivo `.env` para corresponder às suas preferências. A configuração padrão deve funcionar para a maioria das configurações.

#### 2. Instalar Ferramentas de Linha de Comando SQL (apenas Linux)

Se você estiver no Linux e não tiver o `sqlcmd` instalado, use o script fornecido:

```bash
cd db/
chmod +x install_db.sh
./install_db.sh
```

**Nota:** Após instalar o sqlcmd via script, você precisará abrir uma nova sessão de terminal.

#### 3. Iniciar Container do SQL Server

Inicie o container do SQL Server usando Docker:

```bash
docker run -e "ACCEPT_EULA=Y" -e "MSSQL_SA_PASSWORD=Password123!" -p 1433:1433 -d --name sqlserver mcr.microsoft.com/mssql/server:2022-latest
```

> **IMPORTANTE:** Substitua "Password123!" pelo valor da variável `DB_SA_PASSWORD` do seu arquivo `.env`.

#### 4. Criar Banco de Dados e Usuário

Execute o script para criar o banco de dados e usuário:

```bash
cd db/
sleep 60 && ./create_database.sh
```

> **Nota:** A espera de 60 segundos garante que o SQL Server esteja totalmente inicializado. Se o script falhar, aguarde mais 2 minutos antes de tentar novamente.

Este script irá:
- Conectar ao SQL Server usando o usuário SA
- Criar o banco de dados especificado na variável `DB_NAME` do seu arquivo `.env`
- Criar um login com o nome de usuário especificado em `DB_USER` e senha em `DB_PASSWORD`
- Conceder permissões apropriadas ao usuário

#### 5. Instalar Ferramentas do Entity Framework

Instale a ferramenta `dotnet-ef` se você não a tiver:

```bash
dotnet tool install --global dotnet-ef
```

#### 6. Executar Migrações do Entity Framework

Navegue de volta à raiz do projeto e aplique as migrações do banco de dados:

```bash
cd ..
dotnet ef database update --project IntegrationApi/src/IntegrationApi.Infrastructure --startup-project IntegrationApi/src/IntegrationApi.Api --context AppDbContext
```

#### 7. Iniciar a API

Execute a aplicação da API:

```bash
dotnet run --project IntegrationApi/src/IntegrationApi.Api
```

A API estará disponível em `https://localhost:7008` (HTTPS) ou `http://localhost:5084` (HTTP).

### Comandos de Gerenciamento do Banco de Dados

#### Resetar Banco de Dados (Apenas Desenvolvimento)

Para resetar completamente o banco de dados (remover e recriar):

```bash
cd db/
./reset_database.sh
```

**Aviso:** Isso irá excluir permanentemente todos os dados no banco de dados.

### Arquitetura do Projeto

Este projeto segue um padrão de arquitetura limpa com 3 camadas distintas:

#### Camada API (`IntegrationApi.Api`)
A camada de apresentação responsável por:
- Manipular requisições e respostas HTTP
- Controladores de API e roteamento
- Autenticação e autorização
- Validação de entrada e objetos de transferência de dados (DTOs)
- Middleware de tratamento global de exceções

#### Camada Core (`IntegrationApi.Core`)
A camada de lógica de negócio que contém:
- Entidades de domínio e regras de negócio
- Interfaces e contratos de serviços
- Exceções personalizadas e lógica de validação
- Implementação de serviços de aplicação
- Modelos de domínio e objetos de valor

#### Camada Infrastructure (`IntegrationApi.Infrastructure`)
A camada de acesso a dados e integração externa responsável por:
- Contexto do banco de dados e configuração do Entity Framework
- Implementações de repositório
- Integrações de API externa (API GraphQL AniList)
- Persistência de dados e gerenciamento de migração
- Gerenciamento de string de conexão e factory de banco de dados

Esta arquitetura garante:
- **Separação de Responsabilidades**: Cada camada tem uma responsabilidade específica
- **Inversão de Dependência**: Camadas superiores dependem de abstrações, não implementações
- **Testabilidade**: A lógica de negócio é isolada e pode ser testada unitariamente
- **Manutenibilidade**: Mudanças em uma camada não afetam outras
- **Escalabilidade**: Fácil de estender e modificar componentes individuais

### Endpoints da API

A API fornece os seguintes endpoints:

#### Animações
- `GET /api/animations` - Recuperar todas as animações do banco de dados
- `GET /api/animations/{id}` - Recuperar uma animação específica por ID do banco de dados
- `GET /api/animations/search?searchTerm={term}` - Pesquisar animações da API AniList
- `POST /api/animations/search-and-save?searchTerm={term}` - Pesquisar animação da API AniList e salvar o primeiro resultado no banco de dados

#### Personagens
- `GET /api/characters` - Recuperar todos os personagens do banco de dados
- `GET /api/characters/{id}` - Recuperar um personagem específico por ID do banco de dados
- `GET /api/characters/search?searchTerm={term}` - Pesquisar personagens da API AniList
- `POST /api/characters/search-and-save?searchTerm={term}` - Pesquisar personagem da API AniList e salvar o primeiro resultado no banco de dados

### Diretrizes de Desenvolvimento

#### Adicionando Novas Migrações
Quando você modificar entidades ou adicionar novas, crie uma nova migração:

```bash
dotnet ef migrations add NomeDaMigração --project IntegrationApi/src/IntegrationApi.Infrastructure --startup-project IntegrationApi/src/IntegrationApi.Api --context AppDbContext
```

#### Estilo de Código
- Siga as convenções de nomenclatura do C#
- Use async/await para operações assíncronas
- Implemente tratamento adequado de erros e logging
- Escreva testes unitários para lógica de negócio

### Solução de Problemas

#### Problemas Comuns

**Falha na Conexão do SQL Server**
- Certifique-se de que o container Docker está executando: `docker ps`
- Verifique se a porta 1433 está disponível: `netstat -an | grep 1433`
- Verifique se as credenciais no arquivo `.env` correspondem à configuração do SQL Server

**Erros de Migração do Entity Framework**
- Certifique-se de que o banco de dados existe e o usuário tem permissões adequadas
- Verifique a configuração da string de conexão
- Verifique se as ferramentas do Entity Framework estão instaladas globalmente

**Problemas de Inicialização da API**
- Verifique se todas as dependências estão adequadamente instaladas: `dotnet restore`
- Verifique a conexão do banco de dados e se as migrações foram aplicadas
- Revise os logs da aplicação para mensagens de erro específicas

### Licença

Este projeto é licenciado sob a Unlicense - veja o arquivo [LICENSE](LICENSE) para detalhes.

### APIs de Terceiros

Este projeto se integra com APIs externas para fornecer dados abrangentes de anime e personagens:
