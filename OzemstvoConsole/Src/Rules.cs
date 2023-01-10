using System.Text.RegularExpressions;

namespace OzemstvoConsole;

public enum RuleType
{
  Host = 1,
  Regex = 2,
  Path = 3,
  Port = 4
}

public class Rule
{
  public string Id { get; set; } = Guid.NewGuid().ToString();
  public string Name { get; set; }
  public Browser Browser { get; set; }
  public string Data { get; set; }
  public string Template { get; set; }
  public const string TemplateHook = "{{url}}";
  public RuleType Type { get; set; } = RuleType.Host;

  public Rule(
    string name,
    Browser browser,
    RuleType type,
    string data,
    string template = TemplateHook,
    string? id = null)
  {
    Name = name;
    Browser = browser;
    Type = type;
    Template = template;
    Data = data;

    if (id is not null)
    {
      Id = id;
    }

    if (!template.Contains(TemplateHook))
    {
      throw new ArgumentException($"Template must contain {TemplateHook}", nameof(template));
    }
  }

  public bool Match(Uri uri)
  {
    if (Type == RuleType.Host)
    {
      return Data == uri.Host;
    }
    else if (Type == RuleType.Path)
    {
      return Data == uri.AbsolutePath;
    }
    else if (Type == RuleType.Port)
    {
      return Data == uri.Port.ToString();
    }
    else if (Type == RuleType.Regex)
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
