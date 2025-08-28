-- This query return all characteres from Naruto Animation
USE animes;
GO

SELECT
    c.name AS CharacterName
FROM Character c
JOIN Animation a ON c.anime_id = a.id
WHERE UPPER(a.title) = 'NARUTO';
GO