using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Scala.StockSimulation.Core.Entities;
using Scala.StockSimulation.Core.Enums;

namespace Scala.StockSimulation.Web.Data.Seeding
{
    public class Seeder
    {
        public static void Seed(ModelBuilder modelBuilder)
        {
            var suppliers = new Supplier[]
            {
                new Supplier { Id = Guid.Parse("00000000-0000-0000-0000-000000000001"), Created = DateTime.Now, Name = "Electronic Arts" },
                new Supplier { Id = Guid.Parse("00000000-0000-0000-0000-000000000002"),Created = DateTime.Now, Name = "FromSoftware"},
                new Supplier { Id = Guid.Parse("00000000-0000-0000-0000-000000000003"),Created = DateTime.Now, Name = "Ubisoft"}
            };
            var discounts = new []
            {
                new Discount{ 
                    Id = Guid.Parse("00000000-0000-0000-0000-000000000025"),
                    Type = DiscountType.Supplier,
                    Rate = 1.00M
                },
                new Discount() { 
                    Id = Guid.Parse("00000000-0000-0000-0000-000000000026"),
                    Type = DiscountType.Volume,
                    Rate = 1.00M
                    
                },
                new Discount() { 
                    Id = Guid.Parse("00000000-0000-0000-0000-000000000028"),
                    Type = DiscountType.Shipping,
                    Rate = 1.00M
                }
            };
            var products = new Product[]
            {
                new Product {
                    Id = Guid.Parse("00000000-0000-0000-0000-000000000004"),
                    ArticleNumber = "1",
                    Name = "Madden NFL 24",
                    Created = DateTime.Now,
                    SupplierId = suppliers[0].Id,
                    Description = "De nieuwe Madden NFL",
                    Price = 69.99m,
                    InitialMaximumStock = 175,
                    InitialMinimumStock = 20,
                    InitialStock = 50,
                    
                },

                new Product {
                    Id = Guid.Parse("00000000-0000-0000-0000-000000000005"),
                    ArticleNumber = "2",
                    Name = "F1 23",
                    Created = DateTime.Now,
                    SupplierId = suppliers[0].Id,
                    Description = "De nieuwe Formula 1",
                    Price = 69.99m,
                    InitialMaximumStock = 200,
                    InitialMinimumStock = 25,
                    InitialStock = 75 ,
                },

                new Product {
                    Id = Guid.Parse("00000000-0000-0000-0000-000000000006"),
                    ArticleNumber = "3",
                    Name = "EA Sports FC 24",
                    Created = DateTime.Now,
                    SupplierId = suppliers[0].Id,
                    Description = "De nieuwe FIFA",
                    Price = 69.99m,
                    InitialMaximumStock = 250,
                    InitialMinimumStock = 20,
                    InitialStock = 100 },

                 new Product {
                    Id = Guid.Parse("00000000-0000-0000-0000-000000000007"),
                    ArticleNumber = "4",
                    Name = "Need For Speed Unbound",
                    Created = DateTime.Now,
                    SupplierId = suppliers[0].Id,
                    Description = "Het ultieme race spel",
                    Price = 69.99m,
                    InitialMaximumStock = 225,
                    InitialMinimumStock = 15,
                    InitialStock = 200 },

                 new Product {
                    Id = Guid.Parse("00000000-0000-0000-0000-000000000008"),
                    ArticleNumber = "5",
                    Name = "Sekiro: Shadows Die Twice",
                    Created = DateTime.Now,
                    SupplierId = suppliers[1].Id,
                    Description = "Speel als een samoerai zoals nooit ervoor",
                    Price = 49.99m,
                    InitialMaximumStock = 150,
                    InitialMinimumStock = 15,
                    InitialStock = 30 },

                 new Product {
                    Id = Guid.Parse("00000000-0000-0000-0000-000000000009"),
                    ArticleNumber = "6",
                    Name = "Demon's Souls",
                    Created = DateTime.Now,
                    SupplierId = suppliers[1].Id,
                    Description = "Word de doder van demonen",
                    Price = 39.99m,
                    InitialMaximumStock = 190,
                    InitialMinimumStock = 25,
                    InitialStock = 65 },

                 new Product {
                    Id = Guid.Parse("00000000-0000-0000-0000-000000000010"),
                    ArticleNumber = "7",
                    Name = "Dark Souls: Remastered",
                    Created = DateTime.Now,
                    SupplierId = suppliers[1].Id,
                    Description = "Prepare to die",
                    Price = 59.99m,
                    InitialMaximumStock = 215,
                    InitialMinimumStock = 20,
                    InitialStock = 75 },

                 new Product {
                    Id = Guid.Parse("00000000-0000-0000-0000-000000000011"),
                    ArticleNumber = "8",
                    Name = "Elden Ring",
                    Created = DateTime.Now,
                    SupplierId = suppliers[1].Id,
                    Description = "De eerste openwereldgame van FromSoftware",
                    Price = 69.99m,
                    InitialMaximumStock = 225,
                    InitialMinimumStock = 15,
                    InitialStock = 200 },

                 new Product {
                    Id = Guid.Parse("00000000-0000-0000-0000-000000000012"),
                    ArticleNumber = "9",
                    Name = "Avatar: Frontiers of Pandora  Standard Edition",
                    Created = DateTime.Now,
                    SupplierId = suppliers[2].Id,
                    Description = "Kom weer in contact met je verloren erfgoed, ontdek wat het betekent om Na'vi te zijn en werk samen met andere clans om Pandora te beschermen.",
                    Price = 69.99m,
                    InitialMaximumStock = 400,
                    InitialMinimumStock = 50,
                    InitialStock = 250 },
                 new Product {
                    Id = Guid.Parse("00000000-0000-0000-0000-000000000013"),
                    ArticleNumber = "10",
                    Name = "Assassin's Creed Valhalla  Standard Edition",
                    Created = DateTime.Now,
                    SupplierId = suppliers[2].Id,
                    Description = "Word een legendarische Viking op zoek naar glorie. Overval je vijanden, breid je settlement uit en bouw je politieke macht op.",
                    Price = 49.99m,
                    InitialMaximumStock = 225,
                    InitialMinimumStock = 15,
                    InitialStock = 200 },
                 new Product {
                    Id = Guid.Parse("00000000-0000-0000-0000-000000000014"),
                    ArticleNumber = "11",
                    Name = "Tom Clancy's Rainbow Six Siege  Standard Edition",
                    Created = DateTime.Now,
                    SupplierId = suppliers[2].Id,
                    Description = "Een van de beste first-person shooters ooit gemaakt",
                    Price = 19.99m,
                    InitialMaximumStock = 225,
                    InitialMinimumStock = 15,
                    InitialStock = 200 },
                 new Product {
                    Id = Guid.Parse("00000000-0000-0000-0000-000000000015"),
                    ArticleNumber = "12",
                    Name = "Watch Dogs Legion  Deluxe Edition",
                    Created = DateTime.Now,
                    SupplierId = suppliers[2].Id,
                    Description = "In Watch Dogs Legion vorm je in de nabije toekomst een verzet om Londen terug te winnen, voordat het ten onder gaat.",
                    Price = 69.99m,
                    InitialMaximumStock = 225,
                    InitialMinimumStock = 15,
                    InitialStock = 200 },
            };
            foreach (var product in products){
                product.PriceWithDiscounts = product.Price;
            }

            var orderTypes = new OrderType[]
            {
                new OrderType {
                    Id = Guid.Parse("00000000-0000-0000-0000-000000000016"),
                    Created = DateTime.Now,
                    Name = "Bestelling bij leverancier" },
                new OrderType {
                    Id = Guid.Parse("00000000-0000-0000-0000-000000000017"),
                    Created = DateTime.Now,
                    Name = "Bestelling voor klant" },

            };
            IPasswordHasher<ApplicationUser> passwordHasher = new PasswordHasher<ApplicationUser>();


            var applicationUsers = new ApplicationUser[]
                {
                    new ApplicationUser {
                        Id = Guid.Parse("00000000-0000-0000-0000-000000000018"),
                        UserName = "johnny.debeer@school.be",
                        NormalizedUserName = "JOHNNY.DEBEER@SCHOOL.BE",
                        Email = "johnny.debeer@school.be",
                        NormalizedEmail = "JOHNNY.DEBEER@SCHOOL.BE",
                        EmailConfirmed = true,
                        SecurityStamp = Guid.NewGuid().ToString("D"),
                        FirstName = "Johnny",
                        LastName = "De Beer",
                        IsTeacher = true,
                        Created = DateTime.Now
                    },
                    new ApplicationUser {
                        Id = Guid.Parse("00000000-0000-0000-0000-000000000019"),
                        UserName = "mileetoo.dimarko@school.be",
                        NormalizedUserName = "MILEETOO.DIMARKO@SCHOOL.BE",
                        Email = "mileetoo.dimarko@school.be",
                        NormalizedEmail = "MILEETOO.DIMARKO@SCHOOL.BE",
                        EmailConfirmed = true,
                        SecurityStamp = Guid.NewGuid().ToString("D"),
                        FirstName = "Mileetoo",
                        LastName = "Die Marko",
                        IsTeacher = true,
                        Created = DateTime.Now
                    },
                    new ApplicationUser {
                        Id = Guid.Parse("00000000-0000-0000-0000-000000000020"),
                        UserName = "tibo.verkest@student.be",
                        NormalizedUserName = "TIBO.VERKEST@STUDENT.BE",
                        Email = "tibo.verkest@student.be",
                        NormalizedEmail = "TIBO.VERKEST@STUDENT.BE",
                        EmailConfirmed = true,
                        SecurityStamp = Guid.NewGuid().ToString("D"),
                        FirstName = "Tibo",
                        LastName = "Verkest",
                        IsTeacher = false,
                        Created = DateTime.Now
                    },
                    new ApplicationUser {
                        Id = Guid.Parse("00000000-0000-0000-0000-000000000021"),
                        UserName = "mathias.breda@student.be",
                        NormalizedUserName = "MATHIAS.BREDA@STUDENT.BE",
                        Email = "mathias.breda@student.be",
                        NormalizedEmail = "MATHIAS.BREDA@STUDENT.BE",
                        EmailConfirmed = true,
                        SecurityStamp = Guid.NewGuid().ToString("D"),
                        FirstName = "Mathias",
                        LastName = "Breda",
                        IsTeacher = false,
                        Created = DateTime.Now
                    },
                    new ApplicationUser {
                        Id = Guid.Parse("00000000-0000-0000-0000-000000000022"),
                        UserName = "kenny.demulder@student.be",
                        NormalizedUserName = "KENNY.DEMULDER@STUDENT.BE",
                        Email = "kenny.demulder@student.be",
                        NormalizedEmail = "KENNY.DEMULDER@STUDENT.BE",
                        EmailConfirmed = true,
                        SecurityStamp = Guid.NewGuid().ToString("D"),
                        FirstName = "Kenny",
                        LastName = "De Mulder",
                        IsTeacher = false,
                        Created = DateTime.Now
                    },
                    new ApplicationUser {
                        Id = Guid.Parse("00000000-0000-0000-0000-000000000023"),
                        UserName = "joeri.versyck@student.be",
                        NormalizedUserName = "JOERI.VERSYCK@STUDENT.BE",
                        Email = "joeri.versyck@student.be",
                        NormalizedEmail = "JOERI.VERSYCK@STUDENT.BE",
                        EmailConfirmed = true,
                        SecurityStamp = Guid.NewGuid().ToString("D"),
                        FirstName = "Joeri",
                        LastName = "Versyck",
                        IsTeacher = false,
                        Created = DateTime.Now
                    },
                };

            foreach (var user in applicationUsers)
            {
                user.PasswordHash = passwordHasher.HashPassword(user, "GebrokenPasta1");
            }

            var roles = new[]
{
                new IdentityRole<Guid> { Id = Guid.Parse("00000000-0000-0000-0000-000000000001"), Name = "Teacher", NormalizedName = "TEACHER" },
                new IdentityRole<Guid> { Id = Guid.Parse("00000000-0000-0000-0000-000000000002"), Name = "Student", NormalizedName = "STUDENT" }
            };
            //24
           
            var discountsProducts = new[]{
                new{
                    DiscountsId = discounts[0].Id,
                    ProductsId = products[0].Id
                },
                new{
                    DiscountsId = discounts[1].Id,
                    ProductsId = products[0].Id
                },
                new{
                    DiscountsId = discounts[2].Id,
                    ProductsId = products[0].Id
                },
                new{
                    DiscountsId = discounts[0].Id,
                    ProductsId = products[1].Id
                }
            };
            

            var userRoles = new IdentityUserRole<Guid>[]
            {
                new IdentityUserRole<Guid> { RoleId = Guid.Parse("00000000-0000-0000-0000-000000000001"), UserId = Guid.Parse("00000000-0000-0000-0000-000000000018") },
                new IdentityUserRole<Guid> { RoleId = Guid.Parse("00000000-0000-0000-0000-000000000001"), UserId = Guid.Parse("00000000-0000-0000-0000-000000000019") },
                new IdentityUserRole<Guid> { RoleId = Guid.Parse("00000000-0000-0000-0000-000000000002"), UserId = Guid.Parse("00000000-0000-0000-0000-000000000020") },
                new IdentityUserRole<Guid> { RoleId = Guid.Parse("00000000-0000-0000-0000-000000000002"), UserId = Guid.Parse("00000000-0000-0000-0000-000000000021") },
                new IdentityUserRole<Guid> { RoleId = Guid.Parse("00000000-0000-0000-0000-000000000002"), UserId = Guid.Parse("00000000-0000-0000-0000-000000000022") },
                new IdentityUserRole<Guid> { RoleId = Guid.Parse("00000000-0000-0000-0000-000000000002"), UserId = Guid.Parse("00000000-0000-0000-0000-000000000023") }
            };


            modelBuilder.Entity<Supplier>().HasData(suppliers);
           
            modelBuilder.Entity<Discount>().HasData(discounts);
            modelBuilder.Entity<Product>().HasData(products);
            modelBuilder.Entity<OrderType>().HasData(orderTypes);
            modelBuilder.Entity<IdentityRole<Guid>>().HasData(roles);
            modelBuilder.Entity<ApplicationUser>().HasData(applicationUsers);
            modelBuilder
                .Entity($"{nameof(Discount)}{nameof(Product)}")
                .HasData(discountsProducts);
            modelBuilder.Entity<IdentityUserRole<Guid>>().HasData(userRoles);
        }
    }
}
