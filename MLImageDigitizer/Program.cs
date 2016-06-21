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
            var FilePath = @"C:\Users\ifeanyi\Documents\visual studio 2013\Projects\MLImageDigitizer\MLImageDigitizer\Data\trainingdata.csv";
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

        public interface IDistance
        {
            double Between(int[] pixels1, int[] pixels2);
        }

        public class ManhattanDistance : IDistance
        {
            public double Between(int[] pixels1, int[] pixels2)
            {
                if (pixels1.Length != pixels2.Length)
                {
                    throw new ArgumentException("Inconsistent image sizes.");
                }

                var length = pixels1.Length;

                var distance = 0;

                for (int i = 0; i < length; i++)
                {
                    distance += Math.Abs(pixels1[i] - pixels2[i]);
                }
                
                return distance;
            }
        }

        public interface IClassifier
        {
            void Train(IEnumerable<Observation> trainingSet);
            string Predict(int[] pixels);
        }

        public class BasicClassifier : IClassifier
        {
            private IEnumerable<Observation> data;
            private readonly IDistance distance;

            public BasicClassifier(IDistance distance)
            {
                this.distance = distance;
            }

            public void Train(IEnumerable<Observation> trainingSet)
            {
                this.data = trainingSet;
            }

            public string Predict(int[] pixels)
            {
                Observation currentBest = null;
                var shortest = Double.MaxValue;

                foreach (Observation obs in this.data)
                {
                    var dist = this.distance.Between(obs.Pixels, pixels);
                    if (dist < shortest)
                    {
                        shortest = dist;
                        currentBest = obs;
                    }
                }

                return currentBest.Label;
            }
        }

    }
}
