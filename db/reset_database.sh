#!/bin/bash

# Determinar caminho do script e do diretório raiz do projeto
SCRIPT_DIR="$(cd "$(dirname "${BASH_SOURCE[0]}")" &> /dev/null && pwd)"
ENV_FILE="$SCRIPT_DIR/../.env"

# Verificar se o arquivo .env existe
if [ ! -f "$ENV_FILE" ]; then
    echo "Erro: Arquivo .env não encontrado em $ENV_FILE"
    exit 1
fi

echo "Carregando variáveis do arquivo $ENV_FILE"

# Carregar variáveis do arquivo .env
while IFS='=' read -r key value || [ -n "$key" ]; do
    # Ignorar linhas vazias ou comentários
    if [[ -z "$key" || "$key" =~ ^[[:space:]]*# ]]; then
        continue
    fi
    
    # Remover espaços em branco e aspas do nome da variável
    key=$(echo "$key" | xargs)
    
    # Extrair o valor até o primeiro # (comentário)
    value=$(echo "$value" | sed 's/#.*$//' | xargs)
    
    # Remover aspas do valor se existirem
    value=$(echo "$value" | sed -e 's/^"//' -e 's/"$//' -e "s/^'//" -e "s/'$//")
    
    # Exportar a variável
    export "$key"="$value"
done < "$ENV_FILE"

# SQL para remover todos os usuários e depois o banco de dados
SQL_RESET="
USE master;
GO

-- Forçar desconexão de todos os usuários do banco de dados
IF EXISTS (SELECT name FROM sys.databases WHERE name = '${DB_NAME}')
BEGIN
    ALTER DATABASE ${DB_NAME} SET SINGLE_USER WITH ROLLBACK IMMEDIATE;
    DROP DATABASE ${DB_NAME};
    PRINT 'O banco de dados ${DB_NAME} foi removido com sucesso.';
END
GO

-- Remover login se existir
IF EXISTS (SELECT name FROM sys.server_principals WHERE name = '${DB_USER}')
BEGIN
    DROP LOGIN ${DB_USER};
    PRINT 'O login ${DB_USER} foi removido com sucesso.';
END
GO
"

echo "Tentando conectar ao SQL Server em $DB_HOST:$DB_PORT como usuário SA..."
echo "Executando script para remover banco de dados e usuário existentes..."

# Executar o script SQL para resetar o banco de dados
echo "$SQL_RESET" | sqlcmd -S "$DB_HOST,$DB_PORT" -U SA -P "$DB_SA_PASSWORD" 

if [ $? -eq 0 ]; then
    echo "✅ Banco de dados e usuário removidos com sucesso!"
    
    # Executar o script de criação para recriar tudo do zero
    "$SCRIPT_DIR/create_database.sh"
else
    echo "❌ Erro ao remover o banco de dados. Verifique se o SQL Server está em execução e acessível."
    echo "   Verifique também se as credenciais estão corretas."
    exit 1
fi
