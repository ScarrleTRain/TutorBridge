SELECT   u.NameFirst + ' ' + u.NameLast AS [Student],
         s.Name AS [Subject],
         count(b.Id) AS [x Booked]
FROM     Booking AS b
         INNER JOIN
         AspNetUsers AS u
         ON b.UserId = u.Id
         INNER JOIN
         Subject AS s
         ON b.SubjectId = s.SubjectId
GROUP BY u.Id, u.NameFirst, u.NameLast, s.SubjectId, s.Name
ORDER BY [x Booked] DESC, [Student];