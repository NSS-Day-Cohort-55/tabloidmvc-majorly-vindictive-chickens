CREATE TABLE [PostImage] (
  [Id] integer PRIMARY KEY IDENTITY,
  [PostId] integer NOT NULL,
  [Content] varbinary(MAX) NOT NULL,

  CONSTRAINT [FK_PostImage_Post] FOREIGN KEY ([PostId]) REFERENCES [Post] ([Id]) ON DELETE CASCADE
)