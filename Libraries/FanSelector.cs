using FluentValidation;
using Libraries.Description_of_objects;
using Libraries.Description_of_objects.UserInput;
using Libraries.Loader;
using Libraries.Validate_and_sort;

namespace Libraries;

public static class FanSelector
{
    public static void DoIt(UserInput userInput)
    {
        var validator = new UserInputValidator();

        validator.ValidateAndThrow(userInput);

        /*var resultValidation = validator.Validate(userInput);
        var allMessages = resultValidation.ToString();

        if (!string.IsNullOrEmpty(allMessages))
        {
            throw new ArgumentException(allMessages);
        }*/

        var fansList = JsonLoader.Download(UserInput.PathJsonFile);
        var sortFans = SortFans.Sort(fansList, userInput);
        ToPrint.Print(sortFans, userInput);
    }
}
