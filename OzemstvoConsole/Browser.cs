namespace OzemstvoConsole;

public class Browser
{
  public Browser(string name, string path)
  {
    Name = name;
    Path = path;
  }

  public string Name { get; set; }
  public string Path { get; set; }
}
