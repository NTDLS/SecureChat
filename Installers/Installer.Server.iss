#define AppVersion "1.0.2"

[Setup]
 AppName                          = Secure Chat Server
 AppVersion                       = {#AppVersion}
 AppVerName                       = Secure Chat Server {#AppVersion}
 AppCopyright                     = Copyright © 1995-2025 NetworkDLS.
 DefaultDirName                   = {commonpf}\NetworkDLS\Secure Chat Server
 DefaultGroupName                 = NetworkDLS\Secure Chat Server
 UninstallDisplayIcon             = {app}\SecureChat.Server.exe
 SetupIconFile                    = "..\MEdia\Logo.ico"
 PrivilegesRequired               = admin
 Uninstallable                    = Yes
 MinVersion                       = 0.0,7.0
 Compression                      = bZIP/9
 ChangesAssociations              = Yes
 OutputBaseFilename               = SecureChat.Server {#AppVersion}
 ArchitecturesInstallIn64BitMode  = x64compatible
 AppPublisher                     = NetworkDLS
 AppPublisherURL                  = http://www.NetworkDLS.com/
 AppUpdatesURL                    = http://www.NetworkDLS.com/

[Files]
 Source: "publish\SecureChat.Server\*.*"; DestDir: "{app}"; Flags: IgnoreVersion RecurseSubDirs;
 Source: "..\Data\server.db"; DestDir: "{app}\data"; Flags: RecurseSubDirs OnlyIfDoesntExist;
 Source: "..\Media\Logo.ico"; DestDir: "{app}"; Flags: IgnoreVersion;

[Run]
 Filename: "{app}\SecureChat.Server.exe"; Parameters: "install"; Flags: runhidden; StatusMsg: "Installing service...";
 Filename: "{app}\SecureChat.Server.exe"; Parameters: "start"; Flags: runhidden; StatusMsg: "Starting service...";

[UninstallRun]
 Filename: "{app}\SecureChat.Server.exe"; Parameters: "uninstall"; Flags: runhidden; StatusMsg: "Installing service..."; RunOnceId: "ServiceRemoval";
