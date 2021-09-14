IF NOT EXISTS (SELECT * FROM sys.databases WHERE name = 'PersonDb')
BEGIN
  CREATE DATABASE PersonDb;
END;
GO