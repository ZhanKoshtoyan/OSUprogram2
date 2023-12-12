using Libraries.Description_of_objects;
using System.Globalization;

namespace Libraries.SomeFan;

public class OsuDu : IFan
{
    public FanData Data { get; }
    public UserInputRequired UserInput { get; }

    public double InputTotalPressure => UserInput.TotalPressure;

    public double InputVolumeFlow => UserInput.VolumeFlow;

    public string ProjectId
    {
        get
        {
            var selectedTemperatureFan = UserInput
                    .FanOperatingMaxTemperature
                == 0
                    ? FanOperatingMaxTemperatures.Values.GetValue(0)
                    : UserInput.FanOperatingMaxTemperature.ToString();
            var selectedSize = UserInput.Size == 0
                ? Data.Size.PadLeft(3, '0')
                : UserInput.Size.ToString()!.PadLeft(3, '0');
            var selectedCaseLength = UserInput.FanBodyLength == 0
                ? FanBodyLengths.Values.GetValue(0)
                : UserInput.FanBodyLength.ToString();
            var selectedImpellerRotationDirection =
                string.IsNullOrEmpty(UserInput.ImpellerRotationDirection)
                    ? ImpellerRotationDirections.Values.GetValue(0)
                    : UserInput.ImpellerRotationDirection;
            var selectedNominalPower = UserInput.NominalPower == 0
                ? (Data.NominalPower * 100)
                .ToString(CultureInfo.InvariantCulture)
                .PadLeft(4, '0')
                : Math.Round(
                        UserInput.NominalPower.GetValueOrDefault() * 100,
                        1
                    )
                    .ToString(CultureInfo.InvariantCulture)
                    .PadLeft(4, '0');
            var selectedCaseMaterial =
                string.IsNullOrEmpty(UserInput.CaseExecutionMaterial)
                    ? CaseExecutionMaterials.Values.GetValue(0)
                    : UserInput.CaseExecutionMaterial;
            var roundedImpellerRotationSpeed = UserInput
                    .ImpellerRotationSpeed
                == 0
                    ? Data.NominalImpellerRotationSpeed.ToString().PadLeft(4, '0')
                    : Math.Round((double) UserInput.ImpellerRotationSpeed
                                .GetValueOrDefault(),
                            0
                        )
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

    public OsuDu(FanData data, UserInputRequired userInput)
    {
        Data = data;
        UserInput = userInput;
    }
}
