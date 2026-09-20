# TutorBridge

A tutoring discovery and booking platform built so tutors can be found and booked without needing a pre-existing personal connection — and so the actual scheduling doesn't fall back to text messages once contact is made.

![.NET](https://img.shields.io/badge/.NET-10-512BD4)
![ASP.NET Core](https://img.shields.io/badge/ASP.NET%20Core-MVC-512BD4)
![EF Core](https://img.shields.io/badge/EF%20Core-10-68217A)
![SQL Server](https://img.shields.io/badge/SQL%20Server-LocalDB-CC2927)

## Overview

TutorBridge lets students discover tutors, browse their availability on a calendar, and book a session directly through the site. Tutors manage their own availability and bookings through a dedicated dashboard, and admins get oversight of the whole platform — users, bookings, and tutor applications.

Every account registers as a student by default. To teach, a user submits a **tutor application**, which an admin reviews and approves or denies — there's no role picker at sign-up.

## Features

- **Role-based access** for Student, Tutor, and Admin, each with its own dashboard and permissions
- **Live availability calendar** (FullCalendar) with hover tooltips showing tutor and subject before booking
- **Tutor application workflow** — students apply to become tutors; admins approve or deny with a reason
- **Admin analytics dashboard** (Chart.js) — booking volume and trends across the platform
- **Soft-delete throughout** — cancelled bookings, removed users, and deleted timeslots preserve history instead of destroying it
- **Double-booking prevention** enforced at the database level via a filtered unique index, not just in application code

## Tech Stack

| Layer | Technology |
|---|---|
| Framework | ASP.NET Core MVC (.NET 10, LTS) |
| Data access | Entity Framework Core 10 + SQL Server |
| Auth & roles | ASP.NET Core Identity 10 |
| Calendar | FullCalendar |
| Tooltips | Tippy.js |
| Charts | Chart.js |
| Image processing | SixLabors.ImageSharp |
| Styling | Bootstrap 5 |

.NET 10 was chosen as it's the current LTS release, supported until November 2028 — both .NET 8 and .NET 9 reach end of support in November 2026.

## Getting Started

### Prerequisites

- Visual Studio 2026 (or later) with the ASP.NET and web development workload
- SQL Server LocalDB (installed automatically with the Visual Studio workload above — no separate database server needed)

### Installation

1. Clone the repository:
   ```bash
   git clone https://github.com/ScarrleTRain/TutorBridge.git
   ```
2. Open `TutorBridge.slnx` in Visual Studio.
3. Restore NuGet packages — this happens automatically on build, or manually via **Tools → NuGet Package Manager**.
4. The project is pre-configured to use SQL Server LocalDB for local development (see `appsettings.json`).
5. Open the **Package Manager Console** and apply the migrations:
   ```powershell
   Update-Database
   ```
6. Run the project (`F5`) — it will launch in your default browser.

## Test Accounts

Seeded by `DbSeeder.cs` — use these to evaluate the app without registering separate accounts for each role:

| Role | Email | Password |
|---|---|---|
| Student | `lucas.anderson@tutorbridge.com` | `Student@1234` |
| Tutor | `liam.williams@tutorbridge.com` | `Tutor@1234` |
| Admin | `james.smith@tutorbridge.com` | `Admin@1234` |

## Author

Built by Felix Wong as an NCEA Level 3 Digital Technologies assessment project.
