using System.Collections.ObjectModel;

namespace OzemstvoWPF.Models
{
    public class RuleProperty
    {
        virtual public string? Id { get; set; } = null;
        virtual public string Name { get; set; } = string.Empty;
        virtual public string Browser { get; set; } = string.Empty;
        virtual public ObservableCollection<MatchProperty> Matches { get; set; } = new();
        virtual public string Template { get; set; } = "{{url}}";
        virtual public string Example { get; set; } = string.Empty;
        virtual public string Color { get; set; } = "";
    }
}
