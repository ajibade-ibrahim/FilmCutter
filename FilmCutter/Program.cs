using System;

namespace FilmCutter
{
    public class Program
    {
        private static void Main()
        {
            const string VideoFileName = @"D:\Documents\Soccer\Dinho vs Jay-Jay\Friendly  2002  Brazil vs   Yugoslavia.mp4";
            const string PeriodsFilename = @"D:\text files\ajax 01-02.txt";
            const string dimensions = "0112:0004:0624:0456";

            FilmCutter.CutFile(VideoFileName, PeriodsFilename, 0, dimensions);

            Console.WriteLine();
            Console.WriteLine("---------------------------------------------------");
            Console.WriteLine("Finished Cutting Periods");
            Console.ReadLine();
        }
    }
}