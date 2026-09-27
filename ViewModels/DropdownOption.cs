namespace TutorBridge.ViewModels
{
    public class DropdownOption
    {
        public string Value { get; }
        public string Text { get; }
        public string HoverText { get; }

        public DropdownOption(string value, string text)
        {
            Value = value;
            Text = text;
            HoverText = value;
        }

        public DropdownOption(string value, string text, string? hoverText)
        {
            Value = value;
            Text = text;
            HoverText = string.IsNullOrEmpty(hoverText) ? value : hoverText;
        }
    }
}
