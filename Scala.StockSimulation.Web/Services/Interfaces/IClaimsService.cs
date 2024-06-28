using System.Security.Claims;
using Scala.StockSimulation.Core.Entities;

namespace Scala.StockSimulation.Web.Services.Interfaces;

public interface IClaimsService{
    Task<IEnumerable<Claim>> GenerateClaimsForUser(ApplicationUser applicationUser);
}