SELECT t.TimeslotId AS Id,
       CONCAT(u.NameFirst, ' ', u.NameLast) AS [Tutor Name],
       CONVERT (VARCHAR, t.DateTimeStart, 103) AS [Date],
       CONVERT (VARCHAR (5), CAST (t.DateTimeStart AS TIME)) AS StartTime,
       CONVERT (VARCHAR (5), CAST (t.DateTimeEnd AS TIME)) AS StartTime
FROM   Timeslot AS t
       LEFT OUTER JOIN
       AspNetUsers AS u
       ON t.TutorId = u.Id;