using Microsoft.AspNetCore.Mvc;
using SmartBreadcrumbs.Attributes;

namespace Scala.StockSimulation.Web.Controllers;


public class BreadCrumbsController:Controller{
    
    [DefaultBreadcrumb("Home")]
    public IActionResult Index(){
        
        return RedirectToAction("Logout", "Account");
    }
}