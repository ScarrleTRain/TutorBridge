select u.NameFirst + ' ' + u.NameLast [Student], s.Name [Subject], count(b.Id) [x Booked]
from Booking b join AspNetUsers u on b.UserId = u.Id join Subject s on b.SubjectId = s.SubjectId
group by u.Id, u.NameFirst, u.NameLast, s.SubjectId, s.Name
order by [x Booked] desc, [Student]