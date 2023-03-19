using System;
using System.Collections.Generic;

namespace OzemstvoWPF.Models;

public class BackgroundColor
{
    virtual public string Color { get; set; } = "";

    static public List<string> ColorVariants = new List<string> {
        "#151F30",
        "#103778",
        "#0593A2",
        "#FF7A48",
        "#E3371E"
    };

    public BackgroundColor()
    {
        if (string.IsNullOrEmpty(Color))
        {
            var random = new Random();
            var index = random.Next(0, 5);
            Color = ColorVariants[index];
        }
    }
}
