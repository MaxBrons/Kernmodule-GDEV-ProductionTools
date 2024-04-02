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

    public partial class StartNode : GraphNode, IGraphNode, ISavable<StartNodeData>
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

        public StartNodeData GetData()
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
    }
}
