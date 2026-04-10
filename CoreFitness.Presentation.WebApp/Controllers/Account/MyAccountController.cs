using CoreFitness.Application.Abstractions.Authentication;
using CoreFitness.Application.MyAccount;
using CoreFitness.Application.MyAccount.Inputs;
using CoreFitness.Presentation.WebApp.Models.MyAccount;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CoreFitness.Presentation.WebApp.Controllers.Account;

[Authorize]
public class MyAccountController(
    ICurrentUserService currentUserService,
    IMyAccountUserService myAccountUserService,
    IProfileImageStorageService profileImageStorageService,
    IMyAccountMembershipService myAccountMembershipService,
    IMyAccountBookingService myAccountBookingService) : Controller
{
    private const string ProfileImageViewDataKey = "MyAccountProfileImageUrl";

    [HttpGet]
    public async Task<IActionResult> AboutMe()
    {
        if (string.IsNullOrWhiteSpace(currentUserService.UserId))
            return Challenge();

        var user = await myAccountUserService.GetByIdAsync(currentUserService.UserId);
        if (user is null)
            return Challenge();

        SetMyAccountLayoutData(user.ImageUrl);

        AboutMeFormModel model = new()
        {
            FirstName = user.FirstName ?? string.Empty,
            LastName = user.LastName ?? string.Empty,
            Email = user.Email,
            PhoneNumber = user.PhoneNumber,
            ImageUrl = user.ImageUrl
        };

        return View(model);
    }

    [HttpGet]
    public async Task<IActionResult> NewBooking(CancellationToken ct)
    {
        if (string.IsNullOrWhiteSpace(currentUserService.UserId))
            return Challenge();

        var user = await myAccountUserService.GetByIdAsync(currentUserService.UserId);
        if (user is null)
            return Challenge();

        SetMyAccountLayoutData(user.ImageUrl);

        var membershipOverview = await myAccountMembershipService.GetOverviewAsync(currentUserService.UserId, ct);
        var hasActiveMembership = string.Equals(
            membershipOverview?.CurrentMembership?.Status,
            "Active",
            StringComparison.OrdinalIgnoreCase);

        if (!hasActiveMembership)
            return View(new NewBookingViewModel { HasActiveMembership = false });

        var sessions = await myAccountBookingService.GetUpcomingSessionsAsync(currentUserService.UserId, ct);

        NewBookingViewModel model = new()
        {
            HasActiveMembership = true,
            Sessions = sessions
                .Select(session => new NewBookingSessionItemViewModel
                {
                    WorkoutSessionId = session.WorkoutSessionId,
                    WorkoutTitle = session.WorkoutTitle,
                    WorkoutCategory = session.WorkoutCategory,
                    StartsAt = session.StartsAt,
                    DurationMinutes = session.DurationMinutes,
                    BookedCount = session.BookedCount,
                    Capacity = session.Capacity,
                    IsAlreadyBooked = session.IsAlreadyBooked
                })
                .ToList()
        };

        return View(model);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> BookSession(Guid workoutSessionId, CancellationToken ct)
    {
        if (string.IsNullOrWhiteSpace(currentUserService.UserId))
            return Challenge();

        await myAccountBookingService.BookSessionAsync(currentUserService.UserId, workoutSessionId, ct);

        return RedirectToAction(nameof(MyBookings));
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> CancelBooking(Guid workoutSessionId, CancellationToken ct)
    {
        if (string.IsNullOrWhiteSpace(currentUserService.UserId))
            return Challenge();

        await myAccountBookingService.CancelSessionAsync(currentUserService.UserId, workoutSessionId, ct);

        return RedirectToAction(nameof(MyBookings));
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> AboutMe(AboutMeFormModel model, CancellationToken ct)
    {
        if (string.IsNullOrWhiteSpace(currentUserService.UserId))
            return Challenge();

        if (!ModelState.IsValid)
        {
            SetMyAccountLayoutData(model.ImageUrl);
            return View(model);
        }

        string? imageUrl = model.ImageUrl;

        if (model.ProfileImage is not null && model.ProfileImage.Length > 0)
        {
            await using Stream stream = model.ProfileImage.OpenReadStream();
            imageUrl = await profileImageStorageService.SaveProfileImageAsync(stream, model.ProfileImage.FileName, ct);
        }

        var input = new UpdateMyAccountUserInput(
            currentUserService.UserId,
            model.Email,
            model.FirstName,
            model.LastName,
            model.PhoneNumber,
            imageUrl);

        var result = await myAccountUserService.UpdateAsync(input);
        if (result.IsFailure)
        {
            model.ImageUrl = imageUrl;
            SetMyAccountLayoutData(model.ImageUrl);

            ModelState.AddModelError(string.Empty, result.Error?.Message ?? "Could not update profile.");
            return View(model);
        }

        TempData["SuccessMessage"] = "Profile updated successfully.";
        return RedirectToAction(nameof(AboutMe));
    }

    [HttpGet]
    public async Task<IActionResult> MyMemberships(CancellationToken ct)
    {
        if (string.IsNullOrWhiteSpace(currentUserService.UserId))
            return Challenge();

        var user = await myAccountUserService.GetByIdAsync(currentUserService.UserId);
        if (user is null)
            return Challenge();

        SetMyAccountLayoutData(user.ImageUrl);

        var overview = await myAccountMembershipService.GetOverviewAsync(currentUserService.UserId, ct);

        MyMembershipsViewModel model = overview is null
            ? new MyMembershipsViewModel()
            : new MyMembershipsViewModel
            {
                HasMembership = overview.HasMembership,
                CurrentMembership = overview.CurrentMembership is null
                    ? null
                    : new CurrentMembershipViewModel
                    {
                        MembershipId = overview.CurrentMembership.MembershipId,
                        MembershipTypeId = overview.CurrentMembership.MembershipTypeId,
                        MembershipName = overview.CurrentMembership.MembershipName,
                        PricePerMonth = overview.CurrentMembership.PricePerMonth,
                        StartDate = overview.CurrentMembership.StartDate,
                        Status = overview.CurrentMembership.Status,
                        Benefits = overview.CurrentMembership.Benefits
                    },
                AvailablePlans = overview.AvailablePlans
                    .Select(plan => new MembershipPlanViewModel
                    {
                        MembershipTypeId = plan.MembershipTypeId,
                        Name = plan.Name,
                        PricePerMonth = plan.PricePerMonth,
                        Benefits = plan.Benefits
                    })
                    .ToList()
            };

        return View(model);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> SelectPlan(Guid membershipTypeId, CancellationToken ct)
    {
        if (string.IsNullOrWhiteSpace(currentUserService.UserId))
            return Challenge();

        await myAccountMembershipService.SelectPlanAsync(currentUserService.UserId, membershipTypeId, ct);

        return RedirectToAction(nameof(MyMemberships));
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> CancelMembership(CancellationToken ct)
    {
        if (string.IsNullOrWhiteSpace(currentUserService.UserId))
            return Challenge();

        await myAccountMembershipService.CancelActiveMembershipAsync(currentUserService.UserId, ct);

        return RedirectToAction(nameof(MyMemberships));
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> PauseMembership(CancellationToken ct)
    {
        if (string.IsNullOrWhiteSpace(currentUserService.UserId))
            return Challenge();

        await myAccountMembershipService.PauseActiveMembershipAsync(currentUserService.UserId, ct);

        return RedirectToAction(nameof(MyMemberships));
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> ActivateMembership(CancellationToken ct)
    {
        if (string.IsNullOrWhiteSpace(currentUserService.UserId))
            return Challenge();

        await myAccountMembershipService.ActivatePausedMembershipAsync(currentUserService.UserId, ct);

        return RedirectToAction(nameof(MyMemberships));
    }

    [HttpGet]
    public async Task<IActionResult> MyBookings(CancellationToken ct)
    {
        if (string.IsNullOrWhiteSpace(currentUserService.UserId))
            return Challenge();

        var user = await myAccountUserService.GetByIdAsync(currentUserService.UserId);
        if (user is null)
            return Challenge();

        SetMyAccountLayoutData(user.ImageUrl);

        var membershipOverview = await myAccountMembershipService.GetOverviewAsync(currentUserService.UserId, ct);
        var hasActiveMembership = string.Equals(
            membershipOverview?.CurrentMembership?.Status,
            "Active",
            StringComparison.OrdinalIgnoreCase);

        if (!hasActiveMembership)
            return View(new MyBookingsViewModel { HasActiveMembership = false });

        var overview = await myAccountBookingService.GetOverviewAsync(currentUserService.UserId, ct);

        MyBookingsViewModel model = overview is null
            ? new MyBookingsViewModel { HasActiveMembership = true }
            : new MyBookingsViewModel
            {
                HasActiveMembership = true,
                UpcomingBookings = overview.UpcomingBookings
                    .Select(booking => new MyBookingItemViewModel
                    {
                        WorkoutSessionId = booking.WorkoutSessionId,
                        WorkoutTitle = booking.WorkoutTitle,
                        WorkoutCategory = booking.WorkoutCategory,
                        StartsAt = booking.StartsAt,
                        DurationMinutes = booking.DurationMinutes
                    })
                    .ToList(),
                PreviousBookings = overview.PreviousBookings
                    .Select(booking => new MyBookingItemViewModel
                    {
                        WorkoutSessionId = booking.WorkoutSessionId,
                        WorkoutTitle = booking.WorkoutTitle,
                        WorkoutCategory = booking.WorkoutCategory,
                        StartsAt = booking.StartsAt,
                        DurationMinutes = booking.DurationMinutes
                    })
                    .ToList()
            };

        return View(model);
    }





    private void SetMyAccountLayoutData(string? profileImageUrl)
    {
        ViewData[ProfileImageViewDataKey] = profileImageUrl;
    }
}
