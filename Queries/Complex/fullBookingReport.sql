SELECT b.Id,
       b.Status,
       u.NameFirst + ' ' + u.NameLast AS [Student],
       t2.NameFirst + ' ' + t2.NameLast AS [Tutor],
       s.Name AS [Subject],
       CONVERT (VARCHAR, t.DateTimeStart, 103) AS [Date],
       CONVERT (VARCHAR (5), CAST (t.DateTimeStart AS TIME)) AS [Start Time],
       CONVERT (VARCHAR (5), CAST (t.DateTimeEnd AS TIME)) AS [End Time]
FROM   Booking AS b
       INNER JOIN
       AspNetUsers AS u
       ON b.UserId = u.Id
       INNER JOIN
       TimeSlot AS t
       ON b.TimeslotId = t.TimeslotId
       INNER JOIN
       AspNetUsers AS t2
       ON t.TutorId = t2.Id
       INNER JOIN
       Subject AS s
       ON b.SubjectId = s.SubjectId;