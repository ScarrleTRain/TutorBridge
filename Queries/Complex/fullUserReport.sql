SELECT Id,
       Concat(NameFirst, ' ', NameLast) AS [Full Name],
       Email,
       Phone,
       BirthDate AS [Birth Date],
       Blurb,
       CONCAT((CASE WHEN IsAdmin = 1 THEN 'Admin' ELSE '' END), 
              (CASE WHEN IsAdmin = 1 AND IsTutor = 1 THEN ', ' ELSE '' END), 
              (CASE WHEN IsTutor = 1 THEN 'Tutor' ELSE '' END)) AS [Elevation]
FROM   AspNetUsers;