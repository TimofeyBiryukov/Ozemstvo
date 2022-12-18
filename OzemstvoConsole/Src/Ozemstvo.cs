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
  }

  public void Run(Uri uri)
  {
    Ozemstvo.Run(uri, Rules, Browsers);
  }

  public static void Run(Uri uri, List<Rule> rules, List<Browser> browsers)
  {
    foreach (var rule in rules)
    {
      if (rule.Match(uri))
      {
        Start(rule.Browser, rule.GetArguments(uri));
        return;
      }
    }

    // open default with default browser
    var defaultBrowser = browsers.Find(x => x.Default);
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
