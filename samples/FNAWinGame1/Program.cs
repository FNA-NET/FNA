using System;
using FNAWinGame1;

public static class Program
{
    [STAThread]
    static void Main()
    {
        using (var g = new Game1()) {
            g.Run();
        }
    }
}