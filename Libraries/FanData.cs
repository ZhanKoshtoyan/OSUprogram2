namespace Libraries;

/// <summary>
///     Информация о вентиляторе
/// </summary>
public record FanData
{
    public required Guid Id { get; init; }

    /// <summary>
    ///     Типоразмер
    /// </summary>
    public required string Size { get; init; }

    /// <summary>
    ///     Наименование
    /// </summary>
    public required string Name { get; init; }

    /// <summary>
    ///     Скорость вращения крыльчатки, [об/мин]
    /// </summary>
    public required int ImpellerRotationSpeed { get; init; }

    /// <summary>
    ///     Минимальный объем воздуха, [м3/ч]
    /// </summary>
    public required int MinVolumeFlow { get; init; }

    /// <summary>
    ///     Максимальный объем воздуха, [м3/ч]
    /// </summary>
    public required int MaxVolumeFlow { get; init; }

    /// <summary>
    ///     Коэффиуиенты полинома 6-й степени Pv(Q) - полного давления от объемного воздуха
    /// </summary>

    public required List<double> TotalPressureCoefficients { get; init; }

    /// <summary>
    ///     Коэффиуиенты полинома 6-й степени Pv(N) - мощности вентилятора в рабочей точке от объемного воздуха
    /// </summary>
    public required List<double> PowerCoefficients { get; init; }

    /// <summary>
    ///     Площадь сечения на входе, [м2]
    /// </summary>
    public required double InletCrossSection { get; init; }

    /// <summary>
    ///     Номинальная мощность, [кВт]
    /// </summary>
    public required double NominalPower { get; init; }

    /// <summary>
    ///     Коэффициенты полинома 6-й степени Lw(Q) для уровня звуковой мощности на частоте 63Гц от объемног овоздуха
    /// </summary>
    public required List<double> OctaveNoiseCoefficients63 { get; init; }

    /// <summary>
    ///     Коэффициенты полинома 6-й степени Lw(Q) для уровня звуковой мощности на частоте 125Гц от объемного воздуха
    /// </summary>
    public required List<double> OctaveNoiseCoefficients125 { get; init; }

    /// <summary>
    ///     Коэффициенты полинома 6-й степени Lw(Q) для уровня звуковой мощности на частоте 250Гц от объемного воздуха
    /// </summary>
    public required List<double> OctaveNoiseCoefficients250 { get; init; }

    /// <summary>
    ///     Коэффициенты полинома 6-й степени Lw(Q) для уровня звуковой мощности на частоте 500Гц от объемного воздуха
    /// </summary>
    public required List<double> OctaveNoiseCoefficients500 { get; init; }

    /// <summary>
    ///     Коэффициенты полинома 6-й степени Lw(Q) для уровня звуковой мощности на частоте 1000Гц от объемного воздуха
    /// </summary>
    public required List<double> OctaveNoiseCoefficients1000 { get; init; }

    /// <summary>
    ///     Коэффициенты полинома 6-й степени Lw(Q) для уровня звуковой мощности на частоте 2000Гц от объемного воздуха
    /// </summary>
    public required List<double> OctaveNoiseCoefficients2000 { get; init; }

    /// <summary>
    ///     Коэффициенты полинома 6-й степени Lw(Q) для уровня звуковой мощности на частоте 4000Гц от объемного воздуха
    /// </summary>
    public required List<double> OctaveNoiseCoefficients4000 { get; init; }

    /// <summary>
    ///     Коэффициенты полинома 6-й степени Lw(Q) для уровня звуковой мощности на частоте 8000Гц от объемного воздуха
    /// </summary>
    public required List<double> OctaveNoiseCoefficients8000 { get; init; }
}
