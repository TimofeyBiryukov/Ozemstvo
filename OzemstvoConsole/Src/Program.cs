namespace OzemstvoConsole;

internal static class Program
{
  static void Main(string[] args)
  {
    if (args.Length == 0)
    {
      Console.WriteLine("Usage: OzemstvoConsole.exe <url>");
      return;
    }

    string target = args[0];
    Uri.TryCreate(target, UriKind.Absolute, out Uri? uri);
    if (uri is null)
    {
      Console.WriteLine("Invalid URL");
      return;
    }

    var ozemstvo = new Ozemstvo();

    ozemstvo.Init();

    var chromeIndex = ozemstvo.Browsers.FindIndex(b => b.Name == "Google Chrome");
    if (chromeIndex > -1)
    {
      ozemstvo.Browsers[0].Default = false;
      ozemstvo.Browsers[chromeIndex].Default = true;
    }

    var firefox = ozemstvo.Browsers.Find(x => x.Name.Contains("Firefox"));
    if (firefox is not null)
    {
      ozemstvo.Rules.Add(new Rule("Youtube", firefox, Rule.RuleTypes.Host, "youtube.com"));
      ozemstvo.Rules.Add(new Rule("youtu.be", firefox, Rule.RuleTypes.Host, "twitch.tv"));
      ozemstvo.Rules.Add(new Rule("dezn", firefox, Rule.RuleTypes.Host, "dzen.ru"));
    }

    var edge = ozemstvo.Browsers.Find(x => x.Name.Contains("Edge"));
    if (edge is not null)
    {
      ozemstvo.Rules.Add(new Rule("microsoft.com", edge, Rule.RuleTypes.Host, "microsoft.com"));
    }

    var chrome = ozemstvo.Browsers.Find(x => x.Name.Contains("Google Chrome"));
    if (chrome is not null)
    {
      ozemstvo.Rules.Add(new Rule("Google Meet", chrome, Rule.RuleTypes.Host, "meet.google.com"));
      ozemstvo.Rules.Add(new Rule("Tagspace", chrome, Rule.RuleTypes.Host, "tagspace.com", "--profile-email=\"timofeybiryukov@tagspace.com\" {{url}}"));
    }

    var steam = ozemstvo.Browsers.Find(x => x.Name.Contains("Steam"));
    if (steam is not null)
    {
      ozemstvo.Rules.Add(new Rule("Steam", steam, Rule.RuleTypes.Host, "store.steampowered.com", "steam://openurl/{{url}}"));
    }

    ozemstvo.Run(uri);
  }
}
