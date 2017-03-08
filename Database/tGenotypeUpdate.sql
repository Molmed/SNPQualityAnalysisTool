CREATE TRIGGER tGenotypeUpdate ON denorm_genotype
AFTER UPDATE

AS

DECLARE @authorityId INTEGER

SET NOCOUNT ON

IF UPDATE(status_id)
BEGIN
	-- Find the authority id of the current user.
	SELECT @authorityId = authority_id FROM dbo.authority WHERE
	identifier = SYSTEM_USER

	IF @authorityId IS NULL
	BEGIN
		ROLLBACK TRANSACTION
		RAISERROR('Unable to find the user name of the current user.', 11, 1)
		RETURN
	END

	-- Log status changes for all updated rows.
	INSERT INTO dbo.status_log (genotype_id, old_status_id, new_status_id, authority_id)
	SELECT i.genotype_id, d.status_id, i.status_id, @authorityId
	FROM inserted i
	INNER JOIN deleted d ON d.genotype_id = i.genotype_id
END

SET NOCOUNT OFF