using static CoreFitness.Presentation.WebApp.Models.Admin.WorkoutSessionItemViewModel;

namespace CoreFitness.Presentation.WebApp.Models.Admin;

public class AdminWorkoutSessionsViewModel
{
    public List<WorkoutSessionItemViewModel> Sessions { get; set; } = [];

}
