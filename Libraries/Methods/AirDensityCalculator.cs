using SharpProp;

namespace Libraries.Methods;

public static class AirDensityCalculator
{
    /// <summary>
    ///     Расчет параметра {Pv, N} на новую плотность воздуха
    /// </summary>
    /// <param name="parameter"></param>
    /// <param name="air"></param>
    /// <param name="airInStandardConditions"></param>
    /// <returns></returns>
    public static double AirDensityHasChanged(
        double parameter,
        IHumidAirState air,
        IHumidAirState airInStandardConditions
    ) =>
        parameter * air.Density / airInStandardConditions.Density;
}
