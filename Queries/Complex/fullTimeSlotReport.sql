select t.TimeslotId Id, CONCAT(u.NameFirst, ' ', u.NameLast) [Tutor Name],
CONVERT(varchar, t.DateTimeStart, 103) AS [Date], CONVERT(varchar(5), CAST(t.DateTimeStart AS time)) AS StartTime,  CONVERT(varchar(5), CAST(t.DateTimeEnd AS time)) AS StartTime from Timeslot t
left join AspNetUsers u on t.TutorId = u.Id