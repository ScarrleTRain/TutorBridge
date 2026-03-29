select 
    b.Id,
    b.Status,
    u.NameFirst + ' ' + u.NameLast [Student],
    t2.NameFirst + ' ' + t2.NameLast [Tutor],
    s.Name [Subject],
    convert(varchar, t.DateTimeStart, 103) [Date],
    convert(varchar(5), cast(t.DateTimeStart as time)) [Start Time],
    convert(varchar(5), cast(t.DateTimeEnd as time)) [End Time]
from Booking b
join AspNetUsers u
    on b.UserId = u.Id
join Timeslot t
    on b.TimeslotId = t.TimeslotId
join AspNetUsers t2
    on t.TutorId = t2.Id
join Subject s
    on b.SubjectId = s.SubjectId