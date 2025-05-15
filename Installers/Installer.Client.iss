#define AppVersion "1.0.14"

[Setup]
;-- Main Setup Information
 AppName                          = Secure Chat
 AppVersion                       = {#AppVersion}
 AppVerName                       = Secure Chat {#AppVersion}
 AppCopyright                     = Copyright © 1995-2025 NetworkDLS.
 DefaultDirName                   = {commonpf}\NetworkDLS\Secure Chat
 DefaultGroupName                 = NetworkDLS\Secure Chat
 UninstallDisplayIcon             = {app}\Secure Chat.exe
 SetupIconFile                    = "..\Media\Icon.ico"
 PrivilegesRequired               = admin 
 Uninstallable                    = Yes
 MinVersion                       = 0.0,7.0
 Compression                      = bZIP/9
 ChangesAssociations              = Yes
 OutputBaseFilename               = SecureChat.Client {#AppVersion}
 ArchitecturesInstallIn64BitMode  = x64compatible
 AppPublisher                     = NetworkDLS
 AppPublisherURL                  = http://www.NetworkDLS.com/
 AppUpdatesURL                    = http://www.NetworkDLS.com/

[Files]
 Source: "publish\SecureChat.Client\*.*"; DestDir: "{app}"; Flags: IgnoreVersion RecurseSubDirs;
 Source: "..\Media\Icon.ico"; DestDir: "{app}"; Flags: IgnoreVersion;

[Icons]
 Name: "{commondesktop}\Secure Chat"; Filename: "{app}\Secure Chat.exe";
 Name: "{group}\Secure Chat"; Filename: "{app}\Secure Chat.exe";

[Tasks]
  Name: "AutoStartAtLogin"; Description: "Start when I log into Windows?"; GroupDescription: "Startup options:";

[Run]
 Filename: "{app}\Secure Chat.exe"; Description: "Run Secure Chat now?"; Flags: postinstall nowait skipifsilent shellexec;

[Registry]
  Root: HKCU; Subkey: "SOFTWARE\Microsoft\Windows\CurrentVersion\Run"; ValueType: string; ValueName: "Secure Chat"; ValueData: """{app}\Secure Chat.exe"""; Flags: uninsdeletevalue; Tasks: AutoStartAtLogin
 