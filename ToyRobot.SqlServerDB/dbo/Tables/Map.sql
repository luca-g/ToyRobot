CREATE TABLE [dbo].[Map] (
    [MapId]             INT      IDENTITY (1, 1) NOT NULL,
    [CreatedByPlayerId] INT      NULL,
    [Width]             INT      NOT NULL,
    [Height]            INT      NOT NULL,
    [MaxRobots]         INT      NOT NULL,
    [CreationDate]      DATETIME NOT NULL,
    [DeletionDate]      DATETIME NULL,
    CONSTRAINT [PK_Map] PRIMARY KEY CLUSTERED ([MapId] ASC),
    CONSTRAINT [FK_Map_Player] FOREIGN KEY ([CreatedByPlayerId]) REFERENCES [dbo].[Player] ([PlayerId])
);

