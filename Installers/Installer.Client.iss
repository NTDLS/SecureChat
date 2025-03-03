#define AppVersion "1.0.0"

[Setup]
;-- Main Setup Information
 AppName                          = Secure Chat
 AppVersion                       = {#AppVersion}
 AppVerName                       = Secure Chat {#AppVersion}
 AppCopyright                     = Copyright � 1995-2025 NetworkDLS.
 DefaultDirName                   = {commonpf}\NetworkDLS\Secure Chat
 DefaultGroupName                 = NetworkDLS\Secure Chat
 UninstallDisplayIcon             = {app}\SecureChat.Client.exe
 SetupIconFile                    = "..\Media\Logo.ico"
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
 Source: "..\Media\Logo.ico"; DestDir: "{app}"; Flags: IgnoreVersion;

[Icons]
 Name: "{commondesktop}\Secure Chat"; Filename: "{app}\SecureChat.Client.exe";
 Name: "{group}\Secure Chat"; Filename: "{app}\SecureChat.Client.exe";

[Run]
 Filename: "{app}\SecureChat.Client.exe"; Description: "Run Secure Chat now?"; Flags: postinstall nowait skipifsilent shellexec;
