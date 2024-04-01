using Godot;

namespace DialogueSystem.Menu.Hooks
{
    public class AddNodeToGraphHook
    {
        protected Graph.Graph _graph;
        protected string _pathToPrefab;
        protected string _buttonLabelToHookOn;

        public AddNodeToGraphHook(string buttonLabelToHookOn, Graph.Graph graph, string pathToPrefab)
        {
            _graph = graph;
            _pathToPrefab = pathToPrefab;
            _buttonLabelToHookOn = buttonLabelToHookOn;
        }

        public void AddNodeToGraph(MenuButtonInfo.MenuButtonItem button)
        {
            if (button.Name == _buttonLabelToHookOn) {
                var node = _graph.AddNode(_pathToPrefab);

                OnGraphNodeAdded(node);
            }
        }

        protected virtual void OnGraphNodeAdded(GraphNode node)
        {
            var clampedMousePos = _graph.GetLocalMousePosition().Clamp(Vector2.Zero, _graph.Size - (node.Size * _graph.Zoom));
            var finalPosition = clampedMousePos + _graph.ScrollOffset;
            finalPosition /= _graph.Zoom;

            node.PositionOffset = finalPosition;
        }
    }

    public class AddTextNodeToGraphHook : AddNodeToGraphHook
    {
        public AddTextNodeToGraphHook(string buttonLabelToHookOn, Graph.Graph graph, string pathToPrefab) : base(buttonLabelToHookOn, graph, pathToPrefab)
        {
        }

        protected override void OnGraphNodeAdded(GraphNode node)
        {
            (node as Graph.TextNode)?.SetupNode(default, new()
            {
                LSlotEnabled = true,
                RSlotEnabled = true
            });

            base.OnGraphNodeAdded(node);
        }
    }
}
