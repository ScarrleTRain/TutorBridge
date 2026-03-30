SELECT   u.NameFirst + ' ' + u.NameLast AS [Tutor],
         string_agg(s.Name, ', ') WITHIN GROUP (ORDER BY s.Name ASC) AS [Subjects]
FROM     AspNetUsers AS u
         INNER JOIN
         TutorSubject AS ts
         ON u.Id = ts.TutorId
         INNER JOIN
         Subject AS s
         ON ts.SubjectId = s.SubjectId
GROUP BY u.Id, u.NameFirst, u.NameLast;