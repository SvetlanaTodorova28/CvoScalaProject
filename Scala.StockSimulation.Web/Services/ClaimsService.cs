using System.Security.Claims;
using Microsoft.AspNetCore.Identity;
using Scala.StockSimulation.Core.Entities;
using Scala.StockSimulation.Web.Services.Interfaces;

namespace Scala.StockSimulation.Web.Services;

public class ClaimsService:IClaimsService{
    private readonly UserManager<ApplicationUser> _userManager;
  
    public ClaimsService(UserManager<ApplicationUser> userManager)
    {
        _userManager = userManager;
    }

    public async Task<IEnumerable<Claim>> GenerateClaimsForUser(ApplicationUser applicationUser)
    {
        var claims = new List<Claim>
        {
            new Claim(ClaimTypes.Name, applicationUser.UserName),
            new Claim(ClaimTypes.NameIdentifier, applicationUser.Id.ToString()),
        };
       
        var userRoles = await _userManager.GetRolesAsync(applicationUser);
        
        foreach (var userRole in userRoles){
            claims.Add(new Claim(ClaimTypes.Role, userRole));
        }
        
        claims.Add(new Claim("FirstName", applicationUser.FirstName ?? ""));
        claims.Add(new Claim("LastName", applicationUser.LastName ?? ""));
       
        
        return claims;
    }
}