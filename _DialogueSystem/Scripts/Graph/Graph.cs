using Godot;
using System.Collections.Generic;

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

    public partial class Graph : GraphEdit, ISavable<GraphConnectionsData>
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

        // Connection handling
        public override void _Ready()
        {
            ConnectionRequest += OnConnectionRequest;
            //CopyNodesRequest += OnCopyNodesRequest;
            //DeleteNodesRequest += OnDeleteNodesRequest;
            DisconnectionRequest += OnDisconnectionRequest;
            //PasteNodesRequest += OnPasteNodesRequest;

            NodeSelected += OnNodeSelected;
            NodeDeselected += OnNodeDeselected;

            RightDisconnects = true;
            LeftDisconnects = false;
        }

        private void OnNodeDeselected(Node node)
        {
            // set current selected
        }

        private void OnNodeSelected(Node node)
        {
            // unset current selected;
        }

        private void OnConnectionRequest(StringName fromNode, long fromPort, StringName toNode, long toPort)
        {
            if (fromNode != toNode)
                ConnectNode(fromNode, (int)fromPort, toNode, (int)toPort);
        }


        /*private void OnCopyNodesRequest()
        {

        }

        private void OnPasteNodesRequest()
        {
            throw new System.NotImplementedException();
        }

        private void OnDeleteNodesRequest(Godot.Collections.Array nodes)
        {
            throw new System.NotImplementedException();
        }*/


        // Deconnection handling
        private void OnDisconnectionRequest(StringName fromNode, long fromPort, StringName toNode, long toPort)
        {
            DisconnectNode(fromNode, (int)fromPort, toNode, (int)toPort);
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
    }
}
