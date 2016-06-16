using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MLImageDigitizer
{
    class Program
    {
        static void Main(string[] args)
        {
            var FilePath = @"C:\Users\ifeanyi\Documents\visual studio 2013\Projects\MLImageDigitizer\MLImageDigitizer\Data\sampledata.csv";
            var reading = DataReader.ReadObservations(FilePath);

            Console.ReadLine();
        }
        
        public class Observation
        {
            public Observation(string label, int[] pixels)
            {
                this.Label = label;
                this.Pixels = pixels;
            }

            public string Label { get; private set; }
            public int[] Pixels {get; private set; }
        }

        public class DataReader
        {
            public static Observation ObservationFactory(string data)
            {
                var commaSeparated = data.Split(',');
                var label = commaSeparated[0];
                var pixels = commaSeparated.Skip(1).Select(x => Convert.ToInt32(x)).ToArray();

                return new Observation(label, pixels);
            }

            public static Observation[] ReadObservations(string dataPath)
            {
                // Read all the lines, skip the headers, 
                // split each line around the commas, 
                // parse as integer and give new observations
                var data = File.ReadAllLines(dataPath).Skip(1).Select(ObservationFactory).ToArray();
            
                return data;
            }
        }

    }
}
