using DialogueSystem.Graph;
using DialogueSystem.Serialization;
using Godot;
using System.Collections.Generic;
using System.Linq;

namespace DialogueSystem
{
    // TODO: Save graph connections to json and load connections from json.
    public static class GameSave
    {
        public static void SaveGraphToJSON<T>(this Graph.Graph graph, string path) where T : new()
        {
            var nodes = graph.GetChildren<GraphNode>();

            List<T> data = new();

            foreach (ISavable<T> node in nodes.Cast<ISavable<T>>()) {
                data.Add(node.GetData());
            }

            SaveManager.SaveToJSON(path, data);
        }

        public static void LoadNodesFromJSON<T>(this Graph.Graph graph, string path) where T : new()
        {
            List<T> data = SaveManager.ReadFromJSON<List<T>>(path);

            if (typeof(T) == typeof(Graph.TextNode)) {
                foreach (var node in data.Cast<TextNodeData>()) {
                    var textNode = GD.Load<PackedScene>(node.PrefabPath).Instantiate<Graph.TextNode>();
                    textNode.SetupNode(node, new() { LSlotEnabled = true, RSlotEnabled = true });
                }
            }
            else if (typeof(T) == typeof(Graph.StartNode)) {
                foreach (var node in data.Cast<StartNodeData>()) {
                    var startNode = GD.Load<PackedScene>(node.PrefabPath).Instantiate<Graph.StartNode>();
                    startNode.SetupNode(node, new() { RSlotEnabled = true });

                    break;
                }
            }
        }
    }
}
