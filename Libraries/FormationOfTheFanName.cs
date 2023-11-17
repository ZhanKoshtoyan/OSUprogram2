using Libraries.Description_of_objects;

namespace Libraries;

public static class FormationOfTheFanName
{
    public static string DoIt(FanData data, UserInputOptional userInput)
    {

        var selectedTemperatureFan =
            userInput.FanOperatingTemperature == 0
                ? "ххх"
                : userInput.FanOperatingTemperature.ToString();
        var selectedSize = userInput.Size == 0
            ? "ххх"
            : userInput.Size.ToString()!.PadLeft(3, '0');
        var selectedCaseLength =
            userInput.FanBodyLength == 0 ? "х" : userInput.FanBodyLength.ToString();
        var selectedImpellerRotationDirection =
            userInput.ImpellerRotationDirection == "" ? "RRO" : userInput.ImpellerRotationDirection;
        var selectedNominalPower =
            userInput.NominalPower == 0
                ? "хххх"
                : ((int) userInput.NominalPower * 100).ToString().PadLeft(4, '0');
        var selectedCaseMaterial =
            userInput.CaseExecutionMaterial == "" ? "хх" : userInput.CaseExecutionMaterial;
        var roundedImpellerRotationSpeed =
            userInput.ImpellerRotationSpeed == 0
                ?
                data.NominalImpellerRotationSpeed.ToString("0000")
                : userInput.ImpellerRotationSpeed.ToString("0000");

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
