using System;
using System.Buffers;
using System.Buffers.Text;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace BitBayTraderApp.Server.Models
{
    public class DoubleToStringConverter : JsonConverter<double>
    {
        public override double Read(ref Utf8JsonReader reader, Type type, JsonSerializerOptions options)
        {
            if (reader.TokenType == JsonTokenType.String)
            {
                ReadOnlySpan<byte> span = reader.HasValueSequence ? reader.ValueSequence.ToArray() : reader.ValueSpan;
                if (Utf8Parser.TryParse(span, out double number, out int bytesConsumed) && span.Length == bytesConsumed)
                    return number;

                if (double.TryParse(reader.GetString(), out number))
                    return number;
            }

            return reader.GetDouble();
        }

        public override void Write(Utf8JsonWriter writer, double doubleValue, JsonSerializerOptions options)
        {
            writer.WriteStringValue(doubleValue.ToString());
        }
    }

    public class TimeStampToStringConverter : JsonConverter<DateTime>
    {
        public override DateTime Read(ref Utf8JsonReader reader, Type type, JsonSerializerOptions options)
        {
            if (reader.TokenType == JsonTokenType.String)
            {
                ReadOnlySpan<byte> span = reader.HasValueSequence ? reader.ValueSequence.ToArray() : reader.ValueSpan;
                if (Utf8Parser.TryParse(span, out long number, out int bytesConsumed) && span.Length == bytesConsumed)
                    return FromUnixTime(number);

                if (Int64.TryParse(reader.GetString(), out number))
                    return FromUnixTime(number);
            }

            return FromUnixTime(reader.GetInt64());
        }

        public override void Write(Utf8JsonWriter writer, DateTime dateTime, JsonSerializerOptions options)
        {
            writer.WriteStringValue(dateTime.ToString());
        }

        public DateTime FromUnixTime(long unixTime)
        {
            var epoch = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
            return epoch.AddMilliseconds(unixTime);
        }
    }
}
