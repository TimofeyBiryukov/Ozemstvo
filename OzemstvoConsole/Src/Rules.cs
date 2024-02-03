using System.Text.RegularExpressions;

namespace OzemstvoConsole;

public class Rule
{
  public string Id { get; set; } = Guid.NewGuid().ToString();
  public string Name { get; set; }
  public Browser Browser { get; set; }
  public string Template { get; set; }
  public const string TemplateHook = "{{url}}";
  public List<Match> Matches = new();

  public Rule(
    string name,
    Browser browser,
    List<Match> matches,
    string template = TemplateHook,
    string? id = null)
  {
    Name = name;
    Browser = browser;
    Matches = matches;
    Template = template;

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
    foreach (Match match in Matches)
    {
      if (match.Run(uri)) return true;
    }
    return false;
  }

  public string GetArguments(Uri uri)
  {
    if (Template == "{{url}}")
    {
      return '"' + Template.Replace(TemplateHook, uri.ToString()) + '"';
    }
    return Template.Replace(TemplateHook, uri.ToString());
  }
}
