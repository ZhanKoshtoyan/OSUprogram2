namespace Libraries.Description_of_objects;

public class UserInputOptional
{
    /// <summary>
    ///     Относительная влажность температуры ежедневной эксплуатации, которую ввел пользователь (по умолчанию = 0); [%]
    /// </summary>
    public double? RelativeHumidity { get; init; } = 0;

    /// <summary>
    ///     Высота над уровнем моря, которую ввел пользователь (по умолчанию = 20); [м]
    /// </summary>
    public double Altitude { get; set; } = 20;

    public int Size { get; init; }

    public int FanBodyLength { get; init; }

    public int FanOperatingTemperature { get; init; }

    public string? ImpellerRotationDirection { get; init; } = "RRO";

    public double NominalPower { get; init; }

    public int ImpellerRotationSpeed { get; init; }

    public string? CaseExecutionMaterial { get; init; }

    public string? PathJsonFile { get; init; } =
        "C:\\My ProjectCSharp\\OSUprogram2\\OSUprogram2\\OSUConsole\\bin\\Debug\\net7.0\\Fans.json";
}
