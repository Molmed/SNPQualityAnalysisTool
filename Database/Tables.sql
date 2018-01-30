CREATE TABLE denorm_genotype (
	genotype_id		INTEGER NOT NULL PRIMARY KEY NONCLUSTERED,
	sample_id		INTEGER NOT NULL,
	individual_id	INTEGER NOT NULL,
	assay_id		INTEGER NOT NULL,
	marker_id		INTEGER NOT NULL,
	alleles			CHAR(3) NOT NULL,
	status_id		TINYINT NOT NULL,
	plate_id		INTEGER NOT NULL,
	pos_x			TINYINT NULL,
	pos_y			TINYINT NULL,
	locked_wset_id	INTEGER NULL
	)

CREATE TABLE status (
	status_id		TINYINT NOT NULL PRIMARY KEY,
	name			VARCHAR(30) NOT NULL UNIQUE
	)

CREATE TABLE status_log (
	status_log_id	INTEGER NOT NULL PRIMARY KEY IDENTITY(1,1),
	genotype_id		INTEGER NOT NULL,
	old_status_id	TINYINT NOT NULL,
	new_status_id	TINYINT NOT NULL,
	authority_id	INTEGER NOT NULL,
	created			DATETIME NOT NULL DEFAULT GETDATE()
	)			

CREATE TABLE marker (
	marker_id		INTEGER NOT NULL PRIMARY KEY,
	identifier		VARCHAR(255) NOT NULL UNIQUE,
	chrom_name		VARCHAR(20) NULL
)
	
CREATE TABLE assay (
	assay_id		INTEGER	NOT NULL PRIMARY KEY NONCLUSTERED,
	identifier		VARCHAR(255) NOT NULL UNIQUE,
	marker_id		INTEGER	NOT NULL 
)

CREATE TABLE individual (
	individual_id		INTEGER NOT NULL PRIMARY KEY NONCLUSTERED,
	identifier		VARCHAR(255) NOT NULL UNIQUE,
	individual_type_id	TINYINT NOT NULL,
	sex_id			TINYINT NOT NULL,
	father_id		INTEGER NULL,
	mother_id		INTEGER NULL  
)

CREATE TABLE individual_type (
	individual_type_id	TINYINT NOT NULL PRIMARY KEY,
	name			VARCHAR(255) NOT NULL UNIQUE
)

CREATE TABLE sex (
	sex_id			TINYINT NOT NULL PRIMARY KEY,
	name			VARCHAR(255) NOT NULL UNIQUE
)

CREATE TABLE sample (
	sample_id		INTEGER NOT NULL PRIMARY KEY NONCLUSTERED,
	identifier		VARCHAR(255) NOT NULL UNIQUE,
	individual_id		INTEGER NOT NULL
)

CREATE TABLE project (
	project_id		INTEGER NOT NULL PRIMARY KEY NONCLUSTERED,
	identifier		VARCHAR(255) NOT NULL UNIQUE,
	comment			VARCHAR(1024) NULL
)

CREATE TABLE permission (
	project_id		INTEGER NOT NULL,
	authority_id	INTEGER NOT NULL,
	PRIMARY KEY NONCLUSTERED (project_id, authority_id)
)

CREATE TABLE plate (
	plate_id		INTEGER NOT NULL PRIMARY KEY NONCLUSTERED,
	identifier		VARCHAR(255) NOT NULL UNIQUE,
	project_id	INTEGER NULL,
	authority_id	INTEGER NOT NULL,
	created			DATETIME NOT NULL DEFAULT GETDATE(),
	description		VARCHAR(255) NULL
)

CREATE TABLE wset (
	wset_id			INTEGER NOT NULL PRIMARY KEY NONCLUSTERED,
	identifier		VARCHAR(255) NOT NULL UNIQUE,
	authority_id	INTEGER NOT NULL,
	wset_type_id	TINYINT NOT NULL,
	project_id	INTEGER NULL,
	created			DATETIME NOT NULL DEFAULT GETDATE(),
	description		VARCHAR(255) NULL,
	deleted			BIT NOT NULL DEFAULT 0
)

CREATE TABLE wset_type (
	wset_type_id	TINYINT NOT NULL PRIMARY KEY,
	name			VARCHAR(255) NOT NULL UNIQUE,
	description		VARCHAR(255) NULL
)

CREATE TABLE wset_member (
	identifiable_id	INTEGER NOT NULL,
	kind_id			TINYINT NOT NULL,
	wset_id			INTEGER NOT NULL,
	CONSTRAINT wset_member_PK PRIMARY KEY (identifiable_id, kind_id, wset_id)
)

CREATE TABLE kind (
	kind_id		TINYINT NOT NULL PRIMARY KEY,
	name		VARCHAR(255) NOT NULL UNIQUE
)

CREATE TABLE application_version (
	version_id		INTEGER NOT NULL PRIMARY KEY,
	identifier		VARCHAR(255) NOT NULL,
	version			VARCHAR(20)	NOT NULL,
)

CREATE TABLE session_setting (
	session_setting_id	INTEGER NOT NULL PRIMARY KEY IDENTITY(1,1),
	key_name		VARCHAR(255) NOT NULL,
	subkey			VARCHAR(255) NOT NULL,
	value_char		VARCHAR(255) NULL,	
	value_int		INTEGER NULL,
	value_dec		DECIMAL(10,2) NULL,
	CONSTRAINT session_setting_UQ UNIQUE (key_name, subkey)
)

CREATE TABLE authority (
	authority_id	INTEGER NOT NULL PRIMARY KEY,
	identifier		VARCHAR(255) NOT NULL UNIQUE,
	name			VARCHAR(255) NOT NULL,
	user_type		VARCHAR(32) NOT NULL,
	account_status	BIT NOT NULL
)

CREATE TABLE annotation_type ( 
	annotation_type_id INTEGER NOT NULL PRIMARY KEY IDENTITY(1,1),
	identifier		VARCHAR(255) NOT NULL UNIQUE,
	description		VARCHAR(255) NULL
)

CREATE TABLE annotation (
	annotation_id	INTEGER NOT NULL PRIMARY KEY IDENTITY(1,1),
	annotation_type_id INTEGER NOT NULL,
	value			VARCHAR(255) NOT NULL
)

CREATE TABLE annotation_link (
	marker_id		INTEGER NOT NULL,
	annotation_id	INTEGER	NOT NULL,
	CONSTRAINT annotation_link_PK PRIMARY KEY (marker_id, annotation_id)
)

CREATE TABLE reference_genotype (
	item			VARCHAR(255) NOT NULL,
	experiment		VARCHAR(255) NOT NULL,
	alleles			CHAR(3) NOT NULL,
	reference_set_id	TINYINT NOT NULL
)

CREATE TABLE reference_set (
	reference_set_id	TINYINT NOT NULL PRIMARY KEY,
	identifier		VARCHAR(255) NOT NULL UNIQUE
)

ALTER TABLE denorm_genotype ADD CONSTRAINT denorm_genotype_sample_FK  
FOREIGN KEY (sample_id)  
REFERENCES sample (sample_id) 

ALTER TABLE denorm_genotype ADD CONSTRAINT denorm_genotype_individual_FK  
FOREIGN KEY (individual_id)  
REFERENCES individual (individual_id) 

ALTER TABLE denorm_genotype ADD CONSTRAINT denorm_genotype_assay_FK 
FOREIGN KEY (assay_id)  
REFERENCES assay (assay_id)

ALTER TABLE denorm_genotype ADD CONSTRAINT denorm_genotype_marker_FK  
FOREIGN KEY (marker_id)  
REFERENCES marker (marker_id)

ALTER TABLE denorm_genotype ADD CONSTRAINT denorm_genotype_status_FK  
FOREIGN KEY (status_id)  
REFERENCES status (status_id)

ALTER TABLE denorm_genotype ADD CONSTRAINT denorm_genotype_plate_FK  
FOREIGN KEY (plate_id)  
REFERENCES plate (plate_id)

ALTER TABLE status_log ADD CONSTRAINT status_log_denorm_genotype_FK  
FOREIGN KEY (genotype_id)  
REFERENCES denorm_genotype (genotype_id) 

ALTER TABLE status_log ADD CONSTRAINT status_log_old_status_FK  
FOREIGN KEY (old_status_id)  
REFERENCES status (status_id)

ALTER TABLE status_log ADD CONSTRAINT status_log_new_status_FK  
FOREIGN KEY (new_status_id)  
REFERENCES status (status_id)

ALTER TABLE status_log ADD CONSTRAINT status_log_authority_FK  
FOREIGN KEY (authority_id)  
REFERENCES authority (authority_id) 

ALTER TABLE assay ADD CONSTRAINT assay_marker_FK  
FOREIGN KEY (marker_id)  
REFERENCES marker (marker_id) 

ALTER TABLE individual ADD CONSTRAINT individual_individual_type_id_FK
FOREIGN KEY (individual_type_id)
REFERENCES individual_type (individual_type_id)

ALTER TABLE individual ADD CONSTRAINT individual_father_id_FK
FOREIGN KEY (father_id)
REFERENCES individual (individual_id)

ALTER TABLE individual ADD CONSTRAINT individual_mother_id_FK
FOREIGN KEY (mother_id)
REFERENCES individual (individual_id)

ALTER TABLE individual ADD CONSTRAINT individual_sex_FK
FOREIGN KEY (sex_id)
REFERENCES sex (sex_id)

ALTER TABLE sample ADD CONSTRAINT sample_individual_FK  
FOREIGN KEY (individual_id)  
REFERENCES individual (individual_id) 

ALTER TABLE plate ADD CONSTRAINT plate_project_FK
FOREIGN KEY (project_id)
REFERENCES project (project_id)
ON DELETE CASCADE

ALTER TABLE plate ADD CONSTRAINT plate_authority_FK
FOREIGN KEY (authority_id)
REFERENCES authority (authority_id)

ALTER TABLE wset ADD CONSTRAINT wset_wset_type_FK  
FOREIGN KEY (wset_type_id)  
REFERENCES wset_type (wset_type_id) 

ALTER TABLE wset ADD CONSTRAINT wset_project_FK  
FOREIGN KEY (project_id)  
REFERENCES project (project_id)
ON DELETE CASCADE

ALTER TABLE wset ADD CONSTRAINT wset_authority_FK
FOREIGN KEY (authority_id)
REFERENCES authority (authority_id)

ALTER TABLE wset_member ADD CONSTRAINT wset_member_wset_FK  
FOREIGN KEY (wset_id)  
REFERENCES wset (wset_id) 
ON DELETE CASCADE

ALTER TABLE wset_member ADD CONSTRAINT wset_member_kind_FK  
FOREIGN KEY (kind_id)  
REFERENCES kind (kind_id) 

ALTER TABLE annotation ADD CONSTRAINT annotation_annotation_type_FK  
FOREIGN KEY (annotation_type_id)  
REFERENCES annotation_type (annotation_type_id)

ALTER TABLE annotation_link ADD CONSTRAINT annotation_link_marker_FK  
FOREIGN KEY (marker_id)  
REFERENCES marker (marker_id) 
ON DELETE CASCADE

ALTER TABLE annotation_link ADD CONSTRAINT annotation_link_annotation_FK  
FOREIGN KEY (annotation_id)  
REFERENCES annotation (annotation_id) 

ALTER TABLE reference_genotype ADD CONSTRAINT reference_genotype_reference_set_FK
FOREIGN KEY (reference_set_id)
REFERENCES reference_set (reference_set_id)
ON DELETE CASCADE


CREATE NONCLUSTERED INDEX denorm_genotype_idx_marker_id ON denorm_genotype(marker_id) WITH FILLFACTOR = 90

CREATE NONCLUSTERED INDEX denorm_genotype_idx_sample_id ON denorm_genotype(sample_id) WITH FILLFACTOR = 90

CREATE NONCLUSTERED INDEX denorm_genotype_idx_individual_id ON denorm_genotype(individual_id) WITH FILLFACTOR = 90

CREATE NONCLUSTERED INDEX denorm_genotype_idx_assay_id ON denorm_genotype(assay_id) WITH FILLFACTOR = 90

CREATE NONCLUSTERED INDEX denorm_genotype_idx_status_id ON denorm_genotype(status_id) WITH FILLFACTOR = 90

CREATE NONCLUSTERED INDEX denorm_genotype_idx_plate_id ON denorm_genotype(plate_id) WITH FILLFACTOR = 90


CREATE NONCLUSTERED INDEX status_log_idx_genotype_id ON status_log(genotype_id) WITH FILLFACTOR = 80, PAD_INDEX


CREATE UNIQUE CLUSTERED INDEX assay_idx_identifier ON assay(identifier) WITH FILLFACTOR = 90

CREATE NONCLUSTERED INDEX assay_idx_marker_id ON assay(marker_id) WITH FILLFACTOR = 90


CREATE UNIQUE CLUSTERED INDEX individual_idx_identifier ON individual(identifier) WITH FILLFACTOR = 90

CREATE NONCLUSTERED INDEX individual_idx_father_id ON individual(father_id) WITH FILLFACTOR = 90

CREATE NONCLUSTERED INDEX individual_idx_mother_id ON individual(mother_id) WITH FILLFACTOR = 90


CREATE UNIQUE CLUSTERED INDEX sample_idx_identifier ON sample(identifier) WITH FILLFACTOR = 90

CREATE NONCLUSTERED INDEX sample_idx_individual_id ON sample(individual_id) WITH FILLFACTOR = 90



CREATE UNIQUE CLUSTERED INDEX plate_idx_identifier ON plate(identifier) WITH FILLFACTOR = 90

CREATE NONCLUSTERED INDEX plate_idx_project_id ON plate(project_id) WITH FILLFACTOR = 90



CREATE UNIQUE CLUSTERED INDEX wset_idx_identifier ON wset(identifier) WITH FILLFACTOR = 90

CREATE NONCLUSTERED INDEX wset_idx_wset_type_id ON wset(wset_type_id) WITH FILLFACTOR = 90

CREATE NONCLUSTERED INDEX wset_idx_project_id ON wset(project_id) WITH FILLFACTOR = 90


CREATE NONCLUSTERED INDEX wset_member_idx_wset_id ON wset_member(wset_id) WITH FILLFACTOR = 90

CREATE NONCLUSTERED INDEX wset_member_idx_identifiable_id ON wset_member(identifiable_id) WITH FILLFACTOR = 90


CREATE NONCLUSTERED INDEX annotation_link_idx_annotation_id ON annotation_link(annotation_id) WITH FILLFACTOR = 90


CREATE NONCLUSTERED INDEX reference_genotype_idx_item ON reference_genotype(item) WITH FILLFACTOR = 70, PAD_INDEX

CREATE NONCLUSTERED INDEX reference_genotype_idx_experiment ON reference_genotype(experiment) WITH FILLFACTOR = 70, PAD_INDEX

CREATE NONCLUSTERED INDEX reference_genotype_idx_reference_set_id ON reference_genotype(reference_set_id) WITH FILLFACTOR = 70, PAD_INDEX


-- Client hard coded values.
--INSERT INTO status (status_id, name) VALUES (0, 'approved')
--INSERT INTO status (status_id, name) VALUES (1, 'load')
--INSERT INTO status (status_id, name) VALUES (2, 'rejected')

--INSERT INTO sex (sex_id, name) VALUES (0, 'unknown')
--INSERT INTO sex (sex_id, name) VALUES (1, 'male')
--INSERT INTO sex (sex_id, name) VALUES (2, 'female')



-- Used wset types

--'Project'
--'GenotypeSet'
--'MarkerSet'
--'SampleSet'
--'AssaySet'
--'AnalyzedGenotypeSet'
--'ResultGenotypeSet'
--'PlateSet'
--'IndividualSet'
--'SavedSession'
--'ApprovedSession'

-- Used individual types

-- 'BlankControl'
-- 'InheritanceControl'
-- 'HomozygoteControl'

-- Used kinds

--'MARKER'
--'WSET'
--'INDIVIDUAL'
--'SAMPLE'
--'ASSAY'
--'RESULT_PLATE'


