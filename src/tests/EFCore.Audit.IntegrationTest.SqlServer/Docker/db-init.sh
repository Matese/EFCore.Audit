 # Wait to be sure that SQL Server came up
sleep 30s

# Run the setup script to create the DB and the schema in the DB
# Note: make sure that your password matches what is in the docker-compose
/opt/mssql-tools/bin/sqlcmd -S localhost -U sa -P P@ssw0rd123 -d master -i db-create.sql