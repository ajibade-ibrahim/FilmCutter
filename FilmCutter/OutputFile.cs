using System;
using System.Linq;

namespace FilmCutter
{
    internal class OutputFile
    {
        private readonly string period;

        public OutputFile(string period, string fileName)
        {
            if (string.IsNullOrWhiteSpace(period))
            {
                throw new ArgumentException("Empty period");
            }

            if (string.IsNullOrWhiteSpace(fileName))
            {
                throw new ArgumentException("Empty fileName");
            }

            this.period = period;
            FileName = fileName;
            var times = this.period.Split('-');
            var t1 = times[0];
            Console.WriteLine("Time t1 : {0}", t1.Trim());

            var t2 = times[1];
            Console.WriteLine("Time t2 : {0}", t2.Trim());

            Start = GetTimeSpan(t1.Trim());
            End = GetTimeSpan(t2.Trim());

            if (Start > End)
            {
                throw new ArgumentException($"Start ({t1.Trim()}) is greater than end ({t2.Trim()})");
            }
        }

        public TimeSpan End { get; }

        public TimeSpan Start { get; }
        public string FileName { get; }

        internal TimeSpan GetSpan()
        {
            return End - Start;
        }

        private static TimeSpan GetTimeSpan(string time)
        {
            var timeParts = time.Split(':').Reverse().ToList();
            var seconds = GetNumber(timeParts.ElementAtOrDefault(0));
            var minutes = GetNumber(timeParts.ElementAtOrDefault(1));
            var hours = GetNumber(timeParts.ElementAtOrDefault(2));

            return new TimeSpan(hours, minutes, seconds);
        }

        private static int GetNumber(string number)
        {
            try
            {
                return string.IsNullOrWhiteSpace(number)
                    ? 0
                    : Convert.ToInt32(number);
            }
            catch (Exception exception)
            {
                Console.WriteLine($"{exception}");
            }

            return 0;
        }

        public override string ToString()
        {
            return period;
        }
    }
}