using Microsoft.AspNetCore.Identity;
using TutorBridge.Areas.Identity.Data;
using TutorBridge.Models;

namespace TutorBridge.Data
{
    public static class DbSeeder
    {
        public static async Task SeedAsync(TutorBridgeContext context, UserManager<User> userManager)
        {
            // =====================
            // Subject (20)
            // =====================
            if (!context.Subject.Any())
            {
                context.Subject.AddRange(
                    new Subject { Name = "Mathematics", Description = "Algebra, calculus, and statistics" },
                    new Subject { Name = "English", Description = "Writing, grammar, and literature" },
                    new Subject { Name = "Science", Description = "Physics, chemistry, and biology" },
                    new Subject { Name = "History", Description = "World and local history" },
                    new Subject { Name = "Geography", Description = "Physical and human geography" },
                    new Subject { Name = "Computer Science", Description = "Programming and computational thinking" },
                    new Subject { Name = "Art", Description = "Drawing, painting, and design" },
                    new Subject { Name = "Music", Description = "Theory, instruments, and composition" },
                    new Subject { Name = "Economics", Description = "Micro and macroeconomics" },
                    new Subject { Name = "Psychology", Description = "Human behaviour and mental processes" },
                    new Subject { Name = "Chemistry", Description = "Organic and inorganic chemistry" },
                    new Subject { Name = "Physics", Description = "Mechanics, waves, and electricity" },
                    new Subject { Name = "Biology", Description = "Cells, genetics, and ecosystems" },
                    new Subject { Name = "French", Description = "French language and culture" },
                    new Subject { Name = "Spanish", Description = "Spanish language and culture" },
                    new Subject { Name = "Japanese", Description = "Japanese language and culture" },
                    new Subject { Name = "Accounting", Description = "Financial and management accounting" },
                    new Subject { Name = "Statistics", Description = "Data analysis and probability" },
                    new Subject { Name = "Philosophy", Description = "Ethics, logic, and metaphysics" },
                    new Subject { Name = "Physical Education", Description = "Fitness, sport, and health" }
                );
                await context.SaveChangesAsync();
            }

            // =====================
            // USERS (20)
            // =====================
            if (!context.Users.Any())
            {
                // --- ADMINS (2) ---
                var admin1 = new User
                {
                    UserName = "james.smith@tutorbridge.com",
                    Email = "james.smith@tutorbridge.com",
                    NameFirst = "James",
                    NameLast = "Smith",
                    Phone = "0211234567",
                    BirthDate = new DateOnly(1985, 3, 12),
                    IsAdmin = true,
                    IsTutor = false,
                    EmailConfirmed = true
                };
                await userManager.CreateAsync(admin1, "Admin@1234");

                var admin2 = new User
                {
                    UserName = "emma.johnson@tutorbridge.com",
                    Email = "emma.johnson@tutorbridge.com",
                    NameFirst = "Emma",
                    NameLast = "Johnson",
                    Phone = "0219876543",
                    BirthDate = new DateOnly(1990, 7, 22),
                    IsAdmin = true,
                    IsTutor = false,
                    EmailConfirmed = true
                };
                await userManager.CreateAsync(admin2, "Admin@1234");

                // --- TUTORS (8) ---
                var tutor1 = new User
                {
                    UserName = "liam.williams@tutorbridge.com",
                    Email = "liam.williams@tutorbridge.com",
                    NameFirst = "Liam",
                    NameLast = "Williams",
                    Phone = "0213456789",
                    BirthDate = new DateOnly(1992, 1, 5),
                    Blurb = "Passionate about helping students reach their potential.",
                    IsTutor = true,
                    IsAdmin = false,
                    EmailConfirmed = true
                };
                await userManager.CreateAsync(tutor1, "Tutor@1234");

                var tutor2 = new User
                {
                    UserName = "olivia.brown@tutorbridge.com",
                    Email = "olivia.brown@tutorbridge.com",
                    NameFirst = "Olivia",
                    NameLast = "Brown",
                    Phone = "0214567890",
                    BirthDate = new DateOnly(1988, 11, 30),
                    Blurb = "5 years of tutoring experience across multiple Subject.",
                    IsTutor = true,
                    IsAdmin = false,
                    EmailConfirmed = true
                };
                await userManager.CreateAsync(tutor2, "Tutor@1234");

                var tutor3 = new User
                {
                    UserName = "noah.jones@tutorbridge.com",
                    Email = "noah.jones@tutorbridge.com",
                    NameFirst = "Noah",
                    NameLast = "Jones",
                    Phone = "0215678901",
                    BirthDate = new DateOnly(1995, 4, 18),
                    Blurb = "Patient and encouraging tutor with a love for teaching.",
                    IsTutor = true,
                    IsAdmin = false,
                    EmailConfirmed = true
                };
                await userManager.CreateAsync(tutor3, "Tutor@1234");

                var tutor4 = new User
                {
                    UserName = "ava.garcia@tutorbridge.com",
                    Email = "ava.garcia@tutorbridge.com",
                    NameFirst = "Ava",
                    NameLast = "Garcia",
                    Phone = "0216789012",
                    BirthDate = new DateOnly(1993, 9, 25),
                    Blurb = "Former teacher with a focus on exam preparation.",
                    IsTutor = true,
                    IsAdmin = false,
                    EmailConfirmed = true
                };
                await userManager.CreateAsync(tutor4, "Tutor@1234");

                var tutor5 = new User
                {
                    UserName = "william.miller@tutorbridge.com",
                    Email = "william.miller@tutorbridge.com",
                    NameFirst = "William",
                    NameLast = "Miller",
                    Phone = "0217890123",
                    BirthDate = new DateOnly(1987, 6, 14),
                    Blurb = "Specialising in making difficult concepts easy to understand.",
                    IsTutor = true,
                    IsAdmin = false,
                    EmailConfirmed = true
                };
                await userManager.CreateAsync(tutor5, "Tutor@1234");

                var tutor6 = new User
                {
                    UserName = "sophia.davis@tutorbridge.com",
                    Email = "sophia.davis@tutorbridge.com",
                    NameFirst = "Sophia",
                    NameLast = "Davis",
                    Phone = "0218901234",
                    BirthDate = new DateOnly(1996, 2, 8),
                    Blurb = "Passionate about helping students reach their potential.",
                    IsTutor = true,
                    IsAdmin = false,
                    EmailConfirmed = true
                };
                await userManager.CreateAsync(tutor6, "Tutor@1234");

                var tutor7 = new User
                {
                    UserName = "benjamin.wilson@tutorbridge.com",
                    Email = "benjamin.wilson@tutorbridge.com",
                    NameFirst = "Benjamin",
                    NameLast = "Wilson",
                    Phone = "0219012345",
                    BirthDate = new DateOnly(1991, 8, 3),
                    Blurb = "5 years of tutoring experience across multiple Subject.",
                    IsTutor = true,
                    IsAdmin = false,
                    EmailConfirmed = true
                };
                await userManager.CreateAsync(tutor7, "Tutor@1234");

                var tutor8 = new User
                {
                    UserName = "isabella.taylor@tutorbridge.com",
                    Email = "isabella.taylor@tutorbridge.com",
                    NameFirst = "Isabella",
                    NameLast = "Taylor",
                    Phone = "0210123456",
                    BirthDate = new DateOnly(1994, 12, 19),
                    Blurb = "Former teacher with a focus on exam preparation.",
                    IsTutor = true,
                    IsAdmin = false,
                    EmailConfirmed = true
                };
                await userManager.CreateAsync(tutor8, "Tutor@1234");

                // --- STUDENTS (10) ---
                var student1 = new User
                {
                    UserName = "lucas.anderson@tutorbridge.com",
                    Email = "lucas.anderson@tutorbridge.com",
                    NameFirst = "Lucas",
                    NameLast = "Anderson",
                    Phone = "0211122334",
                    BirthDate = new DateOnly(2005, 3, 10),
                    IsTutor = false,
                    IsAdmin = false,
                    EmailConfirmed = true
                };
                await userManager.CreateAsync(student1, "Student@1234");

                var student2 = new User
                {
                    UserName = "mia.thomas@tutorbridge.com",
                    Email = "mia.thomas@tutorbridge.com",
                    NameFirst = "Mia",
                    NameLast = "Thomas",
                    Phone = "0212233445",
                    BirthDate = new DateOnly(2006, 7, 21),
                    IsTutor = false,
                    IsAdmin = false,
                    EmailConfirmed = true
                };
                await userManager.CreateAsync(student2, "Student@1234");

                var student3 = new User
                {
                    UserName = "henry.jackson@tutorbridge.com",
                    Email = "henry.jackson@tutorbridge.com",
                    NameFirst = "Henry",
                    NameLast = "Jackson",
                    Phone = "0213344556",
                    BirthDate = new DateOnly(2004, 11, 5),
                    IsTutor = false,
                    IsAdmin = false,
                    EmailConfirmed = true
                };
                await userManager.CreateAsync(student3, "Student@1234");

                var student4 = new User
                {
                    UserName = "charlotte.white@tutorbridge.com",
                    Email = "charlotte.white@tutorbridge.com",
                    NameFirst = "Charlotte",
                    NameLast = "White",
                    Phone = "0214455667",
                    BirthDate = new DateOnly(2005, 9, 15),
                    IsTutor = false,
                    IsAdmin = false,
                    EmailConfirmed = true
                };
                await userManager.CreateAsync(student4, "Student@1234");

                var student5 = new User
                {
                    UserName = "alexander.harris@tutorbridge.com",
                    Email = "alexander.harris@tutorbridge.com",
                    NameFirst = "Alexander",
                    NameLast = "Harris",
                    Phone = "0215566778",
                    BirthDate = new DateOnly(2006, 1, 28),
                    IsTutor = false,
                    IsAdmin = false,
                    EmailConfirmed = true
                };
                await userManager.CreateAsync(student5, "Student@1234");

                var student6 = new User
                {
                    UserName = "amelia.martin@tutorbridge.com",
                    Email = "amelia.martin@tutorbridge.com",
                    NameFirst = "Amelia",
                    NameLast = "Martin",
                    Phone = "0216677889",
                    BirthDate = new DateOnly(2004, 6, 3),
                    IsTutor = false,
                    IsAdmin = false,
                    EmailConfirmed = true
                };
                await userManager.CreateAsync(student6, "Student@1234");

                var student7 = new User
                {
                    UserName = "mason.thompson@tutorbridge.com",
                    Email = "mason.thompson@tutorbridge.com",
                    NameFirst = "Mason",
                    NameLast = "Thompson",
                    Phone = "0217788990",
                    BirthDate = new DateOnly(2005, 4, 17),
                    IsTutor = false,
                    IsAdmin = false,
                    EmailConfirmed = true
                };
                await userManager.CreateAsync(student7, "Student@1234");

                var student8 = new User
                {
                    UserName = "harper.robinson@tutorbridge.com",
                    Email = "harper.robinson@tutorbridge.com",
                    NameFirst = "Harper",
                    NameLast = "Robinson",
                    Phone = "0218899001",
                    BirthDate = new DateOnly(2006, 10, 9),
                    IsTutor = false,
                    IsAdmin = false,
                    EmailConfirmed = true
                };
                await userManager.CreateAsync(student8, "Student@1234");

                var student9 = new User
                {
                    UserName = "ethan.clark@tutorbridge.com",
                    Email = "ethan.clark@tutorbridge.com",
                    NameFirst = "Ethan",
                    NameLast = "Clark",
                    Phone = "0219900112",
                    BirthDate = new DateOnly(2004, 2, 22),
                    IsTutor = false,
                    IsAdmin = false,
                    EmailConfirmed = true
                };
                await userManager.CreateAsync(student9, "Student@1234");

                var student10 = new User
                {
                    UserName = "evelyn.lewis@tutorbridge.com",
                    Email = "evelyn.lewis@tutorbridge.com",
                    NameFirst = "Evelyn",
                    NameLast = "Lewis",
                    Phone = "0210011223",
                    BirthDate = new DateOnly(2005, 8, 14),
                    IsTutor = false,
                    IsAdmin = false,
                    EmailConfirmed = true
                };
                await userManager.CreateAsync(student10, "Student@1234");

                await context.SaveChangesAsync();
            }

            // =====================
            // TUTOR Subject (20)
            // =====================
            if (!context.TutorSubject.Any())
            {
                var tutors = context.Users.Where(u => u.IsTutor).ToList();
                var Subject = context.Subject.ToList();

                Subject S(string name) => Subject.First(s => s.Name == name);
                User T(string email) => tutors.First(t => t.Email == email);

                context.TutorSubject.AddRange(
                    new TutorSubject { TutorId = T("liam.williams@tutorbridge.com").Id, SubjectId = S("Mathematics").SubjectId },
                    new TutorSubject { TutorId = T("liam.williams@tutorbridge.com").Id, SubjectId = S("Physics").SubjectId },
                    new TutorSubject { TutorId = T("liam.williams@tutorbridge.com").Id, SubjectId = S("Statistics").SubjectId },
                    new TutorSubject { TutorId = T("olivia.brown@tutorbridge.com").Id, SubjectId = S("English").SubjectId },
                    new TutorSubject { TutorId = T("olivia.brown@tutorbridge.com").Id, SubjectId = S("History").SubjectId },
                    new TutorSubject { TutorId = T("noah.jones@tutorbridge.com").Id, SubjectId = S("Science").SubjectId },
                    new TutorSubject { TutorId = T("noah.jones@tutorbridge.com").Id, SubjectId = S("Chemistry").SubjectId },
                    new TutorSubject { TutorId = T("ava.garcia@tutorbridge.com").Id, SubjectId = S("Spanish").SubjectId },
                    new TutorSubject { TutorId = T("ava.garcia@tutorbridge.com").Id, SubjectId = S("French").SubjectId },
                    new TutorSubject { TutorId = T("william.miller@tutorbridge.com").Id, SubjectId = S("Computer Science").SubjectId },
                    new TutorSubject { TutorId = T("william.miller@tutorbridge.com").Id, SubjectId = S("Mathematics").SubjectId },
                    new TutorSubject { TutorId = T("sophia.davis@tutorbridge.com").Id, SubjectId = S("Art").SubjectId },
                    new TutorSubject { TutorId = T("sophia.davis@tutorbridge.com").Id, SubjectId = S("Music").SubjectId },
                    new TutorSubject { TutorId = T("benjamin.wilson@tutorbridge.com").Id, SubjectId = S("Economics").SubjectId },
                    new TutorSubject { TutorId = T("benjamin.wilson@tutorbridge.com").Id, SubjectId = S("Accounting").SubjectId },
                    new TutorSubject { TutorId = T("benjamin.wilson@tutorbridge.com").Id, SubjectId = S("Philosophy").SubjectId },
                    new TutorSubject { TutorId = T("isabella.taylor@tutorbridge.com").Id, SubjectId = S("Biology").SubjectId },
                    new TutorSubject { TutorId = T("isabella.taylor@tutorbridge.com").Id, SubjectId = S("Psychology").SubjectId },
                    new TutorSubject { TutorId = T("isabella.taylor@tutorbridge.com").Id, SubjectId = S("Geography").SubjectId },
                    new TutorSubject { TutorId = T("noah.jones@tutorbridge.com").Id, SubjectId = S("Japanese").SubjectId }
                );
                await context.SaveChangesAsync();
            }

            // =====================
            // Timeslot (20)
            // =====================
            if (!context.Timeslot.Any())
            {
                var tutors = context.Users.Where(u => u.IsTutor).ToList();
                User T(string email) => tutors.First(t => t.Email == email);

                var now = DateTime.Now.Date;

                context.Timeslot.AddRange(
                    new Timeslot { TutorId = T("liam.williams@tutorbridge.com").Id, DateTimeStart = now.AddDays(1).AddHours(9), DateTimeEnd = now.AddDays(1).AddHours(10) },
                    new Timeslot { TutorId = T("liam.williams@tutorbridge.com").Id, DateTimeStart = now.AddDays(2).AddHours(11), DateTimeEnd = now.AddDays(2).AddHours(12) },
                    new Timeslot { TutorId = T("olivia.brown@tutorbridge.com").Id, DateTimeStart = now.AddDays(1).AddHours(13), DateTimeEnd = now.AddDays(1).AddHours(14) },
                    new Timeslot { TutorId = T("olivia.brown@tutorbridge.com").Id, DateTimeStart = now.AddDays(3).AddHours(10), DateTimeEnd = now.AddDays(3).AddHours(11) },
                    new Timeslot { TutorId = T("noah.jones@tutorbridge.com").Id, DateTimeStart = now.AddDays(2).AddHours(9), DateTimeEnd = now.AddDays(2).AddHours(10) },
                    new Timeslot { TutorId = T("noah.jones@tutorbridge.com").Id, DateTimeStart = now.AddDays(4).AddHours(14), DateTimeEnd = now.AddDays(4).AddHours(15) },
                    new Timeslot { TutorId = T("ava.garcia@tutorbridge.com").Id, DateTimeStart = now.AddDays(1).AddHours(15), DateTimeEnd = now.AddDays(1).AddHours(16) },
                    new Timeslot { TutorId = T("ava.garcia@tutorbridge.com").Id, DateTimeStart = now.AddDays(5).AddHours(11), DateTimeEnd = now.AddDays(5).AddHours(12) },
                    new Timeslot { TutorId = T("william.miller@tutorbridge.com").Id, DateTimeStart = now.AddDays(3).AddHours(9), DateTimeEnd = now.AddDays(3).AddHours(10) },
                    new Timeslot { TutorId = T("william.miller@tutorbridge.com").Id, DateTimeStart = now.AddDays(6).AddHours(13), DateTimeEnd = now.AddDays(6).AddHours(14) },
                    new Timeslot { TutorId = T("sophia.davis@tutorbridge.com").Id, DateTimeStart = now.AddDays(2).AddHours(14), DateTimeEnd = now.AddDays(2).AddHours(15) },
                    new Timeslot { TutorId = T("sophia.davis@tutorbridge.com").Id, DateTimeStart = now.AddDays(7).AddHours(10), DateTimeEnd = now.AddDays(7).AddHours(11) },
                    new Timeslot { TutorId = T("benjamin.wilson@tutorbridge.com").Id, DateTimeStart = now.AddDays(4).AddHours(9), DateTimeEnd = now.AddDays(4).AddHours(10) },
                    new Timeslot { TutorId = T("benjamin.wilson@tutorbridge.com").Id, DateTimeStart = now.AddDays(5).AddHours(15), DateTimeEnd = now.AddDays(5).AddHours(16) },
                    new Timeslot { TutorId = T("isabella.taylor@tutorbridge.com").Id, DateTimeStart = now.AddDays(3).AddHours(11), DateTimeEnd = now.AddDays(3).AddHours(12) },
                    new Timeslot { TutorId = T("isabella.taylor@tutorbridge.com").Id, DateTimeStart = now.AddDays(6).AddHours(9), DateTimeEnd = now.AddDays(6).AddHours(10) },
                    new Timeslot { TutorId = T("liam.williams@tutorbridge.com").Id, DateTimeStart = now.AddDays(8).AddHours(13), DateTimeEnd = now.AddDays(8).AddHours(14) },
                    new Timeslot { TutorId = T("olivia.brown@tutorbridge.com").Id, DateTimeStart = now.AddDays(7).AddHours(14), DateTimeEnd = now.AddDays(7).AddHours(15) },
                    new Timeslot { TutorId = T("noah.jones@tutorbridge.com").Id, DateTimeStart = now.AddDays(9).AddHours(10), DateTimeEnd = now.AddDays(9).AddHours(11) },
                    new Timeslot { TutorId = T("ava.garcia@tutorbridge.com").Id, DateTimeStart = now.AddDays(10).AddHours(9), DateTimeEnd = now.AddDays(10).AddHours(10) }
                );
                await context.SaveChangesAsync();
            }

            // =====================
            // Booking (20)
            // =====================
            if (!context.Booking.Any())
            {
                var students = context.Users.Where(u => !u.IsTutor && !u.IsAdmin).ToList();
                var Timeslot = context.Timeslot.ToList();
                var Subject = context.Subject.ToList();

                User St(string email) => students.First(s => s.Email == email);
                Subject S(string name) => Subject.First(s => s.Name == name);

                context.Booking.AddRange(
                    new Booking { UserId = St("lucas.anderson@tutorbridge.com").Id, TimeSlotId = Timeslot[0].TimeSlotId, SubjectId = S("Mathematics").SubjectId, Status = Booking.BookingStatus.Confirmed, NameFirst = "Lucas", NameLast = "Anderson", Phone = "0211122334", Email = "lucas.anderson@tutorbridge.com" },
                    new Booking { UserId = St("mia.thomas@tutorbridge.com").Id, TimeSlotId = Timeslot[1].TimeSlotId, SubjectId = S("Physics").SubjectId, Status = Booking.BookingStatus.Pending, NameFirst = "Mia", NameLast = "Thomas", Phone = "0212233445", Email = "mia.thomas@tutorbridge.com" },
                    new Booking { UserId = St("henry.jackson@tutorbridge.com").Id, TimeSlotId = Timeslot[2].TimeSlotId, SubjectId = S("English").SubjectId, Status = Booking.BookingStatus.Confirmed, NameFirst = "Henry", NameLast = "Jackson", Phone = "0213344556", Email = "henry.jackson@tutorbridge.com" },
                    new Booking { UserId = St("charlotte.white@tutorbridge.com").Id, TimeSlotId = Timeslot[3].TimeSlotId, SubjectId = S("History").SubjectId, Status = Booking.BookingStatus.Cancelled, NameFirst = "Charlotte", NameLast = "White", Phone = "0214455667", Email = "charlotte.white@tutorbridge.com" },
                    new Booking { UserId = St("alexander.harris@tutorbridge.com").Id, TimeSlotId = Timeslot[4].TimeSlotId, SubjectId = S("Science").SubjectId, Status = Booking.BookingStatus.Confirmed, NameFirst = "Alexander", NameLast = "Harris", Phone = "0215566778", Email = "alexander.harris@tutorbridge.com" },
                    new Booking { UserId = St("amelia.martin@tutorbridge.com").Id, TimeSlotId = Timeslot[5].TimeSlotId, SubjectId = S("Chemistry").SubjectId, Status = Booking.BookingStatus.Pending, NameFirst = "Amelia", NameLast = "Martin", Phone = "0216677889", Email = "amelia.martin@tutorbridge.com" },
                    new Booking { UserId = St("mason.thompson@tutorbridge.com").Id, TimeSlotId = Timeslot[6].TimeSlotId, SubjectId = S("Spanish").SubjectId, Status = Booking.BookingStatus.Confirmed, NameFirst = "Mason", NameLast = "Thompson", Phone = "0217788990", Email = "mason.thompson@tutorbridge.com" },
                    new Booking { UserId = St("harper.robinson@tutorbridge.com").Id, TimeSlotId = Timeslot[7].TimeSlotId, SubjectId = S("French").SubjectId, Status = Booking.BookingStatus.Pending, NameFirst = "Harper", NameLast = "Robinson", Phone = "0218899001", Email = "harper.robinson@tutorbridge.com" },
                    new Booking { UserId = St("ethan.clark@tutorbridge.com").Id, TimeSlotId = Timeslot[8].TimeSlotId, SubjectId = S("Computer Science").SubjectId, Status = Booking.BookingStatus.Confirmed, NameFirst = "Ethan", NameLast = "Clark", Phone = "0219900112", Email = "ethan.clark@tutorbridge.com" },
                    new Booking { UserId = St("evelyn.lewis@tutorbridge.com").Id, TimeSlotId = Timeslot[9].TimeSlotId, SubjectId = S("Mathematics").SubjectId, Status = Booking.BookingStatus.Cancelled, NameFirst = "Evelyn", NameLast = "Lewis", Phone = "0210011223", Email = "evelyn.lewis@tutorbridge.com" },
                    new Booking { UserId = St("lucas.anderson@tutorbridge.com").Id, TimeSlotId = Timeslot[10].TimeSlotId, SubjectId = S("Art").SubjectId, Status = Booking.BookingStatus.Confirmed, NameFirst = "Lucas", NameLast = "Anderson", Phone = "0211122334", Email = "lucas.anderson@tutorbridge.com" },
                    new Booking { UserId = St("mia.thomas@tutorbridge.com").Id, TimeSlotId = Timeslot[11].TimeSlotId, SubjectId = S("Music").SubjectId, Status = Booking.BookingStatus.Pending, NameFirst = "Mia", NameLast = "Thomas", Phone = "0212233445", Email = "mia.thomas@tutorbridge.com" },
                    new Booking { UserId = St("henry.jackson@tutorbridge.com").Id, TimeSlotId = Timeslot[12].TimeSlotId, SubjectId = S("Economics").SubjectId, Status = Booking.BookingStatus.Confirmed, NameFirst = "Henry", NameLast = "Jackson", Phone = "0213344556", Email = "henry.jackson@tutorbridge.com" },
                    new Booking { UserId = St("charlotte.white@tutorbridge.com").Id, TimeSlotId = Timeslot[13].TimeSlotId, SubjectId = S("Accounting").SubjectId, Status = Booking.BookingStatus.Confirmed, NameFirst = "Charlotte", NameLast = "White", Phone = "0214455667", Email = "charlotte.white@tutorbridge.com" },
                    new Booking { UserId = St("alexander.harris@tutorbridge.com").Id, TimeSlotId = Timeslot[14].TimeSlotId, SubjectId = S("Biology").SubjectId, Status = Booking.BookingStatus.Pending, NameFirst = "Alexander", NameLast = "Harris", Phone = "0215566778", Email = "alexander.harris@tutorbridge.com" },
                    new Booking { UserId = St("amelia.martin@tutorbridge.com").Id, TimeSlotId = Timeslot[15].TimeSlotId, SubjectId = S("Psychology").SubjectId, Status = Booking.BookingStatus.Cancelled, NameFirst = "Amelia", NameLast = "Martin", Phone = "0216677889", Email = "amelia.martin@tutorbridge.com" },
                    new Booking { UserId = St("mason.thompson@tutorbridge.com").Id, TimeSlotId = Timeslot[16].TimeSlotId, SubjectId = S("Statistics").SubjectId, Status = Booking.BookingStatus.Confirmed, NameFirst = "Mason", NameLast = "Thompson", Phone = "0217788990", Email = "mason.thompson@tutorbridge.com" },
                    new Booking { UserId = St("harper.robinson@tutorbridge.com").Id, TimeSlotId = Timeslot[17].TimeSlotId, SubjectId = S("Geography").SubjectId, Status = Booking.BookingStatus.Pending, NameFirst = "Harper", NameLast = "Robinson", Phone = "0218899001", Email = "harper.robinson@tutorbridge.com" },
                    new Booking { UserId = St("ethan.clark@tutorbridge.com").Id, TimeSlotId = Timeslot[18].TimeSlotId, SubjectId = S("Japanese").SubjectId, Status = Booking.BookingStatus.Confirmed, NameFirst = "Ethan", NameLast = "Clark", Phone = "0219900112", Email = "ethan.clark@tutorbridge.com" },
                    new Booking { UserId = St("evelyn.lewis@tutorbridge.com").Id, TimeSlotId = Timeslot[19].TimeSlotId, SubjectId = S("Philosophy").SubjectId, Status = Booking.BookingStatus.Pending, NameFirst = "Evelyn", NameLast = "Lewis", Phone = "0210011223", Email = "evelyn.lewis@tutorbridge.com" }
                );
                await context.SaveChangesAsync();
            }
        }
    }
}