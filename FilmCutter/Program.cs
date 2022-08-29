using System;

namespace FilmCutter
{
    public class Program
    {
        private static void Main()
        {
            const string VideoFileName = @"D:\Matches\Inter\FC Internazionale vs. Udinese 1997-1998 - Footballia.mp4";
            const string PeriodsFilename = @"D:\Downloads\Video\Bashar\Excitement Excerpts\text\text.txt";

            FilmCutter.CutFile(VideoFileName, PeriodsFilename, 0);

            Console.WriteLine("Finished Cutting Periods");
            Console.ReadLine();
        }
    }
}