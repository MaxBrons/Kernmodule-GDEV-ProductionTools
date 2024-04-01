using DialogueSystem.Serialization;
using Godot;

namespace DialogueSystem.Graph
{
    public struct TextNodeData : IGraphNodeData
    {
        public ulong ID { get; set; } = 0;
        public string Title { get; set; } = string.Empty;
        public string PrefabPath { get; set; } = string.Empty;
        public string Content { get; set; } = string.Empty;
        public Serializable<Vector2> Position { get; set; } = Vector2.Zero;
        public Serializable<Vector2> Size { get; set; } = Vector2.Zero;

        public TextNodeData()
        {
        }
    }

    public partial class TextNode : GraphNode, IGraphNode, ISavable<TextNodeData>
    {
        private LineEdit _titleText;
        private TextEdit _contentText;

        public void SetupNode(IGraphNodeData nodeData, NodeSlotData slotData)
        {
            if (nodeData != null | nodeData != default) {
                var data = (TextNodeData)nodeData;

                _titleText = this.GetChild<LineEdit>();
                _contentText = this.GetChild<TextEdit>();

                _titleText.Name = data.ID.ToString();
                _titleText.Text = data.Title;
                _contentText.Text = data.Content;
                PositionOffset = data.Position;
                Size = data.Size;
            }

            if (!slotData.Equals(null) && !slotData.Equals(default))
                SetSlot((int)slotData.Index, slotData.LSlotEnabled, (int)slotData.LType, slotData.LSlotColor, slotData.RSlotEnabled, (int)slotData.RType, slotData.RSlotColor);
        }

        public TextNodeData GetData()
        {
            return new()
            {
                ID = GetInstanceId(),
                Title = _titleText.Text,
                PrefabPath = SceneFilePath,
                Content = _contentText.Text,
                Position = PositionOffset,
                Size = Size
            };
        }
    }
}
