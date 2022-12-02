namespace OzemstvoConsole;

public class Rule
{
  public Rule(string name, string browser, string host)
  {
    Name = name;
    Browser = browser;
    Host = host;
  }

  public string Name { get; set; }
  public string Browser { get; set; }
  public string Host { get; set; }
}
