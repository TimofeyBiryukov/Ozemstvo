using System;
using System.Text.RegularExpressions;

namespace OzemstvoConsole;

public enum MatchType
{
  Host = 1,
  Regex = 2,
  Path = 3,
  Port = 4
}

public class Match
{
	public string Id = Guid.NewGuid().ToString();
	public string Value { get; set; }
	public MatchType Type { get; set; } = MatchType.Host;

	public Match(string data, MatchType type)
	{
		Value = data;
		Type = type;
	}

	public bool Run(Uri uri)
	{
    if (Type == MatchType.Host)
    {
      return Value == uri.Host;
    }
    else if (Type == MatchType.Path)
    {
      return Value == uri.AbsolutePath;
    }
    else if (Type == MatchType.Port)
    {
      return Value == uri.Port.ToString();
    }
    else if (Type == MatchType.Regex)
    {
      return new Regex(Value)?.IsMatch(uri.ToString()) ?? false;
    }

    return false;
  }
}

