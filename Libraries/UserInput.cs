﻿namespace Libraries;

public record UserInput
{
    /// <summary>
    ///     Объемный расход воздуха, который ввел пользователь;  [м3/ч]
    /// </summary>
    public required double VolumeFlow { get; init; }

    /// <summary>
    ///     Полное давление воздуха, которое ввел пользователь; [Па]
    /// </summary>
    public required double TotalPressure { get; init; }

    /// <summary>
    ///     Допустимая погрешность подбора по полному давлению воздуха, которую ввел пользователь; [%]
    /// </summary>
    public required double TotalPressureDeviation { get; init; }

    /// <summary>
    ///     Температура ежедневной эксплуатации, которую ввел пользователь; [°C]
    /// </summary>
    public required double Temperature { get; init; }

    /// <summary>
    ///     Относительная влажность температуры ежедневной эксплуатации, которую ввел пользователь (по умолчанию = 0); [%]
    /// </summary>
    public required double RelativeHumidity { get; init; }

    /// <summary>
    ///     Высота над уровнем моря, которую ввел пользователь (по умолчанию = 20); [м]
    /// </summary>
    public required double Altitude { get; set; }

    public required int Size { get; init; }

    public required int CaseLength { get; init; }

    public required int TemperatureFan { get; init; }

    public required string? ImpellerRotationDirection { get; init; }

    public required double NominalPower { get; init; }

    public required int ImpellerRotationSpeed { get; init; }

    public required string? CaseMaterial { get; init; }
}
