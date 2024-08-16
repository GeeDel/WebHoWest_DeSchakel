using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Pri.WebApi.DeSchakel.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;

namespace Pri.WebApi.DeSchakel.Core.Data.Seeding
{
    public static class Seeder
    {
        //  private static readonly IWebHostEnvironment _webHostEnvironment;



        public static void Seed(ModelBuilder modelBuilder) //IWebHostEnvironment webHostEnvironment)
        {
            // locations
            var locations = new Location[]
                {
                         new Location{Id = 1,Name = "Schouwburg", Capacity = 500 },
                         new Location{Id = 2,Name = "Schakelbox" , Capacity = 192},
                         new Location{Id = 3, Name = "Cinema", Capacity = 200}
            };
            // genre
            var genres = new Genre[]
             {
                         new Genre{Id = 1,Name = "Circus"},
                         new Genre{Id = 2,Name = "Dans"},
                         new Genre{Id = 3, Name = "Doen" } ,
                         new Genre {Id = 4, Name = "Familie"},
                         new Genre {Id = 5, Name = "Film"},
                         new Genre {Id = 6, Name = "Humor"},
                         new Genre{Id = 7,Name = "Muziek"},
                         new Genre{Id = 8,Name = "Theater"},
             };
            // company
            var companies = new Company[]
            {
                         new Company{Id = 1,Name = "Jan De Smet" , Email= "jan@desmet.be", Phonenumber= "123/456789"},
                         new Company{Id = 2,Name = "Alex Agnew" , Email = "alex@agnew.be", Phonenumber = "789/456123"},
                         new Company{Id = 3, Name= "Selah Sue", Email= "selah@sue.be", Phonenumber= "321/654987"},
                         new Company{Id = 4, Name= "Stedelijke Kunstacademie Waregem", Email= "ka@waregem.be", Phonenumber="056/123456"},
                         new Company{Id = 5, Name= "Eigenproductie-doen", Email="maaike@cc.be", Phonenumber = "056/610061"}
            };

            // role
            const string AdminRoleId = "00000000-0000-0000-0000-000000000001";
            const string AdminRoleName = "Admin";
            const string ProgrammatorRoleId = "00000000-0000-0000-0000-000000000002";
            const string ProgrammatorRoleName = "Programmator";
            const string ReceptionRoleId = "00000000-0000-0000-0000-000000000003";
            const string ReceptionRoleName = "Onthaal";
            const string VisitorRoleId = "00000000-0000-0000-0000-000000000004";
            const string VisitorRoleName = "Bezoeker";
            var roles = new[]
            {
                 new IdentityRole
            {
                Id = AdminRoleId,
                Name = AdminRoleName,
                NormalizedName = AdminRoleName.ToUpper()
            },
                new IdentityRole
                {
                    Id = ProgrammatorRoleId,
                    Name = ProgrammatorRoleName,
                    NormalizedName = ProgrammatorRoleName.ToUpper()
                },
                new IdentityRole
                {
                    Id = ReceptionRoleId,
                    Name = ReceptionRoleName ,
                    NormalizedName = ReceptionRoleName.ToUpper()
                },
                new IdentityRole
                {
                    Id = VisitorRoleId,
                    Name = VisitorRoleName ,
                    NormalizedName = VisitorRoleName.ToUpper()
                }
            };

            // users
            // admin user

            const string AdminUserId = "00000000-0000-0000-0000-000000000001";
            const string AdminUserName = "admin@cc.be";
            const string AdminUserPassword = "12"; // For demo purposes only! Don't do this in real application!
            IPasswordHasher<ApplicationUser> passwordHasher = new PasswordHasher<ApplicationUser>(); // Identity password hasher

            ApplicationUser adminApplicationUser = new ApplicationUser
            {
                Id = AdminUserId,
                UserName = AdminUserName,
                NormalizedUserName = AdminUserName.ToUpper(),
                Email = AdminUserName,
                NormalizedEmail = AdminUserName.ToUpper(),
                EmailConfirmed = true,
                SecurityStamp = "VVPCRDAS3MJWQD5CSW2GWPRADBXEZINA", //Random string
                ConcurrencyStamp = "c8554266-b401-4519-9aeb-a9283053fc58", //Random guid string
                Firstname = "Geert",
                Lastname = "Deloof",
                Zipcode = "8791",
                City = "Beveren-Leie"
            };

            adminApplicationUser.PasswordHash = passwordHasher.HashPassword(adminApplicationUser, AdminUserPassword);
            // programmator users
            // first
            const string progUserId1 = "00000000-0000-0000-0000-000000000002";
            const string progUserName1 = "maaike@cc.be";
            const string progUserPassword1 = "12"; // For demo purposes only! Don't do this in real application!
            IPasswordHasher<ApplicationUser> passwordHasherProg1 = new PasswordHasher<ApplicationUser>(); // Identity password hasher

            ApplicationUser prog1ApplicationUser = new ApplicationUser
            {
                Id = progUserId1,
                UserName = progUserName1,
                NormalizedUserName = progUserName1.ToUpper(),
                Email = progUserName1,
                NormalizedEmail = progUserName1.ToUpper(),
                EmailConfirmed = true,
                SecurityStamp = "XVPCRDAS3MJWQD5CSW2GWPRADBXEZINA", //Random string
                ConcurrencyStamp = "d8554266-b401-4519-9aeb-a9283053fc58", //Random guid string
                Firstname = "Maaike",
                Lastname = "Tubex",
                Zipcode = "9000",
                City = "Gent"
            };

            prog1ApplicationUser.PasswordHash = passwordHasherProg1.HashPassword(prog1ApplicationUser, progUserPassword1);
            //
            string progUserId2 = "00000000-0000-0000-0000-000000000003";
            string progUserName2 = "joost@cc.be";
            string progUserPassword2 = "12"; // For demo purposes only! Don't do this in real application!
            IPasswordHasher<ApplicationUser> passwordHasherProg2 = new PasswordHasher<ApplicationUser>(); // Identity password hasher

            ApplicationUser prog2ApplicationUser = new ApplicationUser
            {
                Id = progUserId2,
                UserName = progUserName2,
                NormalizedUserName = progUserName2.ToUpper(),
                Email = progUserName2,
                NormalizedEmail = progUserName2.ToUpper(),
                EmailConfirmed = true,
                SecurityStamp = "ZVPCRDAS3MJWQD5CSW2GWPRADBXEZINA", //Random string
                ConcurrencyStamp = "e8554266-b401-4519-9aeb-a9283053fc58", //Random guid string
                Firstname = "Joost",
                Lastname = "Van den Kerkhove",
                Zipcode = "8790",
                City = "Waregem"
            };

            prog2ApplicationUser.PasswordHash = passwordHasherProg2.HashPassword(prog2ApplicationUser, progUserPassword2);
            // 3th programmator
            string progUserId3 = "00000000-0000-0000-0000-000000000004";
            string progUserName3 = "veerle@cc.be";
            string progUserPassword3 = "12"; // For demo purposes only! Don't do this in real application!
            IPasswordHasher<ApplicationUser> passwordHasherProg3 = new PasswordHasher<ApplicationUser>(); // Identity password hasher

            ApplicationUser prog3ApplicationUser = new ApplicationUser
            {
                Id = progUserId3,
                UserName = progUserName3,
                NormalizedUserName = progUserName3.ToUpper(),
                Email = progUserName3,
                NormalizedEmail = progUserName3.ToUpper(),
                EmailConfirmed = true,
                SecurityStamp = "ZVPCRDAS3MJWQD5CSW2GWPRADBXEZINA", //Random string
                ConcurrencyStamp = "e8554266-b401-4519-9aeb-a9283053fc58", //Random guid string
                Firstname = "Veerle",
                Lastname = "Hollants",
                Zipcode = "8530",
                City = "Harelbeke"
            };

            prog3ApplicationUser.PasswordHash = passwordHasherProg3.HashPassword(prog3ApplicationUser, progUserPassword3);

            //
            // user reception
            const string receptionUserId = "00000000-0000-0000-0000-000000000005";
            const string receptionUserName = "paulien@cc.be";
            const string receptionUserPassword = "12"; // For demo purposes only! Don't do this in real application!
            IPasswordHasher<ApplicationUser> passwordHasherRec = new PasswordHasher<ApplicationUser>(); // Identity password hasher

            ApplicationUser receptionApplicationUser = new ApplicationUser
            {
                Id = receptionUserId,
                UserName = receptionUserName,
                NormalizedUserName = receptionUserName.ToUpper(),
                Email = receptionUserName,
                NormalizedEmail = receptionUserName.ToUpper(),
                EmailConfirmed = true,
                SecurityStamp = "XVPCRDAS3MJWQD5CSW2GWPRADBXEZINA", //Random string
                ConcurrencyStamp = "d8554266-b401-4519-9aeb-a9283053fc58", //Random guid string
                Firstname = "Paulien",
                Lastname = "Desmet",
                Zipcode = "8790",
                City = "Waregem"
            };

            receptionApplicationUser.PasswordHash = passwordHasherRec.HashPassword(receptionApplicationUser, receptionUserPassword);
            // visitor
            const string visitorUserId = "00000000-0000-0000-0000-000000000006";
            const string visitorUserName = "f.v@telenet.be";
            const string visitorUserPassword = "12"; // For demo purposes only! Don't do this in real application!
            IPasswordHasher<ApplicationUser> passwordHasherVisitor = new PasswordHasher<ApplicationUser>(); // Identity password hasher

            ApplicationUser visitorApplicationUser = new ApplicationUser
            {
                Id = visitorUserId,
                UserName = visitorUserName,
                NormalizedUserName = visitorUserName.ToUpper(),
                Email = visitorUserName,
                NormalizedEmail = visitorUserName.ToUpper(),
                EmailConfirmed = true,
                SecurityStamp = "BVPCRDAS3MJWQD5CSW2GWPRADBXEZINA", //Random string
                ConcurrencyStamp = "k8554266-b401-4519-9aeb-a9283053fc58", //Random guid string
                Firstname = "Filip",
                Lastname = "Verhelst",
                Zipcode = "8790",
                City = "Waregem"
            };

            visitorApplicationUser.PasswordHash = passwordHasherVisitor.HashPassword(visitorApplicationUser, visitorUserPassword);


            //create path to filename
            //          string wwwRoothpath = Path.Combine(_webHostEnvironment.WebRootPath, "images\\events\\"); 

            var events = new Event[]
            {
                    new Event
                    {
                        Id= 1,
                        Title = "May December",
                        CompanyId = 2,
                        EventDate = new DateTime(2024,3,25,20,0,0),
                        Description = "Twintig jaar na een spraakmakende affaire bereiden Gracie en haar man Joe zich voor op het afstudeerfeest van hun tweeling. Maar dan komt Hollywood-actrice Elizabeth Berry langs om zich voor te bereiden op haar rol als Gracie in een nieuwe film. Terwijl ze elkaar bestuderen, ontdekken ze dat ze meer overeenkomsten hebben dan ze hadden verwacht.",
                        Price = 9.00,
                        SuccesRate = 65,
                        LocationId = 3,
                        Imagestring = "may-december.jpg",

                    },
                    new Event
                    {
                        Id= 2,
                        Title = "Ferrari",
                        CompanyId = 1,
                        EventDate = new DateTime(2024,04,13,20,0,0),
                        Description = "Ferrari wordt geprezen als een succesverhaal, dankzij hun talloze triomfen in de Formule 1, met iconen zoals Niki Lauda en Michael Schumacher. Oorspronkelijk opgericht in 1929 als raceteam, heeft het Italiaanse automerk echter een hobbelige ontstaansgeschiedenis gekend. In 1957 verkeerde het bedrijf in ernstige financiële problemen, en Ferrari blikt terug op deze uitdagende periode.\r\nDe uitnodigende subtitel van Brock Yates' biografische boek, waar de film op is gebaseerd, leest: 'De man, de auto's, de races en de machine.'",
                        Price = 9.00,
                        SuccesRate = 70,
                        LocationId = 3,
                        Imagestring = "ferrari.jpg",

                    },
                    new Event
                    {
                        Id= 3,
                        Title = "Jan De Smet en de Grote Luxe",
                        CompanyId = 1,
                        EventDate = new DateTime(2024,01,20,20,0,0),
                        Description = "Zanger, muzikant en conservator van het ontroerend muzikaal erfgoed Jan De Smet werd zeventig. In 2024 viert hij die verjaardag met ons in de Schouwburg en het is een Grote Luxe (vandaar de titel!) om hem te mogen ontvangen met een schare uitgelezen muzikanten en zangeressen die een zeer eclectisch muzikaal programma beheersen.",
                        Price = 23.00,
                        SuccesRate = 90,
                        LocationId = 1,
                        Imagestring = "jan_de_smet_de_grote_luxe.jpg",
                        Audiostring = "Jan De Smet En De Grote Luxe -Mr. Ghost.mp3",
                        Videostring = "Let's Talk Dirty in Hawaiian-The Bonnie Blues.mp4"
                    },
                    new Event
                    {
                        Id= 4,
                        Title = "Wake Me Up When It’s Over",
                        CompanyId = 2,
                        EventDate = new DateTime(2024,04,24,20,0,0),
                        Description = "Alex Agnew geeft in zijn negende comedyshow de post-pandemie-wereld een pandoering",
                        Price = 31.00,
                        SuccesRate = 99,
                        LocationId = 1,
                        Imagestring = "AA_wake_me_up.jpg",

                    },
                    new Event
                    {
                        Id= 5,
                        Title = "Out of the Blue ",
                        CompanyId = 2,
                        EventDate = new DateTime(2024,4,24,20,0,0),
                        Description = "Na hun succesvolle voorstellingen Mining Stories en Pleasant Island maken Silke Huysmans en Hannes Dereere het sluitstuk van hun trilogie over mijnbouw. Deze keer gaan ze dieper in op een compleet nieuwe industrie: diepzeemijnbouw.",
                        Price = 17.00,
                        SuccesRate = 40,
                        LocationId = 2,
                        Imagestring = "silke_huysmans_out-of-the-blue.jpg",

                    },
                    new Event
                    {
                        Id= 6,
                        Title = "Het prethuis",
                        CompanyId = 1,
                        EventDate = new DateTime(2024,05,28,20,0,0),
                        Description = "Een actuele situatiekomedie met kleurrijke personages en hilarische toestanden. De Padelburen verzekert het publiek een avond onvervalste pret! Zoals men stilaan gewoon is van Het Prethuis!",
                        Price = 22.00,
                        SuccesRate = 80,
                        LocationId = 1,
                        Imagestring = "prethuis_de_padelburen.jpg",

                    },
                    new Event
                    {
                        Id= 7,
                        Title = "Selah Sue (try-out)",
                        CompanyId = 3,
                        EventDate = new DateTime(2024,01,16,20,0,0),
                        Description = "Met festivals gepland in Frankrijk, Spanje, Nederland en Bulgarije, wil Selah Sue helemaal klaar zijn voor haar tour. Dus besluit ze met haar band een tijdje in onze clubzaal te bivakkeren. Tot onze grote vreugde sluit ze die repetitieperiode af met een exclusief try-out concert.",
                        Price = 35.00,
                        SuccesRate = 100,
                        LocationId = 2,
                        Imagestring = "selah_sue.jpg",
                        Audiostring = "Selah Sue-This World.mp3",
                    }, //      ProgrammatorId = progUserId2
                    new Event
                    {
                        Id= 8,
                        Title = "Dance 2024",
                        CompanyId = 4,
                        EventDate = new DateTime(2024,5,25,20,0,0),
                        Description = "De kunstacademie brengt dansen uitgevoerd door de eigen leerlingen.",
                        Price = 5.00,
                        SuccesRate = 75,
                        LocationId = 1,

                    },//     ProgrammatorId = progUserId1
                    new Event
                    {
                        Id= 9,
                        Title = "Het theater van de toekomst. Een schakelkamp voor het 4e, 5e en 6e leerjaar",
                        CompanyId = 5,
                        EventDate = new DateTime(2024,7,8,9,0,0),
                        Description = "Samen met een professioneel theatermaker van LARF! breng je je wildste toekomstfantasieën tot leven. Je kruipt in de huid van futuristische helden, intergalactische ontdekkingsreizigers en revolutionaire uitvinders. Je onderzoekt wat theater is en ontdekt hoe je zelf speelt en beweegt. Zo ga je op zoek naar je eigen personage met een eigen geluid, hoe luid of stil dat ook is.",
                        Price = 150.00,
                        SuccesRate = 100,
                        LocationId = 2,
                        Imagestring = "Het theater van de toekomst.jpg",

                    }//    ProgrammatorId = progUserId1
};
            var eventGenre = new[]
{
                         new {EventsId = 1, GenresId = 5},
                         new {EventsId = 2, GenresId = 5},
                         new {EventsId = 3, GenresId = 7},
                         new {EventsId = 4, GenresId = 6},
                         new {EventsId = 5, GenresId = 8},
                         new {EventsId = 5, GenresId = 4},
                         new {EventsId = 6, GenresId = 8},
                         new {EventsId = 7, GenresId = 5},
                         new {EventsId = 8, GenresId = 2},
                         new {EventsId = 8, GenresId = 3},
                         new {EventsId = 9, GenresId = 8},
                         new {EventsId = 9, GenresId = 2},
             };
            var EventApplicationUsers = new[]
            {
                         new {EventsId = 1, ActionUsersId = progUserId3},
                         new {EventsId = 2, ActionUsersId = progUserId3},
                         new {EventsId = 3, ActionUsersId = progUserId2},
                         new {EventsId = 4, ActionUsersId = progUserId2},
                         new {EventsId = 5, ActionUsersId = progUserId1},
                         new {EventsId = 6, ActionUsersId = progUserId1},
                         new { EventsId = 7, ActionUsersId = progUserId2 },
                         new {EventsId = 8, ActionUsersId = progUserId1},
                         new {EventsId = 9, ActionUsersId = progUserId1},
            };
            //
            var users = new[]
            {
                adminApplicationUser,
                prog1ApplicationUser,
                prog2ApplicationUser,
                prog3ApplicationUser,
                receptionApplicationUser,
                visitorApplicationUser
            };
            // tickets
            var tickets = new Ticket[]
{
                new Ticket{
                    Id = 1,
                    EventId = 1,
                    Quantity = 1,
                    VisitorId = "00000000-0000-0000-0000-000000000006",
                },
                new Ticket{
                    Id = 2,
                    EventId = 8,
                    Quantity = 1,
                    VisitorId = "00000000-0000-0000-0000-000000000001",
                }
             };


            var actionLinks = new[]
            {
                new NavigationItem {Id=1, Area="Home", Position= 1, Action="Privacy", Controller="Home", Name="Bescherming"},
                new NavigationItem {Id=2, Area="Staff", Position= 1, Action="Index", Controller="Staff", Name="Voorstellingen"},
                new NavigationItem {Id=3, Area="Staff", Position= 2, Action="Index", Controller="Company", Name="Gezelschappen"},
                new NavigationItem {Id=4, Area="Staff", Position= 3, Action="Index", Controller="Location", Name="Locaties"},
                new NavigationItem {Id=5, Area="Home", Position= 2, Action="NewAbo", Controller="Home", Name="Nieuwe abonee"},
                new NavigationItem {Id=6, Area="Home", Position= 3, Action="Voucher", Controller="Home", Name="Waregembon"},
              };
            //
            // HasData metods
            modelBuilder.Entity<IdentityRole>().HasData(roles);
            modelBuilder.Entity<ApplicationUser>().HasData(users);
            //
            modelBuilder.Entity<IdentityUserRole<string>>().HasData(
    new IdentityUserRole<string>
    {
        RoleId = AdminRoleId,
        UserId = AdminUserId
    },
    new IdentityUserRole<string>
    {
        RoleId = ProgrammatorRoleId,
        UserId = progUserId1
    },

    new IdentityUserRole<string>
    {
        RoleId = ProgrammatorRoleId,
        UserId = progUserId2
    }, new IdentityUserRole<string>
    {
        RoleId = ProgrammatorRoleId,
        UserId = progUserId3
    },

    new IdentityUserRole<string>
    {
        RoleId = ReceptionRoleId,
        UserId = receptionUserId
    }

     ,
    new IdentityUserRole<string>
    {
        RoleId = VisitorRoleId,
        UserId = visitorUserId
    }

);
            //
            modelBuilder.Entity<IdentityUserClaim<string>>().HasData(
                new IdentityUserClaim<string>
                {
                    Id = 1,
                    UserId = "00000000-0000-0000-0000-000000000001",
                    ClaimType = "Name",
                    ClaimValue = "Geert Deloof"
                },
                new IdentityUserClaim<string>
                {
                    Id = 2,
                    UserId = "00000000-0000-0000-0000-000000000001",
                    ClaimType = "email",
                    ClaimValue = "admin@cc.be"
                },
                new IdentityUserClaim<string>
                {
                    Id = 3,
                    UserId = "00000000-0000-0000-0000-000000000002",
                    ClaimType = "Name",
                    ClaimValue = "Maaike Tubex"
                },
                new IdentityUserClaim<string>
                {
                    Id = 4,
                    UserId = "00000000-0000-0000-0000-000000000002",
                    ClaimType = "email",
                    ClaimValue = "maaike@cc.be"
                },
                new IdentityUserClaim<string>
                {
                    Id = 5,
                    UserId = "00000000-0000-0000-0000-000000000003",
                    ClaimType = "Name",
                    ClaimValue = "Joost Van den Kerkhove"
                },
                new IdentityUserClaim<string>
                {
                    Id = 6,
                    UserId = "00000000-0000-0000-0000-000000000003",
                    ClaimType = "email",
                    ClaimValue = "joost@cc.be"
                },
                new IdentityUserClaim<string>
                {
                    Id = 7,
                    UserId = "00000000-0000-0000-0000-000000000004",
                    ClaimType = "Name",
                    ClaimValue = "Veerle Hollants"
                },
                new IdentityUserClaim<string>
                {
                    Id = 8,
                    UserId = "00000000-0000-0000-0000-000000000004",
                    ClaimType = "email",
                    ClaimValue = "veerle@cc.be"
                },
                new IdentityUserClaim<string>
                {
                    Id = 9,
                    UserId = "00000000-0000-0000-0000-000000000005",
                    ClaimType = "Name",
                    ClaimValue = "Paulien Desmet"
                },
                new IdentityUserClaim<string>
                {
                    Id = 10,
                    UserId = "00000000-0000-0000-0000-000000000005",
                    ClaimType = "email",
                    ClaimValue = "paulien@cc.be"
                },
                new IdentityUserClaim<string>
                {
                    Id = 11,
                    UserId = "00000000-0000-0000-0000-000000000006",
                    ClaimType = "Name",
                    ClaimValue = "Filiep Verhelst"
                },
                new IdentityUserClaim<string>
                {
                    Id = 12,
                    UserId = "00000000-0000-0000-0000-000000000006",
                    ClaimType = "email",
                    ClaimValue = "f.v@telenet.be"
                },
                new IdentityUserClaim<string>
                {
                    Id = 13,
                    UserId = "00000000-0000-0000-0000-000000000006",
                    ClaimType = "registration-date",
                    ClaimValue = "2018-12-15"
                },
                new IdentityUserClaim<string>
                {
                    Id = 14,
                    UserId = "00000000-0000-0000-0000-000000000006",
                    ClaimType = "zipcode",
                    ClaimValue = "8793"
                }
            );
            modelBuilder.Entity<NavigationItem>().HasData(actionLinks);
            modelBuilder.Entity<Location>().HasData(locations);
            modelBuilder.Entity<Genre>().HasData(genres);
            modelBuilder.Entity<Company>().HasData(companies);
            modelBuilder.Entity<Event>().HasData(events);
            modelBuilder.Entity<Ticket>().HasData(tickets);
            // manytomany
            modelBuilder.Entity($"{nameof(Event)}{nameof(Genre)}").HasData(eventGenre);
            modelBuilder.Entity($"{nameof(ApplicationUser)}{nameof(Event)}").HasData(EventApplicationUsers);

        }
    }
}