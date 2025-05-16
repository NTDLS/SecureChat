@echo off
@set path=C:\Program Files (x86)\Inno Setup 6\;%path%

cd 
md publish
dotnet publish ..\Talkster.Server -c Release -o publish\Talkster.Server --runtime win-x64 --self-contained false
del publish\Talkster.Server\*.pdb /q

iscc Installer.Server.iss
rd publish\Talkster.Server /s /q
pause
