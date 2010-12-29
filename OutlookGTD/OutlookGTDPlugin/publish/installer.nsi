VIProductVersion              "0.1.0.0" ; set version here
VIAddVersionKey "FileVersion" "0.1.0.0" ; and here!
VIAddVersionKey "CompanyName" "Daniel Sundberg"
VIAddVersionKey "LegalCopyright" "© Daniel Sundberg (daniel.sundberg@gmx.com)"
VIAddVersionKey "FileDescription" "Installer for Outlook GTD"
OutFile OutlookGTD-Setup.exe

SilentInstall silent

Section Main    
    SetOutPath $TEMP\OutlookGTDInstaller
    SetOverwrite on

    ; All files from publish goes here
    File setup.exe
    File OutlookGTDPlugin.vsto
    SetOutPath "$TEMP\OutlookGTDInstaller\Application Files\OutlookGTDPlugin_0_1_0_1"
    File "Application Files\OutlookGTDPlugin_0_1_0_1\Microsoft.Office.Tools.Common.v4.0.Utilities.dll.deploy"
    File "Application Files\OutlookGTDPlugin_0_1_0_1\Microsoft.Office.Tools.Outlook.v4.0.Utilities.dll.deploy"
    File "Application Files\OutlookGTDPlugin_0_1_0_1\OutlookGTDPlugin.dll.deploy"
    File "Application Files\OutlookGTDPlugin_0_1_0_1\OutlookGTDPlugin.dll.manifest"
    File "Application Files\OutlookGTDPlugin_0_1_0_1\OutlookGTDPlugin.vsto"
    File "Application Files\OutlookGTDPlugin_0_1_0_1\OutlookGTP.UI.dll.deploy"

    ; Launch click once setup
    Exec     '"$TEMP\OutlookGTDInstaller\Setup.exe"'

    ; TODO: check for previous version before proceeding with installation
    ; TODO: clean up temporary files after install is finished
SectionEnd
