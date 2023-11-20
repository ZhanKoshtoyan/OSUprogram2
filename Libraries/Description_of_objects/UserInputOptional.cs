namespace Libraries.Description_of_objects;

public class UserInputOptional
{
    /// <summary>
    ///     Относительная влажность температуры ежедневной эксплуатации (по умолчанию = 0), которое ввел пользователь; [%]
    /// </summary>
    public double? RelativeHumidity { get; init; } = 0;

    /// <summary>
    ///     Высота над уровнем моря (по умолчанию = 20), которое ввел пользователь; [м]
    /// </summary>
    public double Altitude { get; set; } = 20;

    /// <summary>
    /// Типоразмер вентилятора, которое ввел пользователь
    /// </summary>
    public int Size { get; init; }

    /// <summary>
    /// Длина корпуса, которое ввел пользователь
    /// </summary>
    public int FanBodyLength { get; init; }

    /// <summary>
    /// Максимальная температура эксплуатации, которое ввел пользователь; [°C]
    /// </summary>
    public int FanOperatingMaxTemperature { get; init; }

    /// <summary>
    /// Направление движения крыльчатки, которое ввел пользователь
    /// </summary>
    public string? ImpellerRotationDirection { get; init; } = "RRO";

    /// <summary>
    /// Номинальная мощность, которое ввел пользователь; [кВт]
    /// </summary>
    public double NominalPower { get; init; }

    /// <summary>
    /// Скорость вращения крыльчатки, которое ввел пользователь; [об/мин]
    /// </summary>
    public int ImpellerRotationSpeed { get; init; }

    /// <summary>
    /// Материал исполнения корпуса, которое ввел пользователь
    /// </summary>
    public string? CaseExecutionMaterial { get; init; }

    /// <summary>
    /// Путь к файлу *.json, которое ввел пользователь
    /// </summary>
    public string? PathJsonFile { get; init; } =
        "C:\\My ProjectCSharp\\OSUprogram2\\OSUprogram2\\OSUConsole\\bin\\Debug\\net7.0\\Fans.json";
}
