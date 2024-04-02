using DialogueSystem.Serialization;
using Godot;

namespace DialogueSystem.Graph
{
    public class StartNodeData : IGraphNodeData
    {

        public ulong ID { get; set; }
        public string Title { get; set; }
        public string PrefabPath { get; set; }
        public Serializable<Vector2> Position { get; set; } = new Vector2();
        public Serializable<Vector2> Size { get; set; } = new Vector2();
    }

    public class StartNodeExportData
    {
        public ulong ID { get; set; }
        public string Title { get; set; }
        public GraphConnectionsData Connections { get; set; }
    }

    public partial class StartNode : GraphNode, IGraphNode, ISavable<StartNodeData>, ISavable<StartNodeExportData>
    {
        private Label _titleText;

        public override void _Ready()
        {
            _titleText = this.GetChild<Label>();
        }

        public void SetupNode(IGraphNodeData nodeData, NodeSlotData slotData)
        {
            if (nodeData != null) {
                var data = (StartNodeData)nodeData;

                Name = data.ID.ToString();
                _titleText.Text = data.Title;
                PositionOffset = data.Position;
                Size = data.Size;
            }

            if (!slotData.Equals(null))
                SetSlot((int)slotData.Index, slotData.LSlotEnabled, (int)slotData.LType, slotData.LSlotColor, slotData.RSlotEnabled, (int)slotData.RType, slotData.RSlotColor);
        }

        StartNodeData ISavable<StartNodeData>.GetData()
        {
            return new()
            {
                ID = GetInstanceId(),
                Title = _titleText.Text,
                PrefabPath = SceneFilePath,
                Position = PositionOffset,
                Size = Size
            };
        }

        StartNodeExportData ISavable<StartNodeExportData>.GetData()
        {
            return new()
            {
                ID = GetInstanceId(),
                Title = _titleText.Text,
                Connections = GetParent<Graph>().GetConnectionsFromNode(Name)
            };
        }
    }
}
