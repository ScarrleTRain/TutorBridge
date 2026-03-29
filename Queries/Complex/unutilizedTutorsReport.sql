select u.NameFirst + ' ' + u.NameLast [Tutor], STRING_AGG(s.Name, ', ') [Subjects]
from AspNetUsers u join TutorSubject ts on u.Id = ts.TutorId
join Subject s on ts.SubjectId = s.SubjectId
where u.IsTutor = 1
and u.Id not in (
    select distinct t.TutorId
    from Booking b
    join Timeslot t on b.TimeslotId = t.TimeslotId
)
group by u.Id, u.NameFirst, u.NameLast