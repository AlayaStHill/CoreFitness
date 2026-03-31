using CoreFitness.Application.MembershipTypes;
using CoreFitness.Presentation.WebApp.Models.Memberships;
using Microsoft.AspNetCore.Mvc;

namespace CoreFitness.Presentation.WebApp.Controllers.Public;

public class MembershipsController(IMembershipTypeService memberTypeService) : Controller
{
    public async Task<IActionResult> Index()
    {
        var result = await memberTypeService.GetFeaturedMembershipTypesAsync();
        if (result.IsFailure)
        {
            return View("Error");
        }

        var viewModel = new MembershipsIndexViewModel()
        {
            Memberships = result.Value
        };
        return View(viewModel);
    }
}
