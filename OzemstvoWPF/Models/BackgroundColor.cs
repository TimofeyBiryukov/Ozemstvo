using System;

namespace OzemstvoWPF.Models;

public class BackgroundColor
{
    virtual public string Color { get; set; } = "";

    public BackgroundColor()
    {
        if (string.IsNullOrEmpty(Color))
        {
            var random = new Random();
            var number = random.Next(1, 6);
            switch (number)
            {
                case 1:
                    Color = "#151F30";
                    break;
                case 2:
                    Color = "#103778";
                    break;
                case 3:
                    Color = "#0593A2";
                    break;
                case 4:
                    Color = "#FF7A48";
                    break;
                case 5:
                    Color = "#E3371E";
                    break;
            }
        }
    }
}
