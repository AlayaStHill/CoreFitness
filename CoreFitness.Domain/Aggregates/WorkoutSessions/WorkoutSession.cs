using corefitness.domain.shared.validators;
using CoreFitness.Domain.Aggregates.Workouts.WorkoutTypes;
using CoreFitness.Domain.Aggregates.WorkoutSessions.Bookings;
using CoreFitness.Domain.Exceptions.Custom;

namespace CoreFitness.Domain.Aggregates.WorkoutSessions;

public sealed class WorkoutSession
{
    private readonly List<Booking> _bookings = [];

    public WorkoutSessionId Id { get; private set; } = default!;
    public WorkoutTypeId WorkoutTypeId { get; private set; } = default!;
    public DateTimeOffset StartsAt { get; private set; }
    public TimeSpan Duration { get; private set; }
    public int Capacity { get; private set; }

    public IReadOnlyCollection<Booking> Bookings => _bookings.AsReadOnly();

    private WorkoutSession(
        WorkoutSessionId id,
        WorkoutTypeId workoutTypeId,
        DateTimeOffset startsAt,
        TimeSpan duration,
        int capacity)
    {
        Id = id;
        WorkoutTypeId = workoutTypeId;
        StartsAt = startsAt;
        Duration = duration;
        Capacity = capacity;
    }

    private WorkoutSession() { }

    public static WorkoutSession Create(
        DateTimeOffset startsAt,
        WorkoutTypeId workoutTypeId,
        TimeSpan duration,
        int capacity)
    {
        if (startsAt < DateTimeOffset.UtcNow)
            throw new ValidationDomainException(WorkoutSessionErrors.InvalidStartsAt);

        if (duration <= TimeSpan.Zero)
            throw new ValidationDomainException(WorkoutSessionErrors.InvalidDuration);

        if (capacity <= 0)
            throw new ValidationDomainException(WorkoutSessionErrors.InvalidCapacity);

        DomainValidator.RequiredGuid(workoutTypeId.Value, WorkoutSessionErrors.WorkoutTypeIdRequired);

        return new WorkoutSession(
            WorkoutSessionId.Create(),
            workoutTypeId,
            startsAt,
            duration,
            capacity);
    }

    public void Book(string userId)
    {
        DomainValidator.RequiredString(userId, WorkoutSessionErrors.UserIdRequired);

        if (StartsAt <= DateTimeOffset.UtcNow)
            throw new ValidationDomainException(WorkoutSessionErrors.BookingNotAllowedForStartedSession);

        if (_bookings.Any(b => b.UserId == userId))
            throw new ValidationDomainException(WorkoutSessionErrors.UserAlreadyBooked);

        if (_bookings.Count >= Capacity)
            throw new ValidationDomainException(WorkoutSessionErrors.SessionIsFull);

        _bookings.Add(Booking.Create(Id, userId));
    }

    public void CancelBooking(string userId)
    {
        DomainValidator.RequiredString(userId, WorkoutSessionErrors.UserIdRequired);

        var booking = _bookings.FirstOrDefault(b => b.UserId == userId);

        if (booking is null)
            throw new ValidationDomainException(WorkoutSessionErrors.BookingNotFound);

        _bookings.Remove(booking);
    }

    public bool HasAvailableSpots()
    {
        return _bookings.Count < Capacity;
    }

    public int AvailableSpots()
    {
        return Capacity - _bookings.Count;
    }
}
