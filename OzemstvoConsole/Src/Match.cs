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
	public string Data { get; set; }
	public MatchType Type { get; set; } = MatchType.Host;

	public Match(string data, MatchType type)
	{
		Data = data;
		Type = type;
	}

	public bool Run(Uri uri)
	{
    if (Type == MatchType.Host)
    {
      return Data == uri.Host;
    }
    else if (Type == MatchType.Path)
    {
      return Data == uri.AbsolutePath;
    }
    else if (Type == MatchType.Port)
    {
      return Data == uri.Port.ToString();
    }
    else if (Type == MatchType.Regex)
    {
      return new Regex(Data)?.IsMatch(uri.ToString()) ?? false;
    }

    return false;
  }
}

