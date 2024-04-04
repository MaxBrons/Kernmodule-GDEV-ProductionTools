using DialogueSystem.Serialization;
using Godot;

namespace DialogueSystem.Graph
{
    public class TextNodeData : IGraphNodeData
    {
        public ulong ID { get; set; }
        public string Title { get; set; }
        public string PrefabPath { get; set; }
        public string Content { get; set; }
        public Serializable<Vector2> Position { get; set; } = Vector2.Zero;
        public Serializable<Vector2> Size { get; set; } = Vector2.Zero;
    }

    public class TextNodeExportData : IGraphNodeData
    {
        public ulong ID { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public GraphConnectionsData Connections { get; set; }
    }

    public partial class TextNode : GraphNode, IGraphNode, ISavable<TextNodeData>, ISavable<TextNodeExportData>
    {
        private LineEdit _titleText;
        private TextEdit _contentText;

        public override void _Ready()
        {
            _titleText = this.GetChild<LineEdit>();
            _contentText = this.GetChild<TextEdit>();
        }

        public void SetupNode(IGraphNodeData nodeData, NodeSlotData slotData)
        {
            if (nodeData != null) {
                var data = (TextNodeData)nodeData;

                Name = GetInstanceId().ToString();
                _titleText.Text = data.Title;
                _contentText.Text = data.Content;
                PositionOffset = data.Position;
                Size = data.Size;
            }

            if (!slotData.Equals(null))
                SetSlot((int)slotData.Index, slotData.LSlotEnabled, (int)slotData.LType, slotData.LSlotColor, slotData.RSlotEnabled, (int)slotData.RType, slotData.RSlotColor);
        }

        TextNodeData ISavable<TextNodeData>.GetData()
        {
            return new()
            {
                ID = GetInstanceId(),
                Title = _titleText?.Text,
                Content = _contentText?.Text,
                Position = PositionOffset,
                Size = Size,
                PrefabPath = SceneFilePath
            };
        }

        TextNodeExportData ISavable<TextNodeExportData>.GetData()
        {
            return new()
            {
                ID = GetInstanceId(),
                Title = _titleText.Text,
                Content = _contentText.Text,
                Connections = GetParent<Graph>().GetConnectionsFromNode(Name)
            };
        }
    }
}
