--Returns information about the selected plate, but only for the subset of data which
--has been previously selected (i.e. genotypes listed in the #all_genotypes table).
--Note that controls are excluded from the experiment genotyping success rate calculation.

CREATE PROCEDURE pSQAT_GetPlateContents(@plateId INTEGER) 
AS
BEGIN
SET NOCOUNT ON

DECLARE @i INTEGER

DECLARE @selected_genotype TABLE(
				genotype_id INTEGER PRIMARY KEY,
				item_id INTEGER NOT NULL,
				experiment_id INTEGER NOT NULL,
				alleles CHAR(3) NOT NULL,
				pos_x TINYINT NULL,
				pos_y TINYINT NULL
				)
				
DECLARE @experiment_success TABLE(
				experiment_id INTEGER PRIMARY KEY,
				genotypes INTEGER NOT NULL
				)
				
DECLARE @experiment_total TABLE(
				experiment_id INTEGER PRIMARY KEY,
				genotypes INTEGER NOT NULL
				)
				
DECLARE @all_experiments TABLE(
				experiment_id INTEGER PRIMARY KEY
				)
				
DECLARE @item_success TABLE(
				item_id INTEGER PRIMARY KEY,
				genotypes INTEGER NOT NULL
				)
				
DECLARE @item_total TABLE(
				item_id INTEGER PRIMARY KEY,
				genotypes INTEGER NOT NULL
				)

DECLARE @well_success TABLE(
				pos_x INTEGER NOT NULL,
				pos_y INTEGER NOT NULL,
				genotypes INTEGER NOT NULL,
				PRIMARY KEY (pos_x, pos_y)
				)
				
DECLARE @well_total TABLE(
				pos_x INTEGER NOT NULL,
				pos_y INTEGER NOT NULL,
				genotypes INTEGER NOT NULL,
				PRIMARY KEY (pos_x, pos_y)
				)
							
DECLARE @plate_position TABLE(  --Used positions.
				pos_x INTEGER NOT NULL,
				pos_y INTEGER NOT NULL,
				PRIMARY KEY (pos_x, pos_y)
				)
				
DECLARE @plate_experiment TABLE(
				experiment_id INTEGER PRIMARY KEY,
				[Experiment//CN] VARCHAR(255) NOT NULL,
				[Genotyping success excluding controls//PN] REAL NOT NULL
				)

DECLARE @plate_item TABLE(
				item_id INTEGER PRIMARY KEY,
				[Item//CN] VARCHAR(255) NOT NULL,
				[Genotyping success excluding controls//PN] REAL NOT NULL
				)

DECLARE @plate_well TABLE(
				well_id VARCHAR(10) NOT NULL PRIMARY KEY, -- Store the ID of a well as (pos_x, pos_y), e.g. "3,10".
				[Well//CN] CHAR(3) NOT NULL UNIQUE,
				[Genotyping success excluding controls//PN] REAL NOT NULL
				)

--
-- Get included genotypes.
--
INSERT INTO @selected_genotype (genotype_id, item_id, experiment_id, alleles, pos_x, pos_y)
SELECT dg.genotype_id, ag.item_id, ag.experiment_id, dg.alleles, dg.pos_x, dg.pos_y
FROM dbo.denorm_genotype dg
INNER JOIN #all_genotypes ag ON ag.genotype_id = dg.genotype_id
WHERE dg.plate_id = @plateId


--
-- Get used plate positions.
--
INSERT INTO @plate_position (pos_x, pos_y)
SELECT DISTINCT sg.pos_x, sg.pos_y
FROM @selected_genotype sg

--
-- Count successful results for all experiments among selected results except controls.
--
INSERT INTO @experiment_success (experiment_id, genotypes)
SELECT experiment_id, COUNT(*)
FROM @selected_genotype
WHERE NOT alleles = 'N/A'
AND NOT item_id IN (SELECT item_id FROM #control_item)
GROUP BY experiment_id

--
-- Count total results for all experiments among selected results except controls.
--
INSERT INTO @experiment_total (experiment_id, genotypes)
SELECT experiment_id, COUNT(*)
FROM @selected_genotype
WHERE NOT item_id IN (SELECT item_id FROM #control_item)
GROUP BY experiment_id

--
-- Get all used experiments, including those run only on controls.
--
INSERT INTO @all_experiments (experiment_id)
SELECT DISTINCT experiment_id
FROM @selected_genotype

--
-- Count successful results for all items except controls among selected results.
--
INSERT INTO @item_success (item_id, genotypes)
SELECT item_id, COUNT(*)
FROM @selected_genotype
WHERE NOT alleles = 'N/A'
AND NOT item_id IN (SELECT item_id FROM #control_item)
GROUP BY item_id

--
-- Count total results for all items except controls among selected results.
--
INSERT INTO @item_total (item_id, genotypes)
SELECT item_id, COUNT(*)
FROM @selected_genotype
WHERE NOT item_id IN (SELECT item_id FROM #control_item)
GROUP BY item_id

--
-- Count successful results for all wells among selected results, excluding control genotypes.
--
INSERT INTO @well_success (pos_x, pos_y, genotypes)
SELECT pos_x, pos_y, COUNT(*)
FROM @selected_genotype
WHERE NOT alleles = 'N/A'
AND NOT item_id IN (SELECT item_id FROM #control_item)
GROUP BY pos_x, pos_y

--
-- Count total results for all wells controls among selected results, excluding control genotypes.
--
INSERT INTO @well_total (pos_x, pos_y, genotypes)
SELECT pos_x, pos_y, COUNT(*)
FROM @selected_genotype
WHERE NOT item_id IN (SELECT item_id FROM #control_item)
GROUP BY pos_x, pos_y

--
-- Store experiment information for these genotypes.
--
INSERT INTO @plate_experiment (experiment_id, [Experiment//CN], [Genotyping success excluding controls//PN])
SELECT es.experiment_id, es.identifier, 
CASE WHEN ISNULL(et.genotypes, 0) > 0 THEN CAST(ISNULL(sc.genotypes, 0) AS REAL) / CAST(et.genotypes AS REAL) ELSE 0 END
FROM @all_experiments ae
LEFT OUTER JOIN @experiment_success sc ON sc.experiment_id = ae.experiment_id
LEFT OUTER JOIN @experiment_total et ON et.experiment_id = ae.experiment_id
INNER JOIN #experiment_stat es ON es.experiment_id = ae.experiment_id


--
-- Store item information for these genotypes.
--
INSERT INTO @plate_item (item_id, [Item//CN], [Genotyping success excluding controls//PN])
SELECT it.item_id, ist.identifier, 
CASE WHEN it.genotypes > 0 THEN CAST(ISNULL(sc.genotypes, 0) AS REAL) / CAST(it.genotypes AS REAL) ELSE 0 END
FROM @item_total it
LEFT OUTER JOIN @item_success sc ON sc.item_id = it.item_id
INNER JOIN #item_stat ist ON ist.item_id = it.item_id


--
-- Store well information for these genotypes.
--
INSERT INTO @plate_well (well_id, [Well//CN], [Genotyping success excluding controls//PN])
SELECT CAST(wt.pos_x AS VARCHAR(10)) + ',' + CAST(wt.pos_y AS VARCHAR(10)), dbo.fTranslateCoord(wt.pos_x, wt.pos_y), 
CASE WHEN wt.genotypes > 0 THEN CAST(ISNULL(sc.genotypes, 0) AS REAL) / CAST(wt.genotypes AS REAL) ELSE 0 END
FROM @well_total wt
LEFT OUTER JOIN @well_success sc ON (sc.pos_x = wt.pos_x AND sc.pos_y = wt.pos_y)


--
-- Return the data.
--
SELECT * FROM @plate_position
				
SELECT * FROM @plate_item ORDER BY [Item//CN]
				
SELECT * FROM @plate_experiment ORDER BY [Experiment//CN]

SELECT * FROM @plate_well ORDER BY [Well//CN]

SET NOCOUNT OFF

END 
