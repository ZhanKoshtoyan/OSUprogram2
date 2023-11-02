// OSUprogram2
// (c) Владимир Портянихин, 2023. Все права защищены.
// Несанкционированное копирование этого файла
// с помощью любого носителя строго запрещено.
// Конфиденциально.

namespace Libraries;

public record UserInput
{
    public required double VolumeFlow { get; init; }
    public required double TotalPressure { get; init; }
    public required double TotalPressureDeviation { get; init; }
    public required double Temperature { get; init; }
    public required double RelativeHumidity { get; init; }
    public required double Altitude { get; set; }
}
