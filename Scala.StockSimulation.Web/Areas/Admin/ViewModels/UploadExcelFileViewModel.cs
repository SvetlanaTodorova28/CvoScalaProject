using Microsoft.AspNetCore.Mvc;

namespace Scala.StockSimulation.Web.Areas.Admin.ViewModels
{
    public class UploadExcelFileViewModel
    {
        [HiddenInput]
        public string FilePath { get; set; }
        public string FileName { get; set; }
    }
}