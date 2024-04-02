using DialogueSystem.Serialization;
using Godot;
using System.Collections.Generic;
using System.Linq;

namespace DialogueSystem.Graph
{
    public class GraphConnectionsData : List<GraphConnectionsData.GraphConnectionPair>
    {
        public class GraphConnectionPair
        {
            public string FromNode { get; set; }
            public int FromPort { get; set; }
            public string ToNode { get; set; }
            public int ToPort { get; set; }
        }
    }

    public partial class Graph : GraphEdit, ISavable<GraphConnectionsData>, IRefreshable
    {
        public bool LeftDisconnects {
            get => _leftDisconnects;
            set
            {
                _leftDisconnects = value;

                if (_leftDisconnects)
                    AddValidLeftDisconnectType(0);
                else
                    RemoveValidLeftDisconnectType(0);
            }
        }

        private string _currentSelectedNode;
        private bool _leftDisconnects;
        private List<Node> _currentSelected = new();
        private List<Node> _currentCopied = new();

        // Connection handling
        public override void _Ready()
        {
            ConnectionRequest += OnConnectionRequest;
            CopyNodesRequest += OnCopyNodesRequest;
            DeleteNodesRequest += OnDeleteNodesRequest;
            DisconnectionRequest += OnDisconnectionRequest;
            PasteNodesRequest += OnPasteNodesRequest;

            NodeSelected += OnNodeSelected;
            NodeDeselected += OnNodeDeselected;

            RightDisconnects = true;
            LeftDisconnects = false;
        }

        public T AddNode<T>(string path, Transform transform = default) where T : GraphNode
        {
            GD.Print("Instantiating graph node of type: " + typeof(T).ToString() + " with path: \n" + path);

            var node = GD.Load<PackedScene>(path).Instantiate() as T;

            if (node != null) {
                node.PositionOffset = transform.Position;
                node.Rotation = transform.Rotation;
                node.Size = transform.Scale;

                AddChild(node);
            }
            else
                GD.PrintErr("Could not add node of type " + typeof(T) + " at path: " + path);

            return node;
        }

        public GraphConnectionsData GetConnectionsFromNode(string nodeName)
        {
            GraphConnectionsData data = new();
            var connections = GetConnectionList().Where(x => x["from_node"].AsStringName() == nodeName);

            foreach (var connection in connections) {
                data.Add(new()
                {
                    FromNode = connection["from_node"].ToString(),
                    FromPort = connection["from_port"].AsInt32(),
                    ToNode = connection["to_node"].ToString(),
                    ToPort = connection["to_port"].AsInt32(),
                });
            }

            return data;
        }

        public GraphConnectionsData GetData()
        {
            var connections = GetConnectionList();

            GraphConnectionsData pairs = new();

            foreach (var connectionList in connections) {
                var fromNodeID = this.GetChild<Node>(connectionList["from_node"].AsStringName()).GetInstanceId();
                var toNodeID = this.GetChild<Node>(connectionList["to_node"].AsStringName()).GetInstanceId();

                pairs.Add(new()
                {
                    FromNode = fromNodeID.ToString(),
                    FromPort = connectionList["from_port"].AsInt32(),
                    ToNode = toNodeID.ToString(),
                    ToPort = connectionList["to_port"].AsInt32(),
                });
            }

            return pairs;
        }

        private void OnNodeDeselected(Node node)
        {
            _currentSelected.Remove(node);
        }

        private void OnNodeSelected(Node node)
        {
            _currentSelected.Add(node);
        }

        private void OnConnectionRequest(StringName fromNode, long fromPort, StringName toNode, long toPort)
        {
            if (fromNode != toNode)
                ConnectNode(fromNode, (int)fromPort, toNode, (int)toPort);
        }

        private void OnCopyNodesRequest()
        {
            _currentCopied = new(_currentSelected);
        }

        private void OnPasteNodesRequest()
        {
            foreach (var node in _currentCopied) {
                if (node.GetType() != typeof(StartNode)) {
                    GraphNode child = node.Duplicate() as GraphNode;
                    child.PositionOffset += new Vector2(10, 10);

                    AddChild(child);
                }
            }
        }

        private void OnDeleteNodesRequest(Godot.Collections.Array nodes)
        {
            foreach (var node in _currentSelected) {
                if (node.GetType() == typeof(StartNode))
                    continue;

                var connections = GetConnectionList().Where(x => x["from_node"].AsStringName() == node.Name || x["to_node"].AsStringName() == node.Name);

                foreach (var connection in connections) {
                    DisconnectNode(connection["from_node"].AsStringName(), connection["from_port"].AsInt32(), connection["to_node"].AsStringName(), connection["to_port"].AsInt32());
                }

                node.QueueFree();
            }

            Refresh();
        }

        private void OnDisconnectionRequest(StringName fromNode, long fromPort, StringName toNode, long toPort)
        {
            DisconnectNode(fromNode, (int)fromPort, toNode, (int)toPort);
        }

        public void Refresh()
        {
            _currentSelected = new();
            _currentCopied = new();
        }
    }
}
