CREATE TABLE [log].[Command] (
    [CommandId]         INT            IDENTITY (1, 1) NOT NULL,
    [RobotId]           INT            NULL,
    [CommandText]       NVARCHAR (255) NULL,
    [CommandDate]       DATETIME       NOT NULL,
    [CommandResult]     NVARCHAR (255) NULL,
    [X]                 INT            NULL,
    [Y]                 INT            NULL,
    [OrientationId]     INT            NULL,
    [RobotDeletionDate] DATETIME       NULL,
    CONSTRAINT [PK__Command__6B410B0682D231AC] PRIMARY KEY CLUSTERED ([CommandId] ASC),
    CONSTRAINT [FK_Command_Robot] FOREIGN KEY ([RobotId]) REFERENCES [dbo].[Robot] ([RobotId])
);





