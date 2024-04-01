using Godot;

namespace DialogueSystem.Graph
{
    public partial class Graph : GraphEdit
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

        public GraphNode AddNode(string path, Transform transform = default)
        {
            var node = GD.Load<PackedScene>(path).Instantiate() as GraphNode;

            if (node != null) {
                node.PositionOffset = transform.Position;
                node.Rotation = transform.Rotation;
                node.Size = transform.Scale;

                AddChild(node);
            }

            return node;
        }
    }
}
