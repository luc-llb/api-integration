# (EN-US) API Integration
Integration with third-party APIs using C# .NET

## Database Setup

This project uses SQL Server as its database. Follow the steps below to set up the database environment:

(Optional) If you use Linux and not have install the **sqlcmd**, use the script `db/install_db.sh`:
```bash
   cd /YOUR_PATH/api-integration/db
   chmod +x install_db.sh
   ./install_db.sh
```

1. **Start SQL Server Container:**
    ```bash
    docker run -e "ACCEPT_EULA=Y" -e "MSSQL_SA_PASSWORD=Password123!" -p 1433:1433 -d --name sqlserver mcr.microsoft.com/mssql/server:2022-latest
    ```
    Replace "Password123!" with the value of `DB_SA_PASSWORD` from your `.env`

2. **Create Database and User:**
    ```bash
    cd /YOUR_PATH/api-integration/db
    chmod +x create_database.sh
    ./create_database.sh
    ```
   
    This script will:
    - Connect to SQL Server using the SA user
    - Create the 'animes' database if it doesn't exist
    - Create the 'admin' login with appropriate permissions

3. **Run Entity Framework Migrations:**
    ```bash
    cd /YOUR_PATH/api-integration/IntegrationApi/src/IntegrationApi.Api
    dotnet ef database update --project ../IntegrationApi.Infrastructure --startup-project . --context AppDbContext
    ```

4. **Configure Environment Variables:**
    Make sure your `.env` file contains the correct database connection information, following example given in [example.env](/exemple.env)

# (PT-BR) Integração de API
Integração com APIs de terceiros usando C# .NET

## Configuração do Banco de Dados

Este projeto utiliza SQL Server como banco de dados. Siga os passos abaixo para configurar o ambiente de banco de dados:

(Opicional) Se você usa Linux e não possui o **sqlcmd** instalado, use o script `db/install_db.sh`:
```bash
   cd /YOUR_PATH/api-integration/db
   chmod +x install_db.sh
   ./install_db.sh
```

1. **Iniciar o Container do SQL Server:**
    ```bash
    docker run -e "ACCEPT_EULA=Y" -e "MSSQL_SA_PASSWORD=Password123!" -p 1433:1433 -d --name sqlserver mcr.microsoft.com/mssql/server:2022-latest
    ```
    Substitua "Password123!" pelo valor de `DB_SA_PASSWORD` do seu `.env`

2. **Criar Banco de Dados e Usuário:**
    ```bash
    cd /YOUR_PATH/api-integration/db
    chmod +x create_database.sh
    ./create_database.sh
    ```
   
    Este script irá:
    - Conectar ao SQL Server usando o usuário SA
    - Criar o banco de dados 'animes' se ele não existir
    - Criar o login 'admin' com as permissões apropriadas

3. **Executar as Migrações do Entity Framework:**
    ```bash
    cd /YOUR_PATH/api-integration/IntegrationApi/src/IntegrationApi.Api
    dotnet ef database update --project ../IntegrationApi.Infrastructure --startup-project . --context AppDbContext
    ```

4. **Configurar Variáveis de Ambiente:**
    Certifique-se de que seu arquivo `.env` contenha as informações corretas de conexão com o banco de dados, seguindo exemplo dado em [example.env](/exemple.env)
