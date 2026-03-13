namespace CoreFitness.Domain.Abstractions.Loggings;

// För att kunna logga (registrera) vad som händer i appen så att man kan analysera det i efterhand.
public interface IDomainLogger
{
    void Log(string message);
    void Log(Exception exception);
}
