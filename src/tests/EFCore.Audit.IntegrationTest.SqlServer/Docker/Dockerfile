FROM mcr.microsoft.com/mssql/server:latest

USER root

# Create app directory
RUN mkdir -p /usr/src/app
WORKDIR /usr/src/app

# Copy initialization scripts
COPY ./db-create.sql .
COPY ./db-init.sh .
COPY ./entrypoint.sh .

# Grant permissions for the db-init script to be executable
RUN chmod +x /usr/src/app/db-create.sql
RUN chmod +x /usr/src/app/db-init.sh
RUN chmod +x /usr/src/app/entrypoint.sh

USER mssql

ENTRYPOINT [ "/bin/bash", "entrypoint.sh" ]
CMD [ "/opt/mssql/bin/sqlservr" ]