SELECT u.Id,
       CONCAT(u.NameFirst, ' ', u.NameLast) AS [Full Name],
       u.Email,
       u.Phone,
       u.BirthDate AS [Birth Date],
       u.Blurb,
       STRING_AGG(r.Name, ', ') WITHIN GROUP (ORDER BY r.Name) AS [Elevation]
FROM   AspNetUsers u
       LEFT JOIN AspNetUserRoles ur ON ur.UserId = u.Id
       LEFT JOIN AspNetRoles r ON r.Id = ur.RoleId
GROUP BY u.Id, u.NameFirst, u.NameLast, u.Email, u.Phone, u.BirthDate, u.Blurb
ORDER BY Elevation;