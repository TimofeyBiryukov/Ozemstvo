using OzemstvoConsole;

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

    // mainWindow GetRules();

    //var chromeIndex = ozemstvo.Browsers.FindIndex(b => b.Name == "Google Chrome");
    //if (chromeIndex > -1)
    //{
    //  ozemstvo.Browsers[0].Default = false;
    //  ozemstvo.Browsers[chromeIndex].Default = true;
    //}

    //var firefox = ozemstvo.Browsers.Find(x => x.Name.Contains("Firefox"));
    //if (firefox is not null)
    //{
    //  ozemstvo.Rules.Add(new Rule("Youtube", firefox, RuleType.Host, "youtube.com"));
    //  ozemstvo.Rules.Add(new Rule("youtu.be", firefox, RuleType.Host, "twitch.tv"));
    //  ozemstvo.Rules.Add(new Rule("dezn", firefox, RuleType.Host, "dzen.ru"));
    //}

    //var edge = ozemstvo.Browsers.Find(x => x.Name.Contains("Edge"));
    //if (edge is not null)
    //{
    //  ozemstvo.Rules.Add(new Rule("microsoft.com", edge, RuleType.Host, "microsoft.com"));
    //}

    //var chrome = ozemstvo.Browsers.Find(x => x.Name.Contains("Google Chrome"));
    //if (chrome is not null)
    //{
    //  ozemstvo.Rules.Add(new Rule("Google Meet", chrome, RuleType.Host, "meet.google.com"));
    //  ozemstvo.Rules.Add(new Rule("Tagspace", chrome, RuleType.Host, "tagspace.com", "--profile-email=\"timofeybiryukov@tagspace.com\" {{url}}"));
    //}

    //var steam = ozemstvo.Browsers.Find(x => x.Name.Contains("Steam"));
    //if (steam is not null)
    //{
    //  ozemstvo.Rules.Add(new Rule("Steam", steam, RuleType.Host, "store.steampowered.com", "steam://openurl/{{url}}"));
    //}

    ozemstvo.Run(uri);
  }
}
