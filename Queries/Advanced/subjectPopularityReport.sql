select s.Name [Subject], count(b.Id) [Total Bookings], count(distinct b.UserId) [Unique Students]
from Subject s join Booking b on s.SubjectId = b.SubjectId
group by s.SubjectId, s.Name
order by [Total Bookings] desc