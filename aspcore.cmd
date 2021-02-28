@REM Cần sử dụng Command Promt hay Powershell để khởi chạy
@ECHO OFF
set "project=ASPCoreMVC"
if %1==build dotnet build
if %1==migrator GOTO Migrator
if %1==run dotnet watch --project ./src/%project%.Web/%project%.Web.csproj run

EXIT /B 0

@REM Start Hàm migration
:Migrator
dotnet build
dotnet ef migrations add %2 --project ./src/%project%.EntityFrameworkCore.DbMigrations/%project%.EntityFrameworkCore.DbMigrations.csproj --startup-project ./src/%project%.Web/%project%.Web.csproj
dotnet ef database update --project ./src/%project%.EntityFrameworkCore.DbMigrations/%project%.EntityFrameworkCore.DbMigrations.csproj --startup-project ./src/%project%.Web/%project%.Web.csproj
@REM dotnet run --project ./src/%project%.DbMigrator/%project%.DbMigrator.csproj --startup-project ./src/%project%.DbMigrator/%project%.DbMigrator.csproj
EXIT /B 0
@REM End Hàm migration
