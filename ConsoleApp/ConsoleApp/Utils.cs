using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json;

namespace ConsoleApp
{
    public  class Utils
    {
        public static Dictionary<string, List<string>> GetErrosFromApiResponse(string bodyResponse)
        {
            var response = new Dictionary<string, List<string>>();

            //we deserialize in the form of an json Object
            var jsonElement = JsonSerializer.Deserialize<JsonElement>(bodyResponse);
            // we select the nested specific jsonelement from the property
            var errorJsonElement = jsonElement.GetProperty("errors");
            //we transform the jsonelement into an iterable object of jsonproperty
            foreach(var error in errorJsonElement.EnumerateObject())
            {
                var field = error.Name;
                var errors = new List<string>();
                foreach (var errorMessage in error.Value.EnumerateArray())
                {
                    errors.Add(errorMessage.GetString());    
                }
                response.Add(field, errors);
            }
            return response;
        }
    }
}
