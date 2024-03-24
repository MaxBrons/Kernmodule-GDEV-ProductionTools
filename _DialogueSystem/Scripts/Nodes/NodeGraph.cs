using DialogueSystem.Serialization;
using Godot;
using System.Collections.Generic;
using System.Linq;

namespace DialogueSystem
{
    public partial class NodeGraph : GraphEdit
    {
        public struct NodeGraphConnection
        {
            public string FromNode { get; set; }
            public int FromPort { get; set; }
            public string ToNode { get; set; }
            public int ToPort { get; set; }
        }

        public class NodeGraphItem
        {
            public string Asset { get; set; }
            public string Name { get; set; }
            public string Title { get; set; }
            public Serializable<Vector2> Position { get; set; }
            public Serializable<Vector2> Size { get; set; }
            public string Text { get; set; }
            public List<NodeGraphConnection> Connections { get; set; }
        }

        public class NodeGraphJSONItem
        {
            public string Title { get; set; }
            public string Text { get; set; }
            public string Connection { get; set; }
        }

        public override void _Ready()
        {
            ConnectionRequest += NodeGraph_ConnectionRequest;
            DisconnectionRequest += NodeGraph_DisconnectionRequest;
            ConnectionDragStarted += NodeGraph_ConnectionDragStarted;

            AddValidLeftDisconnectType(0);
        }

        public List<NodeGraphItem> GetGraphForSaving()
        {
            var startNode = this.GetChild<EntryNode>();
            var children = this.GetChildren<TextNode>();
            List<NodeGraphItem> items = new();
            List<NodeGraphConnection> connections = new();

            foreach (var connection in GetConnectionList()) {
                connections.Add(new NodeGraphConnection
                {
                    FromNode = connection["from_node"].AsGodotObject().GetInstanceId().ToString(),
                    FromPort = connection["from_port"].AsInt32(),
                    ToNode = connection["to_node"].AsGodotObject().GetInstanceId().ToString(),
                    ToPort = connection["to_port"].AsInt32(),
                });
            }

            var startNodeSaveData = new NodeGraphItem()
            {
                Asset = "res://_DialogueSystem/Scenes/Nodes/EntryNode.tscn",
                Name = startNode.GetInstanceId().ToString(),
                Position = startNode.Position,
                Size = startNode.Size,
                Connections = GetConnectionsFromGraph(startNode.Name, connections)
            };

            items.Add(startNodeSaveData);

            foreach (var item in children) {
                NodeGraphItem saveItem = new NodeGraphItem
                {
                    Asset = "res://_DialogueSystem/Scenes/Nodes/TextNode.tscn",
                    Name = item.GetInstanceId().ToString(),
                    Position = item.Position,
                    Size = item.Size,
                    Title = item.GetChild<LineEdit>()?.Text,
                    Text = item.GetChild<TextEdit>()?.Text,
                    Connections = GetConnectionsFromGraph(item.Name, connections)
                };

                items.Add(saveItem);
            }

            return items;
        }

        public List<NodeGraphJSONItem> GetGraphForSavingJSON()
        {
            var children = this.GetChildren<TextNode>();
            List<NodeGraphJSONItem> items = new();
            List<NodeGraphConnection> connections = new();

            foreach (var connection in GetConnectionList()) {
                connections.Add(new NodeGraphConnection
                {
                    FromNode = connection["from_node"].AsString(),
                    FromPort = connection["from_port"].AsInt32(),
                    ToNode = connection["to_node"].AsString(),
                    ToPort = connection["to_port"].AsInt32(),
                });
            }

            foreach (var item in children) {
                NodeGraphJSONItem saveItem = new NodeGraphJSONItem();
                saveItem.Title = item.GetChild<LineEdit>()?.Text;
                saveItem.Text = item.GetChild<TextEdit>()?.Text;

                string toNode = connections.FirstOrDefault(x => x.FromNode == item.Name).ToNode;
                saveItem.Connection = this.GetChildren<TextNode>().FirstOrDefault(child => toNode == child.Name)?.NodeTitle;

                items.Add(saveItem);
            }

            return items;
        }

        private List<NodeGraphConnection> GetConnectionsFromGraph(string name, List<NodeGraphConnection> connections)
        {
            return connections.Where(con => con.FromNode == name).ToList();
        }

        private void NodeGraph_ConnectionDragStarted(StringName fromNode, long fromPort, bool isOutput)
        {
            bool exists = GetConnectionList().Any(x => x["from_node"].AsString() == fromNode && x["from_port"].As<long>() == fromPort);

            if (exists)
                ForceConnectionDragEnd();
        }

        private void NodeGraph_ConnectionRequest(StringName fromNode, long fromPort, StringName toNode, long toPort)
        {
            bool exists = GetConnectionList().Any(x => x["from_node"].AsStringName() == fromNode || x["to_node"].AsStringName() == toNode) != default;

            if (!exists)
                ConnectNode(fromNode, (int)fromPort, toNode, (int)toPort);
        }

        private void NodeGraph_DisconnectionRequest(StringName fromNode, long fromPort, StringName toNode, long toPort)
        {
            DisconnectNode(fromNode, (int)fromPort, toNode, (int)toPort);
        }
    }
}
