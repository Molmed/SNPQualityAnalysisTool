--RETURNS ALL ANNOTATIONS OF A CERTAIN TYPE FOR A MARKER, CONCATENATED INTO A SINGLE STRING,
--AND WITH EACH ANNOTATION SEPARATED BY THE STRING @SEPARATOR.

CREATE FUNCTION dbo.fGetAnnotationConcat (@marker_id INTEGER, @annotation_type_id INTEGER, @separator VARCHAR(30))
RETURNS VARCHAR(2000)

AS
BEGIN

DECLARE @retString VARCHAR(2000)

SET @retString = ''

SELECT @retString = @retString + a.value + @separator
FROM dbo.annotation a 
INNER JOIN dbo.annotation_link al ON al.annotation_id = a.annotation_id
WHERE al.marker_id = @marker_id AND a.annotation_type_id = @annotation_type_id 

IF LEN(@retString) > LEN(@separator)
BEGIN
	SET @retString = LEFT(@retString, LEN(@retString) - LEN(@separator))
END

RETURN @retString

END
