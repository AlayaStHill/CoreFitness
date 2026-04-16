using CoreFitness.Application.Shared.Results;
using CoreFitness.Domain.Aggregates.WorkoutSessions;

namespace CoreFitness.Application.Admin.WorkoutSessions;

public class WorkoutSessionServiceErrors
{
    public static string InputIsRequired = "Input must be provided";
    public static string IdIsRequired = "Id must be provided";

    public static ResultError WorkoutSessionNotFound(WorkoutSessionId id)
        => new(ErrorTypes.NotFound, $"Workout session with ID {id} not found");
    
}
