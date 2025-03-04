@echo off
@set path=C:\Program Files (x86)\Inno Setup 6\;%path%

cd 
md publish
dotnet publish ..\SecureChat.Client -c Release -o publish\SecureChat.Client --runtime win-x64 --self-contained false

iscc Installer.Client.iss
rd publish\SecureChat.Client /s /q
pause
