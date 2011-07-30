!define VERSION "0.3.0.0" 
!define VERSION_PATH "0_3_0_0"
VIProductVersion              "${VERSION}" ; set version here
VIAddVersionKey "FileVersion" "${VERSION}" ; and here!
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
    SetOutPath "$TEMP\OutlookGTDInstaller\Application Files\OutlookGTDPlugin_${VERSION_PATH}"
    File "Application Files\OutlookGTDPlugin_${VERSION_PATH}\Microsoft.Office.Tools.Common.v4.0.Utilities.dll.deploy"
    File "Application Files\OutlookGTDPlugin_${VERSION_PATH}\Microsoft.Office.Tools.Outlook.v4.0.Utilities.dll.deploy"
    File "Application Files\OutlookGTDPlugin_${VERSION_PATH}\OutlookGTDPlugin.dll.deploy"
    File "Application Files\OutlookGTDPlugin_${VERSION_PATH}\OutlookGTDPlugin.dll.manifest"
    File "Application Files\OutlookGTDPlugin_${VERSION_PATH}\OutlookGTDPlugin.vsto"
    File "Application Files\OutlookGTDPlugin_${VERSION_PATH}\OutlookGTP.UI.dll.deploy"
    File "Application Files\OutlookGTDPlugin_${VERSION_PATH}\OutlookGTD.Logic.dll.deploy"
	File "Application Files\OutlookGTDPlugin_${VERSION_PATH}\OutlookGTD.Common.dll.deploy"

    ; Launch click once setup
    Exec     '"$TEMP\OutlookGTDInstaller\Setup.exe"'

    ; TODO: check for previous version before proceeding with installation
    ; TODO: clean up temporary files after install is finished
SectionEnd
