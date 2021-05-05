Echo off
rem batch file to run a script to create a db
rem 9/10/2020

rem sqlcmd -S localhost -E -i imagine_park_db.sql
sqlcmd -S localhost\sqlexpress -E -i imagine_park_db.sql
rem sqlcmd -S localhost\mssqlserver -E -i imagine_park_db.sql


Echo .
Echo if no error messages appear DB was created
Pause