-- This script creates the tables from anime database
USE animes;
GO

CREATE TABLE Animation (
    Id INT PRIMARY KEY IDENTITY(1,1),
    Title VARCHAR(MAX) NOT NULL,
    Episodes INT,
    Duration INT,
    Genres NVARCHAR(MAX),
    Season VARCHAR(50),
    EndDate DATE,
    AlternativeTitle NVARCHAR(MAX),
    AniListId INT
);
GO

CREATE TABLE Character (
    Id INT PRIMARY KEY IDENTITY(1,1),
    Name VARCHAR(MAX) NOT NULL,
    DateOfBirth DATE,
    AlternativeName NVARCHAR(MAX),
    AnimationId INT,
    AniListId INT,

    FOREIGN KEY (AnimationId) REFERENCES Animation(AniListId)
);
GO