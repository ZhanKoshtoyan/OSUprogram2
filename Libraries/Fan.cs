using Libraries.Description_of_objects;
using SharpProp;
using UnitsNet.NumberExtensions.NumberToLength;
using UnitsNet.NumberExtensions.NumberToRelativeHumidity;
using UnitsNet.NumberExtensions.NumberToTemperature;

namespace Libraries;

public class Fan
{
    /// <summary>
    ///     Поправка уровня шума на частотную коррекцию спектра А {ГОСТ 53188.1-2019, стр.15, п.5.5.8, табл.3}
    /// </summary>
    private const double NoiseCorrectionA63 = -26.2;

    /// <summary>
    ///     Поправка уровня шума на частотную коррекцию спектра А {ГОСТ 53188.1-2019, стр.15, п.5.5.8, табл.3}
    /// </summary>
    private const double NoiseCorrectionA125 = -16.1;

    /// <summary>
    ///     Поправка уровня шума на частотную коррекцию спектра А {ГОСТ 53188.1-2019, стр.15, п.5.5.8, табл.3}
    /// </summary>
    private const double NoiseCorrectionA250 = -8.6;

    /// <summary>
    ///     Поправка уровня шума на частотную коррекцию спектра А {ГОСТ 53188.1-2019, стр.15, п.5.5.8, табл.3}
    /// </summary>
    private const double NoiseCorrectionA500 = -3.2;

    /// <summary>
    ///     Поправка уровня шума на частотную коррекцию спектра А {ГОСТ 53188.1-2019, стр.15, п.5.5.8, табл.3}
    /// </summary>
    private const double NoiseCorrectionA1000 = 0;

    /// <summary>
    ///     Поправка уровня шума на частотную коррекцию спектра А {ГОСТ 53188.1-2019, стр.15, п.5.5.8, табл.3}
    /// </summary>
    private const double NoiseCorrectionA2000 = 1.2;

    /// <summary>
    ///     Поправка уровня шума на частотную коррекцию спектра А {ГОСТ 53188.1-2019, стр.15, п.5.5.8, табл.3}
    /// </summary>
    private const double NoiseCorrectionA4000 = 1.0;

    /// <summary>
    ///     Поправка уровня шума на частотную коррекцию спектра А {ГОСТ 53188.1-2019, стр.15, п.5.5.8, табл.3}
    /// </summary>
    private const double NoiseCorrectionA8000 = -1.1;

    /// <summary>
    ///     Расчетная плотность воздуха, температура которого введена пользователем, [кг/м3]
    /// </summary>
    private readonly IHumidAir _air;

    /// <summary>
    ///     Полное давление воздуха, которое ввел пользователь, [Па]
    /// </summary>
    private readonly double _inputTotalPressure;

    /// <summary>
    ///     Объем воздуха введенный пользователем, [м3/ч]
    /// </summary>
    private readonly double _inputVolumeFlow;

    /// <summary>
    ///     Объект Fan, в котором вычисляются значения свойств на основе объекта FanData
    /// </summary>
    /// <param name="data"></param>
    /// <param name="userInput"></param>
    public Fan(FanData data, UserInputRequired userInput)
    {
        Data = data;
        _inputVolumeFlow = userInput.VolumeFlow;
        _inputTotalPressure = userInput.TotalPressure;
        if (userInput.Altitude == 0)
        {
            userInput.Altitude = 20;
        }

        Name = FormationOfTheFanName.DoIt(data, userInput);

        _air = new HumidAir().WithState(
            InputHumidAir.Altitude(userInput.Altitude.Meters()),
            InputHumidAir.Temperature(userInput.FanOperatingMinTemperature.DegreesCelsius()),
            InputHumidAir.RelativeHumidity(
                userInput.RelativeHumidity?.Percent() ?? 0.Percent()
            )
        );
    }

    /// <summary>
    ///     Информация о вентиляторе
    /// </summary>
    public FanData Data { get; }

    public string Name { get; }

    /// <summary>
    ///     Нормальное плотность воздуха при 20[°C], 50[%], 20 [метрах] над ур.моря
    /// </summary>
    private static IHumidAir AirInStandardConditions =>
        new HumidAir().WithState(
            InputHumidAir.Altitude(20.Meters()),
            InputHumidAir.Temperature(20.DegreesCelsius()),
            InputHumidAir.RelativeHumidity(50.Percent())
        );

    /// <summary>
    ///     Расчетное полное давление воздуха, [Па]
    /// </summary>
    public double TotalPressure =>
        Math.Round(
            AirDensityHasChanged(
                CalculatePolynomialCoefficients(Data.TotalPressureCoefficients)
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
                CalculatePolynomialCoefficients(Data.PowerCoefficients)
            ),
            2
        );

    /// <summary>
    ///     Расчетный полный КПД вентилятора, [%]
    /// </summary>
    public double Efficiency =>
        Math.Round(
            _inputVolumeFlow / 3600 * TotalPressure / (Power * 1000) * 100,
            1
        );

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

    //____________________________________________________________________________________________________________________________

    /// <summary>
    ///     Уровень звуковой мощности на частоте 63Гц
    /// </summary>
    public double OctaveNoise63 =>
        Math.Round(
            CalculatePolynomialCoefficients(Data.OctaveNoiseCoefficients63),
            1
        );

    /// <summary>
    ///     Уровень звуковой мощности на частоте 125Гц
    /// </summary>
    public double OctaveNoise125 =>
        Math.Round(
            CalculatePolynomialCoefficients(Data.OctaveNoiseCoefficients125),
            1
        );

    /// <summary>
    ///     Уровень звуковой мощности на частоте 250Гц
    /// </summary>
    public double OctaveNoise250 =>
        Math.Round(
            CalculatePolynomialCoefficients(Data.OctaveNoiseCoefficients250),
            1
        );

    /// <summary>
    ///     Уровень звуковой мощности на частоте 500Гц
    /// </summary>
    public double OctaveNoise500 =>
        Math.Round(
            CalculatePolynomialCoefficients(Data.OctaveNoiseCoefficients500),
            1
        );

    /// <summary>
    ///     Уровень звуковой мощности на частоте 1000Гц
    /// </summary>
    public double OctaveNoise1000 =>
        Math.Round(
            CalculatePolynomialCoefficients(Data.OctaveNoiseCoefficients1000),
            1
        );

    /// <summary>
    ///     Уровень звуковой мощности на частоте 2000Гц
    /// </summary>
    public double OctaveNoise2000 =>
        Math.Round(
            CalculatePolynomialCoefficients(Data.OctaveNoiseCoefficients2000),
            1
        );

    /// <summary>
    ///     Уровень звуковой мощности на частоте 4000Гц
    /// </summary>
    public double OctaveNoise4000 =>
        Math.Round(
            CalculatePolynomialCoefficients(Data.OctaveNoiseCoefficients4000),
            1
        );

    /// <summary>
    ///     Уровень звуковой мощности на частоте 8000Гц
    /// </summary>
    public double OctaveNoise8000 =>
        Math.Round(
            CalculatePolynomialCoefficients(Data.OctaveNoiseCoefficients8000),
            1
        );

    //____________________________________________________________________________________________________________________________

    /// <summary>
    ///     Уровень звуковой мощности частоты 63Гц с поправкой на частотную коррекцию спектра А {ГОСТ 53188.1-2019, стр.15,
    ///     п.5.5.8, табл.3}
    /// </summary>
    private double OctaveNoiseA63 =>
        Math.Round(OctaveNoise63 + NoiseCorrectionA63, 1);

    /// <summary>
    ///     Уровень звуковой мощности частоты 125Гц с поправкой на частотную коррекцию спектра А {ГОСТ 53188.1-2019, стр.15,
    ///     п.5.5.8, табл.3}
    /// </summary>
    private double OctaveNoiseA125 =>
        Math.Round(OctaveNoise125 + NoiseCorrectionA125, 1);

    /// <summary>
    ///     Уровень звуковой мощности частоты 250Гц с поправкой на частотную коррекцию спектра А {ГОСТ 53188.1-2019, стр.15,
    ///     п.5.5.8, табл.3}
    /// </summary>
    private double OctaveNoiseA250 =>
        Math.Round(OctaveNoise250 + NoiseCorrectionA250, 1);

    /// <summary>
    ///     Уровень звуковой мощности частоты 500Гц с поправкой на частотную коррекцию спектра А {ГОСТ 53188.1-2019, стр.15,
    ///     п.5.5.8, табл.3}
    /// </summary>
    private double OctaveNoiseA500 =>
        Math.Round(OctaveNoise500 + NoiseCorrectionA500, 1);

    /// <summary>
    ///     Уровень звуковой мощности частоты 1000Гц с поправкой на частотную коррекцию спектра А {ГОСТ 53188.1-2019, стр.15,
    ///     п.5.5.8, табл.3}
    /// </summary>
    private double OctaveNoiseA1000 =>
        Math.Round(OctaveNoise1000 + NoiseCorrectionA1000, 1);

    /// <summary>
    ///     Уровень звуковой мощности частоты 2000Гц с поправкой на частотную коррекцию спектра А {ГОСТ 53188.1-2019, стр.15,
    ///     п.5.5.8, табл.3}
    /// </summary>
    private double OctaveNoiseA2000 =>
        Math.Round(OctaveNoise2000 + NoiseCorrectionA2000, 1);

    /// <summary>
    ///     Уровень звуковой мощности частоты 4000Гц с поправкой на частотную коррекцию спектра А {ГОСТ 53188.1-2019, стр.15,
    ///     п.5.5.8, табл.3}
    /// </summary>
    private double OctaveNoiseA4000 =>
        Math.Round(OctaveNoise4000 + NoiseCorrectionA4000, 1);

    /// <summary>
    ///     Уровень звуковой мощности частоты 8000Гц с поправкой на частотную коррекцию спектра А {ГОСТ 53188.1-2019, стр.15,
    ///     п.5.5.8, табл.3}
    /// </summary>
    private double OctaveNoiseA8000 =>
        Math.Round(OctaveNoise8000 + NoiseCorrectionA8000, 1);

    /// <summary>
    ///     Суммарный уровень звуковой мощности частот: 63, 125, 250, 500, 1к, 2к, 4к, 8к [Гц]
    /// </summary>
    public double SumNoiseA
    {
        get
        {
            var arrayOctaveNoiseA = new List<double>
            {
                OctaveNoiseA63,
                OctaveNoiseA125,
                OctaveNoiseA250,
                OctaveNoiseA500,
                OctaveNoiseA1000,
                OctaveNoiseA2000,
                OctaveNoiseA4000,
                OctaveNoiseA8000
            };
            var sum = arrayOctaveNoiseA.Sum(
                singleOctave => Math.Pow(10, singleOctave / 10)
            );
            return Math.Round(10 * Math.Log10(sum), 1);
        }
    }

    /// <summary>
    ///     Расчет значения по известным коэффициентам полинома по методу наименьших квадратов
    /// </summary>
    /// <param name="coefficients"></param>
    /// <returns></returns>
    private double CalculatePolynomialCoefficients(
        PolynomialType coefficients
    ) =>
        //Подставляем коэффициенты в уравнение
        coefficients.SixthCoefficient * Math.Pow(_inputVolumeFlow, 6)
        + coefficients.FifthCoefficient * Math.Pow(_inputVolumeFlow, 5)
        + coefficients.FourthCoefficient * Math.Pow(_inputVolumeFlow, 4)
        + coefficients.ThirdCoefficient * Math.Pow(_inputVolumeFlow, 3)
        + coefficients.SecondCoefficient * Math.Pow(_inputVolumeFlow, 2)
        + coefficients.FirstCoefficient * Math.Pow(_inputVolumeFlow, 1)
        + coefficients.ZeroCoefficient;

    /// <summary>
    ///     Расчет параметра {Pv, N} на новую плотность воздуха
    /// </summary>
    /// <param name="parameter"></param>
    /// <returns></returns>
    private double AirDensityHasChanged(double parameter) =>
        parameter * _air.Density / AirInStandardConditions.Density;
}
