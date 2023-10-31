using SharpProp;
using UnitsNet.NumberExtensions.NumberToLength;
using UnitsNet.NumberExtensions.NumberToRelativeHumidity;
using UnitsNet.NumberExtensions.NumberToTemperature;

namespace Libraries;

public class Fan
{
    /// <summary>
    ///     Нормальное плотность воздуха при 20[°C], 50[%], 20 [метров] над ур.моря
    /// </summary>
    private static IHumidAir AirInStandardConditions =>
        new HumidAir().WithState(
            InputHumidAir.Altitude(20.Meters()),
            InputHumidAir.Temperature(20.DegreesCelsius()),
            InputHumidAir.RelativeHumidity(50.Percent())
        );

    /// <summary>
    ///     Полное давление воздуха, которое ввел пользователь, [Па]
    /// </summary>
    private readonly double _inputTotalPressure;

    /// <summary>
    ///     Объем воздуха введенный пользователем, [м3/ч]
    /// </summary>
    private readonly double _inputVolumeFlow;

    /// <summary>
    ///     Расчетная плотность воздуха, температура которого введена пользователем, [кг/м3]
    /// </summary>
    private readonly IHumidAir _air;

    /// <summary>
    ///     Объект Fan, в котором вычисляются значения свойств на основе объекта FanData
    /// </summary>
    /// <param name="data"></param>
    /// <param name="inputVolumeFlow"></param>
    /// <param name="inputTotalPressure"></param>
    /// <param name="inputTemperature"></param>
    /// <param name="inputRelativeHumidity"></param>
    /// <param name="inputAltitude"></param>
    public Fan(
        FanData data,
        double inputVolumeFlow,
        double inputTotalPressure,
        double inputTemperature,
        double inputRelativeHumidity = 0,
        double inputAltitude = 20
    )
    {
        Data = data;
        _inputVolumeFlow = inputVolumeFlow;
        _inputTotalPressure = inputTotalPressure;
        _air = new HumidAir().WithState(
            InputHumidAir.Altitude(inputAltitude.Meters()),
            InputHumidAir.Temperature(inputTemperature.DegreesCelsius()),
            InputHumidAir.RelativeHumidity(inputRelativeHumidity.Percent())
        );
    }

    /// <summary>
    ///     Расчетное полное давление воздуха, [Па]
    /// </summary>
    public double TotalPressure =>
        Math.Round(
            AirDensityHasChanged(
                CalculatePolynomialCoefficients(
                    Data.TotalPressureCoefficients.ToArray()
                )
            ),
            0
        );

    /// <summary>
    ///     Расчетное статическое давление воздуха, [Па]
    /// </summary>
    public double StaticPressure =>
        Math.Round(
            TotalPressure
                - 0.5
                    * AirInStandardConditions.Density.KilogramsPerCubicMeter
                    * Math.Pow(AirVelocity, 2),
            0
        );

    /// <summary>
    ///     Расчетная мощность в рабочей точке, [кВт]
    /// </summary>
    public double Power =>
        Math.Round(
            AirDensityHasChanged(
                CalculatePolynomialCoefficients(
                    Data.PowerCoefficients.ToArray()
                )
            ),
            2
        );

    /// <summary>
    ///     Расчетный полный КПД вентилятора, [%]
    /// </summary>
    public double Efficiency =>
        Math.Round(_inputVolumeFlow / 3600 * TotalPressure / (Power * 1000), 2);

    /// <summary>
    ///     Скорость воздуха, [м/с]
    /// </summary>
    public double AirVelocity =>
        Math.Round(_inputVolumeFlow / 3600 / Data.InletCrossSection, 1);

    /// <summary>
    ///     Погрешность подбора по полному давлению воздуха, [%]
    /// </summary>
    public double TotalPressureDeviation =>
        Math.Round((1 - _inputTotalPressure / TotalPressure) * 100, 2);

    /// <summary>
    ///     Уровень звуковой мощности на частотах: 63, 125, 250, 500, 1к, 2к, 4к, 8к [Гц]
    /// </summary>
    public double[] OctaveNoise
    {
        get
        {
            double[][] parameters =
            {
                Data.OctaveNoiseCoefficients63.ToArray(),
                Data.OctaveNoiseCoefficients125.ToArray(),
                Data.OctaveNoiseCoefficients250.ToArray(),
                Data.OctaveNoiseCoefficients500.ToArray(),
                Data.OctaveNoiseCoefficients1000.ToArray(),
                Data.OctaveNoiseCoefficients2000.ToArray(),
                Data.OctaveNoiseCoefficients4000.ToArray(),
                Data.OctaveNoiseCoefficients8000.ToArray(),
            };

            return parameters
                .Select(t => Math.Round(CalculatePolynomialCoefficients(t), 1))
                .ToArray();
        }
    }

    /// <summary>
    ///     Уровень звуковой мощности с корректировкой фильтра А на частотах: 63, 125, 250, 500, 1к, 2к, 4к, 8к [Гц]
    /// </summary>
    private IEnumerable<double> OctaveNoiseA
    {
        get
        {
            var result = new double[8];
            var correctionFilterA = new[]
            {
                //63Hz
                -26.2,
                //125Hz
                -16.1,
                //250Hz
                -8.6,
                //500Hz
                -3.2,
                //1000Hz
                0,
                //2000Hz
                1.2,
                //4000Hz
                1.0,
                //8000Hz
                -1.1
            };
            for (var i = 0; i < OctaveNoise.Length; i++)
            {
                result[i] = OctaveNoise[i] + correctionFilterA[i];
            }

            return result;
        }
    }

    /// <summary>
    ///     Суммарный уровень звуковой мощности частот: 63, 125, 250, 500, 1к, 2к, 4к, 8к [Гц]
    /// </summary>
    public double SumNoise
    {
        get
        {
            var sum = OctaveNoiseA.Sum(
                singleOctave => Math.Pow(10, singleOctave / 10)
            );
            return Math.Round(10 * Math.Log10(sum), 1);
        }
    }

    /// <summary>
    ///     Информация о вентиляторе
    /// </summary>
    public FanData Data { get; }

    /// <summary>
    ///     Расчет значения по известным коэффициентам полинома по методу наименьших квадратов
    /// </summary>
    /// <param name="coefficients"></param>
    /// <returns></returns>
    private double CalculatePolynomialCoefficients(double[] coefficients) =>
        //Переворачиваем одномерный массив
        // Array.Reverse(coefficients);
        //Подставляем коэффициенты в уравнение
        coefficients[0] * Math.Pow(_inputVolumeFlow, 6)
        + coefficients[1] * Math.Pow(_inputVolumeFlow, 5)
        + coefficients[2] * Math.Pow(_inputVolumeFlow, 4)
        + coefficients[3] * Math.Pow(_inputVolumeFlow, 3)
        + coefficients[4] * Math.Pow(_inputVolumeFlow, 2)
        + coefficients[5] * Math.Pow(_inputVolumeFlow, 1)
        + coefficients[6];

    /// <summary>
    ///     Расчет параметра {Pv, N} на новую плотность воздуха
    /// </summary>
    /// <param name="parameter"></param>
    /// <returns></returns>
    private double AirDensityHasChanged(double parameter) =>
        parameter * _air.Density / AirInStandardConditions.Density;
}
