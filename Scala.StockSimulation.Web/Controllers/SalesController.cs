using DocumentFormat.OpenXml.InkML;
using DocumentFormat.OpenXml.Office2010.Excel;
using Irony.Parsing;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Scala.StockSimulation.Core.Entities;
using Scala.StockSimulation.Utilities.Authorization;
using Scala.StockSimulation.Web.Data;
using Scala.StockSimulation.Web.ViewModels;
using SmartBreadcrumbs.Attributes;
using System.Drawing.Text;
using System.Security.Claims;

namespace Scala.StockSimulation.Web.Controllers
{
    [AuthorizeMultiplePolicy("StudentPolicy,TeacherPolicy", false)]
    public class SalesController : Controller
    {
        private readonly ScalaStockSimulationDbContext _scalaStockSimulationDbContext;
        private readonly UserManager<ApplicationUser> _userManager;

        public SalesController(ScalaStockSimulationDbContext scalaStockSimulationDbContext, UserManager<ApplicationUser> userManager)
        {
            _scalaStockSimulationDbContext = scalaStockSimulationDbContext;
            _userManager = userManager;
        }

        [Breadcrumb("Selecteer grafiektype", FromAction = "Index", FromController = typeof(OverviewController))]
        public IActionResult Index(Guid studentId)
        {
            studentId = GetStudentIdForGraph(studentId);

            if (studentId == Guid.Empty)
            {
                return NotFound("Er is geen student geselecteerd.");
            }

            var student = _scalaStockSimulationDbContext.ApplicationUsers
                .FirstOrDefault(u => u.Email == User.Identity.Name && !u.IsTeacher);

            if (student == null && studentId != Guid.Empty)
            {
                return View(new SalesIndexViewModel { StudentId = studentId });
            }
            else if (student == null)
            {
                return NotFound();
            }
            else
            {
                return View(new SalesIndexViewModel { StudentId = student.Id });
            }
        }

        [Breadcrumb("Selecteer artikelen", FromAction = "Index", FromController = typeof(SalesController))]
        public IActionResult SelectProducts(Guid studentId, string chartType)
        {
            studentId = GetStudentIdForGraph(studentId);

            if (studentId == Guid.Empty)
            {
                return NotFound("Er is geen student geselecteerd.");
            }

            var salesSelectProductsViewModel = new SalesSelectProductsViewModel
            {
                StudentId = studentId,
                ChartType = chartType,
                Products = _scalaStockSimulationDbContext.Products
                    .Select(p => new CheckboxViewModel
                    {
                        ProductId = p.Id,
                        Text = p.Name,
                        Description = p.Description,
                        ArticleNumber = p.ArticleNumber,
                        Price = p.Price,
                    }).ToList()
            };
            return View(salesSelectProductsViewModel);
        }

        [Breadcrumb("Overzicht verkoopcijfers", FromAction = "SelectProducts", FromController = typeof(SalesController))]
        [HttpPost]
        public IActionResult ShowResults(Guid studentId, Guid[] selectedProducts, string chartType)
        {
            if (selectedProducts == null || selectedProducts.Length == 0)
            {
                return BadRequest("Geen producten geselecteerd.");
            }

            chartType = string.IsNullOrEmpty(chartType) ? HttpContext.Session.GetString("chartType") ?? "bar" : chartType;
            HttpContext.Session.SetString("chartType", chartType);

            var salesShowResultsViewModel = new SalesShowResultsViewModel
            {
                StudentId = studentId,
                StudentName = _scalaStockSimulationDbContext.ApplicationUsers
                    .Where(u => u.Id == studentId)
                    .Select(u => $"{u.FirstName} {u.LastName}")
                    .FirstOrDefault(),
                SalesData = _scalaStockSimulationDbContext.UserProductStates
                    .Where(ups => ups.ApplicationUser.Id == studentId && selectedProducts.Contains(ups.ProductId) &&
                                  ups.Order.OrderTypeId != Guid.Parse("00000000-0000-0000-0000-000000000016") &&
                                  ups.Updated == null)
                    .GroupBy(oi => oi.ProductId)
                    .Select(g => new BaseOrdersViewModel
                    {
                        ProductId = g.Key,
                        ProductName = g.FirstOrDefault().Product.Name,
                        Quantity = g.Sum(oi => oi.Quantity)
                    }).ToList()
            };

            var currentUser = _scalaStockSimulationDbContext.ApplicationUsers
                .FirstOrDefault(u => u.Email == User.Identity.Name);

            salesShowResultsViewModel.IsStudent = salesShowResultsViewModel.StudentName == $"{currentUser.FirstName} {currentUser.LastName}";

            switch (chartType)
            {
                case "bar":
                    salesShowResultsViewModel.ChartType = chartType;
                    return View("ShowBarChart", salesShowResultsViewModel);

                case "pie":
                    salesShowResultsViewModel.ChartType = chartType;
                    return View("ShowPieChart", salesShowResultsViewModel);

                default:
                    return NotFound();
            }
        }

        private Guid GetStudentIdForGraph(Guid studentId)
        {
            if (User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Role)?.Value == "Teacher")
            {
                if (HttpContext.Session.Keys.Contains("studentIdForGraph"))
                {
                    var sessionStudentId = Guid.Parse(HttpContext.Session.GetString("studentIdForGraph"));
                    if (studentId == Guid.Empty || studentId == sessionStudentId)
                    {
                        studentId = sessionStudentId;
                    }
                    else
                    {
                        HttpContext.Session.SetString("studentIdForGraph", studentId.ToString());
                    }
                }
                else if (studentId != Guid.Empty)
                {
                    HttpContext.Session.SetString("studentIdForGraph", studentId.ToString());
                }
            }
            else
            {
                studentId = Guid.Parse(_userManager.GetUserId(User));
            }

            return studentId;
        }
    }
}