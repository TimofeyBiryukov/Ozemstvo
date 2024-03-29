﻿namespace OzemstvoConsole;

public class Browser
{
  public string Id = Guid.NewGuid().ToString();
  public string Name { get; set; }
  public string Path { get; set; }
  public bool Default { get; set; } = false;

  public Browser(string name, string path, bool isDefault = false)
  {
    Id = name.ToLower().Replace(" ", "-");
    Name = name;
    Path = path;
    Default = isDefault;
  }
}
