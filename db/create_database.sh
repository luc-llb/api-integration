#!/bin/bash

# Find the script directory and the project root directory
SCRIPT_DIR="$(cd "$(dirname "${BASH_SOURCE[0]}")" &> /dev/null && pwd)"
ENV_FILE="$SCRIPT_DIR/../.env"

if [ ! -f "$ENV_FILE" ]; then
    echo "Error: .env file not found at $ENV_FILE"
    exit 1
fi

echo "Loading variables from $ENV_FILE"

# Load variables from .env file
while IFS='=' read -r key value || [ -n "$key" ]; do
    # Ignore empty lines or comments
    if [[ -z "$key" || "$key" =~ ^[[:space:]]*# ]]; then
        continue
    fi

    # Remove whitespace and quotes from the variable name
    key=$(echo "$key" | xargs)

    # Extract the value up to the first # (comment)
    value=$(echo "$value" | sed 's/#.*$//' | xargs)

    # Remove quotes from the value if they exist
    value=$(echo "$value" | sed -e 's/^"//' -e 's/"$//' -e "s/^'//" -e "s/'$//")

    # Export the variable
    export "$key"="$value"
done < "$ENV_FILE"

echo "Trying to connect to SQL Server at $DB_HOST:$DB_PORT as user SA..."
echo "Executing database creation script..."

# Execute the SQL script as user SA and pass environment variables as SQLCMD variables
sqlcmd -S "$DB_HOST,$DB_PORT" -U SA -P "$DB_SA_PASSWORD" -i create_database.sql \
       -v DB_NAME="$DB_NAME" DB_USER="$DB_USER" DB_PASSWORD="$DB_PASSWORD"

if [ $? -eq 0 ]; then
    echo "   Database and user created/verified successfully!"
    echo "   Database name: $DB_NAME"
    echo "   User: $DB_USER"
else
    echo "   Error creating database. Check if SQL Server is running and accessible."
    echo "   Also verify that the credentials are correct."
fi
