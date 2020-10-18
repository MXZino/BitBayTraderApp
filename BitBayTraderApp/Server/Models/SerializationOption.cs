using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;

namespace BitBayTraderApp.Server.Models
{
    public static class SerializationOption
    {
        public static JsonSerializerOptions Add(Serialization[] options)
        {
            var opts = new JsonSerializerOptions();

            opts.PropertyNameCaseInsensitive = true;

            foreach (var option in options)
            {
                switch (option)
                {
                    case Serialization.DoubleToString:
                        opts.Converters.Add(new DoubleToStringConverter());
                        break;

                    case Serialization.TimeStampToString:
                        opts.Converters.Add(new TimeStampToStringConverter());
                        break;
                }
            }

            return opts;
        }
    }

    public enum Serialization
    {
        DoubleToString,
        TimeStampToString
    }
}
