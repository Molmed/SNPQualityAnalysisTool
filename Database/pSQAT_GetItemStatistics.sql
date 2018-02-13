-- Returns statistics for individual items and a certain experiment.
-- Control items are not included.

CREATE PROCEDURE pSQAT_GetItemStatistics(@experimentId INTEGER) 
AS
BEGIN
SET NOCOUNT ON

DECLARE @total_genotypes TABLE(
					item_id INTEGER PRIMARY KEY,
					number INTEGER NOT NULL
					)
					
DECLARE @no_result TABLE(
					item_id INTEGER PRIMARY KEY,
					number INTEGER NOT NULL
					)
					
DECLARE @approved TABLE(
					item_id INTEGER PRIMARY KEY,
					number INTEGER NOT NULL
					)

DECLARE @dupl_tested TABLE(
					item_id INTEGER PRIMARY KEY,
					tested INTEGER NOT NULL
					)
					
DECLARE @dupl_success TABLE(
					item_id INTEGER PRIMARY KEY,
					success INTEGER NOT NULL
					)

DECLARE @inh_tested TABLE(
					item_id INTEGER PRIMARY KEY,
					tested INTEGER NOT NULL
					)
					
DECLARE @inh_success TABLE(
					item_id INTEGER PRIMARY KEY,
					success INTEGER NOT NULL
					)


--
-- Store the total number of (selected) genotypes for each item. 
--
INSERT INTO @total_genotypes (item_id, number)
SELECT item_id, COUNT(genotype_id)
FROM #all_genotypes
WHERE experiment_id = @experimentId
GROUP BY item_id

--
-- Store the number of N/A genotypes for each item.
--
INSERT INTO @no_result (item_id, number)
SELECT ag.item_id, COUNT(ag.genotype_id)
FROM dbo.denorm_genotype dg
INNER JOIN #all_genotypes ag ON ag.genotype_id = dg.genotype_id
WHERE ag.experiment_id = @experimentId
AND dg.alleles = 'N/A'
GROUP BY ag.item_id

--
-- Store the number of approved genotypes for each item.
--
INSERT INTO @approved (item_id, number)
SELECT item_id, COUNT(genotype_id)
FROM #approved_genotype
WHERE experiment_id = @experimentId
GROUP BY item_id


--
-- Store the number of experiments (1/0) which have been duplicate tested
-- for each item.
--
INSERT INTO @dupl_tested (item_id, tested)
SELECT item_id, COUNT(experiment_id)
FROM #duplicate_test
WHERE experiment_id = @experimentId	
GROUP BY item_id

--
-- Store the number of experiments (1/0) which have been duplicate tested
-- with a successful outcome for each item.
--
INSERT INTO @dupl_success (item_id, success)
SELECT item_id, COUNT(experiment_id)
FROM #duplicate_test
WHERE experiment_id = @experimentId	
AND success = 1
GROUP BY item_id

--
-- Store the number of experiments (1/0) which have been inheritance tested
-- for each item.
--
INSERT INTO @inh_tested (item_id, tested)
SELECT item_id, COUNT(experiment_id)
FROM #inheritance_test
WHERE experiment_id = @experimentId
GROUP BY item_id

--
-- Store the number of experiments which have been inheritance tested
-- with a successful outcome for each item.
--
INSERT INTO @inh_success (item_id, success)
SELECT item_id, COUNT(experiment_id)
FROM #inheritance_test
WHERE experiment_id = @experimentId
AND success = 1
GROUP BY item_id


--
-- Put together all information and make the "tested" and "success" fields
-- have yes/no values instead of the number of experiments in them.
--
SELECT il.item_id AS [item_id],
il.identifier AS [Item],
ISNULL(tg.number, 0) AS [Total genotypes//IN],
ISNULL(nr.number, 0) AS [No result//IN],
CASE WHEN ISNULL(dt.tested, 0) > 0 THEN 'Yes' ELSE 'No' END AS [Dupl tested//CN],
CASE WHEN dt.tested IS NULL THEN '' ELSE (CASE WHEN ISNULL(dts.success, 0) > 0 THEN 'Yes' ELSE 'No' END) END AS [Dupl success//CN],
CASE WHEN ISNULL(it.tested, 0) > 0 THEN 'Yes' ELSE 'No' END AS [Inhrt tested//CN],
CASE WHEN it.tested IS NULL THEN '' ELSE (CASE WHEN ISNULL(its.success, 0) > 0 THEN 'Yes' ELSE 'No' END) END AS [Inhrt success//CN],
CASE WHEN ISNULL(a.number, 0) > 0 THEN 'Yes' ELSE 'No' END AS [Approved result//CN]						
FROM #item_stat il
LEFT OUTER JOIN @total_genotypes tg ON tg.item_id = il.item_id
LEFT OUTER JOIN @no_result nr ON nr.item_id = il.item_id
LEFT OUTER JOIN @dupl_tested dt ON dt.item_id = il.item_id
LEFT OUTER JOIN @dupl_success dts ON dts.item_id = il.item_id
LEFT OUTER JOIN @inh_tested it ON it.item_id = il.item_id
LEFT OUTER JOIN @inh_success its ON its.item_id = il.item_id
LEFT OUTER JOIN @approved a ON a.item_id = il.item_id



SET NOCOUNT OFF

END






