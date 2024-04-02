using System;
using System.IO;
using System.Text.Json;

namespace DialogueSystem
{
    public static class SaveDataReader
    {
        public static T ReadNextObject<T>(this StreamReader stream) where T : class
        {
            string header = stream.ReadLine();
            string data = stream.ReadLine();

            if (header != null && data != null) {
                Type type = Type.GetType(header);

                if (type == typeof(T))
                    return JsonSerializer.Deserialize<T>(data);
            }

            return null;
        }
    }
}
