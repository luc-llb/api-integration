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
    echo "Carregado: $key=$value"
done < "$ENV_FILE"

echo "Tentando conectar ao SQL Server em $DB_HOST:$DB_PORT como usuário SA..."
echo "Executando script de criação do banco de dados..."

# Execute the SQL script as user SA and pass environment variables as SQLCMD variables
sqlcmd -S "$DB_HOST,$DB_PORT" -U SA -P "$DB_SA_PASSWORD" -i create_database.sql \
       -v DB_NAME="$DB_NAME" DB_USER="$DB_USER" DB_PASSWORD="$DB_PASSWORD"

if [ $? -eq 0 ]; then
    echo "✅ Banco de dados e usuário criados/verificados com sucesso!"
    echo "   Nome do banco: $DB_NAME"
    echo "   Usuário: $DB_USER"
else
    echo "❌ Erro ao criar o banco de dados. Verifique se o SQL Server está em execução e acessível."
    echo "   Verifique também se as credenciais estão corretas."
fi
