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
    public IHumidAir UserInputAir =>
        new HumidAir().WithState(
            InputHumidAir.Altitude(
                UserInput.UserInputAir.Altitude.GetValueOrDefault().Meters()
            ),
            InputHumidAir.Temperature(
                UserInput.UserInputAir.FanOperatingMinTemperature.DegreesCelsius()
            ),
            InputHumidAir.RelativeHumidity(
                UserInput.UserInputAir.RelativeHumidity
                    .GetValueOrDefault()
                    .Percent()
            )
        );

    public FanData Data { get; }

    public UserInput UserInput { get; }

    /// <summary>
    ///     Нормальное плотность воздуха при 20[°C], 50[%], 20 [метрах] над ур.моря
    /// </summary>
    public IHumidAir AirInTests =>
        new HumidAir().WithState(
            InputHumidAir.Altitude(20.Meters()),
            InputHumidAir.Temperature(20.DegreesCelsius()),
            InputHumidAir.RelativeHumidity(50.Percent())
        );

    public int Size =>
        Convert.ToInt32(
            UserInput.UserInputFan.Size == 0
                ? Data.Size
                : UserInput.UserInputFan.Size
        );

    /// <summary>
    ///     Проектное наименование вентилятора
    /// </summary>
    public string? ProjectId => null;

    public int ImpellerRotationSpeed { get; }

    /// <summary>
    ///     Расход Объемного воздуха на кривой вентилятора, эквивалентный зависимости Pv=Q^2 - характеристика сети воздуховода
    /// </summary>
    public double VolumeFlow(int impellerRotationSpeed) =>
        SimilarityCalculator.SimilarVolumeFlow(
            MethodOfHalfDivision.Calculate(
                UserInput.UserInputWorkPoint.TotalPressureDeviation,
                Data.MinVolumeFlow,
                Data.MaxVolumeFlow,
                Data.TotalPressureCoefficients,
                UserInput.UserInputWorkPoint.VolumeFlow,
                UserInput.UserInputWorkPoint.TotalPressure
            ),
            Data.ImpellerRotationSpeed,
            Size,
            impellerRotationSpeed,
            Size
        );

    /// <summary>
    ///     Расчетное полное давление воздуха, [Па]
    /// </summary>
    public double TotalPressure(int impellerRotationSpeed) =>
        SimilarityCalculator.SimilarPressure(
            PolynomialCalculator.Calculate(Data.TotalPressureCoefficients, VolumeFlow(impellerRotationSpeed)),
            Data.ImpellerRotationSpeed,
            Size,
            UserInputAir,
            impellerRotationSpeed,
            Size,
            AirInTests
        );

    /// <summary>
    ///     Расчетное статическое давление воздуха, [Па]
    /// </summary>
    public double StaticPressure(int impellerRotationSpeed) =>
        SimilarityCalculator.SimilarPressure(
            TotalPressure(impellerRotationSpeed) - 0.5 * AirInTests.Density.KilogramsPerCubicMeter
            * Math.Pow(AirVelocity(VolumeFlow(impellerRotationSpeed)), 2),
            Data.ImpellerRotationSpeed,
            Size,
            this.UserInputAir,
            impellerRotationSpeed,
            Size,
            ((IFan) this).AirInTests
        );

    /// <summary>
    ///     Расчетный полный КПД вентилятора, [%]
    /// </summary>
    public double Efficiency(int impellerRotationSpeed) =>
        Math.Round(VolumeFlow(impellerRotationSpeed) / 3600 * TotalPressure(impellerRotationSpeed)
            / (Power(impellerRotationSpeed) * 1000) * 100,
            1
        );

    /// <summary>
    ///     Скорость воздуха, [м/с]
    /// </summary>
    public double AirVelocity(double volumeFlow) =>
        Math.Round(volumeFlow / 3600 / Data.InletCrossSection, 1);

    /// <summary>
    ///     Расчетная мощность в рабочей точке, [кВт]
    /// </summary>
    public double Power(int impellerRotationSpeed) =>
        SimilarityCalculator.SimilarPower(
            PolynomialCalculator.Calculate(Data.PowerCoefficients, VolumeFlow(impellerRotationSpeed)),
            Data.ImpellerRotationSpeed,
            Size,
            UserInputAir,
            impellerRotationSpeed,
            Size,
            AirInTests
        );

    /// <summary>
    ///     Погрешность подбора по объемному расходу воздуха, [%]
    /// </summary>
    public double VolumeFlowDeviation(double volumeFlow) =>
        Math.Round((1 - UserInput.UserInputWorkPoint.VolumeFlow / volumeFlow) * 100, 2);

    /// <summary>
    ///     Погрешность подбора по полному давлению воздуха, [%]
    /// </summary>
    public double TotalPressureDeviation(double totalPressure) =>
        Math.Round((1 - UserInput.UserInputWorkPoint.TotalPressure / totalPressure) * 100, 2);

    //____________________________________________________________________________________________________________________________

    /// <summary>
    ///     Уровень звуковой мощности на частоте 63Гц
    /// </summary>
    public double OctaveNoise63(double volumeFlow) =>
        Math.Round(
            PolynomialCalculator.Calculate(
                Data.OctaveNoiseCoefficients63,
                volumeFlow
            ),
            1
        );

    /// <summary>
    ///     Уровень звуковой мощности на частоте 125Гц
    /// </summary>
    public double OctaveNoise125(double volumeFlow) =>
        Math.Round(
            PolynomialCalculator.Calculate(
                Data.OctaveNoiseCoefficients125,
                volumeFlow
            ),
            1
        );

    /// <summary>
    ///     Уровень звуковой мощности на частоте 250Гц
    /// </summary>
    public double OctaveNoise250(double volumeFlow) =>
        Math.Round(
            PolynomialCalculator.Calculate(
                Data.OctaveNoiseCoefficients250,
                volumeFlow
            ),
            1
        );

    /// <summary>
    ///     Уровень звуковой мощности на частоте 500Гц
    /// </summary>
    public double OctaveNoise500(double volumeFlow) =>
        Math.Round(
            PolynomialCalculator.Calculate(
                Data.OctaveNoiseCoefficients500,
                volumeFlow
            ),
            1
        );

    /// <summary>
    ///     Уровень звуковой мощности на частоте 1000Гц
    /// </summary>
    public double OctaveNoise1000(double volumeFlow) =>
        Math.Round(
            PolynomialCalculator.Calculate(
                Data.OctaveNoiseCoefficients1000,
                volumeFlow
            ),
            1
        );

    /// <summary>
    ///     Уровень звуковой мощности на частоте 2000Гц
    /// </summary>
    public double OctaveNoise2000(double volumeFlow) =>
        Math.Round(
            PolynomialCalculator.Calculate(
                Data.OctaveNoiseCoefficients2000,
                volumeFlow
            ),
            1
        );

    /// <summary>
    ///     Уровень звуковой мощности на частоте 4000Гц
    /// </summary>
    public double OctaveNoise4000(double volumeFlow) =>
        Math.Round(
            PolynomialCalculator.Calculate(
                Data.OctaveNoiseCoefficients4000,
                volumeFlow
            ),
            1
        );

    /// <summary>
    ///     Уровень звуковой мощности на частоте 8000Гц
    /// </summary>
    public double OctaveNoise8000(double volumeFlow) =>
        Math.Round(
            PolynomialCalculator.Calculate(
                Data.OctaveNoiseCoefficients8000,
                volumeFlow
            ),
            1
        );

    //____________________________________________________________________________________________________________________________

    /// <summary>
    ///     Уровень звуковой мощности частоты 63Гц с поправкой на частотную коррекцию спектра А {ГОСТ 53188.1-2019, стр.15,
    ///     п.5.5.8, табл.3}
    /// </summary>
    public double OctaveNoiseA63(double volumeFlow) =>
        Math.Round(OctaveNoise63(volumeFlow) + NoiseCorrectionA63, 1);

    /// <summary>
    ///     Уровень звуковой мощности частоты 125Гц с поправкой на частотную коррекцию спектра А {ГОСТ 53188.1-2019, стр.15,
    ///     п.5.5.8, табл.3}
    /// </summary>
    public double OctaveNoiseA125(double volumeFlow) =>
        Math.Round(OctaveNoise125(volumeFlow) + NoiseCorrectionA125, 1);

    /// <summary>
    ///     Уровень звуковой мощности частоты 250Гц с поправкой на частотную коррекцию спектра А {ГОСТ 53188.1-2019, стр.15,
    ///     п.5.5.8, табл.3}
    /// </summary>
    public double OctaveNoiseA250(double volumeFlow) =>
        Math.Round(OctaveNoise250(volumeFlow) + NoiseCorrectionA250, 1);

    /// <summary>
    ///     Уровень звуковой мощности частоты 500Гц с поправкой на частотную коррекцию спектра А {ГОСТ 53188.1-2019, стр.15,
    ///     п.5.5.8, табл.3}
    /// </summary>
    public double OctaveNoiseA500(double volumeFlow) =>
        Math.Round(OctaveNoise500(volumeFlow) + NoiseCorrectionA500, 1);

    /// <summary>
    ///     Уровень звуковой мощности частоты 1000Гц с поправкой на частотную коррекцию спектра А {ГОСТ 53188.1-2019, стр.15,
    ///     п.5.5.8, табл.3}
    /// </summary>
    public double OctaveNoiseA1000(double volumeFlow) =>
        Math.Round(OctaveNoise1000(volumeFlow) + NoiseCorrectionA1000, 1);

    /// <summary>
    ///     Уровень звуковой мощности частоты 2000Гц с поправкой на частотную коррекцию спектра А {ГОСТ 53188.1-2019, стр.15,
    ///     п.5.5.8, табл.3}
    /// </summary>
    public double OctaveNoiseA2000(double volumeFlow) =>
        Math.Round(OctaveNoise2000(volumeFlow) + NoiseCorrectionA2000, 1);

    /// <summary>
    ///     Уровень звуковой мощности частоты 4000Гц с поправкой на частотную коррекцию спектра А {ГОСТ 53188.1-2019, стр.15,
    ///     п.5.5.8, табл.3}
    /// </summary>
    public double OctaveNoiseA4000(double volumeFlow) =>
        Math.Round(OctaveNoise4000(volumeFlow) + NoiseCorrectionA4000, 1);

    /// <summary>
    ///     Уровень звуковой мощности частоты 8000Гц с поправкой на частотную коррекцию спектра А {ГОСТ 53188.1-2019, стр.15,
    ///     п.5.5.8, табл.3}
    /// </summary>
    public double OctaveNoiseA8000(double volumeFlow) =>
        Math.Round(OctaveNoise8000(volumeFlow) + NoiseCorrectionA8000, 1);

    /// <summary>
    ///     Суммарный уровень звуковой мощности частот: 63, 125, 250, 500, 1к, 2к, 4к, 8к [Гц]
    /// </summary>
    public double SumNoiseA(double volumeFlow)
    {
        var arrayOctaveNoiseA = new List<double>
        {
            OctaveNoiseA63(volumeFlow),
            OctaveNoiseA125(volumeFlow),
            OctaveNoiseA250(volumeFlow),
            OctaveNoiseA500(volumeFlow),
            OctaveNoiseA1000(volumeFlow),
            OctaveNoiseA2000(volumeFlow),
            OctaveNoiseA4000(volumeFlow),
            OctaveNoiseA8000(volumeFlow)
        };
        var sum = arrayOctaveNoiseA.Sum(
            singleOctave => Math.Pow(10, singleOctave / 10)
        );
        return Math.Round(10 * Math.Log10(sum), 1);
    }
}
