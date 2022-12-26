namespace OzemstvoWPF
{
    public class RuleProperty
    {
        virtual public string? Id { get; set; } = null;
        virtual public string Name { get; set; } = string.Empty;
        virtual public string Browser { get; set; } = string.Empty;
        virtual public string Data { get; set; } = string.Empty;
        virtual public string Template { get; set; } = "{{url}}";
        virtual public string Type { get; set; } = string.Empty;
        virtual public string Example { get; set; } = string.Empty;
    }
}
