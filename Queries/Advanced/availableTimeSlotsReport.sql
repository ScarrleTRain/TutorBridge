SELECT t2.NameFirst + ' ' + t2.NameLast AS [Tutor],
       CONVERT (VARCHAR, t.DateTimeStart, 103) AS [Date],
       CONVERT (VARCHAR (5), CAST (t.DateTimeStart AS TIME)) AS [Start Time],
       CONVERT (VARCHAR (5), CAST (t.DateTimeEnd AS TIME)) AS [End Time]
FROM   TimeSlot AS t
       INNER JOIN
       AspNetUsers AS t2
       ON t.TutorId = t2.Id
WHERE  t.TimeslotId NOT IN (SELECT TimeslotId
                            FROM   Booking)
       AND t.DateTimeStart > getdate();