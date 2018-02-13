
CREATE ROLE SQATuser


EXEC sp_addrolemember 'db_datareader', 'SQATuser'
EXEC sp_addrolemember 'db_datawriter', 'SQATuser'

GRANT EXECUTE ON fGetAnnotationConcat TO SQATuser
GRANT EXECUTE ON fTranslateCoord TO SQATuser
GRANT EXECUTE ON pSQAT_Approve TO SQATuser
GRANT EXECUTE ON pSQAT_GetControlStatistics TO SQATuser
GRANT EXECUTE ON pSQAT_GetExperimentStatistics TO SQATuser
GRANT EXECUTE ON pSQAT_GetGenotypeHistory TO SQATuser
GRANT EXECUTE ON pSQAT_GetItemStatistics TO SQATuser
GRANT EXECUTE ON pSQAT_GetPlateContents TO SQATuser
GRANT EXECUTE ON pSQAT_GetPlateSelection TO SQATuser
GRANT EXECUTE ON pSQAT_GetRerunItems TO SQATuser
GRANT EXECUTE ON pSQAT_RejectMinority TO SQATuser
GRANT EXECUTE ON pSQAT_SubmitStatus TO SQATuser
GRANT EXECUTE ON pSQAT_TestMarkers TO SQATuser
