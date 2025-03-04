@echo off
@set path=C:\Program Files (x86)\Inno Setup 6\;%path%

cd 
md publish
dotnet publish ..\SecureChat.Server -c Release -o publish\SecureChat.Server --runtime win-x64 --self-contained false
iscc Installer.Server.iss
pause
