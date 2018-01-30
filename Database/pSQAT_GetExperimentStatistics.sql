-- Returns statistics for individual experiments and a certain item.

CREATE PROCEDURE pSQAT_GetExperimentStatistics(@itemId INTEGER) 
AS
BEGIN
SET NOCOUNT ON

DECLARE @total_genotypes TABLE(
					experiment_id INTEGER PRIMARY KEY,
					number INTEGER NOT NULL
					)
					
DECLARE @no_result TABLE(
					experiment_id INTEGER PRIMARY KEY,
					number INTEGER NOT NULL
					)
					
DECLARE @approved TABLE(
					experiment_id INTEGER PRIMARY KEY,
					number INTEGER NOT NULL
					)

DECLARE @dupl_tested TABLE(
					experiment_id INTEGER PRIMARY KEY,
					tested INTEGER NOT NULL
					)
					
DECLARE @dupl_success TABLE(
					experiment_id INTEGER PRIMARY KEY,
					success INTEGER NOT NULL
					)

DECLARE @inh_tested TABLE(
					experiment_id INTEGER PRIMARY KEY,
					tested INTEGER NOT NULL
					)
					
DECLARE @inh_success TABLE(
					experiment_id INTEGER PRIMARY KEY,
					success INTEGER NOT NULL
					)
					
DECLARE @x_tested TABLE(
					experiment_id INTEGER PRIMARY KEY,
					tested INTEGER NOT NULL
					)
					
DECLARE @x_success TABLE(
					experiment_id INTEGER PRIMARY KEY,
					success INTEGER NOT NULL
					)					



--
-- Store the total number of (selected) genotypes for each experiment. 
--
INSERT INTO @total_genotypes (experiment_id, number)
SELECT experiment_id, COUNT(genotype_id)
FROM #all_genotypes
WHERE item_id = @itemId
GROUP BY experiment_id

--
-- Store the number of N/A genotypes for each experiment.
--
INSERT INTO @no_result (experiment_id, number)
SELECT ag.experiment_id, COUNT(ag.genotype_id)
FROM dbo.denorm_genotype dg
INNER JOIN #all_genotypes ag ON ag.genotype_id = dg.genotype_id
WHERE ag.item_id = @itemId
AND dg.alleles = 'N/A'
GROUP BY ag.experiment_id

--
-- Store the number of approved genotypes for each experiment.
--
INSERT INTO @approved (experiment_id, number)
SELECT experiment_id, COUNT(genotype_id)
FROM #approved_genotype
WHERE item_id = @itemId
GROUP BY experiment_id


--
-- Store the number of items (1/0) which have been duplicate tested
-- for each experiment.
--
INSERT INTO @dupl_tested (experiment_id, tested)
SELECT experiment_id, COUNT(item_id)
FROM #duplicate_test
WHERE item_id = @itemId	
GROUP BY experiment_id

--
-- Store the number of items (1/0) which have been duplicate tested
-- with a successful outcome for each experiment.
--
INSERT INTO @dupl_success (experiment_id, success)
SELECT experiment_id, COUNT(item_id)
FROM #duplicate_test
WHERE item_id = @itemId	
AND success = 1
GROUP BY experiment_id

--
-- Store the number of items (1/0) which have been inheritance tested
-- for each experiment.
--
INSERT INTO @inh_tested (experiment_id, tested)
SELECT experiment_id, COUNT(item_id)
FROM #inheritance_test
WHERE item_id = @itemId
GROUP BY experiment_id

--
-- Store the number of items (1/0) which have been inheritance tested
-- with a successful outcome for each experiment.
--
INSERT INTO @inh_success (experiment_id, success)
SELECT experiment_id, COUNT(item_id)
FROM #inheritance_test
WHERE item_id = @itemId
AND success = 1
GROUP BY experiment_id


--
-- Store the number of items (1/0) which have been X tested.
-- for each experiment.
--
INSERT INTO @x_tested (experiment_id, tested)
SELECT experiment_id, COUNT(item_id)
FROM #het_X_male_test
WHERE item_id = @itemId
GROUP BY experiment_id


--
-- Store the number of items (1/0) which have been X tested.
-- with a successful outcome for each experiment.
--
INSERT INTO @x_success (experiment_id, success)
SELECT experiment_id, COUNT(item_id)
FROM #het_X_male_test
WHERE item_id = @itemId
AND success = 1
GROUP BY experiment_id


--
-- Put together all information and make the "tested" and "success" fields
-- have yes/no values instead of the number of genotypes in them.
--
SELECT es.experiment_id AS [experiment_id],
es.identifier AS [Experiment],
ISNULL(tg.number, 0) AS [Total genotypes//IN],
ISNULL(nr.number, 0) AS [No result//IN],
CASE WHEN ISNULL(dt.tested, 0) > 0 THEN 'Yes' ELSE 'No' END AS [Dupl tested//CN],
CASE WHEN dt.tested IS NULL THEN '' ELSE (CASE WHEN ISNULL(dts.success, 0) > 0 THEN 'Yes' ELSE 'No' END) END AS [Dupl success//CN],
CASE WHEN ISNULL(it.tested, 0) > 0 THEN 'Yes' ELSE 'No' END AS [Inhrt tested//CN],
CASE WHEN it.tested IS NULL THEN '' ELSE (CASE WHEN ISNULL(its.success, 0) > 0 THEN 'Yes' ELSE 'No' END) END AS [Inhrt success//CN],
CASE WHEN ISNULL(xt.tested, 0) > 0 THEN 'Yes' ELSE 'No' END AS [ChrX tested//CN],
CASE WHEN xt.tested IS NULL THEN '' ELSE (CASE WHEN ISNULL(xts.success, 0) > 0 THEN 'Yes' ELSE 'No' END) END AS [ChrX success//CN],
CASE WHEN ISNULL(a.number, 0) > 0 THEN 'Yes' ELSE 'No' END AS [Approved result]						
FROM #experiment_stat es
LEFT OUTER JOIN @total_genotypes tg ON tg.experiment_id = es.experiment_id
LEFT OUTER JOIN @no_result nr ON nr.experiment_id = es.experiment_id
LEFT OUTER JOIN @dupl_tested dt ON dt.experiment_id = es.experiment_id
LEFT OUTER JOIN @dupl_success dts ON dts.experiment_id = es.experiment_id
LEFT OUTER JOIN @inh_tested it ON it.experiment_id = es.experiment_id
LEFT OUTER JOIN @inh_success its ON its.experiment_id = es.experiment_id
LEFT OUTER JOIN @x_tested xt ON xt.experiment_id = es.experiment_id
LEFT OUTER JOIN @x_success xts ON xts.experiment_id = es.experiment_id
LEFT OUTER JOIN @approved a ON a.experiment_id = es.experiment_id


SET NOCOUNT OFF

END  





