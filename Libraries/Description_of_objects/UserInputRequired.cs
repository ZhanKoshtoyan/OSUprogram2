﻿namespace Libraries.Description_of_objects;

public class UserInputRequired : UserInputOptional
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
    ///     Допустимая погрешность подбора по полному давлению воздуха, которое ввел пользователь; [%]
    /// </summary>
    public required double TotalPressureDeviation { get; init; }

    /// <summary>
    ///     Минимальная температура эксплуатации, которое ввел пользователь; [°C]
    /// </summary>
    public required double FanOperatingMinTemperature { get; init; }
}
