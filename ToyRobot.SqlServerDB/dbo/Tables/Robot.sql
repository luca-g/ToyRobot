CREATE TABLE [dbo].[Robot] (
    [RobotId]       INT      NOT NULL,
    [PlayerId]      INT      NOT NULL,
    [MapId]         INT      NOT NULL,
    [X]             INT      NULL,
    [Y]             INT      NULL,
    [OrientationId] INT      NULL,
    [CreationDate]  DATETIME NOT NULL,
    [DeletionDate]  DATETIME NULL,
    CONSTRAINT [PK_Robot] PRIMARY KEY CLUSTERED ([RobotId] ASC),
    CONSTRAINT [FK_Robot_Map] FOREIGN KEY ([MapId]) REFERENCES [dbo].[Map] ([MapId]),
    CONSTRAINT [FK_Robot_Orientation] FOREIGN KEY ([OrientationId]) REFERENCES [ref].[Orientation] ([OrientationId]),
    CONSTRAINT [FK_Robot_Player] FOREIGN KEY ([PlayerId]) REFERENCES [dbo].[Player] ([PlayerId])
);

