using System;
using System.Collections.Generic;

namespace OzemstvoWPF.Models;

public class BackgroundColor
{
    virtual public string Color { get; set; } = "";

    static public string[] ColorVariants { get; set; } = new string[]
    {
        "#151F30", // Dark Blue
        "#103778", // Blue
        "#0593A2", // Light Blue
        "#FF7A48", // Orange
        "#E3371E" // Red
    };

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
