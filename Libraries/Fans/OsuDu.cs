﻿using Libraries.Description_of_objects;
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

    private int FanOperatingMaxTemperature =>
        (int)
        (
            UserInput.UserInputAir.FanOperatingMaxTemperature == 0
                ? FanOperatingMaxTemperatures.Values.GetValue(0)
                : UserInput.UserInputAir.FanOperatingMaxTemperature
        )!;

    private int CaseLength =>
        (int)
        (
            UserInput.UserInputFan.FanBodyLength == 0
                ? FanBodyLengths.Values.GetValue(0)
                : UserInput.UserInputFan.FanBodyLength
        )!;

    private string ImpellerRotationDirection =>
        (string)
        (
            string.IsNullOrEmpty(
                UserInput.UserInputFan.ImpellerRotationDirection
            )
                ? ImpellerRotationDirections.Values.GetValue(0)
                : UserInput.UserInputFan.ImpellerRotationDirection
        )!;

    private string CaseMaterial =>
        (string)
        (
            string.IsNullOrEmpty(
                UserInput.UserInputFan.CaseExecutionMaterial
            )
                ? CaseExecutionMaterials.Values.GetValue(0)
                : UserInput.UserInputFan.CaseExecutionMaterial
        )!;

    public FanData Data { get; }
    public UserInput UserInput { get; }

    // Формирование проектного наименования ===========================================================================
    public string ProjectId =>
        $"ОСУ-ДУ.{FanOperatingMaxTemperature}.{((IFan) this).Size:000}.{CaseLength}.{ImpellerRotationDirection}.{((IFan) this).NominalPower:0000}.{((IFan) this).RoundedImpellerRotationSpeed:0000}.{CaseMaterial}.Y2";
    //=================================================================================================================

    public int ImpellerRotationSpeed =>
        Data.ImpellerRotationSpeed;
}
