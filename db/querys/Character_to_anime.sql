-- This query retrieves the anime titles associated with a specific character
USE animes;
GO

SELECT 
    a.title AS AnimeTitle,
    c.name AS CharacterName
FROM Character c
JOIN Animation a ON c.anime_id = a.id;
GO