using System;

namespace OzemstvoWPF.Models;

public class MatchProperty : BackgroundColor
{
    virtual public string? Id { get; set; } = null;
    virtual public string Value { get; set; } = string.Empty;
    virtual public string Type { get; set; } = string.Empty;
}
