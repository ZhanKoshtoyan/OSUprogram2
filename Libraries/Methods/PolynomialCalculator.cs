using Libraries.Description_of_objects;

namespace Libraries.Methods;

public static class PolynomialCalculator
{
    /// <summary>
    ///     Расчет значения по известным коэффициентам полинома по методу наименьших квадратов
    /// </summary>
    /// <param name="coefficients"></param>
    /// <param name="inputVolumeFlow"></param>
    /// <returns></returns>
    public static double CalculatePolynomialCoefficients(PolynomialType coefficients, double inputVolumeFlow) =>
        coefficients.SixthCoefficient * Math.Pow(inputVolumeFlow, 6)
        + coefficients.FifthCoefficient * Math.Pow(inputVolumeFlow, 5)
        + coefficients.FourthCoefficient * Math.Pow(inputVolumeFlow, 4)
        + coefficients.ThirdCoefficient * Math.Pow(inputVolumeFlow, 3)
        + coefficients.SecondCoefficient * Math.Pow(inputVolumeFlow, 2)
        + coefficients.FirstCoefficient * Math.Pow(inputVolumeFlow, 1)
        + coefficients.ZeroCoefficient;
}
