-- Rejects the minority results for a certain experiment when there is a winning majority
-- with at least @majorityPercent %. If @experimentId is NULL, all experiments are included,
-- otherwise only the specified experiment.

CREATE PROCEDURE pSQAT_RejectMinority(@experimentId INTEGER, @majorityPercent INTEGER) 
AS
BEGIN
SET NOCOUNT ON

DECLARE @rejectedStatusId TINYINT

DECLARE @count TABLE(
				item_id INTEGER NOT NULL,
				experiment_id INTEGER NOT NULL,
				alleles VARCHAR(255) NOT NULL
				)
				

DECLARE @frequencies TABLE(
				item_id INTEGER,
				experiment_id INTEGER,			
				alleles VARCHAR(255),
				alleles_count INTEGER,
				PRIMARY KEY(item_id, experiment_id, alleles)				
				)

DECLARE @total TABLE(
				item_id INTEGER NOT NULL,
				experiment_id INTEGER NOT NULL,
				alleles_total INTEGER NOT NULL,
				PRIMARY KEY(item_id, experiment_id)
				)
				
DECLARE @fraction TABLE(
				item_id INTEGER,
				experiment_id INTEGER,
				alleles VARCHAR(255),
				fraction REAL NOT NULL,
				PRIMARY KEY(item_id, experiment_id, alleles)				
				)

DECLARE @winners TABLE(
				item_id INTEGER NOT NULL,
				experiment_id INTEGER NOT NULL,
				alleles VARCHAR(255) NOT NULL,
				PRIMARY KEY(item_id, experiment_id)			
				)

DECLARE @affected_genotype TABLE(
				genotype_id INTEGER PRIMARY KEY
				)

--
-- Define id for rejected status.
--
SET @rejectedStatusId = 2


--
-- Get all results which have been in the duplicate test.
--
INSERT INTO @count (item_id, experiment_id, alleles)
SELECT dt.item_id, dt.experiment_id, dg.alleles
FROM #duplicate_test dt
INNER JOIN #all_genotypes ag ON (ag.item_id = dt.item_id AND ag.experiment_id = dt.experiment_id)
INNER JOIN dbo.denorm_genotype dg ON dg.genotype_id = ag.genotype_id
WHERE (dt.experiment_id = @experimentId OR @experimentId IS NULL)
AND NOT dg.alleles = 'N/A'


--
-- Calculate the number of different results for each item and experiment. 
--
INSERT INTO @frequencies (item_id, experiment_id, alleles, alleles_count)
SELECT 
c.item_id,
c.experiment_id,
c.alleles,
COUNT(c.alleles)
FROM @count c
GROUP BY c.item_id, c.experiment_id, c.alleles

  
--
-- Calculate the total number of results for each item and experiment.
--
INSERT INTO @total (item_id, experiment_id, alleles_total)
SELECT item_id, experiment_id, SUM(alleles_count)
FROM @frequencies
GROUP BY item_id, experiment_id

--
-- Calculate the fraction for each different result for each item and experiment.
--
INSERT INTO @fraction (item_id, experiment_id, alleles, fraction)
SELECT f.item_id, f.experiment_id, 
f.alleles, 
CASE WHEN t.alleles_total > 0 THEN CAST(f.alleles_count AS REAL)/CAST(t.alleles_total AS REAL) ELSE 0 END AS fraction
FROM @frequencies f
INNER JOIN @total t ON (t.item_id = f.item_id AND t.experiment_id = f.experiment_id)

--
-- Store the results which make up more than 50% of the results
-- for each item and experiment.
--
INSERT INTO @winners (item_id, experiment_id, alleles)
SELECT f.item_id, f.experiment_id, f.alleles
FROM @fraction f
WHERE f.fraction >= (CAST(@majorityPercent AS DECIMAL) / 100.0) - 0.001

--
-- Store the genotype ids for all results which have their item in the @winners table
-- but where the alleles are different from that of the winning result.
--
INSERT INTO @affected_genotype (genotype_id)
SELECT dg.genotype_id
FROM #duplicate_test dt
INNER JOIN #all_genotypes ag ON (ag.item_id = dt.item_id AND ag.experiment_id = dt.experiment_id)
INNER JOIN dbo.denorm_genotype dg ON (dg.genotype_id = ag.genotype_id AND NOT dg.alleles = 'N/A')
INNER JOIN @winners w ON w.item_id = dt.item_id AND w.experiment_id = dt.experiment_id AND NOT w.alleles COLLATE latin1_general_ci_as = dg.alleles COLLATE latin1_general_ci_as



--
-- Preliminary change the status of each affected genotype.
--
DELETE FROM #pending_status WHERE genotype_id IN (SELECT genotype_id FROM @affected_genotype)

INSERT INTO #pending_status (genotype_id, new_status_id)
SELECT genotype_id, @rejectedStatusId
FROM @affected_genotype

SET NOCOUNT OFF

END  





