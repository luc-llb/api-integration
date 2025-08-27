# (EN-US) API Integration
Integration with third-party APIs using C# .NET

## Database Setup

This project uses SQL Server as its database. Follow the steps below to set up the database environment:

1. **Configure Environment Variables**

    First, make sure you have an `.env` file in the root directory of the project. You can copy the example file and modify it as needed:

    ```bash
    cp exemple.env .env
    ```

2. **Install SQL Command Line Tools (Linux only)**

    If you're using Linux and don't have `sqlcmd` installed, use the provided script:

    ```bash
    cd api-integration/db
    chmod +x install_db.sh
    ./install_db.sh
    ```

3. **Start SQL Server Container**

    Start the SQL Server container using Docker:

    ```bash
    docker run -e "ACCEPT_EULA=Y" -e "MSSQL_SA_PASSWORD=Password123!" -p 1433:1433 -d --name sqlserver mcr.microsoft.com/mssql/server:2022-latest
    ```

    > **WARNING:** Replace "Password123!" with the value of the `DB_SA_PASSWORD` variable from your `.env` file

4. **Create Database and User**

    Run the script to create the database and user:

    ```bash
    cd api-integration/db
    ./create_database.sh
    ```

    This script will:
    - Connect to the SQL Server using the SA user
    - Create the database specified in the `DB_NAME` variable from the `.env` file
    - Create a login with the name specified in `DB_USER` and password in `DB_PASSWORD`
    - Grant appropriate permissions to the user

5. **Run Entity Framework Migrations**

    Create a migration:
    ```Bash
    cd api-integration/
    dotnet ef migrations add InitialCreate --project IntegrationApi/src/IntegrationApi.Infrastructure --startup-project IntegrationApi/src/IntegrationApi.Api --context AppDbContext
    ```

    Apply the database migrations:

    ```bash
    cd api-integration/
    dotnet ef database update --project IntegrationApi/src/IntegrationApi.Infrastructure --startup-project IntegrationApi/src/IntegrationApi.Api --context AppDbContext
    ```

### Database Management Commands

**Reset Database (Development Only)**
To completely reset the database (remove and recreate):

```bash
cd api-integration/db
./reset_database.sh
```

# (PT-BR) Integração de API
Integração com APIs de terceiros usando C# .NET

## Configuração do Banco de Dados

Este projeto utiliza SQL Server como banco de dados. Siga os passos abaixo para configurar o ambiente de banco de dados:

1. **Configurar Variáveis de Ambiente**

    Primeiramente, certifique-se de ter um arquivo `.env` no diretório raiz do projeto. Você pode copiar o arquivo de exemplo e modificá-lo conforme necessário:

    ```bash
    cp exemple.env .env
    ```

2. **Instalar Ferramentas de Linha de Comando SQL (apenas Linux)**

    Se você estiver usando Linux e não tiver o `sqlcmd` instalado, use o script fornecido:

    ```bash
    cd api-integration/db
    chmod +x install_db.sh
    ./install_db.sh
    ```

3. **Iniciar o Container do SQL Server**

    Inicie o container do SQL Server usando Docker:

    ```bash
    docker run -e "ACCEPT_EULA=Y" -e "MSSQL_SA_PASSWORD=Password123!" -p 1433:1433 -d --name sqlserver mcr.microsoft.com/mssql/server:2022-latest
    ```

    > **IMPORTANTE:** Substitua "Password123!" pelo valor da variável `DB_SA_PASSWORD` do seu arquivo `.env`

4. **Criar Banco de Dados e Usuário**

    Execute o script para criar o banco de dados e o usuário:

    ```bash
    cd api-integration/db
    ./create_database.sh
    ```

    Este script fará:
    - Conectar ao SQL Server usando o usuário SA
    - Criar o banco de dados especificado na variável `DB_NAME` do arquivo `.env`
    - Criar um login com o nome especificado em `DB_USER` e senha em `DB_PASSWORD`
    - Conceder permissões apropriadas para o usuário

5. **Executar as Migrações do Entity Framework**

    Crie uma migração:
    ```Bash
    cd api-integration/
    dotnet ef migrations add InitialCreate --project IntegrationApi/src/IntegrationApi.Infrastructure --startup-project IntegrationApi/src/IntegrationApi.Api --context AppDbContext
    ```

    Aplique as migrações do banco de dados:

    ```bash
    cd api-integration/
    dotnet ef database update --project IntegrationApi/src/IntegrationApi.Infrastructure --startup-project IntegrationApi/src/IntegrationApi.Api --context AppDbContext
    ```

### Comandos de Gerenciamento do Banco de Dados

**Resetar Banco de Dados (Apenas Desenvolvimento)**
Para resetar completamente o banco de dados (remover e recriar):

```bash
cd api-integration/db
./reset_database.sh
```
