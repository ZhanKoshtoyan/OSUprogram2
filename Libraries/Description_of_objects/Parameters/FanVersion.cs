namespace Libraries.Description_of_objects.Parameters;

public static class FanVersion
{
    public enum Values
    {
        OsuDu,
        EuFan
    }

    /// <summary>
    ///     Исполнение вентилятора: полное название
    /// </summary>
    public static readonly string[] Names =
    {
        "0 = 'ОСУ-ДУ' - Осевой вентилятор дымоудаления",
        "1 = 'ЕУ' - Вентилятор для ЕУКЦ"
    };
}
