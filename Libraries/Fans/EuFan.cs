using Libraries.Description_of_objects;
using Libraries.Description_of_objects.Parameters;
using Libraries.Description_of_objects.UserInput;
using System.Globalization;

namespace Libraries.SomeFan;

public class EuFan : IFan
{
    public EuFan(FanData data, UserInput userInput)
    {
        Data = data;
        UserInput = userInput;
    }

    public FanData Data { get; }
    public UserInput UserInput { get; }

    public double InputVolumeFlow => UserInput.UserInputWorkPoint.VolumeFlow;
    public double InputTotalPressure => UserInput.UserInputWorkPoint.TotalPressure;

    public string ProjectId
    {
        get
        {
            var selectedTemperatureFan = UserInput.UserInputAir
                    .FanOperatingMaxTemperature
                == 0
                    ? FanOperatingMaxTemperatures.Values.GetValue(0)
                    : UserInput.UserInputAir.FanOperatingMaxTemperature.ToString();
            var selectedSize = UserInput.UserInputFan.Size == 0
                ? Data.Size.PadLeft(3, '0')
                : UserInput.UserInputFan.Size.ToString()!.PadLeft(3, '0');
            var selectedCaseLength = UserInput.UserInputFan.FanBodyLength == 0
                ? FanBodyLengths.Values.GetValue(0)
                : UserInput.UserInputFan.FanBodyLength.ToString();
            var selectedImpellerRotationDirection =
                string.IsNullOrEmpty(UserInput.UserInputFan.ImpellerRotationDirection)
                    ? ImpellerRotationDirections.Values.GetValue(0)
                    : UserInput.UserInputFan.ImpellerRotationDirection;
            var selectedNominalPower = UserInput.UserInputFan.NominalPower == 0
                ? (Data.NominalPower * 100)
                .ToString(CultureInfo.InvariantCulture)
                .PadLeft(4, '0')
                : Math.Round(
                        UserInput.UserInputFan.NominalPower.GetValueOrDefault() * 100,
                        1
                    )
                    .ToString(CultureInfo.InvariantCulture)
                    .PadLeft(4, '0');
            var selectedCaseMaterial =
                string.IsNullOrEmpty(UserInput.UserInputFan.CaseExecutionMaterial)
                    ? CaseExecutionMaterials.Values.GetValue(0)
                    : UserInput.UserInputFan.CaseExecutionMaterial;
            var roundedImpellerRotationSpeed = UserInput.UserInputFan
                    .ImpellerRotationSpeed
                == 0
                    ? Data.NominalImpellerRotationSpeed.ToString().PadLeft(4, '0')
                    : Math.Round((double) UserInput.UserInputFan.ImpellerRotationSpeed
                                .GetValueOrDefault(),
                            0
                        )
                        .ToString(CultureInfo.InvariantCulture)
                        .PadLeft(4, '0');

            return string.Format(
                "ЕУ.{0}.{1}.{2}.{3}.{4}.{5}.{6}.Y2",
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
}
