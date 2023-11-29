using Libraries.Description_of_objects;
using System.Globalization;

namespace Libraries;

public static class FormationOfTheFanName
{
    public static string DoIt(FanData data, UserInputOptional userInput)
    {
        var selectedTemperatureFan = userInput
                .FanOperatingMaxTemperature
            == 0
            ? FanOperatingMaxTemperatures.Values.GetValue(0)
            : userInput.FanOperatingMaxTemperature.ToString();
        var selectedSize = userInput.Size == 0
            ? data.Size.PadLeft(3, '0')
            : userInput.Size.ToString()!.PadLeft(3, '0');
        var selectedCaseLength = userInput.FanBodyLength == 0
            ? FanBodyLengths.Values.GetValue(0)
            : userInput.FanBodyLength.ToString();
        var selectedImpellerRotationDirection =
            string.IsNullOrEmpty(userInput.ImpellerRotationDirection)
                ? ImpellerRotationDirections.Values.GetValue(0)
                : userInput.ImpellerRotationDirection;
        var selectedNominalPower = userInput.NominalPower == 0
            ? (data.NominalPower * 100)
            .ToString(CultureInfo.InvariantCulture)
            .PadLeft(4, '0')
            : Math.Round(
                userInput.NominalPower.GetValueOrDefault() * 100
                , 1)
            .ToString(CultureInfo.InvariantCulture)
            .PadLeft(4, '0');
        var selectedCaseMaterial =
            string.IsNullOrEmpty(userInput.CaseExecutionMaterial)
                ? CaseExecutionMaterials.Values.GetValue(0)
                : userInput.CaseExecutionMaterial;
        var roundedImpellerRotationSpeed = userInput
                .ImpellerRotationSpeed
            == 0
            ? data.NominalImpellerRotationSpeed.ToString().PadLeft(4, '0')
            : Math.Round((double) userInput.ImpellerRotationSpeed
                    .GetValueOrDefault(),0)
                .ToString(CultureInfo.InvariantCulture)
                .PadLeft(4, '0');

        return string.Format(
            "ОСУ-ДУ.{0}.{1}.{2}.{3}.{4}.{5}.{6}.Y2",
            selectedTemperatureFan,
            selectedSize,
            selectedCaseLength,
            selectedImpellerRotationDirection,
            selectedNominalPower,
            roundedImpellerRotationSpeed,
            selectedCaseMaterial
        );
    }
}
