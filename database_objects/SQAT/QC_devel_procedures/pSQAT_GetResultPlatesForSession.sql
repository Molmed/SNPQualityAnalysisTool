USE [QC_devel]
GO

/****** Object:  StoredProcedure [dbo].[p_LogSessionByWset]    Script Date: 2018-11-13 2:24:49 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[pSQAT_GetResultPlatesForSession] (
	@wset_identifier varchar(255)
)

AS
BEGIN
SET NOCOUNT ON

select p.identifier
from wset w
inner join wset_member wm on w.wset_id = wm.wset_id
inner join kind k on wm.kind_id = k.kind_id
inner join plate p on p.plate_id = wm.identifiable_id
where w.identifier = @wset_identifier and k.name = 'RESULT_PLATE'


SET NOCOUNT OFF
END

GO


