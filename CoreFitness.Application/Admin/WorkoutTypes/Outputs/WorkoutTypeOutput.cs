namespace CoreFitness.Application.Admin.WorkoutTypes.Outputs;

public sealed class WorkoutTypeOutput
{
    public Guid Id { get; set; }
    public string Title { get; set; } = null!;
    public string WorkoutCategory { get; set; } = null!;
}
