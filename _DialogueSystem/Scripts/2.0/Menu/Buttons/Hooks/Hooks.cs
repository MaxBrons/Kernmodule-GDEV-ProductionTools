namespace DialogueSystem.Menu.Hooks
{
    public class AddNodeToGraphHook
    {
        private Graph.Graph _graph;
        private string _pathToPrefab;
        private string _buttonLabelToHookOn;

        public AddNodeToGraphHook(Graph.Graph graph, string buttonLabelToHookOn, string pathToPrefab)
        {
            _graph = graph;
            _pathToPrefab = pathToPrefab;
            _buttonLabelToHookOn = buttonLabelToHookOn;
        }

        public void AddNodeToGraph(MenuButtonInfo.MenuButtonItem button)
        {
            if (button.Name == _buttonLabelToHookOn)
                _graph.AddNode(_pathToPrefab);
        }
    }
}
