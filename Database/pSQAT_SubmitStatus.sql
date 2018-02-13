-- Updates the status of genotypes according to the #pending_status table
-- and removes the entries in the #pending_status table.

CREATE PROCEDURE pSQAT_SubmitStatus
AS
BEGIN
SET NOCOUNT ON

DECLARE @tempStatusId TINYINT


--
-- Check if any of the genotypes is locked.
--
IF EXISTS(SELECT * FROM dbo.denorm_genotype dg INNER JOIN #pending_status ps ON ps.genotype_id = dg.genotype_id WHERE NOT dg.locked_wset_id IS NULL)
BEGIN
	RAISERROR('At least one of the genotypes is locked for editing.', 11, 1)
	RETURN
END


BEGIN TRANSACTION

WHILE EXISTS(SELECT * FROM #pending_status)
BEGIN
	-- Get next status code to process.
	SELECT TOP 1 @tempStatusId = new_status_id FROM #pending_status 
	
	-- Update all genotypes with changes for that particular status code.
	UPDATE dbo.denorm_genotype SET status_id = @tempStatusId
	WHERE genotype_id IN 
	(SELECT ps.genotype_id FROM #pending_status ps
	WHERE ps.new_status_id = @tempStatusId)
	
	-- Remove processed items.
	DELETE FROM #pending_status WHERE new_status_id = @tempStatusId
END

COMMIT TRANSACTION


SET NOCOUNT OFF

END  





