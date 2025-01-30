#!/bin/bash

# Make sure the script is being executed in BASH terminal

# Set environment variables for your .NET project

# Database
export DB_HOST="replace_with_db_host"
export DB_PORT="replace_with_db_port"
export DB_NAME="replace_with_db_name"

# MySQL credentials
export MYSQL_LOCAL_USER="replace_with_mysql_username"
export MYSQL_LOCAL_PASSWORD="replace_with_mysql_password"

export AUTH0_DOMAIN="replace_with_auth0_domain"
export AUTH0_AUDIENCE="replace_with_auth0_audience"

echo "Environment variables set successfully."
