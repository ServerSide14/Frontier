@echo off
dotnet run --project Content.Server --configuration Release --config-file .\ServerFiles\ConfigFrontier.toml --data-dir .\ServerFiles\Database
pause
