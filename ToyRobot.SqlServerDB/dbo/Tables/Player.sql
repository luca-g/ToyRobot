CREATE TABLE [dbo].[Player] (
    [PlayerId]     INT              IDENTITY (1, 1) NOT NULL,
    [Identifier]   UNIQUEIDENTIFIER NOT NULL,
    [CreationDate] DATETIME         NOT NULL,
    [DeletionDate] DATETIME         NULL,
    CONSTRAINT [PK_Player] PRIMARY KEY CLUSTERED ([PlayerId] ASC)
);





