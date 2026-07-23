SELECT   u.NameFirst + ' ' + u.NameLast AS [Tutor],
         STRING_AGG(s.Name, ', ') AS [Subjects]
FROM     AspNetUsers AS u
         INNER JOIN
         TutorSubject AS ts
         ON u.Id = ts.TutorId
         INNER JOIN
         Subject AS s
         ON ts.SubjectId = s.SubjectId
WHERE    u.IsTutor = 1
         AND u.Id NOT IN (SELECT DISTINCT t.TutorId
                          FROM   Booking AS b
                                 INNER JOIN
                                 Timeslot AS t
                                 ON b.TimeslotId = t.TimeslotId)
GROUP BY u.Id, u.NameFirst, u.NameLast;