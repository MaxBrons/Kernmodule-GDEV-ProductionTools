using DialogueSystem.Graph;
using Godot;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;

namespace DialogueSystem
{
    public static class GameSave
    {
        private static List<object> s_data = new();

        public static void SaveGraphToJSON(this Graph.Graph graph, string path)
        {
            File.WriteAllText(path, JsonSerializer.Serialize(s_data, options: new() { WriteIndented = true }));

            GD.Print("Saving file: " + path);

            s_data = new();
        }

        public static void SaveGraphToFile(this Graph.Graph graph, string path, bool header = true)
        {
            var writer = File.CreateText(path);
            foreach (var obj in s_data) {
                if (header)
                    writer.WriteLine(obj.GetType().ToString());

                writer.Write(JsonSerializer.Serialize(obj, obj.GetType()));
                writer.WriteLine();
            }

            writer.Flush();
            writer.Close();

            GD.Print("Saving file: " + path);

            s_data = new();
        }

        public static void MarkGraphForSaving<T>(this Graph.Graph graph) where T : new()
        {
            var nodes = graph.GetChildren<GraphNode>();
            var savables = nodes.Where(x => x is ISavable<T>);

            foreach (var node in savables.Cast<ISavable<T>>()) {
                s_data.Add(node.GetData());
            }
        }

        public static void MarkGraphConnectionsForSaving<T>(this Graph.Graph graph) where T : GraphConnectionsData
        {
            var data = (graph as ISavable<T>).GetData();

            s_data.Add(data);
        }

        public static List<T> LoadGraphFromJSON<T>(this Graph.Graph graph, string path) where T : class
        {
            if (File.Exists(path)) {
                GD.Print("Loading file: " + path);

                var stream = File.OpenText(path);
                var nodes = new List<T>();

                while (!stream.EndOfStream) {
                    var data = stream.ReadNextObject<T>();

                    if (data is not null)
                        nodes.Add(data);
                }

                stream.Close();
                return nodes;
            }

            return null;
        }
    }
}
