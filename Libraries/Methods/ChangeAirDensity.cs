using SharpProp;

namespace Libraries.Methods;

public static class ChangeAirDensity
{
    /// <summary>
    ///     Расчет параметра {Pv, N} на новую плотность воздуха
    /// </summary>
    /// <param name="parameter"></param>
    /// <param name="air"></param>
    /// <param name="airInStandardConditions"></param>
    /// <returns></returns>
    public static double Calculate(
        double parameter,
        IHumidAirState air,
        IHumidAirState airInStandardConditions
    ) =>
        parameter * air.Density / airInStandardConditions.Density;
}
