SELECT   u.NameFirst + ' ' + u.NameLast AS Student,
         count(b.Id) AS [Total Bookings]
FROM     AspNetUsers AS u
         INNER JOIN
         Booking AS b
         ON u.Id = b.UserId
GROUP BY u.Id, u.NameFirst, u.NameLast
ORDER BY [Total Bookings] DESC;