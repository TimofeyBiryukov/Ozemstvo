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

    var firefox = _browsers.Find(x => x.Name.Contains("Firefox"));
    if (firefox is not null)
    {
      _rules.Add(new Rule("Youtube", firefox.Name, "youtube.com"));
    }
  }

  public void Run(Uri uri)
  {
    foreach (var rule in _rules)
    {
      if (rule.Host == uri.Host)
      {
        var browser = _browsers.Find(x => x.Name == rule.Browser);
        if (browser is null) continue;
        Process.Start(browser.Path, uri.ToString());
        return;
      }
    }

    // open default with default browser
    Process.Start(new ProcessStartInfo(uri.ToString()) { UseShellExecute = true });
  }

}
