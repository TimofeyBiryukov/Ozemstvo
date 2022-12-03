using System.Diagnostics;
using Microsoft.Win32;

namespace OzemstvoConsole;

public class Ozemstvo
{
  private readonly List<Browser> _browsers = new ();
  private readonly List<Rule> _rules = new();

  public void Init()
  {
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
        _browsers.Add(new Browser(name, path));
      }
    }

    RegistryKey? steamServiceKey = Registry.LocalMachine.OpenSubKey(@"SOFTWARE\Wow6432Node\Valve\SteamService");
    if (steamServiceKey is not null)
    {
      var path = steamServiceKey.GetValue("installpath_default")!.ToString();
      if (path is not null)
      {
        _browsers.Add(new Browser("Steam", $@"{path}\steam.exe"));
      }
    }

    if (_browsers.Count < 1)
    {
      throw new Exception("No browsers found");
    }
    _browsers[0].Default = true;

    // -- user config --

    var chromeIndex = _browsers.FindIndex(b => b.Name == "Google Chrome");
    if (chromeIndex > -1)
    {
      _browsers[0].Default = false;
      _browsers[chromeIndex].Default = true;
    }

    var firefox = _browsers.Find(x => x.Name.Contains("Firefox"));
    if (firefox is not null)
    {
      _rules.Add(new Rule(firefox, Rule.RuleTypes.Host, "youtube.com"));
      _rules.Add(new Rule(firefox, Rule.RuleTypes.Host, "youtu.be"));
      _rules.Add(new Rule(firefox, Rule.RuleTypes.Host, "twitch.tv"));
      _rules.Add(new Rule(firefox, Rule.RuleTypes.Host, "dzen.ru"));
    }

    var edge = _browsers.Find(x => x.Name.Contains("Edge"));
    if (edge is not null)
    {
      _rules.Add(new Rule(edge, Rule.RuleTypes.Host, "microsoft.com"));
    }

    var chrome = _browsers.Find(x => x.Name.Contains("Google Chrome"));
    if (chrome is not null)
    {
      _rules.Add(new Rule(chrome, Rule.RuleTypes.Host, "meet.google.com"));
      _rules.Add(new Rule(chrome, Rule.RuleTypes.Host, "tagspace.com", "--profile-email=\"timofeybiryukov@tagspace.com\" {{url}}"));
    }

    var steam = _browsers.Find(x => x.Name.Contains("Steam"));
    if (steam is not null)
    {
      _rules.Add(new Rule(steam, Rule.RuleTypes.Host, "store.steampowered.com", "steam://openurl/{{url}}"));
    }
  }

  public void Run(Uri uri)
  {
    foreach (var rule in _rules)
    {
      if (rule.Match(uri))
      {
        if (rule.Template is not null)
        {
          Start(rule.Browser, rule.Template.Replace("{{url}}", uri.ToString()));
        }
        else
        {
          Start(rule.Browser, uri.ToString());
        }
        return;
      }
    }

    // open default with default browser
    var defaultBrowser = _browsers.Find(x => x.Default);
    if (defaultBrowser is not null)
    {
      Start(defaultBrowser, uri.ToString());
      return;
    }

    throw new Exception("No default browser found");
  }

  private static void Start(Browser browser, string target)
  {
    Process.Start(browser.Path, target);
  }
}
