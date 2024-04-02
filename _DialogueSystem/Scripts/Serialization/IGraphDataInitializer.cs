using DialogueSystem.Graph;
using System.Collections.Generic;

namespace DialogueSystem.Serialization
{
    public interface IGraphDataInitializer<T>
    {
        public void InitializeData(Graph.Graph graph, List<T> data);
    }

    public class StartNodeInitializer : IGraphDataInitializer<StartNodeData>
    {
        public void InitializeData(Graph.Graph graph, List<StartNodeData> data)
        {
            foreach (var item in data) {
                if (item is StartNodeData nodeData) {
                    var node = graph.AddNode<StartNode>(nodeData.PrefabPath);
                    node.SetupNode(nodeData, new() { RSlotEnabled = true });

                    break;
                }
            }
        }
    }

    public class TextNodeInitializer : IGraphDataInitializer<TextNodeData>
    {
        public void InitializeData(Graph.Graph graph, List<TextNodeData> data)
        {
            foreach (var item in data) {
                if (item is TextNodeData nodeData) {
                    var node = graph.AddNode<TextNode>(nodeData.PrefabPath);
                    node.SetupNode(item, new() { LSlotEnabled = true, RSlotEnabled = true });
                }
            }
        }
    }

    public class GraphConnectionsInitializer : IGraphDataInitializer<GraphConnectionsData>
    {
        public void InitializeData(Graph.Graph graph, List<GraphConnectionsData> data)
        {
            foreach (var item in data) {
                if (item is not GraphConnectionsData nodeData)
                    continue;

                foreach (var connection in nodeData) {
                    graph.ConnectNode(connection.FromNode, connection.FromPort, connection.ToNode, connection.ToPort);
                }
            }
        }
    }
}
