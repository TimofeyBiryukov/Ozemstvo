using System.Text.RegularExpressions;

namespace OzemstvoConsole;

public class Rule
{
  public string Name { get; set; }
  public Browser Browser { get; set; }
  public string? Host { get; set; }
  public Regex? Regex { get; set; }
  public RuleTypes Type { get; set; } = RuleTypes.Host;

  public enum RuleTypes
  {
    Host = 1,
    Regex = 2
  }

  public Rule(string name, Browser browser, RuleTypes type, string data)
  {
    Name = name;
    Browser = browser;
    Type = type;

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
}
