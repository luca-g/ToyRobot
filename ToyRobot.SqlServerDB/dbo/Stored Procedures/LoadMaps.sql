CREATE PROCEDURE [dbo].[LoadMaps](
	@playerId INT
	)
AS
BEGIN
	SET NOCOUNT ON;
	SELECT
		P.PlayerId,
		M.MapId,
		M.Width,
		M.Height,
		M.CreationDate
	FROM
	dbo.Map AS M
	JOIN dbo.Robot AS R
		ON R.DeletionDate IS NULL
		AND M.DeletionDate IS NULL
		AND R.MapId = M.MapId	
	JOIN dbo.Player AS P
		ON P.DeletionDate IS NULL
		AND R.PlayerId = P.PlayerId
		AND (P.PlayerId = @playerId OR @playerId IS NULL)
	ORDER BY P.PlayerId,M.Width,M.Height
END