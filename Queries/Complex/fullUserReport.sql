select Id, Concat(NameFirst, ' ', NameLast) [Full Name], Email, Phone, BirthDate [Birth Date], Blurb, 
CONCAT((Case When IsAdmin = 1 then 'Admin' else '' END ), (Case when IsAdmin = 1 and IsTutor = 1 then ', ' else '' end), (Case when IsTutor = 1 then 'Tutor' else '' end)) [Elevation]
from AspNetUsers