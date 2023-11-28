using Libraries.Description_of_objects;
using System.Globalization;

namespace Libraries;

public static class FormationOfTheFanName
{
    public static string DoIt(FanData data, UserInputOptional userInput)
    {
        var selectedTemperatureFan = userInput
            .FanOperatingMaxTemperature
            .HasValue
            ? FanOperatingMaxTemperatures.Values.GetValue(1)
            : userInput.FanOperatingMaxTemperature.ToString();
        var selectedSize = userInput.Size.HasValue
            ? data.Size.PadLeft(3, '0')
            : userInput.Size.ToString()!.PadLeft(3, '0');
        var selectedCaseLength = userInput.FanBodyLength.HasValue
            ? FanBodyLengths.Values.GetValue(1)
            : userInput.FanBodyLength.ToString();
        var selectedImpellerRotationDirection =
            userInput.ImpellerRotationDirection
            ?? ImpellerRotationDirections.Values.GetValue(1);
        var selectedNominalPower = userInput.NominalPower.HasValue
            ? (data.NominalPower * 100)
                .ToString(CultureInfo.InvariantCulture)
                .PadLeft(4, '0')
            : (userInput.NominalPower.GetValueOrDefault() * 100)
                .ToString(CultureInfo.InvariantCulture)
                .PadLeft(4, '0');
        var selectedCaseMaterial =
            userInput.CaseExecutionMaterial
            ?? CaseExecutionMaterials.Values.GetValue(1);
        var roundedImpellerRotationSpeed = userInput
            .ImpellerRotationSpeed
            .HasValue
            ? data.NominalImpellerRotationSpeed.ToString().PadLeft(4, '0')
            : userInput.ImpellerRotationSpeed
                .GetValueOrDefault()
                .ToString()
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
