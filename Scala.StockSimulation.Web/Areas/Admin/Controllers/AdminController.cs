
using System.Data;
using ClosedXML.Excel;
using DocumentFormat.OpenXml.Drawing.Charts;
using DocumentFormat.OpenXml.Math;
using DocumentFormat.OpenXml.Office.Word;
using DocumentFormat.OpenXml.VariantTypes;

using ExcelDataReader;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Scala.StockSimulation.Core.Entities;
using Scala.StockSimulation.Web.Areas.Admin.ViewModels;
using Scala.StockSimulation.Web.Controllers;
using Scala.StockSimulation.Web.Data;
using Scala.StockSimulation.Web.Services;
using Scala.StockSimulation.Web.ViewModels;
using SmartBreadcrumbs.Attributes;
using SmartBreadcrumbs.Nodes;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using Microsoft.AspNetCore.Identity;
using Scala.StockSimulation.Core.Enums;
using Scala.StockSimulation.Utilities;
using Scala.StockSimulation.Web.Areas.Admin.Services;

namespace Scala.StockSimulation.Web.Areas.Admin.Controllers{
    [Area("Admin")]
    [Authorize(Policy = "TeacherPolicy")]
    public class AdminController : Controller{
        private readonly ScalaStockSimulationDbContext _scalaStockSimulationDbContext;
        private readonly FileService _fileService;
        private readonly IWebHostEnvironment _hostingEnvironment;
        private readonly UserManager<ApplicationUser> _userManager;



        public AdminController(ScalaStockSimulationDbContext scalaStockSimulationDbContext,
            IWebHostEnvironment hostingEnvironment, UserManager<ApplicationUser> userManager){
            _scalaStockSimulationDbContext = scalaStockSimulationDbContext;
            _fileService = new FileService(_scalaStockSimulationDbContext);
            _hostingEnvironment = hostingEnvironment;
            _userManager = userManager;
        }

        [Breadcrumb("Admin", AreaName = "Admin")]
        public IActionResult Index(){
            return View();
        }


        [HttpGet]
        [Breadcrumb("Exporteer Bestellingen ", AreaName = "Admin", FromAction = "Index",
            FromController = typeof(AdminController))]
        public IActionResult ExportOrderIndex(){

            return View();
        }


        [HttpGet]

        public IActionResult ExportAllOrdersToExcel(){
            var stream = _fileService.ExportOrderItems();

            var fileName = "UserProductStates.xlsx";
            var contentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";

            return File(stream.ToArray(), contentType, fileName);
        }

        [HttpGet]
        public IActionResult ExportStudentOrderItems(Guid userId){
            var stream = _fileService.ExportStudentOrderItems(userId);

            var fileName = "UserProductStates.xlsx";
            var contentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";

            return File(stream.ToArray(), contentType, fileName);
        }

        [Breadcrumb("Studenten", AreaName = "Admin", FromAction = "Index", FromController = typeof(AdminController))]
        public IActionResult ShowStudents(){
            var users = _scalaStockSimulationDbContext.ApplicationUsers.Where(au => !au.IsTeacher);
            var adminShowStudentsViewModel = new AdminShowStudentsViewModel{
                Users = users.Select(u => new BaseViewModel{
                    Value = $"{u.FirstName} {u.LastName}",
                    Id = u.Id,
                })
            };


            return View(adminShowStudentsViewModel);
        }

        [Breadcrumb("Accounts", AreaName = "Admin", FromAction = "Index", FromController = typeof(AdminController))]
        public IActionResult StudentAccounts(){
            var users = _scalaStockSimulationDbContext.ApplicationUsers.Where(au => !au.IsTeacher);
            var adminStudentAccountsViewModel = new AdminStudentAccountsViewModel{
                Users = users.Select(u => new BaseViewModel{
                    Value = $"{u.FirstName} {u.LastName}",
                    Id = u.Id,
                })
            };


            return View(adminStudentAccountsViewModel);
        }

        [Breadcrumb("Transacties", AreaName = "Admin", FromAction = "ShowStudents",
            FromController = typeof(AdminController))]
        public IActionResult ShowTransactions(Guid userId){
            var applicationUser = _scalaStockSimulationDbContext.ApplicationUsers
                .FirstOrDefault(au => au.Id == userId);
            if (applicationUser == null){
                return Redirect(nameof(Index));
            }

            var orders = _scalaStockSimulationDbContext.Orders
                .Include(o => o.OrderType)
                .Include(o => o.UserProductStates)
                .ThenInclude(u => u.Product)
                .Where(o => o.ApplicationUserId == userId)
                .OrderByDescending(o => o.Created);

            var adminShowTransactionsViewModel = new AdminShowTransactionsViewModel{
                Transactions = orders.Select(o => new AdminBaseOrderViewModel{
                    OrderId = o.Id,
                    OrderType = o.OrderType.Name,
                    CustomerName = o.CustomerName == null ? string.Empty : o.CustomerName,
                    OrderNr = o.OrderNumber.ToString(),
                    UserProductStates = o.UserProductStates.Select(u => new BaseOrdersViewModel{
                        ProductName = u.Product.Name,
                        PhysicalStock = u.PhysicalStock,
                        FictionalStock = u.FictionalStock,
                        SoonAvailableStock = u.SoonAvailableStock,
                        ReservedStock = u.ReservedStock,
                        MinimumStock = u.MinimumStock,
                        MaximumStock = u.MaximumStock,
                        Quantity = u.Quantity,
                        Status = u.Updated == null ? "Besteld" : "Geleverd",
                        Created = u.Created,
                        OrderType = u.TransactionType,
                        ProductId = u.ProductId,
                    }).OrderBy(o => o.ProductId).ToList(),
                }).OrderBy(o => o.OrderNr),
                StudentName = $"{applicationUser.FirstName} {applicationUser.LastName}",
            };

            return View(adminShowTransactionsViewModel);
        }

        [HttpGet]
        [Breadcrumb("Zoek", AreaName = "Admin", FromAction = "Index", FromController = typeof(AdminController))]
        public IActionResult Search(){
            var adminSearchViewModel = new AdminSearchViewModel();
            return View(adminSearchViewModel);
        }

        [Breadcrumb("Bevestig reset", AreaName = "Admin", FromAction = "ShowStudents",
            FromController = typeof(AdminController))]
        public IActionResult ConfirmResetStudent(Guid userId){
            var applicationUser = _scalaStockSimulationDbContext.ApplicationUsers
                .FirstOrDefault(au => au.Id == userId);
            if (applicationUser == null) Redirect(nameof(Index));
            AdminConfirmResetStudentViewModel adminConfirmResetStudentViewModel = new AdminConfirmResetStudentViewModel{
                Id = userId,
                Name = $"{applicationUser.FirstName} {applicationUser.LastName}"
            };
            return View(adminConfirmResetStudentViewModel);
        }

        public IActionResult ResetStudent(AdminConfirmResetStudentViewModel adminConfirmResetStudentViewModel){
            var applicationUser = _scalaStockSimulationDbContext.ApplicationUsers
                .FirstOrDefault(au => au.Id == adminConfirmResetStudentViewModel.Id);
            if (applicationUser == null) Redirect(nameof(Index));

            _scalaStockSimulationDbContext.UserProductStates
                .Where(ups => ups.ApplicationUserId == adminConfirmResetStudentViewModel.Id).ToList()
                .ForEach(ups => _scalaStockSimulationDbContext.UserProductStates.Remove(ups));

            _scalaStockSimulationDbContext.Orders
                .Where(o => o.ApplicationUserId == adminConfirmResetStudentViewModel.Id)
                .Include(o => o.OrderItems).ToList()
                .ForEach(o => {
                    o.OrderItems.ToList()
                        .ForEach(oi => _scalaStockSimulationDbContext.OrderItems.Remove(oi));
                    _scalaStockSimulationDbContext.Orders.Remove(o);
                });
            _scalaStockSimulationDbContext.SaveChanges();
            return Redirect(nameof(Index));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Breadcrumb("Zoekresultaten", AreaName = "Admin", FromAction = "Index",
            FromController = typeof(AdminController))]
        public IActionResult ShowResults(AdminSearchViewModel adminSearchViewModel){
            if (!ModelState.IsValid){
                return View("Search", adminSearchViewModel);
            }

            if (string.IsNullOrEmpty(adminSearchViewModel.SearchTerm)){
                ModelState.AddModelError("SearchTerm", "Geen gebruiker gevonden.");

                return View("Search", adminSearchViewModel);
            }

            var searchTerm = adminSearchViewModel.SearchTerm;

            searchTerm = searchTerm.Trim();
            var names = searchTerm.Split(" ");

            var firstName = names.Length > 0 ? names[0] : null;
            var lastName = names.Length > 1 ? names[1] : null;

            if (string.IsNullOrEmpty(firstName) && string.IsNullOrEmpty(lastName)){
                ModelState.AddModelError("SearchTerm", "Geen gebruiker gevonden.");
                return View("ShowResults", adminSearchViewModel);
            }

            var user = _scalaStockSimulationDbContext.ApplicationUsers.Where(u => u.IsTeacher == false).FirstOrDefault(
                u => u.FirstName.Contains(firstName) || u.FirstName.Contains(lastName) ||
                     u.LastName.Contains(firstName) || u.LastName.Contains(lastName));

            if (user == null){
                ModelState.AddModelError("SearchTerm", "Geen gebruiker gevonden.");
                return View("Search", adminSearchViewModel);
            }

            var orders = _scalaStockSimulationDbContext
                .Orders
                .Where(o => o.ApplicationUserId == user.Id)
                .OrderBy(o => o.OrderNumber)
                .ToList();


            var userProductStates = new List<UserProductState>();

            foreach (var order in orders){
                userProductStates.AddRange(_scalaStockSimulationDbContext.UserProductStates
                    .Where(u => u.OrderId == order.Id)
                    .Include(ups => ups.ApplicationUser)
                    .Include(ups => ups.Product)
                    .Include(ups => ups.Order)
                    .Include(ups => ups.Product.Supplier)
                    .OrderBy(ups => ups.ApplicationUser)
                    .ThenBy(ups => ups.OrderId)
                    .ThenBy(ups => ups.Product)
                    .ThenBy(ups => ups.Created)
                    .ToList());
            }

            userProductStates = userProductStates.Distinct().ToList();

            if (orders.Count == 0){
                ModelState.AddModelError("SearchTerm", "Geen bestellingen gevonden.");
            }


            var adminShowResultsViewModel = new AdminShowResultsViewModel{
                Name = user.FirstName + " " + user.LastName,
                ApplicationUserId = user.Id,
                SearchTerm = adminSearchViewModel.SearchTerm,
                Orders = orders.Select(o => new BaseViewModel{
                    Id = o.Id,
                    Value = o.OrderNumber.ToString()
                }),
                UserProductStates = userProductStates.Select(u => new BaseOrdersViewModel{
                    FictionalStock = u.FictionalStock,
                    MaximumStock = u.MaximumStock,
                    MinimumStock = u.MinimumStock,
                    PhysicalStock = u.PhysicalStock,
                    ProductId = u.ProductId,
                    ProductName = u.Name,
                    Quantity = u.Quantity,
                    ReservedStock = u.ReservedStock,
                    SoonAvailableStock = u.SoonAvailableStock,
                    Created = u.Created,
                    Status = u.Updated == null ? "Besteld" : "Geleverd",
                    OrderType = u.TransactionType,
                    OrderId = (Guid)u.OrderId
                }).ToList(),



            };

            return View(adminShowResultsViewModel);
        }


        [HttpGet]
        [Breadcrumb("Import Bestellingen", AreaName = "Admin", FromAction = "Index",
            FromController = typeof(AdminController))]
        public IActionResult ImportOrderIndex(){
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Breadcrumb("Import Bestellingen", AreaName = "Admin", FromAction = "Index",
            FromController = typeof(AdminController))]
        public async Task<IActionResult> UploadExcelFile(IFormFile file){
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);

            if (file == null || Path.GetExtension(file.FileName).ToLower() != ".xlsx" &&
                Path.GetExtension(file.FileName).ToLower() != ".xls"){
                TempData["ErrorMessage"] = "Gelieve een geldig excel bestand te uploaden(xls of xlsx extentie).";
                ModelState.AddModelError("file", "Gelieve een geldig excel bestand te uploaden(xls of xlsx extentie).");
                return RedirectToAction(nameof(ImportOrderIndex));
            }

            UploadExcelFileViewModel uploadExcelFileViewModel = new();

            if (file != null && file.Length >= 0){

                var uploadFolder = $"{Directory.GetCurrentDirectory()}\\wwwroot\\uploads";

                if (!Directory.Exists(uploadFolder)){
                    Directory.CreateDirectory(uploadFolder);
                }

                var filePath = Path.Combine(uploadFolder, file.FileName);

                using (var stream = new FileStream(filePath, FileMode.Create)){
                    await file.CopyToAsync(stream);
                }

                uploadExcelFileViewModel = new UploadExcelFileViewModel{
                    FilePath = filePath,
                    FileName = file.FileName
                };
            }

            return View(nameof(ConfirmExcelUpload), uploadExcelFileViewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult ConfirmExcelUpload(UploadExcelFileViewModel uploadExcelFileViewModel){
            return View();
        }

        public IActionResult CancelExcelUpload(string filePath){
            System.IO.File.Delete(filePath);
            TempData["ErrorMessage"] = "Bestand importeren afgebroken.";
            return RedirectToAction(nameof(ImportOrderIndex));
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ProcessExcelUpload(string filePath){
            var products = await _scalaStockSimulationDbContext.Products
                .Include(p => p.Discounts).ToListAsync();
            var suppliers = await _scalaStockSimulationDbContext.Suppliers.ToListAsync();

            _fileService.ClearDbForImport();

            using (var stream = System.IO.File.Open(filePath, FileMode.Open, FileAccess.Read)){

                using (var reader = ExcelReaderFactory.CreateReader(stream)){
                    do{
                        bool isHeaderSkipped = false;
                        List<string> notifications = new List<string>();
                        int row = 1;
                        while (reader.Read()){
                            if (!isHeaderSkipped){
                                isHeaderSkipped = true;
                                row++;
                                continue;
                            }

                            var productName = reader.GetValue(0)?.ToString().ToLower();
                            var supplierName = reader.GetValue(3)?.ToString().ToLower();
                            var description = reader.GetValue(1)?.ToString().ToLower();
                            var articleNumber = reader.GetValue(2)?.ToString().ToLower();
                            var initialStock = reader.GetValue(4)?.ToString();
                            var initialMaximumStock = reader.GetValue(5)?.ToString();
                            var initialMinimumStock = reader.GetValue(6)?.ToString();
                            var price = reader.GetValue(7)?.ToString();
                            var priceWithDiscounts = reader.GetValue(8)?.ToString();
                            var shippingCosts = reader.GetValue(9)?.ToString();
                            var discountVolume = reader.GetValue(10)?.ToString();
                            var discountSupplier = reader.GetValue(11)?.ToString();

                            if (string.IsNullOrWhiteSpace(productName)){
                                _fileService.UploadFailed(suppliers, products);
                                TempData["ProductNullMessage"] = $"Rij {row}: Productnaam is verplicht.";
                                return RedirectToAction(nameof(ImportOrderIndex));
                            }

                            if (string.IsNullOrWhiteSpace(description)){
                                _fileService.UploadFailed(suppliers, products);
                                TempData["DescriptionNullMessage"] = $"Rij {row}: Beschrijving is verplicht.";
                                return RedirectToAction(nameof(ImportOrderIndex));
                            }

                            if (string.IsNullOrWhiteSpace(articleNumber)){
                                _fileService.UploadFailed(suppliers, products);
                                TempData["ArticleNumberNullMessage"] = $"Rij {row}: Artikelnummer is verplicht.";
                                return RedirectToAction(nameof(ImportOrderIndex));
                            }

                            if (string.IsNullOrWhiteSpace(supplierName)){
                                _fileService.UploadFailed(suppliers, products);
                                TempData["SupplierNullMessage"] = $"Rij {row}: Leverancier is verplicht.";
                                return RedirectToAction(nameof(ImportOrderIndex));
                            }

                            if (!int.TryParse(initialStock, out int initial)){
                                _fileService.UploadFailed(suppliers, products);
                                TempData["InitialStockNullMessage"] =
                                    $"Rij {row}: Initiële voorraad moet een cijfer zijn";
                                return RedirectToAction(nameof(ImportOrderIndex));
                            }

                            if (!int.TryParse(initialMaximumStock, out int max)){
                                _fileService.UploadFailed(suppliers, products);
                                TempData["MaxStockNullMessage"] = $"Rij {row}: Maximum voorraad moet een cijfer zijn";
                                return RedirectToAction(nameof(ImportOrderIndex));
                            }

                            if (!int.TryParse(initialMinimumStock, out int min)){
                                _fileService.UploadFailed(suppliers, products);
                                TempData["MinStockNullMessage"] = $"Rij {row}: Minimum voorraad moet een cijfer zijn";
                                return RedirectToAction(nameof(ImportOrderIndex));
                            }

                            if (!decimal.TryParse(price, out decimal priceDecimal)){
                                _fileService.UploadFailed(suppliers, products);
                                TempData["PriceWithDiscountsNullMessage"] =
                                    $"Rij {row}: Prijs met kortingen moet een numerieke waarde zijn.";
                                return RedirectToAction(nameof(ImportOrderIndex));
                            }

                            if (!decimal.TryParse(priceWithDiscounts, out decimal priceWithDiscountsDecimal)){
                                _fileService.UploadFailed(suppliers, products);
                                TempData["PriceNullMessage"] = $"Rij {row}: Prijs moet een numerieke waarde zijn.";
                                return RedirectToAction(nameof(ImportOrderIndex));
                            }

                            if (!decimal.TryParse(shippingCosts, out decimal shippingCostDecimal)){
                                _fileService.UploadFailed(suppliers, products);
                                TempData["ShippingCostsNullMessage"] =
                                    $"Rij {row}: Verzendkosten moeten een numerieke waarde zijn.";
                                return RedirectToAction(nameof(ImportOrderIndex));
                            }

                            if (!decimal.TryParse(discountVolume, out decimal discountVolumeDecimal)){
                                _fileService.UploadFailed(suppliers, products);
                                TempData["DiscountVolumeNullMessage"] =
                                    $"Rij {row}: Staffelkorting moet een cijfer  zijn.";
                                return RedirectToAction(nameof(ImportOrderIndex));
                            }

                            if (!decimal.TryParse(discountSupplier, out decimal discountSupplierDecimal)){
                                _fileService.UploadFailed(suppliers, products);
                                TempData["DiscountSupplierNullMessage"] =
                                    $"Rij {row}: Leverancierskorting moet een cijfer  zijn.";
                                return RedirectToAction(nameof(ImportOrderIndex));
                            }



                            if (!_scalaStockSimulationDbContext.Suppliers.Any(s => s.Name == supplierName)){
                                Supplier supplier = new Supplier();
                                supplier.Name = supplierName;
                                supplier.Created = DateTime.Now;
                                _scalaStockSimulationDbContext.Suppliers.Add(supplier);
                                _scalaStockSimulationDbContext.SaveChanges();
                            }

                            Guid supplierId = _scalaStockSimulationDbContext.Suppliers
                                .Where(s => s.Name == supplierName)
                                .Select(s => s.Id)
                                .FirstOrDefault();

                            if (!_scalaStockSimulationDbContext.Products.Any(p => p.Name == productName)){
                                Product product = new Product();
                                product.Name = reader.GetValue(0).ToString();
                                product.Description = description;
                                product.ArticleNumber = articleNumber;
                                product.SupplierId = supplierId;
                                product.InitialStock = Convert.ToInt32(reader.GetValue(4));
                                product.InitialMaximumStock = Convert.ToInt32(reader.GetValue(5));
                                product.InitialMinimumStock = Convert.ToInt32(reader.GetValue(6));
                                product.Price = Convert.ToDecimal(reader.GetValue(7));
                                product.PriceWithDiscounts = Convert.ToDecimal(reader.GetValue(8));
                                product.Discounts = new List<Discount>();

                                string shippingCostValue = reader.GetValue(9)?.ToString();
                                if (!string.IsNullOrWhiteSpace(shippingCostValue) &&
                                    decimal.TryParse(shippingCostValue, out decimal shippingCost)){
                                    product.Discounts.Add(new Discount
                                        { Type = DiscountType.Shipping, Rate = shippingCost });
                                }

                                string volumeDiscountValue = reader.GetValue(10)?.ToString();
                                if (!string.IsNullOrWhiteSpace(volumeDiscountValue) &&
                                    decimal.TryParse(volumeDiscountValue, out decimal volumeDiscountRate)){
                                    product.Discounts.Add(new Discount
                                        { Type = DiscountType.Volume, Rate = volumeDiscountRate });
                                }

                                string supplierDiscountValue = reader.GetValue(11)?.ToString();
                                if (!string.IsNullOrWhiteSpace(supplierDiscountValue) &&
                                    decimal.TryParse(supplierDiscountValue, out decimal supplierDiscountRate)){
                                    product.Discounts.Add(new Discount
                                        { Type = DiscountType.Supplier, Rate = supplierDiscountRate });
                                }

                                _scalaStockSimulationDbContext.Products.Add(product);
                                _scalaStockSimulationDbContext.Discounts.AddRange(product.Discounts);
                                _scalaStockSimulationDbContext.SaveChanges();
                            }

                            row++;
                        }

                        _scalaStockSimulationDbContext.SaveChanges();
                    } while (reader.NextResult());

                }

                System.IO.File.Delete(filePath);
            }

            TempData["SuccessMessage"] = "Bestand is succesvol geimporteerd!";
            return RedirectToAction(nameof(ImportOrderIndex));
        }

        [HttpGet]
        public IActionResult DownloadTemplate(){
            var fileName = "TemplateStart.xlsx";
            var filePath = Path.Combine(_hostingEnvironment.WebRootPath, fileName);

            var stream = new FileStream(filePath, FileMode.Open);

            var contentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";

            return File(stream, contentType, fileName);
        }

        [Breadcrumb("Download Template", AreaName = "Admin", FromAction = "Index",
            FromController = typeof(AdminController))]
        public IActionResult DownloadTemplateIndex(){
            return View();
        }

        [HttpGet]
        [Breadcrumb("Import Studenten", AreaName = "Admin", FromAction = "Index",
            FromController = typeof(AdminController))]
        public IActionResult ImportStudentsIndex(){
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Breadcrumb("Import Studenten", AreaName = "Admin", FromAction = "Index",
            FromController = typeof(AdminController))]
        public async Task<IActionResult> UploadExcelFileStudents(IFormFile file){
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);

            if (file == null || Path.GetExtension(file.FileName).ToLower() != ".xlsx" &&
                Path.GetExtension(file.FileName).ToLower() != ".xls"){
                TempData["ErrorMessage"] = "Gelieve een geldig excel bestand te uploaden(xls of xlsx extentie).";
                ModelState.AddModelError("file", "Gelieve een geldig excel bestand te uploaden(xls of xlsx extentie).");
                return RedirectToAction(nameof(ImportStudentsIndex));
            }

            UploadExcelFileViewModel uploadExcelFileViewModel = new();

            if (file != null && file.Length >= 0){

                var uploadFolder = $"{Directory.GetCurrentDirectory()}\\wwwroot\\uploads";

                if (!Directory.Exists(uploadFolder)){
                    Directory.CreateDirectory(uploadFolder);
                }

                var filePath = Path.Combine(uploadFolder, file.FileName);

                using (var stream = new FileStream(filePath, FileMode.Create)){
                    await file.CopyToAsync(stream);
                }

                uploadExcelFileViewModel = new UploadExcelFileViewModel{
                    FilePath = filePath,
                    FileName = file.FileName
                };
            }

            return View(nameof(ConfirmExcelUploadStudents), uploadExcelFileViewModel);
        }

        public IActionResult ConfirmExcelUploadStudents(UploadExcelFileViewModel uploadExcelFileViewModel){
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Breadcrumb("Import Studenten", AreaName = "Admin", FromAction = "Index",
            FromController = typeof(AdminController))]
        public async Task<IActionResult> ProcessExcelUploadForStudents(string filePath){
            var notifications = new List<string>();
            using (var stream = System.IO.File.Open(filePath, FileMode.Open, FileAccess.Read)){
                using (var reader = ExcelReaderFactory.CreateReader(stream)){
                    do{
                        bool isHeaderSkipped = false;
                        int row = 1;
                        while (reader.Read()){
                            if (!isHeaderSkipped){
                                isHeaderSkipped = true;
                                row++;
                                continue;
                            }

                            var userName = reader.GetValue(0)?.ToString().Trim().ToLower();
                            var firstName = reader.GetValue(1)?.ToString().Trim();
                            var lastName = reader.GetValue(2)?.ToString().Trim();

                            if (string.IsNullOrWhiteSpace(userName)){
                                TempData["UserNameNullMessage"] = $"Rij {row}: Gebruikersnaam is verplicht.";
                                return RedirectToAction(nameof(ImportStudentsIndex));

                            }
                            if (!Regex.IsMatch(userName,
                                    @"^[a-zA-Z._%+-]+@student\.be$",
                                    RegexOptions.IgnoreCase)){
                                TempData["NotValidUserNameMessage"] = $"Rij {row}: Je moet een geldige gebruikersnaam invoeren";
                                return RedirectToAction(nameof(ImportStudentsIndex));

                            }

                            if (string.IsNullOrWhiteSpace(firstName)){
                                TempData["FirstNameNullMessage"] = $"Rij {row}: Voornaam is verplicht.";
                                return RedirectToAction(nameof(ImportStudentsIndex));
                            }

                            if (string.IsNullOrWhiteSpace(lastName)){
                                TempData["LastNameNullMessage"] = $"Rij {row}: Familienaam is verplicht.";
                                return RedirectToAction(nameof(ImportStudentsIndex));
                            }

                            var existingUser = await _userManager.FindByEmailAsync(userName);
                            if (existingUser == null){
                                ApplicationUser newStudent = new ApplicationUser{
                                    UserName = userName,
                                    FirstName = firstName,
                                    LastName = lastName,
                                    Email = userName,
                                    EmailConfirmed = true,
                                    Created = DateTime.Now

                                };
                                var result = await _userManager.CreateAsync(newStudent, GlobalConstants.Password);
                                if (result.Succeeded){
                                    await _userManager.AddToRoleAsync(newStudent, "Student");
                                }
                                else{
                                    notifications.Add(
                                        $"Rij {row}: Gebruiker aanmaken mislukt. Fouten: {string.Join(", ", result.Errors.Select(e => e.Description))}");
                                }
                            }
                            else{
                                notifications.Add($"Rij {row}: Gebruiker bestaat al.");
                            }

                            row++;
                        }
                    } while (reader.NextResult());
                }

                System.IO.File.Delete(filePath);
              
            }

            TempData["SuccessMessage"] = "Bestand is succesvol geimporteerd!";
            return RedirectToAction(nameof(ImportStudentsIndex));

        }
        
        
        [HttpGet]
        public IActionResult DownloadTemplateForStudents(){
            var fileName = "TemplateStartStudents.xlsx";
            var filePath = Path.Combine(_hostingEnvironment.WebRootPath, fileName);

            var stream = new FileStream(filePath, FileMode.Open);

            var contentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";

            return File(stream, contentType, fileName);
        }

        [Breadcrumb("Download Template voor import studenten", AreaName = "Admin", FromAction = "Index",
            FromController = typeof(AdminController))]
        public IActionResult DownloadTemplateIndexStudents(){
            return View();
        }
    }
}
