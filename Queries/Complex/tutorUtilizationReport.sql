SELECT   u.NameFirst + ' ' + u.NameLast AS Tutor,
         count(t.TimeslotId) AS [Total Slots],
         count(b.Id) AS [Booked Slots],
         count(t.TimeslotId) - count(b.Id) AS [Available Slots],
         CAST (count(b.Id) * 100.0 / count(t.TimeslotId) AS DECIMAL (5, 1)) AS [Utilization %]
FROM     AspNetUsers AS u
         INNER JOIN Timeslot AS t
                 ON u.Id = t.TutorId
         LEFT OUTER JOIN Booking AS b
                 ON t.TimeslotId = b.TimeslotId
WHERE    EXISTS (
             SELECT 1
             FROM   AspNetUserRoles ur
                    INNER JOIN AspNetRoles r ON r.Id = ur.RoleId
             WHERE  ur.UserId = u.Id
                    AND r.Name = 'Tutor'
         )
GROUP BY u.Id, u.NameFirst, u.NameLast
ORDER BY [Utilization %] DESC;