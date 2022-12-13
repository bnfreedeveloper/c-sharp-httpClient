using System;

namespace WebApi
{
    public class WeatherForecast
    {
        public DateTime Date { get; set; }

        public int TemperatureC { get; set; }

        public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);

        public string Summary { get; set; }

        public override string ToString()
        {
            return $"{TemperatureC} in celcius , {TemperatureF} in farenheit\n so it's {Summary}";
        }
    }
}
