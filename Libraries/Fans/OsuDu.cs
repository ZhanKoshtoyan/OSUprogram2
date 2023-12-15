using Libraries.Description_of_objects;
using Libraries.Description_of_objects.Parameters;
using Libraries.Description_of_objects.UserInput;

namespace Libraries.Fans;

public class OsuDu : IFan
{
    public OsuDu(FanData data, UserInput userInput)
    {
        Data = data;
        UserInput = userInput;
    }

    public int FanOperatingMaxTemperature =>
        (int)
        (
            UserInput.UserInputAir.FanOperatingMaxTemperature == 0
                ? FanOperatingMaxTemperatures.Values.GetValue(0)
                : UserInput.UserInputAir.FanOperatingMaxTemperature
        )!;

    public int CaseLength =>
        (int)
        (
            UserInput.UserInputFan.FanBodyLength == 0
                ? FanBodyLengths.Values.GetValue(0)
                : UserInput.UserInputFan.FanBodyLength
        )!;

    public string ImpellerRotationDirection =>
        (string)
        (
            string.IsNullOrEmpty(
                UserInput.UserInputFan.ImpellerRotationDirection
            )
                ? ImpellerRotationDirections.Values.GetValue(0)
                : UserInput.UserInputFan.ImpellerRotationDirection
        )!;

    public int NominalPower =>
        (int) (
            UserInput.UserInputFan.NominalPower == 0
                ? Math.Round(Data.NominalPower * 100, 1)
                : Math.Round(
                    UserInput.UserInputFan.NominalPower.GetValueOrDefault()
                    * 100,
                    1
                )
        );

    public string CaseMaterial =>
        (string)
        (
            string.IsNullOrEmpty(
                UserInput.UserInputFan.CaseExecutionMaterial
            )
                ? CaseExecutionMaterials.Values.GetValue(0)
                : UserInput.UserInputFan.CaseExecutionMaterial
        )!;

    public int RoundedImpellerRotationSpeed =>
        (int) (
            UserInput.UserInputFan.ImpellerRotationSpeed == 0
                ? Data.NominalImpellerRotationSpeed
                : Math.Round(
                    (double)
                    UserInput.UserInputFan.ImpellerRotationSpeed.GetValueOrDefault(),
                    0
                )
        );

    public double VolumeFlow => ((IFan) this).VolumeFlow(ImpellerRotationSpeed);

    public double TotalPressure => ((IFan) this).TotalPressure(ImpellerRotationSpeed);

    public double StaticPressure => ((IFan) this).StaticPressure(ImpellerRotationSpeed);

    public double Efficiency => ((IFan) this).Efficiency(ImpellerRotationSpeed);

    /// <summary>
    ///     Скорость воздуха, [м/с]
    /// </summary>
    public double AirVelocity => ((IFan) this).AirVelocity(VolumeFlow);

    public double Power => ((IFan) this).Power(ImpellerRotationSpeed);

    public double VolumeFlowDeviation => ((IFan) this).VolumeFlowDeviation(VolumeFlow);

    public double TotalPressureDeviation => ((IFan) this).TotalPressureDeviation(TotalPressure);

    // Расчет шума ====================================================================================================
    public double OctaveNoise63 => ((IFan) this).OctaveNoise63(VolumeFlow);

    public double OctaveNoise125 => ((IFan) this).OctaveNoise125(VolumeFlow);

    public double OctaveNoise250 => ((IFan) this).OctaveNoise250(VolumeFlow);

    public double OctaveNoise500 => ((IFan) this).OctaveNoise500(VolumeFlow);

    public double OctaveNoise1000 => ((IFan) this).OctaveNoise1000(VolumeFlow);

    public double OctaveNoise2000 => ((IFan) this).OctaveNoise2000(VolumeFlow);

    public double OctaveNoise4000 => ((IFan) this).OctaveNoise4000(VolumeFlow);

    public double OctaveNoise8000 => ((IFan) this).OctaveNoise8000(VolumeFlow);

    // Коррекция на спектр А
    public double OctaveNoiseA63 => ((IFan) this).OctaveNoiseA63(VolumeFlow);

    public double OctaveNoiseA125 => ((IFan) this).OctaveNoiseA125(VolumeFlow);

    public double OctaveNoiseA250 => ((IFan) this).OctaveNoiseA250(VolumeFlow);

    public double OctaveNoiseA500 => ((IFan) this).OctaveNoiseA500(VolumeFlow);

    public double OctaveNoiseA1000 => ((IFan) this).OctaveNoiseA1000(VolumeFlow);

    public double OctaveNoiseA2000 => ((IFan) this).OctaveNoiseA2000(VolumeFlow);

    public double OctaveNoiseA4000 => ((IFan) this).OctaveNoiseA4000(VolumeFlow);

    public double OctaveNoiseA8000 => ((IFan) this).OctaveNoiseA8000(VolumeFlow);

    // Сумма шума с коррекцией А
    public double SumNoiseA => ((IFan) this).SumNoiseA(VolumeFlow);

    public FanData Data { get; }
    public UserInput UserInput { get; }

    // Формирование проектного наименования ===========================================================================
    public string ProjectId =>
        $"ЕУ.{FanOperatingMaxTemperature}.{((IFan) this).Size:000}.{CaseLength}.{ImpellerRotationDirection}.{NominalPower:0000}.{RoundedImpellerRotationSpeed:0000}.{CaseMaterial}.Y2";
    //=================================================================================================================

    public int ImpellerRotationSpeed =>
        Data.ImpellerRotationSpeed;
}
