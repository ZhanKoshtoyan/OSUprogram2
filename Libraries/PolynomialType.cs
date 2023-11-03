namespace Libraries;

public record PolynomialType
{
    public required double SixthCoefficient { get; init; }
    public required double FifthCoefficient { get; init; }
    public required double FourthCoefficient { get; init; }
    public required double ThirdCoefficient { get; init; }
    public required double SecondCoefficient { get; init; }
    public required double FirstCoefficient { get; init; }
    public required double ZeroCoefficient { get; init; }
}
