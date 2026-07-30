SELECT   u.NameFirst + ' ' + u.NameLast AS [Tutor],
         STRING_AGG(s.Name, ', ') AS [Subjects]
FROM     AspNetUsers AS u
         INNER JOIN TutorSubject AS ts
                 ON u.Id = ts.TutorId
         INNER JOIN Subject AS s
                 ON ts.SubjectId = s.SubjectId
WHERE    EXISTS (
             SELECT 1
             FROM   AspNetUserRoles ur
                    INNER JOIN AspNetRoles r ON r.Id = ur.RoleId
             WHERE  ur.UserId = u.Id
                    AND r.Name = 'Tutor'
         )
         AND u.Id NOT IN (SELECT DISTINCT t.TutorId
                          FROM   Booking AS b
                                 INNER JOIN Timeslot AS t
                                         ON b.TimeslotId = t.TimeslotId)
GROUP BY u.Id, u.NameFirst, u.NameLast;