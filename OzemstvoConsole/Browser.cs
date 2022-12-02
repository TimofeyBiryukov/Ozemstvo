using System.Diagnostics;

namespace OzemstvoConsole;

public class Browser
{
  public string Name { get; set; }
  public string Path { get; set; }
  public bool Default { get; set; } = false;

  public Browser(string name, string path)
  {
    Name = name;
    Path = path;
  }
}
