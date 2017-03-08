--Returns the items which fulfill the rerun criteria specified in the arguments.

CREATE PROCEDURE pSQAT_GetRerunItems(@UnitMode VARCHAR(10), @missingGenotype BIT, @missingGenotypePercent INTEGER,
										@lowAlleleFrq BIT, @lowAlleleFrqPercent INTEGER, @duplError BIT)
										 
AS
BEGIN
SET NOCOUNT ON

DECLARE @number_exp INTEGER

DECLARE @rerun_item TABLE(
					item_id INTEGER NOT NULL
					)

DECLARE @experiment_fraction TABLE(
					item_id INTEGER PRIMARY KEY,
					fraction REAL NOT NULL
					)

DECLARE @minor_allele_frq TABLE(
					experiment_id INTEGER NOT NULL,
					allele CHAR(1) NULL,
					frq REAL NULL
					)

DECLARE @allele_count TABLE(item_id INTEGER NOT NULL,
					experiment_id INTEGER NOT NULL,
					allele CHAR(1) NOT NULL)

DECLARE @allele_group TABLE(experiment_id INTEGER NOT NULL,
					allele CHAR(1) NOT NULL,
					allele_count INTEGER NOT NULL)

DECLARE @minor_group TABLE (experiment_id INTEGER NOT NULL PRIMARY KEY,
					allele_count INTEGER NOT NULL,
					MAF REAL NOT NULL)

--
-- Get the total number of experiments.
--
SELECT @number_exp = COUNT(experiment_id) FROM #rerun_experiment

--
-- Store the fraction of experiments which have approved genotypes for each item.
--
INSERT INTO @experiment_fraction (item_id, fraction)
SELECT ist.item_id, 
CASE WHEN @number_exp > 0 THEN CAST(COUNT(DISTINCT gt.experiment_id) AS REAL) / CAST(@number_exp AS REAL) ELSE 1 END
FROM #item_stat ist
LEFT OUTER JOIN 
(SELECT ag.item_id, ag.experiment_id
FROM #approved_genotype ag
INNER JOIN #rerun_experiment re ON re.experiment_id = ag.experiment_id
) gt ON gt.item_id = ist.item_id
GROUP BY ist.item_id



--
-- Find the minor allele frequencies for all experiments using the approved results.
--

--
-- Find the used alleles for all item_id, experiment_id combinations.
--
INSERT INTO @allele_count (item_id, experiment_id, allele)
SELECT ag.item_id,
ag.experiment_id,
SUBSTRING(dg.alleles, 1, 1)
FROM dbo.denorm_genotype dg
INNER JOIN #approved_genotype ag ON ag.genotype_id = dg.genotype_id
INNER JOIN #rerun_experiment re ON re.experiment_id = ag.experiment_id
UNION ALL
SELECT ag.item_id,
ag.experiment_id,
SUBSTRING(dg.alleles, 3, 1)
FROM dbo.denorm_genotype dg
INNER JOIN #approved_genotype ag ON ag.genotype_id = dg.genotype_id
INNER JOIN #rerun_experiment re ON re.experiment_id = ag.experiment_id

--
-- Count the alleles for all experiments.
--
INSERT INTO @allele_group (experiment_id, allele, allele_count)
SELECT experiment_id, allele, COUNT(*)
FROM @allele_count
GROUP BY experiment_id, allele

--
-- Find the minor allele frequencies.
--
INSERT INTO @minor_group (experiment_id, allele_count, MAF)
SELECT experiment_id, MIN(allele_count), CAST(MIN(allele_count) AS REAL)/CAST(SUM(allele_count) AS REAL)
FROM @allele_group
GROUP BY experiment_id

--
-- Store the minor allele frequencies along with their alleles.
--
INSERT INTO @minor_allele_frq (experiment_id, allele, frq)
SELECT mg.experiment_id, agr.allele, mg.MAF
FROM @minor_group mg
	INNER JOIN @allele_group agr ON (agr.experiment_id = mg.experiment_id AND agr.allele_count = mg.allele_count)


--
-- Store items with too low experiment success rate.
--
IF @missingGenotype = 1
BEGIN
	INSERT INTO @rerun_item (item_id)
	SELECT ef.item_id
	FROM @experiment_fraction ef 
	WHERE (CAST(1.0 AS REAL) - ef.fraction) > (CAST(@missingGenotypePercent AS REAL) / 100.0) 
END

--
-- Store items having at least one allele for which the allele frequency
-- over the experiment is too low.
--
IF @lowAlleleFrq = 1
BEGIN
	INSERT INTO @rerun_item (item_id)
	SELECT ag.item_id
	FROM #approved_genotype ag
	INNER JOIN dbo.denorm_genotype dg ON dg.genotype_id = ag.genotype_id
	INNER JOIN @minor_allele_frq maf ON maf.experiment_id = ag.experiment_id
	WHERE maf.frq < (CAST (@lowAlleleFrqPercent AS REAL) / 100.0)
	AND (maf.allele COLLATE latin1_general_ci_as = LEFT(dg.alleles, 1) COLLATE latin1_general_ci_as
		OR maf.allele COLLATE latin1_general_ci_as = RIGHT(dg.alleles, 1) COLLATE latin1_general_ci_as
		)		
END

--
-- Store items with duplicate errors.
--
IF @duplError = 1
BEGIN
	INSERT INTO @rerun_item (item_id)
	SELECT dt.item_id
	FROM #duplicate_test dt
	INNER JOIN #rerun_experiment re ON re.experiment_id = dt.experiment_id
	WHERE dt.success = 0
END

--
-- Return data.
--
SELECT DISTINCT item_id FROM @rerun_item


SET NOCOUNT OFF

END 
