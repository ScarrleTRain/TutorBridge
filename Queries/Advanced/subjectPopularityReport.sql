SELECT   s.Name AS [Subject],
         count(b.Id) AS [Total Bookings],
         count(DISTINCT b.UserId) AS [Unique Students]
FROM     Subject AS s
         INNER JOIN
         Booking AS b
         ON s.SubjectId = b.SubjectId
GROUP BY s.SubjectId, s.Name
ORDER BY [Total Bookings] DESC;