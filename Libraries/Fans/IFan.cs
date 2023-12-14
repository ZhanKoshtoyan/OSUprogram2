using Libraries.Description_of_objects;
using Libraries.Description_of_objects.UserInput;
using Libraries.Methods;
using SharpProp;
using UnitsNet.NumberExtensions.NumberToLength;
using UnitsNet.NumberExtensions.NumberToRelativeHumidity;
using UnitsNet.NumberExtensions.NumberToTemperature;

namespace Libraries.Fans;

public interface IFan
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
    private IHumidAir Air =>
        new HumidAir().WithState(
            InputHumidAir.Altitude(UserInput.UserInputAir.Altitude.GetValueOrDefault().Meters()),
            InputHumidAir.Temperature(UserInput.UserInputAir.FanOperatingMinTemperature.DegreesCelsius()),
            InputHumidAir.RelativeHumidity(
                UserInput.UserInputAir.RelativeHumidity.GetValueOrDefault().Percent()
            )
        );

    public FanData Data { get; }

    public UserInput UserInput { get; }

    /// <summary>
    ///     Нормальное плотность воздуха при 20[°C], 50[%], 20 [метрах] над ур.моря
    /// </summary>
    private static IHumidAir AirInStandardConditions =>
        new HumidAir().WithState(
            InputHumidAir.Altitude(20.Meters()),
            InputHumidAir.Temperature(20.DegreesCelsius()),
            InputHumidAir.RelativeHumidity(50.Percent())
        );

    public string? ProjectId => null;

    public double VolumeFlow => Math.Round(MethodOfHalfDivision.Calculate(UserInput.UserInputWorkPoint.TotalPressureDeviation,
    Data.MinVolumeFlow, Data.MaxVolumeFlow, Data.TotalPressureCoefficients, UserInput.UserInputWorkPoint.VolumeFlow,
    UserInput.UserInputWorkPoint.TotalPressure), 0);

    /// <summary>
    ///     Расчетное полное давление воздуха, [Па]
    /// </summary>
    public double TotalPressure =>
        Math.Round(
            ChangeAirDensity.Calculate(
                PolynomialCalculator.Calculate(Data.TotalPressureCoefficients, VolumeFlow),
                Air,
                AirInStandardConditions
            ),
            0
        );

    /// <summary>
    ///     Расчетное статическое давление воздуха, [Па]
    /// </summary>
    public double StaticPressure =>
        Math.Round(
            ChangeAirDensity.Calculate(
                TotalPressure
                - 0.5
                * AirInStandardConditions.Density.KilogramsPerCubicMeter
                * Math.Pow(AirVelocity, 2),
                Air,
                AirInStandardConditions
            ),
            0
        );

    /// <summary>
    ///     Расчетная мощность в рабочей точке, [кВт]
    /// </summary>
    public double Power =>
        Math.Round(
            ChangeAirDensity.Calculate(
                PolynomialCalculator.Calculate(Data.PowerCoefficients, VolumeFlow),
                Air,
                AirInStandardConditions
            ),
            2
        );

    /// <summary>
    ///     Расчетный полный КПД вентилятора, [%]
    /// </summary>
    public double Efficiency =>
        Math.Round(
            VolumeFlow / 3600 * TotalPressure / (Power * 1000) * 100,
            1
        );

    /// <summary>
    ///     Скорость воздуха, [м/с]
    /// </summary>
    public double AirVelocity =>
        Math.Round(VolumeFlow / 3600 / Data.InletCrossSection, 1);

    /// <summary>
    ///     Погрешность подбора по объемному расходу воздуха, [%]
    /// </summary>
    public double VolumeFlowDeviation =>
        Math.Round((1 - UserInput.UserInputWorkPoint.VolumeFlow / VolumeFlow) * 100, 2);

    /// <summary>
    ///     Погрешность подбора по полному давлению воздуха, [%]
    /// </summary>
    public double TotalPressureDeviation =>
        Math.Round((1 - UserInput.UserInputWorkPoint.TotalPressure / TotalPressure) * 100, 2);

    //____________________________________________________________________________________________________________________________

    /// <summary>
    ///     Уровень звуковой мощности на частоте 63Гц
    /// </summary>
    public double OctaveNoise63 =>
        Math.Round(
            PolynomialCalculator.Calculate(Data.OctaveNoiseCoefficients63, VolumeFlow),
            1
        );

    /// <summary>
    ///     Уровень звуковой мощности на частоте 125Гц
    /// </summary>
    public double OctaveNoise125 =>
        Math.Round(
            PolynomialCalculator.Calculate(Data.OctaveNoiseCoefficients125, VolumeFlow),
            1
        );

    /// <summary>
    ///     Уровень звуковой мощности на частоте 250Гц
    /// </summary>
    public double OctaveNoise250 =>
        Math.Round(
            PolynomialCalculator.Calculate(Data.OctaveNoiseCoefficients250, VolumeFlow),
            1
        );

    /// <summary>
    ///     Уровень звуковой мощности на частоте 500Гц
    /// </summary>
    public double OctaveNoise500 =>
        Math.Round(
            PolynomialCalculator.Calculate(Data.OctaveNoiseCoefficients500, VolumeFlow),
            1
        );

    /// <summary>
    ///     Уровень звуковой мощности на частоте 1000Гц
    /// </summary>
    public double OctaveNoise1000 =>
        Math.Round(
            PolynomialCalculator.Calculate(Data.OctaveNoiseCoefficients1000, VolumeFlow),
            1
        );

    /// <summary>
    ///     Уровень звуковой мощности на частоте 2000Гц
    /// </summary>
    public double OctaveNoise2000 =>
        Math.Round(
            PolynomialCalculator.Calculate(Data.OctaveNoiseCoefficients2000, VolumeFlow),
            1
        );

    /// <summary>
    ///     Уровень звуковой мощности на частоте 4000Гц
    /// </summary>
    public double OctaveNoise4000 =>
        Math.Round(
            PolynomialCalculator.Calculate(Data.OctaveNoiseCoefficients4000, VolumeFlow),
            1
        );

    /// <summary>
    ///     Уровень звуковой мощности на частоте 8000Гц
    /// </summary>
    public double OctaveNoise8000 =>
        Math.Round(
            PolynomialCalculator.Calculate(Data.OctaveNoiseCoefficients8000, VolumeFlow),
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
}
