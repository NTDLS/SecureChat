@echo off
@set path=C:\Program Files (x86)\Inno Setup 6\;%path%

cd 
md publish
dotnet publish ..\Talkster.Client -c Release -o publish\Talkster.Client --runtime win-x64 --self-contained false
del publish\Talkster.Client\*.pdb /q

iscc Installer.Client.iss
rd publish\Talkster.Client /s /q
pause
