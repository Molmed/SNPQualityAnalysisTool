-- Returns statistics for individual experiments and a certain control item.

CREATE PROCEDURE pSQAT_GetControlStatistics(@controlItemId INTEGER) 
AS
BEGIN
SET NOCOUNT ON

DECLARE @tested TABLE(
					experiment_id INTEGER PRIMARY KEY,	
					genotypes INTEGER NOT NULL
					)
					
DECLARE @failed TABLE(
					experiment_id INTEGER PRIMARY KEY,
					genotypes INTEGER NOT NULL
					)						
						

--
-- Sum performed tests for this item in all tests.
--
INSERT INTO @tested (experiment_id, genotypes)
SELECT un.experiment_id, COUNT(un.experiment_id)
FROM
(
SELECT experiment_id
FROM #inheritance_test
WHERE item_id = @controlItemId
UNION
SELECT experiment_id
FROM #blank_test bt
WHERE item_id = @controlItemId
UNION
SELECT experiment_id
FROM #homozygote_test ht
WHERE item_id = @controlItemId
) un
GROUP BY un.experiment_id


--
-- Sum failed tests for this item in all tests.
--
INSERT INTO @failed (experiment_id, genotypes)
SELECT un.experiment_id, COUNT(un.experiment_id)
FROM
(
SELECT experiment_id
FROM #inheritance_test
WHERE item_id = @controlItemId AND success = 0
UNION
SELECT experiment_id
FROM #blank_test bt
WHERE item_id = @controlItemId AND success = 0
UNION
SELECT experiment_id
FROM #homozygote_test ht
WHERE item_id = @controlItemId AND success = 0
) un
GROUP BY un.experiment_id


--
-- Combine the statistics into the return table.
--
SELECT es.experiment_id AS [experiment_id],
es.identifier AS [Experiment//CN],
ISNULL(t.genotypes, 0) AS [Tests//IN],
ISNULL(f.genotypes, 0) AS [Failures//IF]
FROM #experiment_stat es
LEFT OUTER JOIN @tested t ON t.experiment_id = es.experiment_id
LEFT OUTER JOIN @failed f ON f.experiment_id = es.experiment_id


SET NOCOUNT OFF

END  





