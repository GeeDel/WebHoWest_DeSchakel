using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Pri.WebApi.DeSchakel.Core.Migrations
{
    public partial class Starting : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AspNetRoles",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    NormalizedName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetRoles", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUsers",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Firstname = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Lastname = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DateOfBirth = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Zipcode = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    City = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UserName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    NormalizedUserName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    Email = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    NormalizedEmail = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    EmailConfirmed = table.Column<bool>(type: "bit", nullable: false),
                    PasswordHash = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SecurityStamp = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PhoneNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PhoneNumberConfirmed = table.Column<bool>(type: "bit", nullable: false),
                    TwoFactorEnabled = table.Column<bool>(type: "bit", nullable: false),
                    LockoutEnd = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    LockoutEnabled = table.Column<bool>(type: "bit", nullable: false),
                    AccessFailedCount = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUsers", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Companies",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Phonenumber = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Companies", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Genres",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Genres", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Locations",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Capacity = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Locations", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Navigations",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Area = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Position = table.Column<int>(type: "int", nullable: false),
                    Controller = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Action = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Navigations", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AspNetRoleClaims",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RoleId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ClaimType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ClaimValue = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetRoleClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetRoleClaims_AspNetRoles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "AspNetRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserClaims",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ClaimType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ClaimValue = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetUserClaims_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserLogins",
                columns: table => new
                {
                    LoginProvider = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ProviderKey = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ProviderDisplayName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserLogins", x => new { x.LoginProvider, x.ProviderKey });
                    table.ForeignKey(
                        name: "FK_AspNetUserLogins_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserRoles",
                columns: table => new
                {
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    RoleId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserRoles", x => new { x.UserId, x.RoleId });
                    table.ForeignKey(
                        name: "FK_AspNetUserRoles_AspNetRoles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "AspNetRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AspNetUserRoles_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserTokens",
                columns: table => new
                {
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    LoginProvider = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Value = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserTokens", x => new { x.UserId, x.LoginProvider, x.Name });
                    table.ForeignKey(
                        name: "FK_AspNetUserTokens_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Events",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    EventDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Price = table.Column<double>(type: "float", nullable: false),
                    SuccesRate = table.Column<int>(type: "int", nullable: false),
                    Imagestring = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Audiostring = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Videostring = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CompanyId = table.Column<int>(type: "int", nullable: false),
                    LocationId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Events", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Events_Companies_CompanyId",
                        column: x => x.CompanyId,
                        principalTable: "Companies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Events_Locations_LocationId",
                        column: x => x.LocationId,
                        principalTable: "Locations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ApplicationUserEvent",
                columns: table => new
                {
                    ActionUsersId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    EventsId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ApplicationUserEvent", x => new { x.ActionUsersId, x.EventsId });
                    table.ForeignKey(
                        name: "FK_ApplicationUserEvent_AspNetUsers_ActionUsersId",
                        column: x => x.ActionUsersId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ApplicationUserEvent_Events_EventsId",
                        column: x => x.EventsId,
                        principalTable: "Events",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "EventGenre",
                columns: table => new
                {
                    EventsId = table.Column<int>(type: "int", nullable: false),
                    GenresId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EventGenre", x => new { x.EventsId, x.GenresId });
                    table.ForeignKey(
                        name: "FK_EventGenre_Events_EventsId",
                        column: x => x.EventsId,
                        principalTable: "Events",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_EventGenre_Genres_GenresId",
                        column: x => x.GenresId,
                        principalTable: "Genres",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Tickets",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Quantity = table.Column<int>(type: "int", nullable: false),
                    VisitorId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    EventId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Tickets", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Tickets_AspNetUsers_VisitorId",
                        column: x => x.VisitorId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Tickets_Events_EventId",
                        column: x => x.EventId,
                        principalTable: "Events",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "00000000-0000-0000-0000-000000000001", "df1a5969-0b38-42d1-8e85-6501e5135f02", "Admin", "ADMIN" },
                    { "00000000-0000-0000-0000-000000000002", "895268fc-12e5-4e92-801e-6f75d6c1b6e1", "Programmator", "PROGRAMMATOR" },
                    { "00000000-0000-0000-0000-000000000003", "aebce31d-3636-46d3-adaa-cd351cb617b5", "Onthaal", "ONTHAAL" },
                    { "00000000-0000-0000-0000-000000000004", "ff4648ca-26f9-454e-b14a-c44c733c10b0", "Bezoeker", "BEZOEKER" }
                });

            migrationBuilder.InsertData(
                table: "AspNetUsers",
                columns: new[] { "Id", "AccessFailedCount", "City", "ConcurrencyStamp", "DateOfBirth", "Email", "EmailConfirmed", "Firstname", "Lastname", "LockoutEnabled", "LockoutEnd", "NormalizedEmail", "NormalizedUserName", "PasswordHash", "PhoneNumber", "PhoneNumberConfirmed", "SecurityStamp", "TwoFactorEnabled", "UserName", "Zipcode" },
                values: new object[,]
                {
                    { "00000000-0000-0000-0000-000000000001", 0, "Beveren-Leie", "c8554266-b401-4519-9aeb-a9283053fc58", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "admin@cc.be", true, "Geert", "Deloof", false, null, "ADMIN@CC.BE", "ADMIN@CC.BE", "AQAAAAEAACcQAAAAECEBykEfUx2QNuKIOW+J4TAPWvGhMR54IUN0zRR+JA5lF5yX3Jno61GQIs39bQLDgQ==", null, false, "VVPCRDAS3MJWQD5CSW2GWPRADBXEZINA", false, "admin@cc.be", "8791" },
                    { "00000000-0000-0000-0000-000000000002", 0, "Gent", "d8554266-b401-4519-9aeb-a9283053fc58", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "maaike@cc.be", true, "Maaike", "Tubex", false, null, "MAAIKE@CC.BE", "MAAIKE@CC.BE", "AQAAAAEAACcQAAAAEPhv/khkRqRIEaEXnW4F2aeDs/zWAzdp4e5phNISckqNl9MA8nagKAlPhJbf3y9Ftg==", null, false, "XVPCRDAS3MJWQD5CSW2GWPRADBXEZINA", false, "maaike@cc.be", "9000" },
                    { "00000000-0000-0000-0000-000000000003", 0, "Waregem", "e8554266-b401-4519-9aeb-a9283053fc58", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "joost@cc.be", true, "Joost", "Van den Kerkhove", false, null, "JOOST@CC.BE", "JOOST@CC.BE", "AQAAAAEAACcQAAAAEGEF8pY9md0JAOBZc685NNyDhN0oinmK4ueXhtHwCCNknmodWjgHpBwKt6e/HJ3y6w==", null, false, "ZVPCRDAS3MJWQD5CSW2GWPRADBXEZINA", false, "joost@cc.be", "8790" },
                    { "00000000-0000-0000-0000-000000000004", 0, "Harelbeke", "e8554266-b401-4519-9aeb-a9283053fc58", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "veerle@cc.be", true, "Veerle", "Hollants", false, null, "VEERLE@CC.BE", "VEERLE@CC.BE", "AQAAAAEAACcQAAAAEI5CINLtL8SH9mKgiXmTFW5V8XkWCRkPoZWHThBNbZmoa1k/wp3AN+PIk8zHm3BPmQ==", null, false, "ZVPCRDAS3MJWQD5CSW2GWPRADBXEZINA", false, "veerle@cc.be", "8530" },
                    { "00000000-0000-0000-0000-000000000005", 0, "Waregem", "d8554266-b401-4519-9aeb-a9283053fc58", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "paulien@cc.be", true, "Paulien", "Desmet", false, null, "PAULIEN@CC.BE", "PAULIEN@CC.BE", "AQAAAAEAACcQAAAAENk4t02iiHpd4MXiaBBDoYlHyjlaRKMUtn6vwEA/XfWDpmk3WUmcyOeBZLGlhRHr8A==", null, false, "XVPCRDAS3MJWQD5CSW2GWPRADBXEZINA", false, "paulien@cc.be", "8790" },
                    { "00000000-0000-0000-0000-000000000006", 0, "Waregem", "k8554266-b401-4519-9aeb-a9283053fc58", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "f.v@telenet.be", true, "Filip", "Verhelst", false, null, "F.V@TELENET.BE", "F.V@TELENET.BE", "AQAAAAEAACcQAAAAEM0Kl8l6aWOoc6YpdBuUe/iDe4GR7YTp/8meOLRvqpmK8f83sofBYr8SpuR2ADECEg==", null, false, "BVPCRDAS3MJWQD5CSW2GWPRADBXEZINA", false, "f.v@telenet.be", "8790" }
                });

            migrationBuilder.InsertData(
                table: "Companies",
                columns: new[] { "Id", "Email", "Name", "Phonenumber" },
                values: new object[,]
                {
                    { 1, "jan@desmet.be", "Jan De Smet", "123/456789" },
                    { 2, "alex@agnew.be", "Alex Agnew", "789/456123" },
                    { 3, "selah@sue.be", "Selah Sue", "321/654987" },
                    { 4, "ka@waregem.be", "Stedelijke Kunstacademie Waregem", "056/123456" },
                    { 5, "maaike@cc.be", "Eigenproductie-doen", "056/610061" }
                });

            migrationBuilder.InsertData(
                table: "Genres",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { 1, "Circus" },
                    { 2, "Dans" },
                    { 3, "Doen" },
                    { 4, "Familie" },
                    { 5, "Film" },
                    { 6, "Humor" },
                    { 7, "Muziek" },
                    { 8, "Theater" }
                });

            migrationBuilder.InsertData(
                table: "Locations",
                columns: new[] { "Id", "Capacity", "Name" },
                values: new object[,]
                {
                    { 1, 500, "Schouwburg" },
                    { 2, 192, "Schakelbox" },
                    { 3, 200, "Cinema" }
                });

            migrationBuilder.InsertData(
                table: "Navigations",
                columns: new[] { "Id", "Action", "Area", "Controller", "Name", "Position" },
                values: new object[,]
                {
                    { 1, "Privacy", "Home", "Home", "Bescherming", 1 },
                    { 2, "Index", "Staff", "Staff", "Voorstellingen", 1 },
                    { 3, "Index", "Staff", "Company", "Gezelschappen", 2 },
                    { 4, "Index", "Staff", "Location", "Locaties", 3 },
                    { 5, "NewAbo", "Home", "Home", "Nieuwe abonee", 2 },
                    { 6, "Voucher", "Home", "Home", "Waregembon", 3 }
                });

            migrationBuilder.InsertData(
                table: "AspNetUserClaims",
                columns: new[] { "Id", "ClaimType", "ClaimValue", "UserId" },
                values: new object[,]
                {
                    { 1, "Name", "Geert Deloof", "00000000-0000-0000-0000-000000000001" },
                    { 2, "email", "admin@cc.be", "00000000-0000-0000-0000-000000000001" },
                    { 3, "Name", "Maaike Tubex", "00000000-0000-0000-0000-000000000002" },
                    { 4, "email", "maaike@cc.be", "00000000-0000-0000-0000-000000000002" },
                    { 5, "Name", "Joost Van den Kerkhove", "00000000-0000-0000-0000-000000000003" },
                    { 6, "email", "joost@cc.be", "00000000-0000-0000-0000-000000000003" },
                    { 7, "Name", "Veerle Hollants", "00000000-0000-0000-0000-000000000004" },
                    { 8, "email", "veerle@cc.be", "00000000-0000-0000-0000-000000000004" },
                    { 9, "Name", "Paulien Desmet", "00000000-0000-0000-0000-000000000005" },
                    { 10, "email", "paulien@cc.be", "00000000-0000-0000-0000-000000000005" },
                    { 11, "Name", "Filiep Verhelst", "00000000-0000-0000-0000-000000000006" },
                    { 12, "email", "f.v@telenet.be", "00000000-0000-0000-0000-000000000006" },
                    { 13, "registration-date", "2018-12-15", "00000000-0000-0000-0000-000000000006" },
                    { 14, "zipcode", "8793", "00000000-0000-0000-0000-000000000006" }
                });

            migrationBuilder.InsertData(
                table: "AspNetUserRoles",
                columns: new[] { "RoleId", "UserId" },
                values: new object[,]
                {
                    { "00000000-0000-0000-0000-000000000001", "00000000-0000-0000-0000-000000000001" },
                    { "00000000-0000-0000-0000-000000000002", "00000000-0000-0000-0000-000000000002" },
                    { "00000000-0000-0000-0000-000000000002", "00000000-0000-0000-0000-000000000003" },
                    { "00000000-0000-0000-0000-000000000002", "00000000-0000-0000-0000-000000000004" },
                    { "00000000-0000-0000-0000-000000000003", "00000000-0000-0000-0000-000000000005" },
                    { "00000000-0000-0000-0000-000000000004", "00000000-0000-0000-0000-000000000006" }
                });

            migrationBuilder.InsertData(
                table: "Events",
                columns: new[] { "Id", "Audiostring", "CompanyId", "Description", "EventDate", "Imagestring", "LocationId", "Price", "SuccesRate", "Title", "Videostring" },
                values: new object[,]
                {
                    { 1, null, 2, "Twintig jaar na een spraakmakende affaire bereiden Gracie en haar man Joe zich voor op het afstudeerfeest van hun tweeling. Maar dan komt Hollywood-actrice Elizabeth Berry langs om zich voor te bereiden op haar rol als Gracie in een nieuwe film. Terwijl ze elkaar bestuderen, ontdekken ze dat ze meer overeenkomsten hebben dan ze hadden verwacht.", new DateTime(2024, 3, 25, 20, 0, 0, 0, DateTimeKind.Unspecified), "may-december.jpg", 3, 9.0, 65, "May December", null },
                    { 2, null, 1, "Ferrari wordt geprezen als een succesverhaal, dankzij hun talloze triomfen in de Formule 1, met iconen zoals Niki Lauda en Michael Schumacher. Oorspronkelijk opgericht in 1929 als raceteam, heeft het Italiaanse automerk echter een hobbelige ontstaansgeschiedenis gekend. In 1957 verkeerde het bedrijf in ernstige financiële problemen, en Ferrari blikt terug op deze uitdagende periode.\r\nDe uitnodigende subtitel van Brock Yates' biografische boek, waar de film op is gebaseerd, leest: 'De man, de auto's, de races en de machine.'", new DateTime(2024, 4, 13, 20, 0, 0, 0, DateTimeKind.Unspecified), "ferrari.jpg", 3, 9.0, 70, "Ferrari", null },
                    { 3, "Jan De Smet En De Grote Luxe -Mr. Ghost.mp3", 1, "Zanger, muzikant en conservator van het ontroerend muzikaal erfgoed Jan De Smet werd zeventig. In 2024 viert hij die verjaardag met ons in de Schouwburg en het is een Grote Luxe (vandaar de titel!) om hem te mogen ontvangen met een schare uitgelezen muzikanten en zangeressen die een zeer eclectisch muzikaal programma beheersen.", new DateTime(2024, 1, 20, 20, 0, 0, 0, DateTimeKind.Unspecified), "jan_de_smet_de_grote_luxe.jpg", 1, 23.0, 90, "Jan De Smet en de Grote Luxe", "Let's Talk Dirty in Hawaiian-The Bonnie Blues.mp4" },
                    { 4, null, 2, "Alex Agnew geeft in zijn negende comedyshow de post-pandemie-wereld een pandoering", new DateTime(2024, 4, 24, 20, 0, 0, 0, DateTimeKind.Unspecified), "AA_wake_me_up.jpg", 1, 31.0, 99, "Wake Me Up When It’s Over", null },
                    { 5, null, 2, "Na hun succesvolle voorstellingen Mining Stories en Pleasant Island maken Silke Huysmans en Hannes Dereere het sluitstuk van hun trilogie over mijnbouw. Deze keer gaan ze dieper in op een compleet nieuwe industrie: diepzeemijnbouw.", new DateTime(2024, 4, 24, 20, 0, 0, 0, DateTimeKind.Unspecified), "silke_huysmans_out-of-the-blue.jpg", 2, 17.0, 40, "Out of the Blue ", null },
                    { 6, null, 1, "Een actuele situatiekomedie met kleurrijke personages en hilarische toestanden. De Padelburen verzekert het publiek een avond onvervalste pret! Zoals men stilaan gewoon is van Het Prethuis!", new DateTime(2024, 5, 28, 20, 0, 0, 0, DateTimeKind.Unspecified), "prethuis_de_padelburen.jpg", 1, 22.0, 80, "Het prethuis", null },
                    { 7, "Selah Sue-This World.mp3", 3, "Met festivals gepland in Frankrijk, Spanje, Nederland en Bulgarije, wil Selah Sue helemaal klaar zijn voor haar tour. Dus besluit ze met haar band een tijdje in onze clubzaal te bivakkeren. Tot onze grote vreugde sluit ze die repetitieperiode af met een exclusief try-out concert.", new DateTime(2024, 1, 16, 20, 0, 0, 0, DateTimeKind.Unspecified), "selah_sue.jpg", 2, 35.0, 100, "Selah Sue (try-out)", null },
                    { 8, null, 4, "De kunstacademie brengt dansen uitgevoerd door de eigen leerlingen.", new DateTime(2024, 5, 25, 20, 0, 0, 0, DateTimeKind.Unspecified), null, 1, 5.0, 75, "Dance 2024", null },
                    { 9, null, 5, "Samen met een professioneel theatermaker van LARF! breng je je wildste toekomstfantasieën tot leven. Je kruipt in de huid van futuristische helden, intergalactische ontdekkingsreizigers en revolutionaire uitvinders. Je onderzoekt wat theater is en ontdekt hoe je zelf speelt en beweegt. Zo ga je op zoek naar je eigen personage met een eigen geluid, hoe luid of stil dat ook is.", new DateTime(2024, 7, 8, 9, 0, 0, 0, DateTimeKind.Unspecified), "Het theater van de toekomst.jpg", 2, 150.0, 100, "Het theater van de toekomst. Een schakelkamp voor het 4e, 5e en 6e leerjaar", null }
                });

            migrationBuilder.InsertData(
                table: "ApplicationUserEvent",
                columns: new[] { "ActionUsersId", "EventsId" },
                values: new object[,]
                {
                    { "00000000-0000-0000-0000-000000000002", 5 },
                    { "00000000-0000-0000-0000-000000000002", 6 },
                    { "00000000-0000-0000-0000-000000000002", 8 },
                    { "00000000-0000-0000-0000-000000000002", 9 },
                    { "00000000-0000-0000-0000-000000000003", 3 },
                    { "00000000-0000-0000-0000-000000000003", 4 },
                    { "00000000-0000-0000-0000-000000000003", 7 },
                    { "00000000-0000-0000-0000-000000000004", 1 },
                    { "00000000-0000-0000-0000-000000000004", 2 }
                });

            migrationBuilder.InsertData(
                table: "EventGenre",
                columns: new[] { "EventsId", "GenresId" },
                values: new object[,]
                {
                    { 1, 5 },
                    { 2, 5 },
                    { 3, 7 },
                    { 4, 6 },
                    { 5, 4 },
                    { 5, 8 },
                    { 6, 8 },
                    { 7, 5 },
                    { 8, 2 },
                    { 8, 3 },
                    { 9, 2 },
                    { 9, 8 }
                });

            migrationBuilder.InsertData(
                table: "Tickets",
                columns: new[] { "Id", "EventId", "Quantity", "VisitorId" },
                values: new object[,]
                {
                    { 1, 1, 1, "00000000-0000-0000-0000-000000000006" },
                    { 2, 8, 1, "00000000-0000-0000-0000-000000000001" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_ApplicationUserEvent_EventsId",
                table: "ApplicationUserEvent",
                column: "EventsId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetRoleClaims_RoleId",
                table: "AspNetRoleClaims",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "RoleNameIndex",
                table: "AspNetRoles",
                column: "NormalizedName",
                unique: true,
                filter: "[NormalizedName] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserClaims_UserId",
                table: "AspNetUserClaims",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserLogins_UserId",
                table: "AspNetUserLogins",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserRoles_RoleId",
                table: "AspNetUserRoles",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "EmailIndex",
                table: "AspNetUsers",
                column: "NormalizedEmail");

            migrationBuilder.CreateIndex(
                name: "UserNameIndex",
                table: "AspNetUsers",
                column: "NormalizedUserName",
                unique: true,
                filter: "[NormalizedUserName] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_EventGenre_GenresId",
                table: "EventGenre",
                column: "GenresId");

            migrationBuilder.CreateIndex(
                name: "IX_Events_CompanyId",
                table: "Events",
                column: "CompanyId");

            migrationBuilder.CreateIndex(
                name: "IX_Events_LocationId",
                table: "Events",
                column: "LocationId");

            migrationBuilder.CreateIndex(
                name: "IX_Tickets_EventId",
                table: "Tickets",
                column: "EventId");

            migrationBuilder.CreateIndex(
                name: "IX_Tickets_VisitorId",
                table: "Tickets",
                column: "VisitorId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ApplicationUserEvent");

            migrationBuilder.DropTable(
                name: "AspNetRoleClaims");

            migrationBuilder.DropTable(
                name: "AspNetUserClaims");

            migrationBuilder.DropTable(
                name: "AspNetUserLogins");

            migrationBuilder.DropTable(
                name: "AspNetUserRoles");

            migrationBuilder.DropTable(
                name: "AspNetUserTokens");

            migrationBuilder.DropTable(
                name: "EventGenre");

            migrationBuilder.DropTable(
                name: "Navigations");

            migrationBuilder.DropTable(
                name: "Tickets");

            migrationBuilder.DropTable(
                name: "AspNetRoles");

            migrationBuilder.DropTable(
                name: "Genres");

            migrationBuilder.DropTable(
                name: "AspNetUsers");

            migrationBuilder.DropTable(
                name: "Events");

            migrationBuilder.DropTable(
                name: "Companies");

            migrationBuilder.DropTable(
                name: "Locations");
        }
    }
}
