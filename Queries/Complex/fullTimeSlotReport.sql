SELECT t.TimeslotId AS Id,
       CONCAT(u.NameFirst, ' ', u.NameLast) AS [Tutor Name],
       CONVERT (VARCHAR, t.DateTimeStart, 103) AS [Date],
       CONVERT (VARCHAR (5), CAST (t.DateTimeStart AS TIME)) AS [Start Time],
       CONVERT (VARCHAR (5), CAST (t.DateTimeEnd AS TIME)) AS [End Time]
FROM   TimeSlot AS t
       LEFT OUTER JOIN
       AspNetUsers AS u
       ON t.TutorId = u.Id;