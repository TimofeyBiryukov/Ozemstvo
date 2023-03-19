using OzemstvoWPF.Config;
using System;

namespace OzemstvoWPF.Models;

public class BackgroundColor
{
    virtual public string Color { get; set; } = "";

    static public string[] ColorVariants { get; set; } = ColorConfig.ColorVariants;

    public BackgroundColor()
    {
        if (string.IsNullOrEmpty(Color))
        {
            var random = new Random();
            var index = random.Next(0, ColorVariants.Length);
            Color = ColorVariants[index];
        }
    }
}
