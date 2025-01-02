using System;
using NJsonSchema;

namespace KeymapsCards.Helpers;

public static class JsonValidator
{
    public static bool IsJsonSchemaValid(string json, string file)
    {
        var schema = JsonSchema.FromJsonAsync(App.Current?.KeymapJsonSchema).Result;
        var validationErrors = schema.Validate(json);

        if (validationErrors.Count != 0)
        {
            Console.WriteLine($"Json validation failed for file: {file}");

            foreach (var error in validationErrors)
            {
                Console.WriteLine($"  {error.Path}: {error.Kind}");
            }
                    
            return false;
        }

        return true;
    }
}