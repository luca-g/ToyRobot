CREATE TABLE [dbo].[Map] (
    [MapId]        INT      IDENTITY (1, 1) NOT NULL,
    [Width]        INT      NOT NULL,
    [Height]       INT      NOT NULL,
    [CreationDate] DATETIME NOT NULL,
    [DeletionDate] DATETIME NULL,
    CONSTRAINT [PK_Map] PRIMARY KEY CLUSTERED ([MapId] ASC)
);



