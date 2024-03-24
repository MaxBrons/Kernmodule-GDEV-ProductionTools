using Godot;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;

namespace DialogueSystem
{
    namespace Serialization
    {
        public static class SaveManager
        {
            private static JsonSerializerOptions s_options = new JsonSerializerOptions() { WriteIndented = true };

            public static void SaveToJSON<T>(string filePath, T data)
            {
                string json = JsonSerializer.Serialize(data, s_options);

                File.WriteAllText(filePath, json);

                GD.Print("Saving file to: " + filePath);
            }

            public static T ReadFromJSON<T>(string filePath)
            {
                if (File.Exists(filePath)) {
                    GD.Print("Loading file from: " + filePath);

                    string json = File.ReadAllText(filePath);
                    return JsonSerializer.Deserialize<T>(json, s_options);
                }

                return default;
            }
        }

        public class Serializable<[MustBeVariant] T>
        {
            public string Value { get => _value; private set => _value = value; }

            private string _value;

            public Serializable(string value)
            {
                _value = value;
            }
            public static implicit operator Serializable<T>(T value)
            {
                return new Serializable<T>(GD.VarToStr(Variant.From(value)));
            }

            public static implicit operator T(Serializable<T> value)
            {
                return GD.StrToVar(value._value).As<T>();
            }
        }
    }

    public static class Tools
    {
        public static T GetChild<T>(this Node node) where T : Node
        {
            return node.GetChildren().FirstOrDefault(child => {
                System.Type type = child.GetType();

                return type.IsSubclassOf(typeof(T)) || type == typeof(T);
            }) as T;
        }

        public static IEnumerable<T> GetChildren<T>(this Node node) where T : Node
        {
            return node.GetChildren().Where(child => {
                System.Type type = child.GetType();

                return type.IsSubclassOf(typeof(T)) || type == typeof(T);
            }).Cast<T>();
        }

        public static void RemoveChildren(this Node node, bool emediate = false)
        {
            foreach (var item in node.GetChildren()) {
                if (emediate)
                    item.Free();
                else
                    item.QueueFree();
            }
        }
    }
}
