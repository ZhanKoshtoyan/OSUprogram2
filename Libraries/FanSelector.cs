﻿using FluentValidation;
using Libraries.Description_of_objects;
using Libraries.Loader;
using Libraries.Validate_and_sort;

namespace Libraries;

public static class FanSelector
{
    public static void DoIt(UserInputRequired userInput)
    {
        var validator = new UserInputValidator();

        validator.ValidateAndThrowAsync(userInput);

        /*var resultValidation = validator.Validate(userInput);
        var allMessages = resultValidation.ToString();

        if (!string.IsNullOrEmpty(allMessages))
        {
            throw new ArgumentException(allMessages);
        }*/

        var fansList = JsonLoader.Download(UserInputOptional.PathJsonFile);
        //TODO Добавить витвление. Пользователь указывает назначение вентилятора. В зависимости от этого тут осуществить выбор интерфейса, который возвратит корректное Name.
        var sortFans = SortFans.Sort(fansList, userInput);
        ToPrint.Print(sortFans, userInput);
    }
}
