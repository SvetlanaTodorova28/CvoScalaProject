using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Scala.StockSimulation.Web.Migrations
{
    public partial class Initial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AspNetRoles",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
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
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Created = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Updated = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Deleted = table.Column<DateTime>(type: "datetime2", nullable: true),
                    FirstName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    LastName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    IsTeacher = table.Column<bool>(type: "bit", nullable: false),
                    UserName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
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
                name: "Discounts",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Type = table.Column<int>(type: "int", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    Rate = table.Column<decimal>(type: "decimal(18,2)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Discounts", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "OrderTypes",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Created = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Updated = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Deleted = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OrderTypes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Suppliers",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Created = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Updated = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Deleted = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Suppliers", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AspNetRoleClaims",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RoleId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
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
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
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
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
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
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    RoleId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
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
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
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
                name: "Orders",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ApplicationUserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    OrderTypeId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CustomerName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DateDelivered = table.Column<DateTime>(type: "datetime2", nullable: true),
                    OrderNumber = table.Column<long>(type: "bigint", nullable: false),
                    Created = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Updated = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Deleted = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Orders", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Orders_AspNetUsers_ApplicationUserId",
                        column: x => x.ApplicationUserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Orders_OrderTypes_OrderTypeId",
                        column: x => x.OrderTypeId,
                        principalTable: "OrderTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Products",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Price = table.Column<decimal>(type: "money", nullable: false),
                    PriceWithDiscounts = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    ArticleNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SupplierId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    InitialStock = table.Column<int>(type: "int", nullable: false),
                    InitialMaximumStock = table.Column<int>(type: "int", nullable: false),
                    InitialMinimumStock = table.Column<int>(type: "int", nullable: false),
                    Created = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Updated = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Deleted = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Products", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Products_Suppliers_SupplierId",
                        column: x => x.SupplierId,
                        principalTable: "Suppliers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "DiscountProduct",
                columns: table => new
                {
                    DiscountsId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ProductsId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DiscountProduct", x => new { x.DiscountsId, x.ProductsId });
                    table.ForeignKey(
                        name: "FK_DiscountProduct_Discounts_DiscountsId",
                        column: x => x.DiscountsId,
                        principalTable: "Discounts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_DiscountProduct_Products_ProductsId",
                        column: x => x.ProductsId,
                        principalTable: "Products",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "OrderItems",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ProductId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    OrderId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Quantity = table.Column<int>(type: "int", nullable: false),
                    Created = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Updated = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Deleted = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OrderItems", x => x.Id);
                    table.ForeignKey(
                        name: "FK_OrderItems_Orders_OrderId",
                        column: x => x.OrderId,
                        principalTable: "Orders",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_OrderItems_Products_ProductId",
                        column: x => x.ProductId,
                        principalTable: "Products",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UserProductStates",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ApplicationUserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    PhysicalStock = table.Column<int>(type: "int", nullable: false),
                    FictionalStock = table.Column<int>(type: "int", nullable: false),
                    MinimumStock = table.Column<int>(type: "int", nullable: false),
                    MaximumStock = table.Column<int>(type: "int", nullable: false),
                    SoonAvailableStock = table.Column<int>(type: "int", nullable: false),
                    ReservedStock = table.Column<int>(type: "int", nullable: false),
                    ProductId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    OrderId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    TransactionType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Quantity = table.Column<int>(type: "int", nullable: false),
                    Created = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Updated = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Deleted = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserProductStates", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UserProductStates_AspNetUsers_ApplicationUserId",
                        column: x => x.ApplicationUserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UserProductStates_Orders_OrderId",
                        column: x => x.OrderId,
                        principalTable: "Orders",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_UserProductStates_Products_ProductId",
                        column: x => x.ProductId,
                        principalTable: "Products",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { new Guid("00000000-0000-0000-0000-000000000001"), "114b6852-47fe-4f5b-bd79-f80bee1f6c9b", "Teacher", "TEACHER" },
                    { new Guid("00000000-0000-0000-0000-000000000002"), "f4e9af34-9831-448c-9347-136357e60089", "Student", "STUDENT" }
                });

            migrationBuilder.InsertData(
                table: "AspNetUsers",
                columns: new[] { "Id", "AccessFailedCount", "ConcurrencyStamp", "Created", "Deleted", "Email", "EmailConfirmed", "FirstName", "IsTeacher", "LastName", "LockoutEnabled", "LockoutEnd", "NormalizedEmail", "NormalizedUserName", "PasswordHash", "PhoneNumber", "PhoneNumberConfirmed", "SecurityStamp", "TwoFactorEnabled", "Updated", "UserName" },
                values: new object[,]
                {
                    { new Guid("00000000-0000-0000-0000-000000000018"), 0, "34c12c71-1195-471a-9d2e-f7eddbc071be", new DateTime(2024, 6, 14, 19, 51, 22, 504, DateTimeKind.Local).AddTicks(379), null, "johnny.debeer@school.be", true, "Johnny", true, "De Beer", false, null, "JOHNNY.DEBEER@SCHOOL.BE", "JOHNNY.DEBEER@SCHOOL.BE", "AQAAAAEAACcQAAAAEHAbyoeWpq9kWoYzcM09MVAQxL+kq6wpZYhzhVQWvElewyWLocfWXoLWtpTMxBF6ig==", null, false, "2a30f8fb-4f61-4124-90ae-53687dd31f3a", false, null, "johnny.debeer@school.be" },
                    { new Guid("00000000-0000-0000-0000-000000000019"), 0, "881f5c3e-4a76-4278-8822-991214ab584c", new DateTime(2024, 6, 14, 19, 51, 22, 504, DateTimeKind.Local).AddTicks(384), null, "mileetoo.dimarko@school.be", true, "Mileetoo", true, "Die Marko", false, null, "MILEETOO.DIMARKO@SCHOOL.BE", "MILEETOO.DIMARKO@SCHOOL.BE", "AQAAAAEAACcQAAAAEGuGHcLJHjr0wFmvBHAxKQAdMEhMHGze0h5OAE1oVOxeOrHpPIKS3YQgCea7g1xsTw==", null, false, "c54419c8-b54a-4214-9778-6c4370404d4d", false, null, "mileetoo.dimarko@school.be" },
                    { new Guid("00000000-0000-0000-0000-000000000020"), 0, "4fdc9e0c-4f07-4921-a34a-d85832e31341", new DateTime(2024, 6, 14, 19, 51, 22, 504, DateTimeKind.Local).AddTicks(403), null, "tibo.verkest@student.be", true, "Tibo", false, "Verkest", false, null, "TIBO.VERKEST@STUDENT.BE", "TIBO.VERKEST@STUDENT.BE", "AQAAAAEAACcQAAAAEHeXdcgIKK2hFv65OvdYsgnusgF2uOMAMTK+OEojRNxnIepEhAmyC6nBB2eHEOxAvQ==", null, false, "8dd3721c-dd67-46af-98e8-f1a9185a49b4", false, null, "tibo.verkest@student.be" },
                    { new Guid("00000000-0000-0000-0000-000000000021"), 0, "7215d60a-93dc-496e-ab6b-ad1adc98ec3e", new DateTime(2024, 6, 14, 19, 51, 22, 504, DateTimeKind.Local).AddTicks(409), null, "mathias.breda@student.be", true, "Mathias", false, "Breda", false, null, "MATHIAS.BREDA@STUDENT.BE", "MATHIAS.BREDA@STUDENT.BE", "AQAAAAEAACcQAAAAECI1NL1cSEqrD3LqGZ7ZYxHn8w86VVh8DDrW5xHrhxItCWD4uASQiC6KCEiUeyWxrg==", null, false, "93d375a6-cc07-4a0a-b1d5-20263427a57c", false, null, "mathias.breda@student.be" },
                    { new Guid("00000000-0000-0000-0000-000000000022"), 0, "c9e108b3-4605-4605-8197-6499ef4d14ae", new DateTime(2024, 6, 14, 19, 51, 22, 504, DateTimeKind.Local).AddTicks(415), null, "kenny.demulder@student.be", true, "Kenny", false, "De Mulder", false, null, "KENNY.DEMULDER@STUDENT.BE", "KENNY.DEMULDER@STUDENT.BE", "AQAAAAEAACcQAAAAEEeMoLpeBb/Pl8E0ZcWtadhKxd8HLowt1BAHo8sbNm4MQKpLTEmMNPob/PKy5PzfGQ==", null, false, "327f5b2d-8e87-499e-8e34-b2cc10f325cd", false, null, "kenny.demulder@student.be" },
                    { new Guid("00000000-0000-0000-0000-000000000023"), 0, "0bff6a3f-f93d-4ad5-b51b-6dea20ff44bc", new DateTime(2024, 6, 14, 19, 51, 22, 504, DateTimeKind.Local).AddTicks(420), null, "joeri.versyck@student.be", true, "Joeri", false, "Versyck", false, null, "JOERI.VERSYCK@STUDENT.BE", "JOERI.VERSYCK@STUDENT.BE", "AQAAAAEAACcQAAAAEL+MHYP0M3eiUQeZPWfWR92KXERErWn7/kzh+zRTxMtNAA8pRRR4woGQG+faQa2Oqw==", null, false, "889e52c4-8f78-420a-99a7-1af8aa53e26c", false, null, "joeri.versyck@student.be" }
                });

            migrationBuilder.InsertData(
                table: "Discounts",
                columns: new[] { "Id", "IsActive", "Rate", "Type" },
                values: new object[,]
                {
                    { new Guid("00000000-0000-0000-0000-000000000025"), false, 1.00m, 0 },
                    { new Guid("00000000-0000-0000-0000-000000000026"), false, 1.00m, 1 },
                    { new Guid("00000000-0000-0000-0000-000000000028"), false, 1.00m, 2 }
                });

            migrationBuilder.InsertData(
                table: "OrderTypes",
                columns: new[] { "Id", "Created", "Deleted", "Name", "Updated" },
                values: new object[,]
                {
                    { new Guid("00000000-0000-0000-0000-000000000016"), new DateTime(2024, 6, 14, 19, 51, 22, 504, DateTimeKind.Local).AddTicks(270), null, "Bestelling bij leverancier", null },
                    { new Guid("00000000-0000-0000-0000-000000000017"), new DateTime(2024, 6, 14, 19, 51, 22, 504, DateTimeKind.Local).AddTicks(272), null, "Bestelling voor klant", null }
                });

            migrationBuilder.InsertData(
                table: "Suppliers",
                columns: new[] { "Id", "Created", "Deleted", "Name", "Updated" },
                values: new object[,]
                {
                    { new Guid("00000000-0000-0000-0000-000000000001"), new DateTime(2024, 6, 14, 19, 51, 22, 504, DateTimeKind.Local).AddTicks(150), null, "Electronic Arts", null },
                    { new Guid("00000000-0000-0000-0000-000000000002"), new DateTime(2024, 6, 14, 19, 51, 22, 504, DateTimeKind.Local).AddTicks(192), null, "FromSoftware", null },
                    { new Guid("00000000-0000-0000-0000-000000000003"), new DateTime(2024, 6, 14, 19, 51, 22, 504, DateTimeKind.Local).AddTicks(194), null, "Ubisoft", null }
                });

            migrationBuilder.InsertData(
                table: "AspNetUserRoles",
                columns: new[] { "RoleId", "UserId" },
                values: new object[,]
                {
                    { new Guid("00000000-0000-0000-0000-000000000001"), new Guid("00000000-0000-0000-0000-000000000018") },
                    { new Guid("00000000-0000-0000-0000-000000000001"), new Guid("00000000-0000-0000-0000-000000000019") },
                    { new Guid("00000000-0000-0000-0000-000000000002"), new Guid("00000000-0000-0000-0000-000000000020") },
                    { new Guid("00000000-0000-0000-0000-000000000002"), new Guid("00000000-0000-0000-0000-000000000021") },
                    { new Guid("00000000-0000-0000-0000-000000000002"), new Guid("00000000-0000-0000-0000-000000000022") },
                    { new Guid("00000000-0000-0000-0000-000000000002"), new Guid("00000000-0000-0000-0000-000000000023") }
                });

            migrationBuilder.InsertData(
                table: "Products",
                columns: new[] { "Id", "ArticleNumber", "Created", "Deleted", "Description", "InitialMaximumStock", "InitialMinimumStock", "InitialStock", "Name", "Price", "PriceWithDiscounts", "SupplierId", "Updated" },
                values: new object[,]
                {
                    { new Guid("00000000-0000-0000-0000-000000000004"), "1", new DateTime(2024, 6, 14, 19, 51, 22, 504, DateTimeKind.Local).AddTicks(229), null, "De nieuwe Madden NFL", 175, 20, 50, "Madden NFL 24", 69.99m, 69.99m, new Guid("00000000-0000-0000-0000-000000000001"), null },
                    { new Guid("00000000-0000-0000-0000-000000000005"), "2", new DateTime(2024, 6, 14, 19, 51, 22, 504, DateTimeKind.Local).AddTicks(233), null, "De nieuwe Formula 1", 200, 25, 75, "F1 23", 69.99m, 69.99m, new Guid("00000000-0000-0000-0000-000000000001"), null },
                    { new Guid("00000000-0000-0000-0000-000000000006"), "3", new DateTime(2024, 6, 14, 19, 51, 22, 504, DateTimeKind.Local).AddTicks(236), null, "De nieuwe FIFA", 250, 20, 100, "EA Sports FC 24", 69.99m, 69.99m, new Guid("00000000-0000-0000-0000-000000000001"), null },
                    { new Guid("00000000-0000-0000-0000-000000000007"), "4", new DateTime(2024, 6, 14, 19, 51, 22, 504, DateTimeKind.Local).AddTicks(238), null, "Het ultieme race spel", 225, 15, 200, "Need For Speed Unbound", 69.99m, 69.99m, new Guid("00000000-0000-0000-0000-000000000001"), null },
                    { new Guid("00000000-0000-0000-0000-000000000008"), "5", new DateTime(2024, 6, 14, 19, 51, 22, 504, DateTimeKind.Local).AddTicks(241), null, "Speel als een samoerai zoals nooit ervoor", 150, 15, 30, "Sekiro: Shadows Die Twice", 49.99m, 49.99m, new Guid("00000000-0000-0000-0000-000000000002"), null },
                    { new Guid("00000000-0000-0000-0000-000000000009"), "6", new DateTime(2024, 6, 14, 19, 51, 22, 504, DateTimeKind.Local).AddTicks(243), null, "Word de doder van demonen", 190, 25, 65, "Demon's Souls", 39.99m, 39.99m, new Guid("00000000-0000-0000-0000-000000000002"), null },
                    { new Guid("00000000-0000-0000-0000-000000000010"), "7", new DateTime(2024, 6, 14, 19, 51, 22, 504, DateTimeKind.Local).AddTicks(246), null, "Prepare to die", 215, 20, 75, "Dark Souls: Remastered", 59.99m, 59.99m, new Guid("00000000-0000-0000-0000-000000000002"), null },
                    { new Guid("00000000-0000-0000-0000-000000000011"), "8", new DateTime(2024, 6, 14, 19, 51, 22, 504, DateTimeKind.Local).AddTicks(251), null, "De eerste openwereldgame van FromSoftware", 225, 15, 200, "Elden Ring", 69.99m, 69.99m, new Guid("00000000-0000-0000-0000-000000000002"), null },
                    { new Guid("00000000-0000-0000-0000-000000000012"), "9", new DateTime(2024, 6, 14, 19, 51, 22, 504, DateTimeKind.Local).AddTicks(254), null, "Kom weer in contact met je verloren erfgoed, ontdek wat het betekent om Na'vi te zijn en werk samen met andere clans om Pandora te beschermen.", 400, 50, 250, "Avatar: Frontiers of Pandora  Standard Edition", 69.99m, 69.99m, new Guid("00000000-0000-0000-0000-000000000003"), null },
                    { new Guid("00000000-0000-0000-0000-000000000013"), "10", new DateTime(2024, 6, 14, 19, 51, 22, 504, DateTimeKind.Local).AddTicks(258), null, "Word een legendarische Viking op zoek naar glorie. Overval je vijanden, breid je settlement uit en bouw je politieke macht op.", 225, 15, 200, "Assassin's Creed Valhalla  Standard Edition", 49.99m, 49.99m, new Guid("00000000-0000-0000-0000-000000000003"), null },
                    { new Guid("00000000-0000-0000-0000-000000000014"), "11", new DateTime(2024, 6, 14, 19, 51, 22, 504, DateTimeKind.Local).AddTicks(261), null, "Een van de beste first-person shooters ooit gemaakt", 225, 15, 200, "Tom Clancy's Rainbow Six Siege  Standard Edition", 19.99m, 19.99m, new Guid("00000000-0000-0000-0000-000000000003"), null },
                    { new Guid("00000000-0000-0000-0000-000000000015"), "12", new DateTime(2024, 6, 14, 19, 51, 22, 504, DateTimeKind.Local).AddTicks(264), null, "In Watch Dogs Legion vorm je in de nabije toekomst een verzet om Londen terug te winnen, voordat het ten onder gaat.", 225, 15, 200, "Watch Dogs Legion  Deluxe Edition", 69.99m, 69.99m, new Guid("00000000-0000-0000-0000-000000000003"), null }
                });

            migrationBuilder.InsertData(
                table: "DiscountProduct",
                columns: new[] { "DiscountsId", "ProductsId" },
                values: new object[,]
                {
                    { new Guid("00000000-0000-0000-0000-000000000025"), new Guid("00000000-0000-0000-0000-000000000004") },
                    { new Guid("00000000-0000-0000-0000-000000000025"), new Guid("00000000-0000-0000-0000-000000000005") },
                    { new Guid("00000000-0000-0000-0000-000000000026"), new Guid("00000000-0000-0000-0000-000000000004") },
                    { new Guid("00000000-0000-0000-0000-000000000028"), new Guid("00000000-0000-0000-0000-000000000004") }
                });

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
                name: "IX_DiscountProduct_ProductsId",
                table: "DiscountProduct",
                column: "ProductsId");

            migrationBuilder.CreateIndex(
                name: "IX_OrderItems_OrderId",
                table: "OrderItems",
                column: "OrderId");

            migrationBuilder.CreateIndex(
                name: "IX_OrderItems_ProductId",
                table: "OrderItems",
                column: "ProductId");

            migrationBuilder.CreateIndex(
                name: "IX_Orders_ApplicationUserId",
                table: "Orders",
                column: "ApplicationUserId");

            migrationBuilder.CreateIndex(
                name: "IX_Orders_OrderTypeId",
                table: "Orders",
                column: "OrderTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_Products_SupplierId",
                table: "Products",
                column: "SupplierId");

            migrationBuilder.CreateIndex(
                name: "IX_UserProductStates_ApplicationUserId",
                table: "UserProductStates",
                column: "ApplicationUserId");

            migrationBuilder.CreateIndex(
                name: "IX_UserProductStates_OrderId",
                table: "UserProductStates",
                column: "OrderId");

            migrationBuilder.CreateIndex(
                name: "IX_UserProductStates_ProductId",
                table: "UserProductStates",
                column: "ProductId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
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
                name: "DiscountProduct");

            migrationBuilder.DropTable(
                name: "OrderItems");

            migrationBuilder.DropTable(
                name: "UserProductStates");

            migrationBuilder.DropTable(
                name: "AspNetRoles");

            migrationBuilder.DropTable(
                name: "Discounts");

            migrationBuilder.DropTable(
                name: "Orders");

            migrationBuilder.DropTable(
                name: "Products");

            migrationBuilder.DropTable(
                name: "AspNetUsers");

            migrationBuilder.DropTable(
                name: "OrderTypes");

            migrationBuilder.DropTable(
                name: "Suppliers");
        }
    }
}
