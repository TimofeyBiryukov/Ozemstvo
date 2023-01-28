namespace OzemstvoWPF.Models
{
    public class BrowserProperty
    {
        virtual public string? Id { get; set; } = null;
        public string Name { get; set; } = string.Empty;
        public string Path { get; set; } = string.Empty;
        public bool UserDefined { get; set; } = true;
        public bool Default { get; set; } = false;
    }
}
