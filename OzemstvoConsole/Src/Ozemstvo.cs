using System.Diagnostics;
using Microsoft.Win32;

namespace OzemstvoConsole;

public class Ozemstvo
{
  public List<Browser> Browsers = new ();
  public List<Rule> Rules = new();

  public void Init()
  {
    // Browsers
    RegistryKey? browserKeys = Registry.LocalMachine.OpenSubKey(@"SOFTWARE\WOW6432Node\Clients\StartMenuInternet");
    browserKeys ??= Registry.LocalMachine.OpenSubKey(@"SOFTWARE\Clients\StartMenuInternet");
    if (browserKeys is not null)
    {
      foreach (var name in browserKeys.GetSubKeyNames())
      {
        var browserKey = browserKeys.OpenSubKey(name);
        if (browserKey is null) continue;
        var browserKeyPath = browserKey.OpenSubKey(@"shell\open\command");
        if (browserKeyPath is null) continue;
        var path = browserKeyPath.GetValue("")!.ToString();
        if (path is null) continue;
        Browsers.Add(new Browser(name, path));
      }
    }

    // Steam
    RegistryKey? steamServiceKey = Registry.LocalMachine.OpenSubKey(@"SOFTWARE\Wow6432Node\Valve\SteamService");
    if (steamServiceKey is not null)
    {
      var path = steamServiceKey.GetValue("installpath_default")!.ToString();
      if (path is not null)
      {
        Browsers.Add(new Browser("Steam", $@"{path}\steam.exe"));
      }
    }

    if (Browsers.Count < 1)
    {
      throw new Exception("No browsers found");
    }
    Browsers[0].Default = true;

    // -- user config --

    var chromeIndex = Browsers.FindIndex(b => b.Name == "Google Chrome");
    if (chromeIndex > -1)
    {
      Browsers[0].Default = false;
      Browsers[chromeIndex].Default = true;
    }

    var firefox = Browsers.Find(x => x.Name.Contains("Firefox"));
    if (firefox is not null)
    {
      Rules.Add(new Rule(firefox, Rule.RuleTypes.Host, "youtube.com"));
      Rules.Add(new Rule(firefox, Rule.RuleTypes.Host, "youtu.be"));
      Rules.Add(new Rule(firefox, Rule.RuleTypes.Host, "twitch.tv"));
      Rules.Add(new Rule(firefox, Rule.RuleTypes.Host, "dzen.ru"));
    }

    var edge = Browsers.Find(x => x.Name.Contains("Edge"));
    if (edge is not null)
    {
      Rules.Add(new Rule(edge, Rule.RuleTypes.Host, "microsoft.com"));
    }

    var chrome = Browsers.Find(x => x.Name.Contains("Google Chrome"));
    if (chrome is not null)
    {
      Rules.Add(new Rule(chrome, Rule.RuleTypes.Host, "meet.google.com"));
      Rules.Add(new Rule(chrome, Rule.RuleTypes.Host, "tagspace.com", "--profile-email=\"timofeybiryukov@tagspace.com\" {{url}}"));
    }

    var steam = Browsers.Find(x => x.Name.Contains("Steam"));
    if (steam is not null)
    {
      Rules.Add(new Rule(steam, Rule.RuleTypes.Host, "store.steampowered.com", "steam://openurl/{{url}}"));
    }
  }

  public void Run(Uri uri)
  {
    foreach (var rule in Rules)
    {
      if (rule.Match(uri))
      {
        Start(rule.Browser, rule.GetArguments(uri));
        return;
      }
    }

    // open default with default browser
    var defaultBrowser = Browsers.Find(x => x.Default);
    if (defaultBrowser is not null)
    {
      Start(defaultBrowser, uri.ToString());
      return;
    }

    throw new Exception("No default browser found");
  }

  private static void Start(Browser browser, string argumets)
  {
    Process.Start(browser.Path, argumets);
  }
}
