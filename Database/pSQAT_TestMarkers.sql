-- Performs all quality tests and return the statistics.

CREATE PROCEDURE pSQAT_TestMarkers(@Initiate BIT,
									@SelCmd VARCHAR(3000),
									@ItemMode VARCHAR(10),
									@ExperimentMode VARCHAR(10), 
									@doInheritanceTest BIT, 
									@detectX BIT, 
									@demandDuplicates BIT,
									@doHWTest BIT, 
									@HWSkipChildren BIT,  
									@HWChi2Limit REAL)
AS
BEGIN
SET NOCOUNT ON


------------------------------------------------- Input ------------------------------------------
--
-- Saved for debugging purposes.
--

-- DECLARE @Initiate BIT
-- DECLARE @SelCmd VARCHAR(3000)
-- DECLARE @ItemMode VARCHAR(10)
-- DECLARE @ExperimentMode VARCHAR(10) 
-- DECLARE @doInheritanceTest BIT
-- DECLARE @detectX BIT
-- DECLARE @demandDuplicates BIT
-- DECLARE @doHWTest BIT 
-- DECLARE @HWSkipChildren BIT  
-- DECLARE @HWChi2Limit REAL

-- SET @Initiate = 1
-- SET @SelCmd = 'SELECT dg.genotype_id,
-- 				dg.individual_id AS item_id,
-- 				dg.marker_id AS experiment_id,
-- 				dg.alleles,
-- 				dg.status_id
-- 				FROM dbo.denorm_genotype dg
-- 				WHERE 1 = 1
-- 				AND dg.plate_id IN (SELECT plate_id FROM #plate)'
-- SET @ItemMode = 'SUBJECT'
-- SET @ExperimentMode = 'MARKER' 
-- SET @doInheritanceTest = 1
-- SET @detectX = 0
-- SET @demandDuplicates = 0
-- SET @doHWTest = 0 
-- SET @HWSkipChildren = 0  
-- SET @HWChi2Limit = 4




------------------------------------------------- Declarations -----------------------------------
DECLARE @InsertCmd VARCHAR(3000)
DECLARE @valid_status TABLE(status_id TINYINT NOT NULL PRIMARY KEY)
DECLARE @maleSexId TINYINT
DECLARE @femaleSexId TINYINT
DECLARE @unknownSexId TINYINT
DECLARE @notApplicableSexId TINYINT
DECLARE @approvedStatusId TINYINT
DECLARE @loadStatusId TINYINT
DECLARE @rejectedStatusId TINYINT
DECLARE @blankControlType VARCHAR(255)
DECLARE @inhControlType VARCHAR(255)
DECLARE @homozygControlType VARCHAR(255)


DECLARE @blank_control_item TABLE(item_id INTEGER NOT NULL PRIMARY KEY)
DECLARE @inh_control_item TABLE(item_id INTEGER NOT NULL PRIMARY KEY)
DECLARE @homozyg_control_item TABLE(item_id INTEGER NOT NULL PRIMARY KEY)


DECLARE @selected_experiment TABLE(
				experiment_id	INTEGER NOT NULL PRIMARY KEY,
				identifier		VARCHAR(255) NOT NULL UNIQUE,			
				chrom_name		VARCHAR(20) NULL
				)

DECLARE @selected_item TABLE(
				item_id			INTEGER NOT NULL PRIMARY KEY,
				identifier		VARCHAR(255) NOT NULL UNIQUE,
				sex_id			TINYINT NOT NULL,
				father_id		INTEGER NULL,
				mother_id		INTEGER NULL
				)
				
DECLARE @zygosity_count_preparation TABLE(
				experiment_id	INTEGER NOT NULL,
				alleles			CHAR(3) NOT NULL,
				number			INTEGER NOT NULL,
				PRIMARY KEY (experiment_id, alleles)
				)

DECLARE @zygosity_count_preparation_corrected TABLE(
				experiment_id	INTEGER NOT NULL,
				alleles			CHAR(3) NOT NULL,
				number			INTEGER NOT NULL,
				PRIMARY KEY (experiment_id, alleles)
				)

DECLARE @zygosity_count TABLE(
				experiment_id	INTEGER NOT NULL PRIMARY KEY,
				identifier		VARCHAR(255) NOT NULL,
				XX				INTEGER NOT NULL,
				XY				INTEGER NOT NULL,
				YY				INTEGER NOT NULL,
				XX_alleles		CHAR(3) NOT NULL,
				XY_alleles		CHAR(3) NOT NULL,
				YY_alleles		CHAR(3) NOT NULL,
				X				INTEGER NOT NULL,
				Y				INTEGER NOT NULL,
				exp_XX			REAL NOT NULL,
				exp_XY			REAL NOT NULL,
				exp_YY			REAL NOT NULL
				)

DECLARE @zygosity_count_corrected TABLE(
				experiment_id	INTEGER NOT NULL PRIMARY KEY,
				XX				INTEGER NOT NULL,
				XY				INTEGER NOT NULL,
				YY				INTEGER NOT NULL,
				XX_alleles		CHAR(3) NOT NULL,
				XY_alleles		CHAR(3) NOT NULL,
				YY_alleles		CHAR(3) NOT NULL,
				exp_XX			REAL NOT NULL,
				exp_XY			REAL NOT NULL,
				exp_YY			REAL NOT NULL,
				chi2			REAL NULL
				)
				
DECLARE @run_items TABLE(
				experiment_id	INTEGER NOT NULL PRIMARY KEY,
				nr_items		INTEGER NOT NULL
				)				

DECLARE @dupl_stat TABLE(
				experiment_id	INTEGER NOT NULL PRIMARY KEY,
				failed_items		INTEGER NOT NULL,
				tested_items		INTEGER NOT NULL
				)

DECLARE @inh_stat TABLE(
				experiment_id	INTEGER NOT NULL PRIMARY KEY,
				failed_items		INTEGER NOT NULL,
				tested_items		INTEGER NOT NULL
				)

DECLARE @inh_control_stat TABLE(
				experiment_id	INTEGER NOT NULL PRIMARY KEY,
				failed_items		INTEGER NOT NULL,
				tested_items		INTEGER NOT NULL
				)

DECLARE @rejected_stat TABLE(
				experiment_id	INTEGER NOT NULL PRIMARY KEY,
				rejected_gt		INTEGER NOT NULL
				)

DECLARE @blank_control_stat TABLE(
				experiment_id	INTEGER NOT NULL PRIMARY KEY,
				failed_items		INTEGER NOT NULL,
				tested_items		INTEGER NOT NULL
				)

DECLARE @homozyg_control_stat TABLE(
				experiment_id	INTEGER NOT NULL PRIMARY KEY,
				failed_items		INTEGER NOT NULL,
				tested_items		INTEGER NOT NULL
				)

DECLARE @consensus_genotype TABLE( 
            item_id INTEGER NOT NULL, 
            experiment_id INTEGER NOT NULL, 
            genotype_id INTEGER NULL, 
            alleles CHAR(3) NULL, 
            duplicate_tested BIT NOT NULL, 
            PRIMARY KEY (item_id, experiment_id) )

DECLARE @inheritance_test TABLE( 
            item_id INTEGER NOT NULL, 
            experiment_id INTEGER NOT NULL, 
            success BIT NOT NULL, 
            PRIMARY KEY (item_id, experiment_id)) 

DECLARE @inh_failed_fam TABLE (
				experiment_id INTEGER NOT NULL,
				father_id INTEGER NOT NULL,
				mother_id INTEGER NOT NULL,
				PRIMARY KEY (experiment_id, father_id, mother_id))
	
DECLARE @children_in_failed_fam TABLE (
				item_id INTEGER NOT NULL,
				experiment_id INTEGER NOT NULL,
				PRIMARY KEY (item_id, experiment_id))

DECLARE @het_X_male_test TABLE( 
			item_id INTEGER NOT NULL, 
            experiment_id INTEGER NOT NULL, 
            success BIT NOT NULL, 
            PRIMARY KEY(item_id, experiment_id)) 

DECLARE @blank_test_preparation TABLE( 
            item_id INTEGER NOT NULL, 
            experiment_id INTEGER NOT NULL, 
            success BIT NOT NULL)   --No primary key since uses all available genotypes, not only consensus. 

DECLARE @blank_test TABLE( 
            item_id INTEGER NOT NULL, 
            experiment_id INTEGER NOT NULL, 
            success BIT NOT NULL,
            PRIMARY KEY(item_id, experiment_id))

DECLARE @homozygote_test_preparation TABLE( 
            item_id INTEGER NOT NULL, 
            experiment_id INTEGER NOT NULL, 
            success BIT NOT NULL)   --No primary key since uses all available genotypes, not only consensus. 

DECLARE @homozygote_test TABLE( 
            item_id INTEGER NOT NULL, 
            experiment_id INTEGER NOT NULL, 
            success BIT NOT NULL,
            PRIMARY KEY(item_id, experiment_id))



CREATE TABLE #result(
genotype_id INTEGER PRIMARY KEY,
item_id INTEGER NOT NULL,
experiment_id INTEGER NOT NULL,
alleles CHAR(3) NOT NULL,
status_id TINYINT NOT NULL
)

CREATE TABLE #inheritance_case ( 
			item_id INTEGER NOT NULL, 
            experiment_id INTEGER NOT NULL, 
            alleles CHAR(3) NOT NULL, 
            sex_id TINYINT NOT NULL, 
            chrom_name VARCHAR(20) NULL, 
            father_id INTEGER NULL, 
            mother_id INTEGER NULL, 
            father_alleles CHAR(3) NULL, 
            mother_alleles CHAR(3) NULL, 
            success BIT NOT NULL, 
            PRIMARY KEY (item_id, experiment_id)) 
	

--------------------------------------------- Clear tables on first round --------------------------------------

IF @Initiate = 1
BEGIN
	TRUNCATE TABLE #all_genotypes
	TRUNCATE TABLE #experiment_stat
	TRUNCATE TABLE #item_stat
	TRUNCATE TABLE #control_item_stat
	TRUNCATE TABLE #approved_genotype
	TRUNCATE TABLE #experiment_count
	TRUNCATE TABLE #experiment_count_nonfailed
	TRUNCATE TABLE #duplicate_test
	TRUNCATE TABLE #inheritance_test
	TRUNCATE TABLE #inheritance_trio
	TRUNCATE TABLE #het_X_male_test
	TRUNCATE TABLE #blank_test
	TRUNCATE TABLE #homozygote_test
	TRUNCATE TABLE #control_item
END

---------------------------------------------- Definitions ---------------------------------------------
SET @approvedStatusId = 0
SET @loadStatusId = 1
SET @rejectedStatusId = 2
INSERT INTO @valid_status (status_id) VALUES (@approvedStatusId)
INSERT INTO @valid_status (status_id) VALUES (@loadStatusId)
SET @unknownSexId = 0
SET @maleSexId = 1
SET @femaleSexId = 2
SET @notApplicableSexId = 3
SET @blankControlType = 'BlankControl'
SET @inhControlType = 'InheritanceControl'
SET @homozygControlType = 'HomozygoteControl'



--
-- Read genotype data.
--

SET @InsertCmd = 'INSERT INTO #result (genotype_id, item_id, experiment_id, alleles, status_id) ' + @SelCmd

EXEC (@InsertCmd)

CREATE NONCLUSTERED INDEX result_idx_item_id ON #result(item_id) WITH FILLFACTOR = 100
CREATE NONCLUSTERED INDEX result_idx_experiment_id ON #result(experiment_id) WITH FILLFACTOR = 100
CREATE NONCLUSTERED INDEX result_idx_alleles_id ON #result(alleles) WITH FILLFACTOR = 100
CREATE NONCLUSTERED INDEX result_idx_status_id ON #result(status_id) WITH FILLFACTOR = 100


--
-- Get information for selected items.
--
IF @ItemMode = 'SAMPLE'
BEGIN
	INSERT INTO @selected_item (item_id, identifier, sex_id, father_id, mother_id)
	SELECT smp.sample_id, smp.identifier, idv.sex_id, idv.father_id, idv.mother_id 
	FROM dbo.sample smp
	INNER JOIN dbo.individual idv ON idv.individual_id = smp.individual_id
	WHERE smp.sample_id IN (SELECT item_id FROM #result)
END
ELSE
BEGIN
	INSERT INTO @selected_item (item_id, identifier, sex_id, father_id, mother_id)
	SELECT idv.individual_id, idv.identifier, idv.sex_id, idv.father_id, idv.mother_id 
	FROM dbo.individual idv
	WHERE idv.individual_id IN (SELECT item_id FROM #result)
END


--
-- Get information for selected experiments.
--
IF @ExperimentMode = 'ASSAY'
BEGIN
	INSERT INTO @selected_experiment (experiment_id, identifier, chrom_name)
	SELECT a.assay_id, a.identifier, m.chrom_name 
	FROM dbo.assay a
	INNER JOIN dbo.marker m ON m.marker_id = a.marker_id
	WHERE a.assay_id IN (SELECT experiment_id FROM #result)
END
ELSE
BEGIN
	INSERT INTO @selected_experiment (experiment_id, identifier, chrom_name)
	SELECT m.marker_id, m.identifier, m.chrom_name 
	FROM dbo.marker m
	WHERE m.marker_id IN (SELECT experiment_id FROM #result)
END


--
-- Get control items.
--
IF @ItemMode = 'SAMPLE'
BEGIN
	INSERT INTO @blank_control_item (item_id)
	SELECT smp.sample_id
	FROM dbo.sample smp
	INNER JOIN dbo.individual idv ON idv.individual_id = smp.individual_id
	INNER JOIN dbo.individual_type idt ON idt.individual_type_id = idv.individual_type_id
	WHERE idt.name = @blankControlType
	
	INSERT INTO @homozyg_control_item (item_id)
	SELECT smp.sample_id
	FROM dbo.sample smp
	INNER JOIN dbo.individual idv ON idv.individual_id = smp.individual_id
	INNER JOIN dbo.individual_type idt ON idt.individual_type_id = idv.individual_type_id
	WHERE idt.name = @homozygControlType
	
	INSERT INTO @inh_control_item (item_id)
	SELECT smp.sample_id
	FROM dbo.sample smp
	INNER JOIN dbo.individual idv ON idv.individual_id = smp.individual_id
	INNER JOIN dbo.individual_type idt ON idt.individual_type_id = idv.individual_type_id
	WHERE idt.name = @inhControlType
END	
ELSE
BEGIN
	INSERT INTO @blank_control_item (item_id)
	SELECT idv.individual_id
	FROM dbo.individual idv
	INNER JOIN dbo.individual_type idt ON idt.individual_type_id = idv.individual_type_id
	WHERE idt.name = @blankControlType
	
	INSERT INTO @homozyg_control_item (item_id)
	SELECT idv.individual_id
	FROM dbo.individual idv
	INNER JOIN dbo.individual_type idt ON idt.individual_type_id = idv.individual_type_id
	WHERE idt.name = @homozygControlType
	
	INSERT INTO @inh_control_item (item_id)
	SELECT idv.individual_id
	FROM dbo.individual idv
	INNER JOIN dbo.individual_type idt ON idt.individual_type_id = idv.individual_type_id
	WHERE idt.name = @inhControlType
END


IF @Initiate = 1
BEGIN
	INSERT INTO #control_item (item_id)
	SELECT item_id FROM @blank_control_item
	UNION
	SELECT item_id FROM @homozyg_control_item
	UNION
	SELECT item_id FROM @inh_control_item
END

-------------------------------- Prepare the item statistics list  --------------------------------

--
-- Add new items to the item statistics list.
--
INSERT INTO #item_stat (
		item_id,
		identifier,
		success_experiments,
		total_gt,
		no_result_gt,
		failed_dup_exp,
		failed_inh_exp,
		failed_HW_exp,
		rejected_gt,
		chrX_warning_exp
		)
SELECT	item_id,
		identifier,
		0,
		0,
		0,
		0,
		CASE WHEN @ItemMode = 'SUBJECT' THEN 0 ELSE NULL END,
		CASE WHEN @doHWTest = 1 THEN 0 ELSE NULL END,
		0,
		CASE WHEN @detectX = 1 THEN 0 ELSE NULL END
FROM @selected_item
WHERE item_id NOT IN (SELECT item_id FROM #control_item)
AND item_id NOT IN (SELECT item_id FROM #item_stat)		

--
-- Add new items to the control item statistics list.
--
INSERT INTO #control_item_stat (item_id, identifier, failed_exp, tested_exp)
(SELECT si.item_id, si.identifier, 0, 0 
FROM @blank_control_item bci
INNER JOIN @selected_item si ON si.item_id = bci.item_id
WHERE si.item_id NOT IN (SELECT item_id FROM #control_item_stat)
)
UNION
(SELECT si.item_id, si.identifier, 0, 0 
FROM @homozyg_control_item hci
INNER JOIN @selected_item si ON si.item_id = hci.item_id
WHERE si.item_id NOT IN (SELECT item_id FROM #control_item_stat)	
)
UNION
(SELECT si.item_id, si.identifier, CASE WHEN @ItemMode = 'SUBJECT' THEN 0 ELSE NULL END, CASE WHEN @ItemMode = 'SUBJECT' THEN 0 ELSE NULL END 
FROM @inh_control_item ici
INNER JOIN @selected_item si ON si.item_id = ici.item_id
WHERE si.item_id NOT IN (SELECT item_id FROM #control_item_stat)	
)	

--
-- Add new items to the experiment counter.
--
INSERT INTO #experiment_count (item_id, run_experiments)
SELECT si.item_id, 0
FROM @selected_item si
WHERE si.item_id NOT IN (SELECT item_id FROM #control_item)
AND si.item_id NOT IN (SELECT item_id FROM #experiment_count)

--
-- Add new items to the experiment counter for experiments with at least one nonfailed result.
--
INSERT INTO #experiment_count_nonfailed (item_id, run_experiments)
SELECT si.item_id, 0
FROM @selected_item si
WHERE si.item_id NOT IN (SELECT item_id FROM #control_item)
AND si.item_id NOT IN (SELECT item_id FROM #experiment_count_nonfailed)										




--------------------------------- Duplicate test ---------------------------------------------------------------

--
-- Perform duplicate test and extract consensus genotypes.
--
INSERT INTO @consensus_genotype (item_id, experiment_id, genotype_id, duplicate_tested)
SELECT item_id, experiment_id,
CASE WHEN (COUNT(DISTINCT alleles) > 1) OR (@demandDuplicates = 1 AND COUNT(alleles) < 2) THEN NULL ELSE MAX(genotype_id) END AS consensus_genotype_id,
CASE WHEN COUNT(alleles) > 1 THEN 1 ELSE 0 END AS duplicate_tested
FROM #result
WHERE status_id IN (SELECT status_id FROM @valid_status) AND NOT alleles = 'N/A'
AND item_id NOT IN (SELECT item_id FROM @blank_control_item)
AND item_id NOT IN (SELECT item_id FROM @homozyg_control_item)
GROUP BY item_id, experiment_id


--
-- Fill out the alleles of the consensus genotypes so that they can be used easily later.
--
UPDATE @consensus_genotype 
SET alleles = r.alleles
FROM @consensus_genotype cg, #result r
WHERE cg.genotype_id = r.genotype_id


--------------------------------- Inheritance test ---------------------------------------------------------------


IF @ItemMode = 'SUBJECT' AND @doInheritanceTest = 1
BEGIN
  	--
	-- Store the cases which will be inheritance tested.
	--
	INSERT INTO #inheritance_case(
				item_id,
				experiment_id,
				alleles,
				sex_id,
				chrom_name,
				father_id,
				mother_id,
				father_alleles,
				mother_alleles,
				success
				)			
	SELECT cg.item_id, cg.experiment_id, cg.alleles, si.sex_id, se.chrom_name, si.father_id, si.mother_id, cgf.alleles, cgm.alleles, 0
	FROM @consensus_genotype cg
	INNER JOIN @selected_experiment se ON se.experiment_id = cg.experiment_id
	INNER JOIN @selected_item si ON si.item_id = cg.item_id
	LEFT OUTER JOIN @consensus_genotype cgf ON (cgf.item_id = si.father_id AND cgf.experiment_id = cg.experiment_id)
	LEFT OUTER JOIN @consensus_genotype cgm ON (cgm.item_id = si.mother_id AND cgm.experiment_id = cg.experiment_id)	
	WHERE (NOT cg.alleles IS NULL)  -- Cannot test if the child is NULL 
	AND NOT (cgf.alleles IS NULL AND cgm.alleles IS NULL) -- Cannot test if both parents are NULL
	AND NOT (ISNULL(se.chrom_name, '') = 'X' AND cgm.alleles IS NULL AND si.sex_id = @maleSexId AND @detectX = 1)  -- Cannot test on human X chrom if child is male and mother is NULL.			

	CREATE NONCLUSTERED INDEX inheritance_case_idx_experiment_id_father_id ON #inheritance_case(experiment_id, father_id) WITH FILLFACTOR = 100	
	
	CREATE NONCLUSTERED INDEX inheritance_case_idx_experiment_id_mother_id ON #inheritance_case(experiment_id, mother_id) WITH FILLFACTOR = 100


	--
	-- Set success for the triplets where the child's alleles are possible according to the signature.
	--
	IF @detectX = 1
	BEGIN
		UPDATE #inheritance_case
		SET success = 1
		WHERE 
		--Ordinary case, not known to be on X or not a male child.
		(	(
		((SUBSTRING(alleles, 1, 1) = SUBSTRING(father_alleles, 1, 1) OR SUBSTRING(alleles, 1, 1) = SUBSTRING(father_alleles, 3, 1) OR father_alleles IS NULL)
		AND
		(SUBSTRING(alleles, 3, 1) = SUBSTRING(mother_alleles, 1, 1) OR SUBSTRING(alleles, 3, 1) = SUBSTRING(mother_alleles, 3, 1) OR mother_alleles IS NULL)
		)
		OR
		(
		(SUBSTRING(alleles, 1, 1) = SUBSTRING(mother_alleles, 1, 1) OR SUBSTRING(alleles, 1, 1) = SUBSTRING(mother_alleles, 3, 1) OR mother_alleles IS NULL)
		AND
		(SUBSTRING(alleles, 3, 1) = SUBSTRING(father_alleles, 1, 1) OR SUBSTRING(alleles, 3, 1) = SUBSTRING(father_alleles, 3, 1) OR father_alleles IS NULL))
		) AND (NOT chrom_name = 'X' OR NOT sex_id = @maleSexId OR chrom_name IS NULL))
		OR
		--Male child, X chromosome. Child MUST be homozygote with either of the mother's alleles.		
		(	(
		(SUBSTRING(alleles, 1, 1) = SUBSTRING(mother_alleles, 1, 1) AND SUBSTRING(alleles, 3, 1) = SUBSTRING(mother_alleles, 1, 1))
		OR
		(SUBSTRING(alleles, 1, 1) = SUBSTRING(mother_alleles, 3, 1) AND SUBSTRING(alleles, 3, 1) = SUBSTRING(mother_alleles, 3, 1))
		) AND chrom_name = 'X' AND sex_id = @maleSexId)
		OR
		--Unknown child sex, X chromosome. Child CAN be homozygote with either of the mother's alleles.		
		(	(
		(SUBSTRING(alleles, 1, 1) = SUBSTRING(mother_alleles, 1, 1) AND SUBSTRING(alleles, 3, 1) = SUBSTRING(mother_alleles, 1, 1))
		OR
		(SUBSTRING(alleles, 1, 1) = SUBSTRING(mother_alleles, 3, 1) AND SUBSTRING(alleles, 3, 1) = SUBSTRING(mother_alleles, 3, 1))
		) AND chrom_name = 'X' AND (sex_id = @unknownSexId OR sex_id = @notApplicableSexId))
	END
	ELSE
	BEGIN
		--Ordinary case.
		UPDATE #inheritance_case
		SET success = 1 WHERE 	(
		((SUBSTRING(alleles, 1, 1) = SUBSTRING(father_alleles, 1, 1) OR SUBSTRING(alleles, 1, 1) = SUBSTRING(father_alleles, 3, 1) OR father_alleles IS NULL)
		AND
		(SUBSTRING(alleles, 3, 1) = SUBSTRING(mother_alleles, 1, 1) OR SUBSTRING(alleles, 3, 1) = SUBSTRING(mother_alleles, 3, 1) OR mother_alleles IS NULL)
		)
		OR
		(
		(SUBSTRING(alleles, 1, 1) = SUBSTRING(mother_alleles, 1, 1) OR SUBSTRING(alleles, 1, 1) = SUBSTRING(mother_alleles, 3, 1) OR mother_alleles IS NULL)
		AND
		(SUBSTRING(alleles, 3, 1) = SUBSTRING(father_alleles, 1, 1) OR SUBSTRING(alleles, 3, 1) = SUBSTRING(father_alleles, 3, 1) OR father_alleles IS NULL))
		)
	END

	
	--
	-- Store information about the inheritance test outcome for each trio/duo and experiment.
	-- (Cannot use the #inheritance_case table as "global" table because need to create
	-- indexes on that table in each round). 
	--
	INSERT INTO #inheritance_trio (father_id, mother_id, child_id, experiment_id, success)
	SELECT father_id, mother_id, item_id, experiment_id, success
	FROM #inheritance_case
	
	--
	-- Make a table of all items participating in the inheritance test (children, fathers, and mothers).
	--
	INSERT INTO @inheritance_test (item_id, experiment_id, success)
	SELECT item_id, experiment_id, 1
	FROM #inheritance_case
	UNION
	SELECT father_id, experiment_id, 1
	FROM #inheritance_case
	WHERE NOT father_id IS NULL
	UNION
	SELECT mother_id, experiment_id, 1
	FROM #inheritance_case
	WHERE NOT mother_id IS NULL

	--
	-- Set failure for children in failed test cases.
	--
	UPDATE @inheritance_test
	SET success = 0
	FROM @inheritance_test it
	INNER JOIN #inheritance_case ic ON (ic.item_id = it.item_id AND ic.experiment_id = it.experiment_id AND ic.success = 0)

	--
	-- Set failure for fathers in failed test cases.
	--
	UPDATE @inheritance_test
	SET success = 0
	FROM @inheritance_test it
	INNER JOIN #inheritance_case ic ON (ic.father_id = it.item_id AND ic.experiment_id = it.experiment_id AND ic.success = 0)	
	
	--
	-- Set failure for mothers in failed test cases.
	--
	UPDATE @inheritance_test
	SET success = 0
	FROM @inheritance_test it
	INNER JOIN #inheritance_case ic ON (ic.mother_id = it.item_id AND ic.experiment_id = it.experiment_id AND ic.success = 0)
	
	--
	-- Store fathers and mothers for failed test cases, where both the father and mother are known.
	--								
	INSERT INTO @inh_failed_fam (experiment_id, father_id, mother_id)
	SELECT DISTINCT experiment_id, father_id, mother_id
	FROM #inheritance_case
	WHERE success = 0
	AND NOT father_id IS NULL AND NOT mother_id IS NULL
	
	--
	-- Store all children for failed families (fathers and mothers).
	--
	INSERT INTO @children_in_failed_fam (item_id, experiment_id)
	SELECT ic.item_id, ic.experiment_id
	FROM #inheritance_case ic
	INNER JOIN @inh_failed_fam ff ON (ff.experiment_id = ic.experiment_id
	AND ff.father_id = ic.father_id AND ff.mother_id = ic.mother_id)
	
	--
	-- Set failure for all children in failed families.
	-- (Altogether, all family members in families with known fathers and mothers are failed,
	-- and the child and parent in failed test cases where either of the parents are unknown).
	--
	UPDATE @inheritance_test
	SET success = 0
	FROM @inheritance_test it
	INNER JOIN @children_in_failed_fam cff ON (cff.item_id = it.item_id AND cff.experiment_id = it.experiment_id)




END


---------------------------- Frequency and Hardy-Weinberg calculations  -----------------------------------------

--
-- Count the number of different genotypes for each experiment.
--			
INSERT INTO @zygosity_count_preparation (experiment_id, alleles, number)
SELECT cg.experiment_id, cg.alleles, COUNT(cg.alleles)
FROM @consensus_genotype cg
LEFT OUTER JOIN @inheritance_test it ON (it.experiment_id = cg.experiment_id AND it.item_id = cg.item_id AND it.success = 0)
WHERE NOT cg.alleles IS NULL
AND it.item_id IS NULL  --Remove items failed in the inheritance test.
AND cg.item_id NOT IN (SELECT item_id FROM #control_item)
GROUP BY cg.experiment_id, cg.alleles			

--
-- Make sure there is no experiment with more than three different genotypes
--
IF EXISTS(SELECT COUNT(*) FROM @zygosity_count_preparation GROUP BY experiment_id HAVING COUNT(*) > 3)
BEGIN
	RAISERROR('At least one experiment has more than three kinds of genotypes.', 11, 1)
	RETURN
END


--
-- Create a table with the count and expected count of the different genotypes with one row per experiment.
--
INSERT INTO @zygosity_count (experiment_id, identifier, XX, XY, YY, XX_alleles, XY_alleles, YY_alleles, X, Y, exp_XX, exp_XY, exp_YY)
SELECT se.experiment_id, se.identifier, ISNULL(xx.number, 0), ISNULL(xy.number, 0), ISNULL(yy.number, 0),
ISNULL(xx.alleles, ''), ISNULL(xy.alleles, ''), ISNULL(yy.alleles, ''),
ISNULL(2*xx.number, 0) + ISNULL(xy.number, 0), ISNULL(2*yy.number, 0) + ISNULL(xy.number, 0),
CASE  --exp_XX
WHEN ((2*ISNULL(xx.number, 0) + ISNULL(xy.number, 0)) + (2*ISNULL(yy.number, 0) + ISNULL(xy.number, 0))) > 0 THEN 
	(((2*ISNULL(xx.number, 0) + ISNULL(xy.number, 0)) + (2*ISNULL(yy.number, 0) + ISNULL(xy.number, 0))) / 2) * (CAST((2*ISNULL(xx.number, 0) + ISNULL(xy.number, 0)) AS REAL) / 
	(CAST((2*ISNULL(xx.number, 0) + ISNULL(xy.number, 0)) + (2*ISNULL(yy.number, 0) + ISNULL(xy.number, 0)) AS REAL))) * (CAST((2*ISNULL(xx.number, 0) + ISNULL(xy.number, 0)) AS REAL) / 
	(CAST((2*ISNULL(xx.number, 0) + ISNULL(xy.number, 0)) + (2*ISNULL(yy.number, 0) + ISNULL(xy.number, 0)) AS REAL))) 
ELSE 
	0
END,
CASE  --exp_XY
WHEN ((2*ISNULL(xx.number, 0) + ISNULL(xy.number, 0)) + (2*ISNULL(yy.number, 0) + ISNULL(xy.number, 0))) > 0 THEN 
	2 * (((2*ISNULL(xx.number, 0) + ISNULL(xy.number, 0)) + (2*ISNULL(yy.number, 0) + ISNULL(xy.number, 0))) / 2) * (CAST((2*ISNULL(xx.number, 0) + ISNULL(xy.number, 0)) AS REAL) / 
	(CAST((2*ISNULL(xx.number, 0) + ISNULL(xy.number, 0)) + (2*ISNULL(yy.number, 0) + ISNULL(xy.number, 0)) AS REAL))) * (CAST((2*ISNULL(yy.number, 0) + ISNULL(xy.number, 0)) AS REAL) / 
	(CAST((2*ISNULL(xx.number, 0) + ISNULL(xy.number, 0)) + (2*ISNULL(yy.number, 0) + ISNULL(xy.number, 0)) AS REAL))) 
ELSE 
	0
END,
CASE  --exp_YY
WHEN ((2*ISNULL(xx.number, 0) + ISNULL(xy.number, 0)) + (2*ISNULL(yy.number, 0) + ISNULL(xy.number, 0))) > 0 THEN 
	(((2*ISNULL(xx.number, 0) + ISNULL(xy.number, 0)) + (2*ISNULL(yy.number, 0) + ISNULL(xy.number, 0))) / 2) * (CAST((2*ISNULL(yy.number, 0) + ISNULL(xy.number, 0)) AS REAL) / 
	(CAST((2*ISNULL(xx.number, 0) + ISNULL(xy.number, 0)) + (2*ISNULL(yy.number, 0) + ISNULL(xy.number, 0)) AS REAL))) * (CAST((2*ISNULL(yy.number, 0) + ISNULL(xy.number, 0)) AS REAL) / 
	(CAST((2*ISNULL(xx.number, 0) + ISNULL(xy.number, 0)) + (2*ISNULL(yy.number, 0) + ISNULL(xy.number, 0)) AS REAL))) 
ELSE 
	0
END 
FROM @selected_experiment se
LEFT OUTER JOIN @zygosity_count_preparation xx ON 
	( xx.experiment_id = se.experiment_id
	  AND SUBSTRING(xx.alleles, 1, 1) = SUBSTRING(xx.alleles, 3, 1)
	)
LEFT OUTER JOIN @zygosity_count_preparation xy ON 
	( xy.experiment_id = se.experiment_id
	  AND NOT SUBSTRING(xy.alleles, 1, 1) = SUBSTRING(xy.alleles, 3, 1)
	)
LEFT OUTER JOIN @zygosity_count_preparation yy ON 
	( yy.experiment_id = se.experiment_id
	  AND SUBSTRING(yy.alleles, 1, 1) = SUBSTRING(yy.alleles, 3, 1) 
	  AND NOT SUBSTRING(yy.alleles, 1, 1) = SUBSTRING(xx.alleles, 1, 1)
	)
WHERE ASCII(ISNULL(xx.alleles, 'A')) < ASCII(ISNULL(yy.alleles, 'Z'))


--
-- Also count the number of different genotypes for each experiment but excluding children.
--			
INSERT INTO @zygosity_count_preparation_corrected (experiment_id, alleles, number)
SELECT cg.experiment_id, cg.alleles, COUNT(cg.alleles)
FROM @consensus_genotype cg
LEFT OUTER JOIN @selected_item si ON si.item_id = cg.item_id
LEFT OUTER JOIN @inheritance_test it ON (it.experiment_id = cg.experiment_id AND it.item_id = cg.item_id AND it.success = 0)
WHERE NOT cg.alleles IS NULL
AND it.item_id IS NULL  --Remove items failed in the inheritance test.
AND ((si.father_id IS NULL AND si.mother_id IS NULL AND @itemMode = 'SUBJECT') OR @HWSkipChildren = 0)  --Remove items which have parents if skip children is on. 
AND cg.item_id NOT IN (SELECT item_id FROM #control_item)
GROUP BY cg.experiment_id, cg.alleles


--
-- Create a table with the count and expected count of the different genotypes with one row per experiment, excluding children.
--
INSERT INTO @zygosity_count_corrected (experiment_id, XX, XY, YY, XX_alleles, XY_alleles, YY_alleles, exp_XX, exp_XY, exp_YY)
SELECT se.experiment_id, ISNULL(xx.number, 0), ISNULL(xy.number, 0), ISNULL(yy.number, 0),
ISNULL(xx.alleles, ''), ISNULL(xy.alleles, ''), ISNULL(yy.alleles, ''),
CASE  --exp_XX
WHEN ((2*ISNULL(xx.number, 0) + ISNULL(xy.number, 0)) + (2*ISNULL(yy.number, 0) + ISNULL(xy.number, 0))) > 0 THEN 
	(((2*ISNULL(xx.number, 0) + ISNULL(xy.number, 0)) + (2*ISNULL(yy.number, 0) + ISNULL(xy.number, 0))) / 2) * (CAST((2*ISNULL(xx.number, 0) + ISNULL(xy.number, 0)) AS REAL) / 
	(CAST((2*ISNULL(xx.number, 0) + ISNULL(xy.number, 0)) + (2*ISNULL(yy.number, 0) + ISNULL(xy.number, 0)) AS REAL))) * (CAST((2*ISNULL(xx.number, 0) + ISNULL(xy.number, 0)) AS REAL) / 
	(CAST((2*ISNULL(xx.number, 0) + ISNULL(xy.number, 0)) + (2*ISNULL(yy.number, 0) + ISNULL(xy.number, 0)) AS REAL))) 
ELSE 
	0
END,
CASE  --exp_XY
WHEN ((2*ISNULL(xx.number, 0) + ISNULL(xy.number, 0)) + (2*ISNULL(yy.number, 0) + ISNULL(xy.number, 0))) > 0 THEN 
	2 * (((2*ISNULL(xx.number, 0) + ISNULL(xy.number, 0)) + (2*ISNULL(yy.number, 0) + ISNULL(xy.number, 0))) / 2) * (CAST((2*ISNULL(xx.number, 0) + ISNULL(xy.number, 0)) AS REAL) / 
	(CAST((2*ISNULL(xx.number, 0) + ISNULL(xy.number, 0)) + (2*ISNULL(yy.number, 0) + ISNULL(xy.number, 0)) AS REAL))) * (CAST((2*ISNULL(yy.number, 0) + ISNULL(xy.number, 0)) AS REAL) / 
	(CAST((2*ISNULL(xx.number, 0) + ISNULL(xy.number, 0)) + (2*ISNULL(yy.number, 0) + ISNULL(xy.number, 0)) AS REAL))) 
ELSE 
	0
END,
CASE  --exp_YY
WHEN ((2*ISNULL(xx.number, 0) + ISNULL(xy.number, 0)) + (2*ISNULL(yy.number, 0) + ISNULL(xy.number, 0))) > 0 THEN 
	(((2*ISNULL(xx.number, 0) + ISNULL(xy.number, 0)) + (2*ISNULL(yy.number, 0) + ISNULL(xy.number, 0))) / 2) * (CAST((2*ISNULL(yy.number, 0) + ISNULL(xy.number, 0)) AS REAL) / 
	(CAST((2*ISNULL(xx.number, 0) + ISNULL(xy.number, 0)) + (2*ISNULL(yy.number, 0) + ISNULL(xy.number, 0)) AS REAL))) * (CAST((2*ISNULL(yy.number, 0) + ISNULL(xy.number, 0)) AS REAL) / 
	(CAST((2*ISNULL(xx.number, 0) + ISNULL(xy.number, 0)) + (2*ISNULL(yy.number, 0) + ISNULL(xy.number, 0)) AS REAL))) 
ELSE 
	0
END 
FROM @selected_experiment se
LEFT OUTER JOIN @zygosity_count_preparation_corrected xx ON 
	( xx.experiment_id = se.experiment_id
	  AND SUBSTRING(xx.alleles, 1, 1) = SUBSTRING(xx.alleles, 3, 1)
	)
LEFT OUTER JOIN @zygosity_count_preparation_corrected xy ON 
	( xy.experiment_id = se.experiment_id
	  AND NOT SUBSTRING(xy.alleles, 1, 1) = SUBSTRING(xy.alleles, 3, 1)
	)
LEFT OUTER JOIN @zygosity_count_preparation_corrected yy ON 
	( yy.experiment_id = se.experiment_id
	  AND SUBSTRING(yy.alleles, 1, 1) = SUBSTRING(yy.alleles, 3, 1) 
	  AND NOT SUBSTRING(yy.alleles, 1, 1) = SUBSTRING(xx.alleles, 1, 1)
	)
WHERE ASCII(ISNULL(xx.alleles, 'A')) < ASCII(ISNULL(yy.alleles, 'Z'))


--
-- Calculate chi2 test value.
--
UPDATE @zygosity_count_corrected
SET chi2 = 
	CASE WHEN exp_XX > 0 AND exp_XY > 0 AND exp_YY > 0 THEN
	((CAST(XX AS REAL) - exp_XX)*(XX - exp_XX)) / (exp_XX) +
	((CAST(XY AS REAL) - exp_XY)*(XY - exp_XY)) / (exp_XY) +
	((CAST(YY AS REAL) - exp_YY)*(YY - exp_YY)) / (exp_YY)
	ELSE 0
	END



--------------------------------------- Test for heterozygosity on X for males ------------------------------------

IF @detectX = 1
BEGIN
	INSERT INTO @het_X_male_test (item_id, experiment_id, success)
	SELECT cg.item_id, 
	cg.experiment_id,
	CASE WHEN SUBSTRING(cg.alleles, 1, 1) = SUBSTRING(cg.alleles, 3, 1) THEN 1 ELSE 0 END 		
	FROM @consensus_genotype cg
	INNER JOIN @selected_experiment se ON se.experiment_id = cg.experiment_id
	INNER JOIN @selected_item si ON si.item_id = cg.item_id
	WHERE se.chrom_name = 'X'
	AND si.sex_id = @maleSexId
	AND NOT cg.alleles IS NULL
END


--------------------------------------------- Blank control test --------------------------------------------

--
-- All genotypes.
--
INSERT INTO @blank_test_preparation (item_id, experiment_id, success)
SELECT r.item_id, r.experiment_id, CASE WHEN r.alleles = 'N/A' THEN 1 ELSE 0 END 
FROM #result r
WHERE r.item_id IN (SELECT item_id FROM @blank_control_item)

--
-- Condense to one result per item and experiment.
--
INSERT INTO @blank_test (item_id, experiment_id, success)
SELECT item_id, experiment_id, MIN(CAST(success AS TINYINT))  --MIN to indicate failure if any row had no success.
FROM @blank_test_preparation
GROUP BY item_id, experiment_id

--------------------------------------------- Homozygote control test ---------------------------------------

--
-- All genotypes.
--
INSERT INTO @homozygote_test_preparation (item_id, experiment_id, success)
SELECT r.item_id,
r.experiment_id,
CASE WHEN SUBSTRING(r.alleles, 1, 1) = SUBSTRING(r.alleles, 3, 1) THEN 1 ELSE 0 END
FROM #result r
WHERE r.item_id IN (SELECT item_id FROM @homozyg_control_item)
AND NOT r.alleles = 'N/A'

--
-- Condense to one result per item and experiment.
--
INSERT INTO @homozygote_test (item_id, experiment_id, success)
SELECT item_id, experiment_id, MIN(CAST(success AS TINYINT))  --MIN to indicate failure if any row had no success.
FROM @homozygote_test_preparation
GROUP BY item_id, experiment_id

--------------------------------------------- Store approved genotypes --------------------------------------



INSERT INTO #approved_genotype (genotype_id, item_id, experiment_id, alleles)
SELECT cg.genotype_id, cg.item_id, cg.experiment_id, cg.alleles
FROM @consensus_genotype cg
LEFT OUTER JOIN @inheritance_test it ON (it.item_id = cg.item_id AND it.experiment_id = cg.experiment_id AND it.success = 0)
WHERE
(it.item_id IS NULL)
AND
(cg.experiment_id NOT IN (SELECT experiment_id FROM @zygosity_count_corrected WHERE chi2 > @HWChi2Limit) OR @doHWTest = 0)
AND
(cg.item_id NOT IN (SELECT item_id FROM #control_item))
AND
(NOT cg.genotype_id IS NULL)




--------------------------------------------- Prepare experiment statistics --------------------------------------


--
-- Count the number of run items for each experiment.
--
INSERT INTO @run_items (experiment_id, nr_items)
SELECT experiment_id, COUNT(DISTINCT item_id)
FROM #result r
WHERE item_id NOT IN (SELECT item_id FROM #control_item)
GROUP BY experiment_id

--
-- Calculated duplicate test statistics.
--
INSERT INTO @dupl_stat (experiment_id, failed_items, tested_items)
SELECT se.experiment_id, ISNULL(failed.failed_items, 0), ISNULL(tested.tested_items, 0)
FROM @selected_experiment se
LEFT OUTER JOIN
	(SELECT experiment_id, COUNT(item_id) AS failed_items
	FROM @consensus_genotype
	WHERE duplicate_tested = 1 AND genotype_id IS NULL
	AND item_id NOT IN (SELECT item_id FROM #control_item)
	GROUP BY experiment_id) failed ON failed.experiment_id = se.experiment_id
LEFT OUTER JOIN
	(SELECT experiment_id, COUNT(item_id) AS tested_items
	FROM @consensus_genotype
	WHERE duplicate_tested = 1
	AND item_id NOT IN (SELECT item_id FROM #control_item)
	GROUP BY experiment_id) tested ON tested.experiment_id = se.experiment_id


--
-- Calculated inheritance test statistics.
--
INSERT INTO @inh_stat (experiment_id, failed_items, tested_items)
SELECT se.experiment_id, ISNULL(failed.failed_items, 0), ISNULL(tested.tested_items, 0)
FROM @selected_experiment se
LEFT OUTER JOIN
	(SELECT experiment_id, COUNT(item_id) AS failed_items
	FROM @inheritance_test
	WHERE success = 0 AND item_id NOT IN (SELECT item_id FROM #control_item)
	GROUP BY experiment_id) failed ON failed.experiment_id = se.experiment_id
LEFT OUTER JOIN
	(SELECT experiment_id, COUNT(item_id) AS tested_items
	FROM @inheritance_test
	WHERE item_id NOT IN (SELECT item_id FROM #control_item)
	GROUP BY experiment_id) tested ON tested.experiment_id = se.experiment_id

--
-- Calculated inheritance control test statistics.
--
INSERT INTO @inh_control_stat (experiment_id, failed_items, tested_items)
SELECT se.experiment_id, ISNULL(failed.failed_items, 0), ISNULL(tested.tested_items, 0)
FROM @selected_experiment se
LEFT OUTER JOIN
	(SELECT experiment_id, COUNT(item_id) AS failed_items
	FROM @inheritance_test
	WHERE success = 0 AND item_id IN (SELECT item_id FROM @inh_control_item)
	GROUP BY experiment_id) failed ON failed.experiment_id = se.experiment_id
LEFT OUTER JOIN
	(SELECT experiment_id, COUNT(item_id) AS tested_items
	FROM @inheritance_test
	WHERE item_id IN (SELECT item_id FROM @inh_control_item)
	GROUP BY experiment_id) tested ON tested.experiment_id = se.experiment_id

--
-- Count rejected genotypes.
--
INSERT INTO @rejected_stat (experiment_id, rejected_gt)
SELECT se.experiment_id, ISNULL(rejected_gt, 0)
FROM @selected_experiment se
LEFT OUTER JOIN
	(SELECT experiment_id, COUNT(genotype_id) AS rejected_gt
	FROM #result r
	WHERE r.status_id = @rejectedStatusId
	GROUP BY experiment_id) rgt ON rgt.experiment_id = se.experiment_id

--
-- Calculate blank control statistics.
--
INSERT INTO @blank_control_stat (experiment_id, failed_items, tested_items)
SELECT se.experiment_id, ISNULL(failed.failed_items, 0), ISNULL(tested.tested_items, 0)
FROM @selected_experiment se
LEFT OUTER JOIN
	(SELECT experiment_id, COUNT(item_id) AS failed_items
	FROM @blank_test
	WHERE success = 0
	GROUP BY experiment_id) failed ON failed.experiment_id = se.experiment_id
LEFT OUTER JOIN
	(SELECT experiment_id, COUNT(item_id) AS tested_items
	FROM @blank_test
	GROUP BY experiment_id) tested ON tested.experiment_id = se.experiment_id


--
-- Calculate homozygote control statistics.
--
INSERT INTO @homozyg_control_stat (experiment_id, failed_items, tested_items)
SELECT se.experiment_id, ISNULL(failed.failed_items, 0), ISNULL(tested.tested_items, 0)
FROM @selected_experiment se
LEFT OUTER JOIN
	(SELECT experiment_id, COUNT(item_id) AS failed_items
	FROM @homozygote_test
	WHERE success = 0
	GROUP BY experiment_id) failed ON failed.experiment_id = se.experiment_id
LEFT OUTER JOIN
	(SELECT experiment_id, COUNT(item_id) AS tested_items
	FROM @homozygote_test
	GROUP BY experiment_id) tested ON tested.experiment_id = se.experiment_id

					
--
-- Add the experiment statistics to the list.
--					
INSERT INTO #experiment_stat (
					experiment_id,
					identifier,
					fq_XX,
					fq_YY,
					fq_XY,
					not_approved,
					success_rate,
					fq_XX_exp,
					fq_YY_exp,
					fq_XY_exp,
					HW_chi2,
					chrom_name,
					fq_X,
					fq_Y,
					dupl_failed_item,
					dupl_tested_item,
					item_validation,
					item_mismatch,
					inh_failed_item,
					inh_tested_item,
					HW_failure,
					rejected_gt,
					inh_ctrl_failed_item,
					inh_ctrl_tested_item,
					blank_ctrl_failed_item,
					blank_ctrl_tested_item,
					homozyg_ctrl_failed_item,
					homozyg_ctrl_tested_item
					)					
SELECT	zc.experiment_id, 
		zc.identifier,
		zc.XX,
		zc.YY,
		zc.XY,
		ISNULL(ri.nr_items - (zc.XX + zc.YY + zc.XY), 0),
		CASE WHEN ri.nr_items > 0 THEN CAST(zc.XX + zc.YY + zc.XY AS REAL) / CAST(ri.nr_items AS REAL) ELSE 0 END,
		zc.exp_XX,
		zc.exp_YY,
		zc.exp_XY,
		zcc.chi2,
		se.chrom_name,
		CASE WHEN (zc.X + zc.Y) > 0 THEN CAST(zc.X AS REAL) / CAST(zc.X + zc.Y AS REAL) ELSE 0 END,
		CASE WHEN (zc.X + zc.Y) > 0 THEN CAST(zc.Y AS REAL) / CAST(zc.X + zc.Y AS REAL) ELSE 0 END,
		ds.failed_items,
		ds.tested_items,
		CASE WHEN ri.nr_items > 0 THEN CAST(ds.tested_items AS REAL) / CAST(ri.nr_items AS REAL) ELSE 0 END,
		CASE WHEN ds.tested_items > 0 THEN CAST(ds.failed_items AS REAL) / CAST(ds.tested_items AS REAL) ELSE 0 END,
		ihs.failed_items,
		ihs.tested_items,				
		CASE WHEN zcc.chi2 > @HWChi2Limit AND @doHWTest = 1 THEN 'Failed' WHEN zcc.chi2 <= @HWChi2Limit AND @doHWTest = 1 THEN 'Passed' ELSE 'Not done' END,						 
		rs.rejected_gt,
		ihcs.failed_items,
		ihcs.tested_items,
		bcs.failed_items,
		bcs.tested_items,
		hcs.failed_items,
		hcs.tested_items
FROM @zygosity_count zc
LEFT OUTER JOIN @run_items ri ON ri.experiment_id = zc.experiment_id
INNER JOIN @selected_experiment se ON se.experiment_id = zc.experiment_id
INNER JOIN @zygosity_count_corrected zcc ON zcc.experiment_id = zc.experiment_id
INNER JOIN @dupl_stat ds ON ds.experiment_id = zc.experiment_id
INNER JOIN @inh_stat ihs ON ihs.experiment_id = zc.experiment_id
INNER JOIN @rejected_stat rs ON rs.experiment_id = zc.experiment_id
INNER JOIN @inh_control_stat ihcs ON ihcs.experiment_id = zc.experiment_id
INNER JOIN @blank_control_stat bcs ON bcs.experiment_id = zc.experiment_id
INNER JOIN @homozyg_control_stat hcs ON hcs.experiment_id = zc.experiment_id 





--------------------------------------------- Prepare item statistics --------------------------------------

UPDATE #item_stat
SET success_experiments = success_experiments + c.number
FROM #item_stat ist
INNER JOIN (
SELECT ag.item_id, COUNT(ag.experiment_id) AS number 
FROM #approved_genotype ag
INNER JOIN @selected_experiment se ON se.experiment_id = ag.experiment_id
GROUP BY ag.item_id
) c ON c.item_id = ist.item_id

UPDATE #item_stat
SET total_gt = total_gt + c.number
FROM #item_stat ist
INNER JOIN (
SELECT item_id, COUNT(genotype_id) AS number FROM #result GROUP BY item_id
) c ON c.item_id = ist.item_id

UPDATE #item_stat
SET no_result_gt = no_result_gt + c.number
FROM #item_stat ist
INNER JOIN (
SELECT item_id, COUNT(genotype_id) AS number FROM #result WHERE alleles = 'N/A' GROUP BY item_id
) c ON c.item_id = ist.item_id

UPDATE #item_stat
SET failed_dup_exp = failed_dup_exp + c.number
FROM #item_stat ist
INNER JOIN (
SELECT item_id, COUNT(DISTINCT experiment_id) AS number FROM @consensus_genotype WHERE duplicate_tested = 1 AND genotype_id IS NULL GROUP BY item_id
) c ON c.item_id = ist.item_id

UPDATE #item_stat
SET failed_inh_exp = failed_inh_exp + c.number
FROM #item_stat ist
INNER JOIN (
SELECT item_id, COUNT(DISTINCT experiment_id) AS number FROM @inheritance_test WHERE success = 0 GROUP BY item_id
) c ON c.item_id = ist.item_id

IF @doHWTest = 1
BEGIN
	UPDATE #item_stat
	SET failed_HW_exp = failed_HW_exp + c.number
	FROM #item_stat ist
	INNER JOIN (
	SELECT r.item_id, COUNT(DISTINCT r.experiment_id) AS number
	FROM #result r
	INNER JOIN @zygosity_count_corrected zcc ON (zcc.experiment_id = r.experiment_id AND zcc.chi2 > @HWChi2Limit)
	GROUP BY r.item_id
	) c ON c.item_id = ist.item_id
END

UPDATE #item_stat
SET rejected_gt = rejected_gt + c.number
FROM #item_stat ist
INNER JOIN (
SELECT item_id, COUNT(genotype_id) AS number FROM #result WHERE status_id = @rejectedStatusId GROUP BY item_id
) c ON c.item_id = ist.item_id

IF @detectX = 1
BEGIN
	UPDATE #item_stat
	SET chrX_warning_exp = chrX_warning_exp + c.number
	FROM #item_stat ist
	INNER JOIN (
	SELECT item_id, COUNT(experiment_id) AS number FROM @het_X_male_test WHERE success = 0 GROUP BY item_id
	) c ON c.item_id = ist.item_id
END


--------------------------------------------- Prepare control item statistics -----------------------------

UPDATE #control_item_stat
SET failed_exp = failed_exp + ISNULL(f.number, 0), tested_exp = tested_exp + ISNULL(t.number, 0)
FROM #control_item_stat cis
LEFT OUTER JOIN (
SELECT item_id, COUNT(experiment_id) AS number FROM @blank_test WHERE success = 0 GROUP BY item_id
) f ON f.item_id = cis.item_id
LEFT OUTER JOIN (
SELECT item_id, COUNT(experiment_id) AS number FROM @blank_test GROUP BY item_id
) t ON t.item_id = cis.item_id


UPDATE #control_item_stat
SET failed_exp = failed_exp + ISNULL(f.number, 0), tested_exp = tested_exp + ISNULL(t.number, 0)
FROM #control_item_stat cis
LEFT OUTER JOIN (
SELECT item_id, COUNT(experiment_id) AS number FROM @inheritance_test WHERE success = 0 GROUP BY item_id
) f ON f.item_id = cis.item_id
LEFT OUTER JOIN (
SELECT item_id, COUNT(experiment_id) AS number FROM @inheritance_test GROUP BY item_id
) t ON t.item_id = cis.item_id


UPDATE #control_item_stat
SET failed_exp = failed_exp + ISNULL(f.number, 0), tested_exp = tested_exp + ISNULL(t.number, 0)
FROM #control_item_stat cis
LEFT OUTER JOIN (
SELECT item_id, COUNT(experiment_id) AS number FROM @homozygote_test WHERE success = 0 GROUP BY item_id
) f ON f.item_id = cis.item_id
LEFT OUTER JOIN (
SELECT item_id, COUNT(experiment_id) AS number FROM @homozygote_test GROUP BY item_id
) t ON t.item_id = cis.item_id


-------------------------------------------- Update the experiment counter -----------------------------------

UPDATE #experiment_count
SET run_experiments = run_experiments + c.number
FROM #experiment_count ec
INNER JOIN (
SELECT r.item_id, COUNT(DISTINCT r.experiment_id) AS number
FROM #result r
GROUP BY r.item_id) c ON c.item_id = ec.item_id

UPDATE #experiment_count_nonfailed
SET run_experiments = run_experiments + c.number
FROM #experiment_count_nonfailed ecnf
INNER JOIN (
SELECT r.item_id, COUNT(DISTINCT r.experiment_id) AS number
FROM #result r
WHERE r.experiment_id IN (SELECT experiment_id FROM #approved_genotype)
GROUP BY r.item_id) c ON c.item_id = ecnf.item_id

--------------------------------------------- Store test results ---------------------------------------------

INSERT INTO #duplicate_test(item_id, experiment_id, success)
SELECT item_id, experiment_id, CASE WHEN genotype_id IS NULL THEN 0 ELSE 1 END
FROM @consensus_genotype WHERE duplicate_tested = 1

INSERT INTO #inheritance_test(item_id, experiment_id, success)
SELECT item_id, experiment_id, success FROM @inheritance_test

INSERT INTO #het_X_male_test(item_id, experiment_id, success)
SELECT item_id, experiment_id, success FROM @het_X_male_test

INSERT INTO #blank_test(item_id, experiment_id, success)
SELECT item_id, experiment_id, success FROM @blank_test

INSERT INTO #homozygote_test(item_id, experiment_id, success)
SELECT item_id, experiment_id, success FROM @homozygote_test

----------------------------------------------- Store all genotypes --------------------------------------------

INSERT INTO #all_genotypes(genotype_id, item_id, experiment_id)
SELECT genotype_id, item_id, experiment_id FROM #result

SET NOCOUNT OFF

END

