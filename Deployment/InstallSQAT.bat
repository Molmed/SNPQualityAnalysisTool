rmdir /S /Q "%ProgramFiles%\SNP Quality Analysis Tool"

mkdir "%ProgramFiles%\SNP Quality Analysis Tool"

xcopy ".\SNP Quality Analysis Tool.exe" "%ProgramFiles%\SNP Quality Analysis Tool\"
xcopy ".\SQATconfig.xml" "%ProgramFiles%\SNP Quality Analysis Tool\"
xcopy ".\SQATconnect.xml" "%ProgramFiles%\SNP Quality Analysis Tool\"

xcopy ".\SNP Quality Analysis Tool.lnk" "%ALLUSERSPROFILE%\Desktop\" /Y
 