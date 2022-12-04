using System.Runtime.InteropServices;
using System.Text.RegularExpressions;

namespace OzemstvoConsole;

public class Rule
{
  public Browser Browser { get; set; }
  public string? Host { get; set; }
  public Regex? Regex { get; set; }
  public string Template { get; set; }
  public const string TemplateHook = "{{url}}";

  public RuleTypes Type { get; set; } = RuleTypes.Host;
  public enum RuleTypes
  {
    Host = 1,
    Regex = 2
  }

  public Rule(Browser browser, RuleTypes type, string data, string template = TemplateHook)
  {
    Browser = browser;
    Type = type;

    if (!template.Contains(TemplateHook))
    {
      throw new ArgumentException($"Template must contain {TemplateHook}", nameof(template));
    }

    Template = template;

    if (Type == RuleTypes.Host)
    {
      Host = data;
    }
    else if (Type == RuleTypes.Regex)
    {
      Regex = new Regex(data);
    }
  }

  public bool Match(Uri uri)
  {
    if (Type == RuleTypes.Host)
    {
      return Host == uri.Host;
    }
    else if (Type == RuleTypes.Regex)
    {
      return Regex?.IsMatch(uri.ToString()) ?? false;
    }

    return false;
  }

  public string GetArguments(Uri uri)
  {
    return Template.Replace(TemplateHook, uri.ToString());
  }
}
