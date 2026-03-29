select
    u.NameFirst + ' ' + u.NameLast Tutor,
    count(t.TimeslotId) [Total Slots],
    count(b.Id) [Booked Slots],
    count(t.TimeslotId) - count(b.Id) [Available Slots],
    cast(count(b.Id) * 100.0 / count(t.TimeslotId) as decimal(5,1)) [Utilization %]
from AspNetUsers u join Timeslot t on u.Id = t.TutorId left join Booking b on t.TimeslotId = b.TimeslotId
where u.IsTutor = 1
group by u.Id, u.NameFirst, u.NameLast
order by [Utilization %] desc