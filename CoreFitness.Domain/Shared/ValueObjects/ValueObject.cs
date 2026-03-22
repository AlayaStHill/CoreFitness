namespace CoreFitness.Domain.Shared.ValueObjects;

// Innehåller logik för värdebaserad jämförelse.
public abstract class ValueObject
{
    // Abstrakt metod som varje value object måste implementera för att definiera vilka värden som avgör likhet.
    protected abstract IEnumerable<object?> GetEqualityComponents();

    public override bool Equals(object? obj)
    {
        // Är objekten av samma typ?
        if (obj is null || obj.GetType() != GetType())
            return false;

        // Castar objektet till ValueObject efter att typen har verifierats.
        ValueObject other = (ValueObject)obj;

        //Jämför equality komponenterna och returnerar true om alla värden är lika och i samma ordning
        return GetEqualityComponents()
            .SequenceEqual(other.GetEqualityComponents());
    }

    public override int GetHashCode()
    {
        // Skapar ett hashvärde, lika objekt får samma hashvärde för korrekt beteende i Hash-baserade samlingar, ex. Dictionary. 
        return GetEqualityComponents()
            .Aggregate(
                0,
                (current, obj) => HashCode.Combine(current, obj)
            );
    }

    public static bool operator ==(ValueObject? a, ValueObject? b)
    {
        // Om båda är null anses de lika.
        if (a is null && b is null)
            return true;

        // Om bara ena är null anses de inte lika.
        if (a is null || b is null)
            return false;

        // Annars används Equals för värdebaserad jämförelse.
        return a.Equals(b);
    }

    public static bool operator !=(ValueObject? a, ValueObject? b)
        => !(a == b);
}