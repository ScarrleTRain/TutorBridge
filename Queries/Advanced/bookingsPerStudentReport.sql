select u.NameFirst + ' ' + u.NameLast Student, count(b.Id) [Total Bookings] from AspNetUsers u join Booking b on u.Id = b.UserId
group by u.Id, u.NameFirst, u.NameLast
order by [Total Bookings] desc