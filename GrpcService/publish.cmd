dotnet publish -c release -r ubuntu.20.04-x64 --no-self-contained
rem dotnet publish -c release -r ubuntu.22.04-x64 --no-self-contained
xcopy bin\release\net6.0\ubuntu.20.04-x64\publish\*.* c:\temp\GrpcService\ /Y
pause
