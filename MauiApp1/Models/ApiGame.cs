using System;
using System.Collections.Generic;
using System.Text;

namespace MauiApp1.Models;

public class ApiGame
{
    public Guid Id { get; set; }
    public List<string> Board { get; set; } = new();
    public int Status { get; set; }
    public int MovesCount { get; set; }
}
