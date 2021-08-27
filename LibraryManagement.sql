Use Library
GO

DROP TABLE IF EXISTS BORROW;
DROP TABLE IF EXISTS USERS;
DROP TABLE IF EXISTS BOOKS;


CREATE TABLE [dbo].[Users] (
    [Id]           INT           IDENTITY (1, 1) NOT NULL,
    [username]     NVARCHAR (50) NOT NULL,
    [creationTime] DATETIME  NULL,
    PRIMARY KEY CLUSTERED ([Id] ASC),
    UNIQUE NONCLUSTERED ([username] ASC)
);

CREATE TABLE [dbo].[Books] (
    [id]           INT           IDENTITY (1, 1) NOT NULL,
    [BookName]     NVARCHAR (50) NOT NULL,
    [Author]       NVARCHAR (50) NOT NULL,
    [Quantity]     NVARCHAR (50) NULL,
    [UsernameID]   INT           NOT NULL,
    [BookStatus]   NVARCHAR (50) NULL,
    [creationTime] DATETIME  NULL,
    CONSTRAINT [PK_Books] PRIMARY KEY CLUSTERED ([id] ASC),
    FOREIGN KEY ([UsernameID]) REFERENCES [dbo].[Users] ([Id])
);

CREATE TABLE [dbo].[Borrow] (
    [BorrowID]       INT           IDENTITY (1, 1) NOT NULL,
    [UserId]         INT           NOT NULL,
    [BookId]         INT           NOT NULL,
    [borrowDateTime] DATETIME  NULL,
    CONSTRAINT [PK_Borrow] PRIMARY KEY CLUSTERED ([BorrowID] ASC),
    FOREIGN KEY ([UserId]) REFERENCES [dbo].[Users] ([Id]),
    FOREIGN KEY ([BookId]) REFERENCES [dbo].[Books] ([id])
);

INSERT INTO USERS (username, creationTime) VALUES ('jack@gmail.com', '2021-08-14 2:37:39 AM');
INSERT INTO USERS (username, creationTime) VALUES ('hulk@gmail.com', '2021-08-14 2:37:39 AM');
INSERT INTO USERS (username, creationTime) VALUES ('james@gmail.com', '2021-08-14 2:37:39 AM');
INSERT INTO USERS (username, creationTime) VALUES ('tina@gmail.com', '2021-08-14 2:37:39 AM');
INSERT INTO USERS (username, creationTime) VALUES ('sam@gmail.com', '2021-08-14 2:37:39 AM');
INSERT INTO USERS (username, creationTime) VALUES ('bruno@gmail.com', '2021-08-14 2:37:39 AM');
INSERT INTO USERS (username, creationTime) VALUES ('chris@gmail.com', '2021-08-14 2:37:39 AM');

/* This will need to be created by users from the front-end to have the userId available. 
INSERT INTO BOOKS (BookName, Author, Quantity, UsernameID, BookStatus, creationTime) VALUES ('In Search of Lost Time', 'Marcel Proust','3', UsernameID, 'Available', '2021-08-14 2:37:39 AM' );
INSERT INTO BOOKS (BookName, Author, Quantity, UsernameID, BookStatus, creationTime) VALUES ('Ulysses', 'James Joyce','3', UsernameID, 'Available', '2021-08-14 2:37:39 AM' );
INSERT INTO BOOKS (BookName, Author, Quantity, UsernameID, BookStatus, creationTime) VALUES ('One Hundred Years', 'Gabriel Garcia','3', UsernameID, 'Available', '2021-08-14 2:37:39 AM' );
INSERT INTO BOOKS (BookName, Author, Quantity, UsernameID, BookStatus, creationTime) VALUES ('The Great Gatsby', 'F. Scott Fitzgerald','3', UsernameID, 'Available', '2021-08-14 2:37:39 AM' );
INSERT INTO BOOKS (BookName, Author, Quantity, UsernameID, BookStatus, creationTime) VALUES ('Moby Dick', 'Herman Melville','3', UsernameID, 'Available', '2021-08-14 2:37:39 AM' );
INSERT INTO BOOKS (BookName, Author, Quantity, UsernameID, BookStatus, creationTime) VALUES ('War and Peace', 'Leo Tolstoy','3', UsernameID, 'Available', '2021-08-14 2:37:39 AM' );
INSERT INTO BOOKS (BookName, Author, Quantity, UsernameID, BookStatus, creationTime) VALUES ('Hamlet' ,'William Shakespeare','3', UsernameID, 'Available', '2021-08-14 2:37:39 AM' );
INSERT INTO BOOKS (BookName, Author, Quantity, UsernameID, BookStatus, creationTime) VALUES ('The Odyssey','Homer','3', UsernameID, 'Available', '2021-08-14 2:37:39 AM' );
INSERT INTO BOOKS (BookName, Author, Quantity, UsernameID, BookStatus, creationTime) VALUES ('Don Quixote', 'Miguel de Cervantes','3', UsernameID, 'Available', '2021-08-14 2:37:39 AM' );

// Same goes for this table. It will need be created from the front-end to have both the userId and bookId. 
INSERT INTO BORROW (UserId, BookId, borrowDateTime) VALUES (userId, BookId, borrowDateTime);
*/
