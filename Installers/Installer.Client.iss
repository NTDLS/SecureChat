#define AppVersion "1.0.21"

[Setup]
;-- Main Setup Information
 AppName                          = Talkster
 AppVersion                       = {#AppVersion}
 AppVerName                       = Talkster {#AppVersion}
 AppCopyright                     = Copyright © 1995-2025 NetworkDLS.
 DefaultDirName                   = {commonpf}\NetworkDLS\Talkster
 DefaultGroupName                 = NetworkDLS\Talkster
 UninstallDisplayIcon             = {app}\Talkster.Client.exe
 SetupIconFile                    = "..\Media\Icon.ico"
 PrivilegesRequired               = admin 
 Uninstallable                    = Yes
 MinVersion                       = 0.0,7.0
 Compression                      = bZIP/9
 ChangesAssociations              = Yes
 OutputBaseFilename               = Talkster.Client {#AppVersion}
 ArchitecturesInstallIn64BitMode  = x64compatible
 AppPublisher                     = NetworkDLS
 AppPublisherURL                  = http://www.NetworkDLS.com/
 AppUpdatesURL                    = http://www.NetworkDLS.com/

[Files]
 Source: "publish\Talkster.Client\*.*"; DestDir: "{app}"; Flags: IgnoreVersion RecurseSubDirs;
 Source: "..\Media\Icon.ico"; DestDir: "{app}"; Flags: IgnoreVersion;

[Icons]
 Name: "{commondesktop}\Talkster"; Filename: "{app}\Talkster.Client.exe";
 Name: "{group}\Talkster"; Filename: "{app}\Talkster.Client.exe";

[Tasks]
  Name: "AutoStartAtLogin"; Description: "Start when I log into Windows?"; GroupDescription: "Startup options:";

[Run]
 Filename: "{app}\Talkster.Client.exe"; Description: "Run Talkster now?"; Flags: postinstall nowait skipifsilent shellexec;

[Registry]
  Root: HKCU; Subkey: "SOFTWARE\Microsoft\Windows\CurrentVersion\Run"; ValueType: string; ValueName: "Talkster"; ValueData: """{app}\Talkster.Client.exe"""; Flags: uninsdeletevalue; Tasks: AutoStartAtLogin
 