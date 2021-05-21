CREATE TABLE [dbo].[Results] (
    [Id]         INT   IDENTITY(1,1)   NOT NULL,
    [player1_id] INT      NULL,
    [player2_id] INT      NULL,
    [win]        BIT      NOT NULL,
    [time_point] DATETIME NOT NULL,
    [last_hp]    INT      NOT NULL,
    PRIMARY KEY CLUSTERED ([Id] ASC),
    FOREIGN KEY ([player1_id]) REFERENCES [dbo].[Players] ([Id]),
    FOREIGN KEY ([player2_id]) REFERENCES [dbo].[Players] ([Id])
);

