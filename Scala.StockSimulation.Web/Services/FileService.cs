using ClosedXML.Excel;
using DocumentFormat.OpenXml.Spreadsheet;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using Scala.StockSimulation.Web.Data;
using Scala.StockSimulation.Core.Entities;

namespace Scala.StockSimulation.Web.Services
{
    public class FileService
    {
        private readonly ScalaStockSimulationDbContext _scalaStockSimulationDbContext;

        public FileService(ScalaStockSimulationDbContext scalaStockSimulationDbContext)
        {
            _scalaStockSimulationDbContext = scalaStockSimulationDbContext;
        }

        public byte[] ExportStudentOrderItems(Guid userId)
        {
            var workbook = new XLWorkbook();
            var worksheet = workbook.Worksheets.Add("Blad1");
            var data = _scalaStockSimulationDbContext.UserProductStates.Where(ups => ups.TransactionType != "Start" && ups.ApplicationUserId == userId)
                .Include(ups => ups.ApplicationUser)
                .Include(ups => ups.Product)
                .Include(ups => ups.Order)
                .Include(ups => ups.Product.Supplier)
                .Include(ups => ups.Product.Discounts)
                .OrderBy(ups => ups.ApplicationUser)
                .ThenBy(ups => ups.Order.OrderNumber)
                .ThenBy(ups => ups.Product)
                .ThenBy(ups => ups.Created)
                .ToList();

            worksheet.Cell(1, 1).Value = "ApplicationUser";
            worksheet.Cell(1, 2).Value = "Transactie Type";
            worksheet.Cell(1, 3).Value = "ProductName";
            worksheet.Cell(1, 4).Value = "Aantal besteld";
            worksheet.Cell(1, 5).Value = "Fysieke voorraad";
            worksheet.Cell(1, 6).Value = "Fictieve voorraad";
            worksheet.Cell(1, 7).Value = "Minimum voorraad";
            worksheet.Cell(1, 8).Value = "Maximum voorrad";
            worksheet.Cell(1, 9).Value = "Binnenkort beschikbaar ";
            worksheet.Cell(1, 10).Value = "Gereserveerd";
            worksheet.Cell(1, 11).Value = "Ordernummer";
            worksheet.Cell(1, 12).Value = "Created";
            worksheet.Cell(1, 13).Value = "Updated";
            worksheet.Cell(1, 14).Value = "Deleted";

            int row = 2;
            foreach (var item in data)
            {
                worksheet.Cell(row, 1).Value = $"{item.ApplicationUser.FirstName} {item.ApplicationUser.LastName}";
                worksheet.Cell(row, 2).Value = item.TransactionType;
                worksheet.Cell(row, 3).Value = item.Name;
                worksheet.Cell(row, 4).Value = item.Quantity;
                worksheet.Cell(row, 5).Value = item.PhysicalStock;
                worksheet.Cell(row, 6).Value = item.FictionalStock;
                worksheet.Cell(row, 7).Value = item.MinimumStock;
                worksheet.Cell(row, 8).Value = item.MaximumStock;
                worksheet.Cell(row, 9).Value = item.SoonAvailableStock;
                worksheet.Cell(row, 10).Value = item.ReservedStock;
                worksheet.Cell(row, 11).Value = item.Order.OrderNumber.ToString();
                worksheet.Cell(row, 12).Value = item.Created;
                worksheet.Cell(row, 13).Value = item.Updated == null ? "Besteld" : "Geleverd";
                worksheet.Cell(row, 14).Value = item.Order.Deleted;
                row++;
            }
            worksheet.Columns().AdjustToContents();
            using (var stream = new MemoryStream())
            {
                workbook.SaveAs(stream);
                stream.Position = 0;


                return stream.ToArray();
            }
        }

        public byte[] ExportOrderItems()
        {
            var workbook = new XLWorkbook();
            var worksheet = workbook.Worksheets.Add("Blad1");
            var data = _scalaStockSimulationDbContext.UserProductStates.Where(ups => ups.TransactionType != "Start")
                .Include(ups => ups.ApplicationUser)
                .Include(ups => ups.Product)
                .Include(ups => ups.Order)
                .Include(ups => ups.Product.Supplier)
                .Include(ups => ups.Product.Discounts)
                .OrderBy(ups => ups.ApplicationUser)
                .ThenBy(ups => ups.Order.OrderNumber)
                .ThenBy(ups => ups.Product)
                .ThenBy(ups => ups.Created)
                .ToList();

            worksheet.Cell(1, 1).Value = "ApplicationUser";
            worksheet.Cell(1, 2).Value = "Transactie Type";
            worksheet.Cell(1, 3).Value = "ProductName";
            worksheet.Cell(1, 4).Value = "Aantal besteld";
            worksheet.Cell(1, 5).Value = "Fysieke voorraad";
            worksheet.Cell(1, 6).Value = "Fictieve voorraad";
            worksheet.Cell(1, 7).Value = "Minimum voorraad";
            worksheet.Cell(1, 8).Value = "Maximum voorrad";
            worksheet.Cell(1, 9).Value = "Binnenkort beschikbaar ";
            worksheet.Cell(1, 10).Value = "Gereserveerd";
            worksheet.Cell(1, 11).Value = "Ordernummer";
            worksheet.Cell(1, 12).Value = "Created";
            worksheet.Cell(1, 13).Value = "Updated";
            worksheet.Cell(1, 14).Value = "Deleted";

            int row = 2;
            foreach (var item in data)
            {
                worksheet.Cell(row, 1).Value = $"{item.ApplicationUser.FirstName} {item.ApplicationUser.LastName}";
                worksheet.Cell(row, 2).Value = item.TransactionType;
                worksheet.Cell(row, 3).Value = item.Name;
                worksheet.Cell(row, 4).Value = item.Quantity;
                worksheet.Cell(row, 5).Value = item.PhysicalStock;
                worksheet.Cell(row, 6).Value = item.FictionalStock;
                worksheet.Cell(row, 7).Value = item.MinimumStock;
                worksheet.Cell(row, 8).Value = item.MaximumStock;
                worksheet.Cell(row, 9).Value = item.SoonAvailableStock;
                worksheet.Cell(row, 10).Value = item.ReservedStock;
                worksheet.Cell(row, 11).Value = item.Order.OrderNumber.ToString();
                worksheet.Cell(row, 12).Value = item.Created;
                worksheet.Cell(row, 13).Value = item.Updated == null ? "Besteld" : "Geleverd";
                worksheet.Cell(row, 14).Value = item.Order.Deleted;
                row++;
            }
            worksheet.Columns().AdjustToContents();
            using (var stream = new MemoryStream())
            {
                workbook.SaveAs(stream);
                stream.Position = 0;


                return stream.ToArray();
            }
        }
        public void ClearDbForImport()
        {
            _scalaStockSimulationDbContext.UserProductStates.RemoveRange(_scalaStockSimulationDbContext.UserProductStates);
            _scalaStockSimulationDbContext.Products.RemoveRange(_scalaStockSimulationDbContext.Products);
            _scalaStockSimulationDbContext.Orders.RemoveRange(_scalaStockSimulationDbContext.Orders);
            _scalaStockSimulationDbContext.OrderItems.RemoveRange(_scalaStockSimulationDbContext.OrderItems);
            _scalaStockSimulationDbContext.Suppliers.RemoveRange(_scalaStockSimulationDbContext.Suppliers);
            _scalaStockSimulationDbContext.Discounts.RemoveRange(_scalaStockSimulationDbContext.Discounts);
            _scalaStockSimulationDbContext.SaveChanges();
        }
        public void UploadFailed(List<Supplier> suppliers, List<Product> products)
        {
            try
            {
                ClearDbForImport();
                foreach (var supplier in suppliers)
                {

                    Supplier supplierToAdd = new Supplier();
                    supplierToAdd.Name = supplier.Name;
                    supplierToAdd.Created = DateTime.Now;                    
                    _scalaStockSimulationDbContext.Suppliers.Add(supplierToAdd);
                    _scalaStockSimulationDbContext.SaveChanges();
                }
                foreach (var product in products)
                {
                    Product productToAdd = new Product();
                    productToAdd.Name = product.Name;
                    productToAdd.Description = product.Description;
                    productToAdd.ArticleNumber = product.ArticleNumber;
                    productToAdd.SupplierId = _scalaStockSimulationDbContext.Suppliers
                                    .Where(s => s.Name == product.Supplier.Name)
                                    .Select(s => s.Id)
                                    .FirstOrDefault();
                    productToAdd.InitialStock = product.InitialStock;
                    productToAdd.InitialMaximumStock = product.InitialMaximumStock;
                    productToAdd.InitialMinimumStock = product.InitialMinimumStock;
                    productToAdd.Price = product.Price;
                    productToAdd.PriceWithDiscounts = product.PriceWithDiscounts;
                    productToAdd.Discounts = product.Discounts.Where(d => d != null ).ToList();
                    
                    _scalaStockSimulationDbContext.Products.Add(productToAdd);
                }
                _scalaStockSimulationDbContext.SaveChanges();
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }
    }
}
