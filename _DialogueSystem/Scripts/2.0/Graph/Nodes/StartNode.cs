using DialogueSystem.Serialization;
using Godot;

namespace DialogueSystem.Graph
{
    public struct StartNodeData : IGraphNodeData
    {

        public ulong ID { get; set; } = 0;
        public string Title { get; set; } = string.Empty;
        public string PrefabPath { get; set; } = string.Empty;
        public Serializable<Vector2> Position { get; set; } = new Vector2();
        public Serializable<Vector2> Size { get; set; } = new Vector2();

        public StartNodeData()
        {
        }
    }

    public partial class StartNode : GraphNode, IGraphNode, ISavable<StartNodeData>
    {
        private LineEdit _titleText;

        public void SetupNode(IGraphNodeData nodeData, NodeSlotData slotData)
        {
            if (nodeData != null && nodeData != default) {
                var data = (StartNodeData)nodeData;

                _titleText = this.GetChild<LineEdit>();

                _titleText.Name = data.ID.ToString();
                _titleText.Text = data.Title;
                PositionOffset = data.Position;
                Size = data.Size;
            }

            if (!slotData.Equals(null) && !slotData.Equals(default))
                SetSlot((int)slotData.Index, slotData.LSlotEnabled, (int)slotData.LType, slotData.LSlotColor, slotData.RSlotEnabled, (int)slotData.RType, slotData.RSlotColor);
        }

        public StartNodeData GetData()
        {
            return new()
            {
                ID = GetInstanceId(),
                PrefabPath = SceneFilePath,
                Position = PositionOffset,
                Size = Size
            };
        }
    }
}
