#define AppVersion "1.0.21"

[Setup]
 AppName                          = Talkster Server
 AppVersion                       = {#AppVersion}
 AppVerName                       = Talkster Server {#AppVersion}
 AppCopyright                     = Copyright © 1995-2025 NetworkDLS.
 DefaultDirName                   = {commonpf}\NetworkDLS\Talkster Server
 DefaultGroupName                 = NetworkDLS\Talkster Server
 UninstallDisplayIcon             = {app}\Talkster.Server.exe
 SetupIconFile                    = "..\MEdia\Icon.ico"
 PrivilegesRequired               = admin
 Uninstallable                    = Yes
 MinVersion                       = 0.0,7.0
 Compression                      = bZIP/9
 ChangesAssociations              = Yes
 OutputBaseFilename               = Talkster.Server {#AppVersion}
 ArchitecturesInstallIn64BitMode  = x64compatible
 AppPublisher                     = NetworkDLS
 AppPublisherURL                  = http://www.NetworkDLS.com/
 AppUpdatesURL                    = http://www.NetworkDLS.com/

[Files]
 Source: "publish\Talkster.Server\*.*"; DestDir: "{app}"; Flags: IgnoreVersion RecurseSubDirs;
 Source: "..\Data\server.db"; DestDir: "{app}\data"; Flags: RecurseSubDirs OnlyIfDoesntExist;
 Source: "..\Media\Icon.ico"; DestDir: "{app}"; Flags: IgnoreVersion;

[Run]
 Filename: "{app}\Talkster.Server.exe"; Parameters: "install"; Flags: runhidden; StatusMsg: "Installing service...";
 Filename: "{app}\Talkster.Server.exe"; Parameters: "start"; Flags: runhidden; StatusMsg: "Starting service...";

[UninstallRun]
 Filename: "{app}\Talkster.Server.exe"; Parameters: "uninstall"; Flags: runhidden; StatusMsg: "Installing service..."; RunOnceId: "ServiceRemoval";
