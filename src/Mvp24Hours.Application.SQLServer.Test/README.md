#Command 
docker run --name sqlserver -e "ACCEPT_EULA=Y" -e "MSSQL_SA_PASSWORD=MyPass@word" -p 1433:1433 -d mcr.microsoft.com/mssql/server

#ConnectionString
Server=localhost,1433;Initial Catalog=MyTestDb;Integrated Security=True;User Id=sa;Password=MyPass@word;

#Tools
https://dbeaver.io/download/