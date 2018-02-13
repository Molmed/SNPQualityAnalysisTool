-- Approves a saved sessions wset by updating the genotype
-- status to approved for the approved genotypes, locking
-- the genotype statuses and changing the wset type to
-- an approved session.

CREATE PROCEDURE pSQAT_Approve(@wsetId INTEGER)
AS
BEGIN
SET NOCOUNT ON

DECLARE @lockedWsetTypeId INTEGER
DECLARE @loadId TINYINT
DECLARE @approvedId TINYINT


--
-- Retrieve ID for wset type.
--
SELECT @lockedWsetTypeId = wt.wset_type_id FROM dbo.wset_type wt WHERE wt.name = 'ApprovedSession'
IF @lockedWsetTypeId IS NULL
BEGIN
	RAISERROR('Could not find the type id for ApprovedSession.', 11, 1)
	RETURN
END

--
-- Retrieve IDs for status codes.
--
SELECT @loadId = s.status_id FROM dbo.status s WHERE s.name = 'load'

SELECT @approvedId = s.status_id FROM dbo.status s WHERE s.name = 'approved'

IF @loadId IS NULL OR @approvedId IS NULL 
BEGIN
	RAISERROR('At least one genotype status code could not be found.', 11, 1)
	RETURN
END


--
-- Check if any of the genotypes is locked.
--
IF EXISTS(SELECT * FROM dbo.denorm_genotype dg INNER JOIN #all_genotypes ag ON ag.genotype_id = dg.genotype_id WHERE NOT dg.locked_wset_id IS NULL)
BEGIN
	RAISERROR('At least one of the genotypes is locked for editing.', 11, 1)
	RETURN
END

BEGIN TRANSACTION

--
-- First reset any genotypes in the session which currently have approved status
-- but should not have.
--
UPDATE dbo.denorm_genotype SET status_id = @loadId
WHERE status_id = @approvedId
AND genotype_id IN (SELECT genotype_id FROM #all_genotypes)
AND genotype_id NOT IN (SELECT genotype_id FROM #approved_genotype)

--
-- Now change the status for the approved genotypes.
--
UPDATE dbo.denorm_genotype SET status_id = @approvedId
WHERE genotype_id IN (SELECT genotype_id FROM #approved_genotype)

--
-- Lock the genotypes.
--
UPDATE dbo.denorm_genotype SET locked_wset_id = @wsetId
WHERE genotype_id IN (SELECT genotype_id FROM #all_genotypes)

--
-- Finally change the type of the working set to indicate that it has been locked.
--
UPDATE wset SET wset_type_id = @lockedWsetTypeId WHERE wset_id = @wsetId

COMMIT TRANSACTION

SET NOCOUNT OFF

END  





