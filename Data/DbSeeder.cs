using Microsoft.AspNetCore.Identity;
using TutorBridge.Areas.Identity.Data;
using TutorBridge.Models;

namespace TutorBridge.Data
{
    public static class DbSeeder
    {
        public static async Task SeedAsync(
            TutorBridgeContext context,
            UserManager<User> userManager,
            RoleManager<IdentityRole> roleManager)
        {
            string[] roles = { "Admin", "Tutor", "Student" };
            foreach (var role in roles)
            {
                if (!await roleManager.RoleExistsAsync(role))
                    await roleManager.CreateAsync(new IdentityRole(role));
            }

            // Reference point for backdating CreatedAt values so seeded accounts
            // look like they've existed for a while, rather than all being "created now".
            var seedTime = DateTime.UtcNow;

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
                    CreatedAt = seedTime.AddDays(-150),
                    EmailConfirmed = true
                };
                await userManager.CreateAsync(admin1, "Admin@1234");
                await userManager.AddToRoleAsync(admin1, "Admin");

                var admin2 = new User
                {
                    UserName = "emma.johnson@tutorbridge.com",
                    Email = "emma.johnson@tutorbridge.com",
                    NameFirst = "Emma",
                    NameLast = "Johnson",
                    Phone = "0219876543",
                    BirthDate = new DateOnly(1990, 7, 22),
                    CreatedAt = seedTime.AddDays(-148),
                    EmailConfirmed = true
                };
                await userManager.CreateAsync(admin2, "Admin@1234");
                await userManager.AddToRoleAsync(admin2, "Admin");

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
                    ProfilePhoto = "liam_williams.jpg",
                    CreatedAt = seedTime.AddDays(-120),
                    EmailConfirmed = true
                };
                await userManager.CreateAsync(tutor1, "Tutor@1234");
                await userManager.AddToRoleAsync(tutor1, "Tutor");

                var tutor2 = new User
                {
                    UserName = "olivia.brown@tutorbridge.com",
                    Email = "olivia.brown@tutorbridge.com",
                    NameFirst = "Olivia",
                    NameLast = "Brown",
                    Phone = "0214567890",
                    BirthDate = new DateOnly(1988, 11, 30),
                    Blurb = "5 years of tutoring experience across multiple Subject.",
                    ProfilePhoto = "olivia_brown.jpg",
                    CreatedAt = seedTime.AddDays(-118),
                    EmailConfirmed = true
                };
                await userManager.CreateAsync(tutor2, "Tutor@1234");
                await userManager.AddToRoleAsync(tutor2, "Tutor");

                var tutor3 = new User
                {
                    UserName = "noah.jones@tutorbridge.com",
                    Email = "noah.jones@tutorbridge.com",
                    NameFirst = "Noah",
                    NameLast = "Jones",
                    Phone = "0215678901",
                    BirthDate = new DateOnly(1995, 4, 18),
                    Blurb = "Patient and encouraging tutor with a love for teaching.",
                    ProfilePhoto = "noah_jones.jpg",
                    CreatedAt = seedTime.AddDays(-116),
                    EmailConfirmed = true
                };
                await userManager.CreateAsync(tutor3, "Tutor@1234");
                await userManager.AddToRoleAsync(tutor3, "Tutor");

                var tutor4 = new User
                {
                    UserName = "ava.garcia@tutorbridge.com",
                    Email = "ava.garcia@tutorbridge.com",
                    NameFirst = "Ava",
                    NameLast = "Garcia",
                    Phone = "0216789012",
                    BirthDate = new DateOnly(1993, 9, 25),
                    Blurb = "Former teacher with a focus on exam preparation.",
                    ProfilePhoto = "ava_garcia.jpg",
                    CreatedAt = seedTime.AddDays(-114),
                    EmailConfirmed = true
                };
                await userManager.CreateAsync(tutor4, "Tutor@1234");
                await userManager.AddToRoleAsync(tutor4, "Tutor");

                var tutor5 = new User
                {
                    UserName = "william.miller@tutorbridge.com",
                    Email = "william.miller@tutorbridge.com",
                    NameFirst = "William",
                    NameLast = "Miller",
                    Phone = "0217890123",
                    BirthDate = new DateOnly(1987, 6, 14),
                    Blurb = "Specialising in making difficult concepts easy to understand.",
                    ProfilePhoto = "william_miller.jpg",
                    CreatedAt = seedTime.AddDays(-112),
                    EmailConfirmed = true
                };
                await userManager.CreateAsync(tutor5, "Tutor@1234");
                await userManager.AddToRoleAsync(tutor5, "Tutor");

                var tutor6 = new User
                {
                    UserName = "sophia.davis@tutorbridge.com",
                    Email = "sophia.davis@tutorbridge.com",
                    NameFirst = "Sophia",
                    NameLast = "Davis",
                    Phone = "0218901234",
                    BirthDate = new DateOnly(1996, 2, 8),
                    Blurb = "Passionate about helping students reach their potential.",
                    ProfilePhoto = "sophia_davis.jpg",
                    CreatedAt = seedTime.AddDays(-110),
                    EmailConfirmed = true
                };
                await userManager.CreateAsync(tutor6, "Tutor@1234");
                await userManager.AddToRoleAsync(tutor6, "Tutor");

                var tutor7 = new User
                {
                    UserName = "benjamin.wilson@tutorbridge.com",
                    Email = "benjamin.wilson@tutorbridge.com",
                    NameFirst = "Benjamin",
                    NameLast = "Wilson",
                    Phone = "0219012345",
                    BirthDate = new DateOnly(1991, 8, 3),
                    Blurb = "5 years of tutoring experience across multiple Subject.",
                    ProfilePhoto = "benjamin_wilson.jpg",
                    CreatedAt = seedTime.AddDays(-108),
                    EmailConfirmed = true
                };
                await userManager.CreateAsync(tutor7, "Tutor@1234");
                await userManager.AddToRoleAsync(tutor7, "Tutor");

                var tutor8 = new User
                {
                    UserName = "isabella.taylor@tutorbridge.com",
                    Email = "isabella.taylor@tutorbridge.com",
                    NameFirst = "Isabella",
                    NameLast = "Taylor",
                    Phone = "0210123456",
                    BirthDate = new DateOnly(1994, 12, 19),
                    Blurb = "Former teacher with a focus on exam preparation.",
                    ProfilePhoto = "isabella_taylor.jpg",
                    CreatedAt = seedTime.AddDays(-106),
                    EmailConfirmed = true
                };
                await userManager.CreateAsync(tutor8, "Tutor@1234");
                await userManager.AddToRoleAsync(tutor8, "Tutor");

                // --- STUDENTS (10) ---
                var student1 = new User
                {
                    UserName = "lucas.anderson@tutorbridge.com",
                    Email = "lucas.anderson@tutorbridge.com",
                    NameFirst = "Lucas",
                    NameLast = "Anderson",
                    Phone = "0211122334",
                    BirthDate = new DateOnly(2005, 3, 10),
                    CreatedAt = seedTime.AddDays(-90),
                    EmailConfirmed = true
                };
                await userManager.CreateAsync(student1, "Student@1234");
                await userManager.AddToRoleAsync(student1, "Student");

                var student2 = new User
                {
                    UserName = "mia.thomas@tutorbridge.com",
                    Email = "mia.thomas@tutorbridge.com",
                    NameFirst = "Mia",
                    NameLast = "Thomas",
                    Phone = "0212233445",
                    BirthDate = new DateOnly(2006, 7, 21),
                    CreatedAt = seedTime.AddDays(-85),
                    EmailConfirmed = true
                };
                await userManager.CreateAsync(student2, "Student@1234");
                await userManager.AddToRoleAsync(student2, "Student");

                var student3 = new User
                {
                    UserName = "henry.jackson@tutorbridge.com",
                    Email = "henry.jackson@tutorbridge.com",
                    NameFirst = "Henry",
                    NameLast = "Jackson",
                    Phone = "0213344556",
                    BirthDate = new DateOnly(2004, 11, 5),
                    CreatedAt = seedTime.AddDays(-80),
                    EmailConfirmed = true
                };
                await userManager.CreateAsync(student3, "Student@1234");
                await userManager.AddToRoleAsync(student3, "Student");

                var student4 = new User
                {
                    UserName = "charlotte.white@tutorbridge.com",
                    Email = "charlotte.white@tutorbridge.com",
                    NameFirst = "Charlotte",
                    NameLast = "White",
                    Phone = "0214455667",
                    BirthDate = new DateOnly(2005, 9, 15),
                    CreatedAt = seedTime.AddDays(-75),
                    EmailConfirmed = true
                };
                await userManager.CreateAsync(student4, "Student@1234");
                await userManager.AddToRoleAsync(student4, "Student");

                var student5 = new User
                {
                    UserName = "alexander.harris@tutorbridge.com",
                    Email = "alexander.harris@tutorbridge.com",
                    NameFirst = "Alexander",
                    NameLast = "Harris",
                    Phone = "0215566778",
                    BirthDate = new DateOnly(2006, 1, 28),
                    CreatedAt = seedTime.AddDays(-70),
                    EmailConfirmed = true
                };
                await userManager.CreateAsync(student5, "Student@1234");
                await userManager.AddToRoleAsync(student5, "Student");

                var student6 = new User
                {
                    UserName = "amelia.martin@tutorbridge.com",
                    Email = "amelia.martin@tutorbridge.com",
                    NameFirst = "Amelia",
                    NameLast = "Martin",
                    Phone = "0216677889",
                    BirthDate = new DateOnly(2004, 6, 3),
                    CreatedAt = seedTime.AddDays(-65),
                    EmailConfirmed = true
                };
                await userManager.CreateAsync(student6, "Student@1234");
                await userManager.AddToRoleAsync(student6, "Student");

                var student7 = new User
                {
                    UserName = "mason.thompson@tutorbridge.com",
                    Email = "mason.thompson@tutorbridge.com",
                    NameFirst = "Mason",
                    NameLast = "Thompson",
                    Phone = "0217788990",
                    BirthDate = new DateOnly(2005, 4, 17),
                    CreatedAt = seedTime.AddDays(-60),
                    EmailConfirmed = true
                };
                await userManager.CreateAsync(student7, "Student@1234");
                await userManager.AddToRoleAsync(student7, "Student");

                var student8 = new User
                {
                    UserName = "harper.robinson@tutorbridge.com",
                    Email = "harper.robinson@tutorbridge.com",
                    NameFirst = "Harper",
                    NameLast = "Robinson",
                    Phone = "0218899001",
                    BirthDate = new DateOnly(2006, 10, 9),
                    CreatedAt = seedTime.AddDays(-55),
                    EmailConfirmed = true
                };
                await userManager.CreateAsync(student8, "Student@1234");
                await userManager.AddToRoleAsync(student8, "Student");

                var student9 = new User
                {
                    UserName = "ethan.clark@tutorbridge.com",
                    Email = "ethan.clark@tutorbridge.com",
                    NameFirst = "Ethan",
                    NameLast = "Clark",
                    Phone = "0219900112",
                    BirthDate = new DateOnly(2004, 2, 22),
                    CreatedAt = seedTime.AddDays(-50),
                    EmailConfirmed = true
                };
                await userManager.CreateAsync(student9, "Student@1234");
                await userManager.AddToRoleAsync(student9, "Student");

                var student10 = new User
                {
                    UserName = "evelyn.lewis@tutorbridge.com",
                    Email = "evelyn.lewis@tutorbridge.com",
                    NameFirst = "Evelyn",
                    NameLast = "Lewis",
                    Phone = "0210011223",
                    BirthDate = new DateOnly(2005, 8, 14),
                    CreatedAt = seedTime.AddDays(-45),
                    EmailConfirmed = true
                };
                await userManager.CreateAsync(student10, "Student@1234");
                await userManager.AddToRoleAsync(student10, "Student");

                await context.SaveChangesAsync();
            }

            // =====================
            // TUTOR Subject (20)
            // =====================
            if (!context.TutorSubject.Any())
            {
                //var tutors = context.Users.Where(u => u.IsTutor).ToList();
                var tutors = (await userManager.GetUsersInRoleAsync("Tutor")).ToList();
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
            // Timeslot (90)
            // Spread from ~1 week in the past through ~6 weeks ahead,
            // so the calendar always has slots centred around the current date.
            // =====================
            if (!context.Timeslot.Any())
            {
                //var tutors = context.Users.Where(u => u.IsTutor).ToList();
                var tutors = (await userManager.GetUsersInRoleAsync("Tutor")).ToList();
                User T(string email) => tutors.First(t => t.Email == email);

                var now = DateTime.Now.Date;

                var timeslotSeed = new List<Timeslot>
                {
                    new Timeslot { TutorId = T("liam.williams@tutorbridge.com").Id, DateTimeStart = now.AddDays(33).AddHours(15), DateTimeEnd = now.AddDays(33).AddHours(16) },
                    new Timeslot { TutorId = T("olivia.brown@tutorbridge.com").Id, DateTimeStart = now.AddDays(0).AddHours(14), DateTimeEnd = now.AddDays(0).AddHours(15) },
                    new Timeslot { TutorId = T("noah.jones@tutorbridge.com").Id, DateTimeStart = now.AddDays(-6).AddHours(14), DateTimeEnd = now.AddDays(-6).AddHours(15) },
                    new Timeslot { TutorId = T("ava.garcia@tutorbridge.com").Id, DateTimeStart = now.AddDays(40).AddHours(10), DateTimeEnd = now.AddDays(40).AddHours(12) },
                    new Timeslot { TutorId = T("william.miller@tutorbridge.com").Id, DateTimeStart = now.AddDays(10).AddHours(13), DateTimeEnd = now.AddDays(10).AddHours(14) },
                    new Timeslot { TutorId = T("sophia.davis@tutorbridge.com").Id, DateTimeStart = now.AddDays(8).AddHours(15), DateTimeEnd = now.AddDays(8).AddHours(16) },
                    new Timeslot { TutorId = T("benjamin.wilson@tutorbridge.com").Id, DateTimeStart = now.AddDays(7).AddHours(15), DateTimeEnd = now.AddDays(7).AddHours(16) },
                    new Timeslot { TutorId = T("isabella.taylor@tutorbridge.com").Id, DateTimeStart = now.AddDays(1).AddHours(16), DateTimeEnd = now.AddDays(1).AddHours(17) },
                    new Timeslot { TutorId = T("liam.williams@tutorbridge.com").Id, DateTimeStart = now.AddDays(40).AddHours(10), DateTimeEnd = now.AddDays(40).AddHours(11) },
                    new Timeslot { TutorId = T("olivia.brown@tutorbridge.com").Id, DateTimeStart = now.AddDays(-1).AddHours(16), DateTimeEnd = now.AddDays(-1).AddHours(17) },
                    new Timeslot { TutorId = T("noah.jones@tutorbridge.com").Id, DateTimeStart = now.AddDays(36).AddHours(13), DateTimeEnd = now.AddDays(36).AddHours(14) },
                    new Timeslot { TutorId = T("ava.garcia@tutorbridge.com").Id, DateTimeStart = now.AddDays(40).AddHours(9), DateTimeEnd = now.AddDays(40).AddHours(10) },
                    new Timeslot { TutorId = T("william.miller@tutorbridge.com").Id, DateTimeStart = now.AddDays(27).AddHours(14), DateTimeEnd = now.AddDays(27).AddHours(15) },
                    new Timeslot { TutorId = T("sophia.davis@tutorbridge.com").Id, DateTimeStart = now.AddDays(-2).AddHours(10), DateTimeEnd = now.AddDays(-2).AddHours(12) },
                    new Timeslot { TutorId = T("benjamin.wilson@tutorbridge.com").Id, DateTimeStart = now.AddDays(30).AddHours(13), DateTimeEnd = now.AddDays(30).AddHours(15) },
                    new Timeslot { TutorId = T("isabella.taylor@tutorbridge.com").Id, DateTimeStart = now.AddDays(20).AddHours(10), DateTimeEnd = now.AddDays(20).AddHours(11) },
                    new Timeslot { TutorId = T("liam.williams@tutorbridge.com").Id, DateTimeStart = now.AddDays(-5).AddHours(10), DateTimeEnd = now.AddDays(-5).AddHours(11) },
                    new Timeslot { TutorId = T("olivia.brown@tutorbridge.com").Id, DateTimeStart = now.AddDays(-6).AddHours(15), DateTimeEnd = now.AddDays(-6).AddHours(16) },
                    new Timeslot { TutorId = T("noah.jones@tutorbridge.com").Id, DateTimeStart = now.AddDays(-2).AddHours(15), DateTimeEnd = now.AddDays(-2).AddHours(17) },
                    new Timeslot { TutorId = T("ava.garcia@tutorbridge.com").Id, DateTimeStart = now.AddDays(6).AddHours(14), DateTimeEnd = now.AddDays(6).AddHours(16) },
                    new Timeslot { TutorId = T("william.miller@tutorbridge.com").Id, DateTimeStart = now.AddDays(7).AddHours(11), DateTimeEnd = now.AddDays(7).AddHours(12) },
                    new Timeslot { TutorId = T("sophia.davis@tutorbridge.com").Id, DateTimeStart = now.AddDays(25).AddHours(10), DateTimeEnd = now.AddDays(25).AddHours(12) },
                    new Timeslot { TutorId = T("benjamin.wilson@tutorbridge.com").Id, DateTimeStart = now.AddDays(31).AddHours(9), DateTimeEnd = now.AddDays(31).AddHours(10) },
                    new Timeslot { TutorId = T("isabella.taylor@tutorbridge.com").Id, DateTimeStart = now.AddDays(-6).AddHours(16), DateTimeEnd = now.AddDays(-6).AddHours(17) },
                    new Timeslot { TutorId = T("liam.williams@tutorbridge.com").Id, DateTimeStart = now.AddDays(28).AddHours(10), DateTimeEnd = now.AddDays(28).AddHours(11) },
                    new Timeslot { TutorId = T("olivia.brown@tutorbridge.com").Id, DateTimeStart = now.AddDays(5).AddHours(16), DateTimeEnd = now.AddDays(5).AddHours(18) },
                    new Timeslot { TutorId = T("noah.jones@tutorbridge.com").Id, DateTimeStart = now.AddDays(38).AddHours(14), DateTimeEnd = now.AddDays(38).AddHours(15) },
                    new Timeslot { TutorId = T("ava.garcia@tutorbridge.com").Id, DateTimeStart = now.AddDays(34).AddHours(13), DateTimeEnd = now.AddDays(34).AddHours(15) },
                    new Timeslot { TutorId = T("william.miller@tutorbridge.com").Id, DateTimeStart = now.AddDays(37).AddHours(14), DateTimeEnd = now.AddDays(37).AddHours(16) },
                    new Timeslot { TutorId = T("sophia.davis@tutorbridge.com").Id, DateTimeStart = now.AddDays(27).AddHours(14), DateTimeEnd = now.AddDays(27).AddHours(15) },
                    new Timeslot { TutorId = T("benjamin.wilson@tutorbridge.com").Id, DateTimeStart = now.AddDays(19).AddHours(14), DateTimeEnd = now.AddDays(19).AddHours(15) },
                    new Timeslot { TutorId = T("isabella.taylor@tutorbridge.com").Id, DateTimeStart = now.AddDays(7).AddHours(15), DateTimeEnd = now.AddDays(7).AddHours(16) },
                    new Timeslot { TutorId = T("liam.williams@tutorbridge.com").Id, DateTimeStart = now.AddDays(21).AddHours(15), DateTimeEnd = now.AddDays(21).AddHours(16) },
                    new Timeslot { TutorId = T("olivia.brown@tutorbridge.com").Id, DateTimeStart = now.AddDays(30).AddHours(16), DateTimeEnd = now.AddDays(30).AddHours(17) },
                    new Timeslot { TutorId = T("noah.jones@tutorbridge.com").Id, DateTimeStart = now.AddDays(10).AddHours(9), DateTimeEnd = now.AddDays(10).AddHours(10) },
                    new Timeslot { TutorId = T("ava.garcia@tutorbridge.com").Id, DateTimeStart = now.AddDays(-7).AddHours(13), DateTimeEnd = now.AddDays(-7).AddHours(14) },
                    new Timeslot { TutorId = T("william.miller@tutorbridge.com").Id, DateTimeStart = now.AddDays(41).AddHours(13), DateTimeEnd = now.AddDays(41).AddHours(14) },
                    new Timeslot { TutorId = T("sophia.davis@tutorbridge.com").Id, DateTimeStart = now.AddDays(3).AddHours(15), DateTimeEnd = now.AddDays(3).AddHours(16) },
                    new Timeslot { TutorId = T("benjamin.wilson@tutorbridge.com").Id, DateTimeStart = now.AddDays(37).AddHours(14), DateTimeEnd = now.AddDays(37).AddHours(15) },
                    new Timeslot { TutorId = T("isabella.taylor@tutorbridge.com").Id, DateTimeStart = now.AddDays(20).AddHours(14), DateTimeEnd = now.AddDays(20).AddHours(15) },
                    new Timeslot { TutorId = T("liam.williams@tutorbridge.com").Id, DateTimeStart = now.AddDays(14).AddHours(16), DateTimeEnd = now.AddDays(14).AddHours(17) },
                    new Timeslot { TutorId = T("olivia.brown@tutorbridge.com").Id, DateTimeStart = now.AddDays(10).AddHours(16), DateTimeEnd = now.AddDays(10).AddHours(17) },
                    new Timeslot { TutorId = T("noah.jones@tutorbridge.com").Id, DateTimeStart = now.AddDays(2).AddHours(10), DateTimeEnd = now.AddDays(2).AddHours(11) },
                    new Timeslot { TutorId = T("ava.garcia@tutorbridge.com").Id, DateTimeStart = now.AddDays(6).AddHours(16), DateTimeEnd = now.AddDays(6).AddHours(17) },
                    new Timeslot { TutorId = T("william.miller@tutorbridge.com").Id, DateTimeStart = now.AddDays(41).AddHours(14), DateTimeEnd = now.AddDays(41).AddHours(15) },
                    new Timeslot { TutorId = T("sophia.davis@tutorbridge.com").Id, DateTimeStart = now.AddDays(14).AddHours(14), DateTimeEnd = now.AddDays(14).AddHours(15) },
                    new Timeslot { TutorId = T("benjamin.wilson@tutorbridge.com").Id, DateTimeStart = now.AddDays(-1).AddHours(13), DateTimeEnd = now.AddDays(-1).AddHours(14) },
                    new Timeslot { TutorId = T("isabella.taylor@tutorbridge.com").Id, DateTimeStart = now.AddDays(-2).AddHours(9), DateTimeEnd = now.AddDays(-2).AddHours(10) },
                    new Timeslot { TutorId = T("liam.williams@tutorbridge.com").Id, DateTimeStart = now.AddDays(17).AddHours(16), DateTimeEnd = now.AddDays(17).AddHours(17) },
                    new Timeslot { TutorId = T("olivia.brown@tutorbridge.com").Id, DateTimeStart = now.AddDays(-1).AddHours(10), DateTimeEnd = now.AddDays(-1).AddHours(11) },
                    new Timeslot { TutorId = T("noah.jones@tutorbridge.com").Id, DateTimeStart = now.AddDays(15).AddHours(10), DateTimeEnd = now.AddDays(15).AddHours(11) },
                    new Timeslot { TutorId = T("ava.garcia@tutorbridge.com").Id, DateTimeStart = now.AddDays(15).AddHours(9), DateTimeEnd = now.AddDays(15).AddHours(11) },
                    new Timeslot { TutorId = T("william.miller@tutorbridge.com").Id, DateTimeStart = now.AddDays(31).AddHours(16), DateTimeEnd = now.AddDays(31).AddHours(17) },
                    new Timeslot { TutorId = T("sophia.davis@tutorbridge.com").Id, DateTimeStart = now.AddDays(9).AddHours(16), DateTimeEnd = now.AddDays(9).AddHours(17) },
                    new Timeslot { TutorId = T("benjamin.wilson@tutorbridge.com").Id, DateTimeStart = now.AddDays(-5).AddHours(10), DateTimeEnd = now.AddDays(-5).AddHours(12) },
                    new Timeslot { TutorId = T("isabella.taylor@tutorbridge.com").Id, DateTimeStart = now.AddDays(39).AddHours(14), DateTimeEnd = now.AddDays(39).AddHours(15) },
                    new Timeslot { TutorId = T("liam.williams@tutorbridge.com").Id, DateTimeStart = now.AddDays(22).AddHours(11), DateTimeEnd = now.AddDays(22).AddHours(13) },
                    new Timeslot { TutorId = T("olivia.brown@tutorbridge.com").Id, DateTimeStart = now.AddDays(27).AddHours(10), DateTimeEnd = now.AddDays(27).AddHours(11) },
                    new Timeslot { TutorId = T("noah.jones@tutorbridge.com").Id, DateTimeStart = now.AddDays(0).AddHours(15), DateTimeEnd = now.AddDays(0).AddHours(16) },
                    new Timeslot { TutorId = T("ava.garcia@tutorbridge.com").Id, DateTimeStart = now.AddDays(17).AddHours(13), DateTimeEnd = now.AddDays(17).AddHours(14) },
                    new Timeslot { TutorId = T("william.miller@tutorbridge.com").Id, DateTimeStart = now.AddDays(-2).AddHours(13), DateTimeEnd = now.AddDays(-2).AddHours(15) },
                    new Timeslot { TutorId = T("sophia.davis@tutorbridge.com").Id, DateTimeStart = now.AddDays(28).AddHours(9), DateTimeEnd = now.AddDays(28).AddHours(10) },
                    new Timeslot { TutorId = T("benjamin.wilson@tutorbridge.com").Id, DateTimeStart = now.AddDays(11).AddHours(10), DateTimeEnd = now.AddDays(11).AddHours(11) },
                    new Timeslot { TutorId = T("isabella.taylor@tutorbridge.com").Id, DateTimeStart = now.AddDays(33).AddHours(11), DateTimeEnd = now.AddDays(33).AddHours(12) },
                    new Timeslot { TutorId = T("liam.williams@tutorbridge.com").Id, DateTimeStart = now.AddDays(32).AddHours(14), DateTimeEnd = now.AddDays(32).AddHours(15) },
                    new Timeslot { TutorId = T("olivia.brown@tutorbridge.com").Id, DateTimeStart = now.AddDays(16).AddHours(14), DateTimeEnd = now.AddDays(16).AddHours(15) },
                    new Timeslot { TutorId = T("noah.jones@tutorbridge.com").Id, DateTimeStart = now.AddDays(29).AddHours(9), DateTimeEnd = now.AddDays(29).AddHours(10) },
                    new Timeslot { TutorId = T("ava.garcia@tutorbridge.com").Id, DateTimeStart = now.AddDays(5).AddHours(15), DateTimeEnd = now.AddDays(5).AddHours(16) },
                    new Timeslot { TutorId = T("william.miller@tutorbridge.com").Id, DateTimeStart = now.AddDays(38).AddHours(10), DateTimeEnd = now.AddDays(38).AddHours(11) },
                    new Timeslot { TutorId = T("sophia.davis@tutorbridge.com").Id, DateTimeStart = now.AddDays(-3).AddHours(9), DateTimeEnd = now.AddDays(-3).AddHours(10) },
                    new Timeslot { TutorId = T("benjamin.wilson@tutorbridge.com").Id, DateTimeStart = now.AddDays(-5).AddHours(9), DateTimeEnd = now.AddDays(-5).AddHours(10) },
                    new Timeslot { TutorId = T("isabella.taylor@tutorbridge.com").Id, DateTimeStart = now.AddDays(35).AddHours(11), DateTimeEnd = now.AddDays(35).AddHours(13) },
                    new Timeslot { TutorId = T("liam.williams@tutorbridge.com").Id, DateTimeStart = now.AddDays(7).AddHours(10), DateTimeEnd = now.AddDays(7).AddHours(11) },
                    new Timeslot { TutorId = T("olivia.brown@tutorbridge.com").Id, DateTimeStart = now.AddDays(11).AddHours(15), DateTimeEnd = now.AddDays(11).AddHours(17) },
                    new Timeslot { TutorId = T("noah.jones@tutorbridge.com").Id, DateTimeStart = now.AddDays(-2).AddHours(10), DateTimeEnd = now.AddDays(-2).AddHours(12) },
                    new Timeslot { TutorId = T("ava.garcia@tutorbridge.com").Id, DateTimeStart = now.AddDays(7).AddHours(16), DateTimeEnd = now.AddDays(7).AddHours(18) },
                    new Timeslot { TutorId = T("william.miller@tutorbridge.com").Id, DateTimeStart = now.AddDays(-1).AddHours(10), DateTimeEnd = now.AddDays(-1).AddHours(11) },
                    new Timeslot { TutorId = T("sophia.davis@tutorbridge.com").Id, DateTimeStart = now.AddDays(17).AddHours(9), DateTimeEnd = now.AddDays(17).AddHours(11) },
                    new Timeslot { TutorId = T("benjamin.wilson@tutorbridge.com").Id, DateTimeStart = now.AddDays(10).AddHours(11), DateTimeEnd = now.AddDays(10).AddHours(13) },
                    new Timeslot { TutorId = T("isabella.taylor@tutorbridge.com").Id, DateTimeStart = now.AddDays(22).AddHours(13), DateTimeEnd = now.AddDays(22).AddHours(15) },
                    new Timeslot { TutorId = T("liam.williams@tutorbridge.com").Id, DateTimeStart = now.AddDays(33).AddHours(16), DateTimeEnd = now.AddDays(33).AddHours(17) },
                    new Timeslot { TutorId = T("olivia.brown@tutorbridge.com").Id, DateTimeStart = now.AddDays(16).AddHours(15), DateTimeEnd = now.AddDays(16).AddHours(16) },
                    new Timeslot { TutorId = T("noah.jones@tutorbridge.com").Id, DateTimeStart = now.AddDays(3).AddHours(9), DateTimeEnd = now.AddDays(3).AddHours(11) },
                    new Timeslot { TutorId = T("ava.garcia@tutorbridge.com").Id, DateTimeStart = now.AddDays(16).AddHours(15), DateTimeEnd = now.AddDays(16).AddHours(16) },
                    new Timeslot { TutorId = T("william.miller@tutorbridge.com").Id, DateTimeStart = now.AddDays(15).AddHours(16), DateTimeEnd = now.AddDays(15).AddHours(17) },
                    new Timeslot { TutorId = T("sophia.davis@tutorbridge.com").Id, DateTimeStart = now.AddDays(6).AddHours(10), DateTimeEnd = now.AddDays(6).AddHours(11) },
                    new Timeslot { TutorId = T("benjamin.wilson@tutorbridge.com").Id, DateTimeStart = now.AddDays(35).AddHours(10), DateTimeEnd = now.AddDays(35).AddHours(12) },
                    new Timeslot { TutorId = T("isabella.taylor@tutorbridge.com").Id, DateTimeStart = now.AddDays(10).AddHours(10), DateTimeEnd = now.AddDays(10).AddHours(12) },
                    new Timeslot { TutorId = T("liam.williams@tutorbridge.com").Id, DateTimeStart = now.AddDays(37).AddHours(10), DateTimeEnd = now.AddDays(37).AddHours(11) },
                    new Timeslot { TutorId = T("olivia.brown@tutorbridge.com").Id, DateTimeStart = now.AddDays(36).AddHours(13), DateTimeEnd = now.AddDays(36).AddHours(14) }
                };

                // A tutor opens a slot up some days before it happens, never after "now".
                for (int i = 0; i < timeslotSeed.Count; i++)
                {
                    var ts = timeslotSeed[i];
                    var leadDays = 3 + (i % 12); // stagger 3-14 days of lead time
                    var candidate = ts.DateTimeStart.AddDays(-leadDays);
                    ts.CreatedAt = candidate < seedTime ? candidate : seedTime.AddDays(-(1 + i % 3));
                }

                context.Timeslot.AddRange(timeslotSeed);
                await context.SaveChangesAsync();
            }

            // =====================
            // Booking (72)
            // =====================
            if (!context.Booking.Any())
            {
                //var students = context.Users.Where(u => !u.IsTutor && !u.IsAdmin).ToList();
                var students = (await userManager.GetUsersInRoleAsync("Student")).ToList();
                var Timeslot = context.Timeslot.ToList();
                var Subject = context.Subject.ToList();

                User St(string email) => students.First(s => s.Email == email);
                Subject S(string name) => Subject.First(s => s.Name == name);

                var bookingSeed = new List<Booking>
                {
                    new Booking { UserId = St("lucas.anderson@tutorbridge.com").Id, TimeslotId = Timeslot[0].TimeslotId, SubjectId = S("Physics").SubjectId, Status = Booking.BookingStatus.Confirmed },
                    new Booking { UserId = St("mia.thomas@tutorbridge.com").Id, TimeslotId = Timeslot[1].TimeslotId, SubjectId = S("English").SubjectId, Status = Booking.BookingStatus.Pending },
                    new Booking { UserId = St("henry.jackson@tutorbridge.com").Id, TimeslotId = Timeslot[2].TimeslotId, SubjectId = S("Science").SubjectId, Status = Booking.BookingStatus.Cancelled },
                    new Booking { UserId = St("charlotte.white@tutorbridge.com").Id, TimeslotId = Timeslot[3].TimeslotId, SubjectId = S("French").SubjectId, Status = Booking.BookingStatus.Confirmed },
                    new Booking { UserId = St("alexander.harris@tutorbridge.com").Id, TimeslotId = Timeslot[4].TimeslotId, SubjectId = S("Computer Science").SubjectId, Status = Booking.BookingStatus.Confirmed },
                    new Booking { UserId = St("amelia.martin@tutorbridge.com").Id, TimeslotId = Timeslot[5].TimeslotId, SubjectId = S("Art").SubjectId, Status = Booking.BookingStatus.Pending },
                    new Booking { UserId = St("mason.thompson@tutorbridge.com").Id, TimeslotId = Timeslot[6].TimeslotId, SubjectId = S("Philosophy").SubjectId, Status = Booking.BookingStatus.Confirmed },
                    new Booking { UserId = St("harper.robinson@tutorbridge.com").Id, TimeslotId = Timeslot[7].TimeslotId, SubjectId = S("Biology").SubjectId, Status = Booking.BookingStatus.Pending },
                    new Booking { UserId = St("ethan.clark@tutorbridge.com").Id, TimeslotId = Timeslot[8].TimeslotId, SubjectId = S("Statistics").SubjectId, Status = Booking.BookingStatus.Cancelled },
                    new Booking { UserId = St("evelyn.lewis@tutorbridge.com").Id, TimeslotId = Timeslot[9].TimeslotId, SubjectId = S("History").SubjectId, Status = Booking.BookingStatus.Confirmed },
                    new Booking { UserId = St("lucas.anderson@tutorbridge.com").Id, TimeslotId = Timeslot[10].TimeslotId, SubjectId = S("Science").SubjectId, Status = Booking.BookingStatus.Confirmed },
                    new Booking { UserId = St("mia.thomas@tutorbridge.com").Id, TimeslotId = Timeslot[11].TimeslotId, SubjectId = S("French").SubjectId, Status = Booking.BookingStatus.Pending },
                    new Booking { UserId = St("henry.jackson@tutorbridge.com").Id, TimeslotId = Timeslot[12].TimeslotId, SubjectId = S("Computer Science").SubjectId, Status = Booking.BookingStatus.Confirmed },
                    new Booking { UserId = St("charlotte.white@tutorbridge.com").Id, TimeslotId = Timeslot[13].TimeslotId, SubjectId = S("Art").SubjectId, Status = Booking.BookingStatus.Pending },
                    new Booking { UserId = St("alexander.harris@tutorbridge.com").Id, TimeslotId = Timeslot[14].TimeslotId, SubjectId = S("Accounting").SubjectId, Status = Booking.BookingStatus.Cancelled },
                    new Booking { UserId = St("amelia.martin@tutorbridge.com").Id, TimeslotId = Timeslot[15].TimeslotId, SubjectId = S("Psychology").SubjectId, Status = Booking.BookingStatus.Confirmed },
                    new Booking { UserId = St("mason.thompson@tutorbridge.com").Id, TimeslotId = Timeslot[18].TimeslotId, SubjectId = S("Science").SubjectId, Status = Booking.BookingStatus.Confirmed },
                    new Booking { UserId = St("harper.robinson@tutorbridge.com").Id, TimeslotId = Timeslot[19].TimeslotId, SubjectId = S("French").SubjectId, Status = Booking.BookingStatus.Pending },
                    new Booking { UserId = St("ethan.clark@tutorbridge.com").Id, TimeslotId = Timeslot[20].TimeslotId, SubjectId = S("Mathematics").SubjectId, Status = Booking.BookingStatus.Confirmed },
                    new Booking { UserId = St("evelyn.lewis@tutorbridge.com").Id, TimeslotId = Timeslot[21].TimeslotId, SubjectId = S("Art").SubjectId, Status = Booking.BookingStatus.Pending },
                    new Booking { UserId = St("lucas.anderson@tutorbridge.com").Id, TimeslotId = Timeslot[22].TimeslotId, SubjectId = S("Philosophy").SubjectId, Status = Booking.BookingStatus.Cancelled },
                    new Booking { UserId = St("mia.thomas@tutorbridge.com").Id, TimeslotId = Timeslot[25].TimeslotId, SubjectId = S("History").SubjectId, Status = Booking.BookingStatus.Confirmed },
                    new Booking { UserId = St("henry.jackson@tutorbridge.com").Id, TimeslotId = Timeslot[26].TimeslotId, SubjectId = S("Japanese").SubjectId, Status = Booking.BookingStatus.Confirmed },
                    new Booking { UserId = St("charlotte.white@tutorbridge.com").Id, TimeslotId = Timeslot[27].TimeslotId, SubjectId = S("Spanish").SubjectId, Status = Booking.BookingStatus.Pending },
                    new Booking { UserId = St("alexander.harris@tutorbridge.com").Id, TimeslotId = Timeslot[29].TimeslotId, SubjectId = S("Art").SubjectId, Status = Booking.BookingStatus.Confirmed },
                    new Booking { UserId = St("amelia.martin@tutorbridge.com").Id, TimeslotId = Timeslot[30].TimeslotId, SubjectId = S("Accounting").SubjectId, Status = Booking.BookingStatus.Pending },
                    new Booking { UserId = St("mason.thompson@tutorbridge.com").Id, TimeslotId = Timeslot[31].TimeslotId, SubjectId = S("Biology").SubjectId, Status = Booking.BookingStatus.Cancelled },
                    new Booking { UserId = St("harper.robinson@tutorbridge.com").Id, TimeslotId = Timeslot[32].TimeslotId, SubjectId = S("Mathematics").SubjectId, Status = Booking.BookingStatus.Confirmed },
                    new Booking { UserId = St("ethan.clark@tutorbridge.com").Id, TimeslotId = Timeslot[33].TimeslotId, SubjectId = S("English").SubjectId, Status = Booking.BookingStatus.Confirmed },
                    new Booking { UserId = St("evelyn.lewis@tutorbridge.com").Id, TimeslotId = Timeslot[34].TimeslotId, SubjectId = S("Chemistry").SubjectId, Status = Booking.BookingStatus.Pending },
                    new Booking { UserId = St("lucas.anderson@tutorbridge.com").Id, TimeslotId = Timeslot[36].TimeslotId, SubjectId = S("Mathematics").SubjectId, Status = Booking.BookingStatus.Confirmed },
                    new Booking { UserId = St("mia.thomas@tutorbridge.com").Id, TimeslotId = Timeslot[37].TimeslotId, SubjectId = S("Art").SubjectId, Status = Booking.BookingStatus.Pending },
                    new Booking { UserId = St("henry.jackson@tutorbridge.com").Id, TimeslotId = Timeslot[38].TimeslotId, SubjectId = S("Philosophy").SubjectId, Status = Booking.BookingStatus.Cancelled },
                    new Booking { UserId = St("charlotte.white@tutorbridge.com").Id, TimeslotId = Timeslot[39].TimeslotId, SubjectId = S("Psychology").SubjectId, Status = Booking.BookingStatus.Confirmed },
                    new Booking { UserId = St("alexander.harris@tutorbridge.com").Id, TimeslotId = Timeslot[40].TimeslotId, SubjectId = S("Mathematics").SubjectId, Status = Booking.BookingStatus.Confirmed },
                    new Booking { UserId = St("amelia.martin@tutorbridge.com").Id, TimeslotId = Timeslot[41].TimeslotId, SubjectId = S("History").SubjectId, Status = Booking.BookingStatus.Pending },
                    new Booking { UserId = St("mason.thompson@tutorbridge.com").Id, TimeslotId = Timeslot[43].TimeslotId, SubjectId = S("French").SubjectId, Status = Booking.BookingStatus.Confirmed },
                    new Booking { UserId = St("harper.robinson@tutorbridge.com").Id, TimeslotId = Timeslot[46].TimeslotId, SubjectId = S("Accounting").SubjectId, Status = Booking.BookingStatus.Pending },
                    new Booking { UserId = St("ethan.clark@tutorbridge.com").Id, TimeslotId = Timeslot[47].TimeslotId, SubjectId = S("Biology").SubjectId, Status = Booking.BookingStatus.Cancelled },
                    new Booking { UserId = St("evelyn.lewis@tutorbridge.com").Id, TimeslotId = Timeslot[48].TimeslotId, SubjectId = S("Mathematics").SubjectId, Status = Booking.BookingStatus.Confirmed },
                    new Booking { UserId = St("lucas.anderson@tutorbridge.com").Id, TimeslotId = Timeslot[49].TimeslotId, SubjectId = S("History").SubjectId, Status = Booking.BookingStatus.Confirmed },
                    new Booking { UserId = St("mia.thomas@tutorbridge.com").Id, TimeslotId = Timeslot[51].TimeslotId, SubjectId = S("French").SubjectId, Status = Booking.BookingStatus.Pending },
                    new Booking { UserId = St("henry.jackson@tutorbridge.com").Id, TimeslotId = Timeslot[52].TimeslotId, SubjectId = S("Computer Science").SubjectId, Status = Booking.BookingStatus.Confirmed },
                    new Booking { UserId = St("charlotte.white@tutorbridge.com").Id, TimeslotId = Timeslot[53].TimeslotId, SubjectId = S("Art").SubjectId, Status = Booking.BookingStatus.Pending },
                    new Booking { UserId = St("alexander.harris@tutorbridge.com").Id, TimeslotId = Timeslot[54].TimeslotId, SubjectId = S("Accounting").SubjectId, Status = Booking.BookingStatus.Cancelled },
                    new Booking { UserId = St("amelia.martin@tutorbridge.com").Id, TimeslotId = Timeslot[55].TimeslotId, SubjectId = S("Biology").SubjectId, Status = Booking.BookingStatus.Confirmed },
                    new Booking { UserId = St("mason.thompson@tutorbridge.com").Id, TimeslotId = Timeslot[56].TimeslotId, SubjectId = S("Statistics").SubjectId, Status = Booking.BookingStatus.Confirmed },
                    new Booking { UserId = St("harper.robinson@tutorbridge.com").Id, TimeslotId = Timeslot[57].TimeslotId, SubjectId = S("History").SubjectId, Status = Booking.BookingStatus.Pending },
                    new Booking { UserId = St("ethan.clark@tutorbridge.com").Id, TimeslotId = Timeslot[58].TimeslotId, SubjectId = S("Science").SubjectId, Status = Booking.BookingStatus.Confirmed },
                    new Booking { UserId = St("evelyn.lewis@tutorbridge.com").Id, TimeslotId = Timeslot[59].TimeslotId, SubjectId = S("French").SubjectId, Status = Booking.BookingStatus.Pending },
                    new Booking { UserId = St("lucas.anderson@tutorbridge.com").Id, TimeslotId = Timeslot[61].TimeslotId, SubjectId = S("Music").SubjectId, Status = Booking.BookingStatus.Cancelled },
                    new Booking { UserId = St("mia.thomas@tutorbridge.com").Id, TimeslotId = Timeslot[62].TimeslotId, SubjectId = S("Philosophy").SubjectId, Status = Booking.BookingStatus.Confirmed },
                    new Booking { UserId = St("henry.jackson@tutorbridge.com").Id, TimeslotId = Timeslot[64].TimeslotId, SubjectId = S("Mathematics").SubjectId, Status = Booking.BookingStatus.Confirmed },
                    new Booking { UserId = St("charlotte.white@tutorbridge.com").Id, TimeslotId = Timeslot[66].TimeslotId, SubjectId = S("Science").SubjectId, Status = Booking.BookingStatus.Pending },
                    new Booking { UserId = St("alexander.harris@tutorbridge.com").Id, TimeslotId = Timeslot[67].TimeslotId, SubjectId = S("Spanish").SubjectId, Status = Booking.BookingStatus.Confirmed },
                    new Booking { UserId = St("amelia.martin@tutorbridge.com").Id, TimeslotId = Timeslot[68].TimeslotId, SubjectId = S("Computer Science").SubjectId, Status = Booking.BookingStatus.Pending },
                    new Booking { UserId = St("mason.thompson@tutorbridge.com").Id, TimeslotId = Timeslot[69].TimeslotId, SubjectId = S("Art").SubjectId, Status = Booking.BookingStatus.Cancelled },
                    new Booking { UserId = St("harper.robinson@tutorbridge.com").Id, TimeslotId = Timeslot[70].TimeslotId, SubjectId = S("Accounting").SubjectId, Status = Booking.BookingStatus.Confirmed },
                    new Booking { UserId = St("ethan.clark@tutorbridge.com").Id, TimeslotId = Timeslot[71].TimeslotId, SubjectId = S("Geography").SubjectId, Status = Booking.BookingStatus.Confirmed },
                    new Booking { UserId = St("evelyn.lewis@tutorbridge.com").Id, TimeslotId = Timeslot[72].TimeslotId, SubjectId = S("Statistics").SubjectId, Status = Booking.BookingStatus.Pending },
                    new Booking { UserId = St("lucas.anderson@tutorbridge.com").Id, TimeslotId = Timeslot[73].TimeslotId, SubjectId = S("English").SubjectId, Status = Booking.BookingStatus.Confirmed },
                    new Booking { UserId = St("mia.thomas@tutorbridge.com").Id, TimeslotId = Timeslot[74].TimeslotId, SubjectId = S("Chemistry").SubjectId, Status = Booking.BookingStatus.Pending },
                    new Booking { UserId = St("henry.jackson@tutorbridge.com").Id, TimeslotId = Timeslot[77].TimeslotId, SubjectId = S("Art").SubjectId, Status = Booking.BookingStatus.Cancelled },
                    new Booking { UserId = St("charlotte.white@tutorbridge.com").Id, TimeslotId = Timeslot[78].TimeslotId, SubjectId = S("Economics").SubjectId, Status = Booking.BookingStatus.Confirmed },
                    new Booking { UserId = St("alexander.harris@tutorbridge.com").Id, TimeslotId = Timeslot[79].TimeslotId, SubjectId = S("Psychology").SubjectId, Status = Booking.BookingStatus.Confirmed },
                    new Booking { UserId = St("amelia.martin@tutorbridge.com").Id, TimeslotId = Timeslot[80].TimeslotId, SubjectId = S("Physics").SubjectId, Status = Booking.BookingStatus.Pending },
                    new Booking { UserId = St("mason.thompson@tutorbridge.com").Id, TimeslotId = Timeslot[81].TimeslotId, SubjectId = S("English").SubjectId, Status = Booking.BookingStatus.Confirmed },
                    new Booking { UserId = St("harper.robinson@tutorbridge.com").Id, TimeslotId = Timeslot[83].TimeslotId, SubjectId = S("French").SubjectId, Status = Booking.BookingStatus.Pending },
                    new Booking { UserId = St("ethan.clark@tutorbridge.com").Id, TimeslotId = Timeslot[84].TimeslotId, SubjectId = S("Computer Science").SubjectId, Status = Booking.BookingStatus.Cancelled },
                    new Booking { UserId = St("evelyn.lewis@tutorbridge.com").Id, TimeslotId = Timeslot[85].TimeslotId, SubjectId = S("Art").SubjectId, Status = Booking.BookingStatus.Confirmed },
                    new Booking { UserId = St("lucas.anderson@tutorbridge.com").Id, TimeslotId = Timeslot[86].TimeslotId, SubjectId = S("Philosophy").SubjectId, Status = Booking.BookingStatus.Confirmed },
                    new Booking { UserId = St("mia.thomas@tutorbridge.com").Id, TimeslotId = Timeslot[89].TimeslotId, SubjectId = S("English").SubjectId, Status = Booking.BookingStatus.Pending }
                };

                // A booking is made a day or so after its timeslot became available,
                // but never after the session's start time (or after "now" for future slots).
                foreach (var booking in bookingSeed)
                {
                    var relatedTimeslot = Timeslot.First(t => t.TimeslotId == booking.TimeslotId);
                    var cap = relatedTimeslot.DateTimeStart < seedTime ? relatedTimeslot.DateTimeStart : seedTime;
                    var candidate = relatedTimeslot.CreatedAt.AddDays(1);
                    booking.CreatedAt = candidate < cap ? candidate : cap.AddHours(-1);
                }

                context.Booking.AddRange(bookingSeed);
                await context.SaveChangesAsync();
            }
        }
    }
}