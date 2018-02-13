-- Returns the genotypes which belong to the selected plate and
-- which fulfill the criterium of being in the union of #plate_well,
-- #plate_experiment and #plate_item, as well as being in the #all_genotypes table.

CREATE PROCEDURE pSQAT_GetPlateSelection(@plateId INTEGER) 
AS
BEGIN
SET NOCOUNT ON


DECLARE @affected_genotypes TABLE(
				genotype_id INTEGER PRIMARY KEY
				)


--
-- Store affected genotype IDs.
--
INSERT INTO @affected_genotypes (genotype_id)
SELECT dg.genotype_id
FROM #all_genotypes ag
INNER JOIN dbo.denorm_genotype dg ON dg.genotype_id = ag.genotype_id
LEFT OUTER JOIN #plate_well pw ON (1 = 1)
WHERE 
(ag.item_id IN (SELECT item_id FROM #plate_item) OR NOT EXISTS(SELECT * FROM #plate_item))
AND
(ag.experiment_id IN (SELECT experiment_id FROM #plate_experiment) OR NOT EXISTS(SELECT * FROM #plate_experiment))
AND
((dg.pos_x = pw.pos_x AND dg.pos_y = pw.pos_y) OR NOT EXISTS(SELECT * FROM #plate_well)) 
AND
dg.plate_id = @plateId


--
-- Return the data.
--
SELECT dg.genotype_id, p.identifier AS [Plate], 
dbo.fTranslateCoord(dg.pos_x, dg.pos_y) AS [Position], 
idv.identifier AS [Individual], 
smp.identifier AS [Sample], m.identifier AS [Marker], a.identifier AS [Assay], 
dg.alleles AS [Result], 
CASE WHEN ps.new_status_id IS NULL THEN sn.name ELSE '(' + psn.name + ')' END AS [Status] 
FROM @affected_genotypes ag
INNER JOIN dbo.denorm_genotype dg ON dg.genotype_id = ag.genotype_id 
INNER JOIN dbo.individual idv ON idv.individual_id = dg.individual_id 
INNER JOIN dbo.sample smp ON smp.sample_id = dg.sample_id 
INNER JOIN dbo.assay a ON a.assay_id = dg.assay_id 
INNER JOIN dbo.marker m ON m.marker_id = dg.marker_id 
INNER JOIN dbo.plate p ON p.plate_id = dg.plate_id 
INNER JOIN dbo.status sn ON sn.status_id = dg.status_id 
LEFT OUTER JOIN #pending_status ps ON ps.genotype_id = dg.genotype_id 
LEFT OUTER JOIN dbo.status psn ON psn.status_id = ps.new_status_id 






SET NOCOUNT OFF

END  





