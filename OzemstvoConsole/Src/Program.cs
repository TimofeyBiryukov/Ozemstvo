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
    ozemstvo.Run(uri);
  }
}
