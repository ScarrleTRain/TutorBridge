select u.NameFirst + ' ' + u.NameLast [Tutor], string_agg(s.Name, ', ') within group (order by s.Name asc) [Subjects] from AspNetUsers u
inner join TutorSubject ts on u.Id = ts.TutorId
inner join Subject s on ts.SubjectId = s.SubjectId
group by u.Id, u.NameFirst, u.NameLast