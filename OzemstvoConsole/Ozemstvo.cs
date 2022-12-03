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
    }

    var edge = _browsers.Find(x => x.Name.Contains("Edge"));
    if (edge is not null)
    {
      _rules.Add(new Rule(edge, Rule.RuleTypes.Host, "microsoft.com"));
    }
  }

  public void Run(Uri uri)
  {
    foreach (var rule in _rules)
    {
      if (rule.Match(uri))
      {
        Start(rule.Browser, uri);
        return;
      }
    }

    // open default with default browser
    var defaultBrowser = _browsers.Find(x => x.Default);
    if (defaultBrowser is not null)
    {
      Start(defaultBrowser, uri);
      return;
    }

    throw new Exception("No default browser found");
  }

  private void Start(Browser browser, Uri uri)
  {
    Process.Start(browser.Path, uri.ToString());
    //Process.Start(new ProcessStartInfo(defaultBrowser.Path)
    //{
    //  Arguments = uri.ToString(),
    //  WindowStyle = ProcessWindowStyle.Hidden,
    //  CreateNoWindow = true,
    //  RedirectStandardInput = true,
    //  RedirectStandardError = true,
    //  UseShellExecute = false,
    //});
  }
}
