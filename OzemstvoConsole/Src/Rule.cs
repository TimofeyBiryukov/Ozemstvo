using System.Text.RegularExpressions;

namespace OzemstvoConsole;

public class Rule
{
  public string Name { get; set; }
  public Browser Browser { get; set; }
  public string Data { get; set; }
  public string Template { get; set; }
  public const string TemplateHook = "{{url}}";

  public RuleTypes Type { get; set; } = RuleTypes.Host;
  public enum RuleTypes
  {
    Host = 1,
    Regex = 2
  }

  public Rule(string name, Browser browser, RuleTypes type, string data, string template = TemplateHook)
  {
    Name = name;
    Browser = browser;
    Type = type;
    Template = template;
    Data = data;

    if (!template.Contains(TemplateHook))
    {
      throw new ArgumentException($"Template must contain {TemplateHook}", nameof(template));
    }
  }

  public bool Match(Uri uri)
  {
    if (Type == RuleTypes.Host)
    {
      return Data == uri.Host;
    }
    else if (Type == RuleTypes.Regex)
    {
      return new Regex(Data)?.IsMatch(uri.ToString()) ?? false;
    }

    return false;
  }

  public string GetArguments(Uri uri)
  {
    return Template.Replace(TemplateHook, uri.ToString());
  }
}
