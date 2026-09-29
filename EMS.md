mkdir EmployeeManagementSystem
--frontend
cd EmployeeManagementSystem
node -v
npm -v
ng version
npm install -g @angular/cli
ng new frontend --routing --style=css
cd frontend
ng serve
--backend
cd ..
dotnet --version
mkdir backend
cd backend
dotnet new webapi -n EmployeeManagement.API
cd EmployeeManagement.API
--sql
dotnet run
docker --version
docker run -e ACCEPT_EULA=Y -e MSSQL_SA_PASSWORD=YourStrongPassword123! -p 1433:1433 --name employee-sqlserver -d mcr.microsoft.com/mssql/server:2022-latest
docker ps

--run
docker start employee-sqlserver
cd EmployeeManagementSystem/backend/EmployeeManagement.API
dotnet run
cd EmployeeManagementSystem/frontend
ng serve
