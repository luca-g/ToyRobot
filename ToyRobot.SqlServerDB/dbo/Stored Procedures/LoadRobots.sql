CREATE PROCEDURE [dbo].[LoadRobots](
	@playerId INT,
	@robotId INT,
	@mapId INT
	)
AS
BEGIN
	SET NOCOUNT ON;
	SELECT
		P.PlayerId,
		M.MapId,
		R.RobotId,
		R.X,
		R.Y,
		R.OrientationId,
		R.CreationDate
	FROM
	dbo.Map AS M
	JOIN dbo.Robot AS R
		ON R.DeletionDate IS NULL
		AND M.DeletionDate IS NULL
		AND R.MapId = M.MapId	
		AND (M.MapId = @mapId OR @mapId IS NULL)
		AND (R.RobotId = @robotId OR @robotId IS NULL)
	JOIN dbo.Player AS P
		ON P.DeletionDate IS NULL
		AND P.PlayerId = R.PlayerId
		AND (P.PlayerId = @playerId OR @playerId IS NULL)
	ORDER BY P.PlayerId,M.MapId,R.RobotId
END