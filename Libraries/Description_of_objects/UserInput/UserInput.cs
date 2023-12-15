namespace Libraries.Description_of_objects.UserInput;

/// <summary>
///     Введенные пользователем данные
/// </summary>
public record UserInput
{
    public static readonly string PathJsonFile = Path.Combine(Directory.GetCurrentDirectory(), "Fans.json");

    public required UserInputWorkPoint UserInputWorkPoint { get; init; }

    public required UserInputAir UserInputAir { get; init; }

    public required UserInputFan UserInputFan { get; init; }
}
