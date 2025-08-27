#!/bin/bash

# Loading variables from .env file
source <(grep -v '^#' ../.env | sed -E 's/(.*)=(.*)$/export \1=\2/' | sed -E 's/\r//')

echo "Try connect in SQL Server into $DB_HOST:$DB_PORT as SA..."
echo "Executing database creation script..."

# Execute the SQL script as user SA
sqlcmd -S "$DB_HOST,$DB_PORT" -U SA -P "$DB_SA_PASSWORD" -i create_database.sql

if [ $? -eq 0 ]; then
    echo "Database created/verified successfully!"
else
    echo "Error creating database. Check if SQL Server is running and accessible."
fi
