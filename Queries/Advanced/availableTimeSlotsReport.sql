select t2.NameFirst + ' ' + t2.NameLast [Tutor], convert(varchar, t.DateTimeStart, 103) [Date], convert(varchar(5), cast(t.DateTimeStart as time)) [Start Time], convert(varchar(5), cast(t.DateTimeEnd as time)) [End Time]
from Timeslot t join AspNetUsers t2 on t.TutorId = t2.Id
where t.TimeslotId not in (select TimeslotId from Booking) and t.DateTimeStart > getdate()