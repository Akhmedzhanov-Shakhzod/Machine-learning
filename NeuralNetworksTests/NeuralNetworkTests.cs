using Microsoft.VisualStudio.TestTools.UnitTesting;
using NeuralNetworks;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NeuralNetworks.Tests
{
    [TestClass()]
    public class NeuralNetworkTests
    {
        [TestMethod()]
        public void FeedForwardTest()
        {
            var outputs = new double[] { 0, 0, 1, 0, 0, 0, 1, 0, 1, 1, 1, 1, 1, 0, 1, 1 };
            var inputs = new double[,]
            {
                // Результат - Пациент болен - 1
                //             Пациент Здоров - 0

                // Неправильная температура T
                // Хороший возраст A
                // Курит S
                // Правильно питается F
                //T  A  S  F
                { 0, 0, 0, 0 },
                { 0, 0, 0, 1 },
                { 0, 0, 1, 0 },
                { 0, 0, 1, 1 },
                { 0, 1, 0, 0 },
                { 0, 1, 0, 1 },
                { 0, 1, 1, 0 },
                { 0, 1, 1, 1 },
                { 1, 0, 0, 0 },
                { 1, 0, 0, 1 },
                { 1, 0, 1, 0 },
                { 1, 0, 1, 1 },
                { 1, 1, 0, 0 },
                { 1, 1, 0, 1 },
                { 1, 1, 1, 0 },
                { 1, 1, 1, 1 }
            };

            var topology = new Topology(4, 1, 0.1, 2);
            var neuralNetwork = new NeuralNetwork(topology);

            var difference = neuralNetwork.Learn(outputs, inputs, 100000);

            var results = new List<double>();

            for (int i = 0; i < outputs.Length; i++)
            {
                var row = NeuralNetwork.GetRow(inputs, i);
                var result = neuralNetwork.Predict(row).Output;
                results.Add(result);
            }

            //neuralNetwork.Layers[1].Neurons[0].SetWeights(0.5, -0.1, 0.3, -0.1);
            //neuralNetwork.Layers[1].Neurons[1].SetWeights(0.1, -0.3, 0.7, -0.3);
            //neuralNetwork.Layers[2].Neurons[0].SetWeights(1.2, 0.8);
               
            for(int i = 0; i < results.Count; i++)
            {
                var expected = Math.Round(outputs[i], 3);
                var actual = Math.Round(results[i], 3);
                
                Assert.AreEqual(expected,actual);
            }
        }

        [TestMethod()]
        public void DatasetTest()
        {
            var outputs = new List<double>();
            var inputs = new List<double[]>();
            using (var sr = new StreamReader(@"D:\Machine Learning\NeuralNetworksTests\images\heart.csv"))
            {
               var header = sr.ReadLine();

                while (!sr.EndOfStream)
                {
                    var row = sr.ReadLine();
                    var values = row.Split(",").Select(item=> Convert.ToDouble(item.Replace(".", ","))).ToList();
                    
                    var output = values.Last();
                    var input = values.Take(values.Count - 1).ToArray();

                    outputs.Add(output);
                    inputs.Add(input);
                }
            }

            var inputSignals = new double[inputs.Count, inputs[0].Length];

            for (int i = 0; i < inputSignals.GetLength(0); i++)
            {
                for (int j = 0; j < inputSignals.GetLength(1); j++)
                {
                    inputSignals[i,j] = inputs[i][j];
                }
            }

            var topology = new Topology(outputs.Count, 1, 0.1, outputs.Count / 2);
            var neuralNetwork = new NeuralNetwork(topology);

            var difference = neuralNetwork.Learn(outputs.ToArray(), inputSignals, 1000);

            var results = new List<double>();

            for (int i = 0; i < outputs.Count; i++)
            {
                var result = neuralNetwork.Predict(inputs[i]).Output;
                results.Add(result);
            }

            //neuralNetwork.Layers[1].Neurons[0].SetWeights(0.5, -0.1, 0.3, -0.1);
            //neuralNetwork.Layers[1].Neurons[1].SetWeights(0.1, -0.3, 0.7, -0.3);
            //neuralNetwork.Layers[2].Neurons[0].SetWeights(1.2, 0.8);

            for (int i = 0; i < results.Count; i++)
            {
                var expected = Math.Round(outputs[i], 3);
                var actual = Math.Round(results[i], 3);

                Assert.AreEqual(expected, actual);
            }
        }

        [TestMethod()]
        public void ReccognizeImages()
        {
            var parasitizedPath = @"";
            var unParasitizedPath = @"";

            var converter = new PictureConverter();
            var testParasitizedImageInputs = converter.Convert(@"D:\Machine Learning\NeuralNetworksTests\images\Parasitized.png");
            var testUnparasitizedImageInputs = converter.Convert(@"D:\Machine Learning\NeuralNetworksTests\images\Unparasitized.png");

            var topology = new Topology(testParasitizedImageInputs.Length, 1, 0.1, testParasitizedImageInputs.Length / 2);
            var neuralNetwork = new NeuralNetwork(topology);

            var parasitizedInputs = GetData(parasitizedPath, converter, testParasitizedImageInputs);
            neuralNetwork.Learn(new double[] { 1.0 }, parasitizedInputs, 1);    
            
            var unParasitizedInputs = GetData(unParasitizedPath, converter, testParasitizedImageInputs);
            neuralNetwork.Learn(new double[] { 0.0 }, unParasitizedInputs, 1);

            var par = neuralNetwork.Predict(testParasitizedImageInputs.Select(t=> (double)t).ToArray());
            var unpar = neuralNetwork.Predict(testUnparasitizedImageInputs.Select(t=> (double)t).ToArray());

            Assert.AreEqual(1, Math.Round(par.Output, 2));
            Assert.AreEqual(0, Math.Round(unpar.Output, 2));
        }

        private static double [,] GetData(string imagesPath, PictureConverter converter, double[] testImageInputs, int size = 100)
        {
            var images = Directory.GetFiles(imagesPath);
            var result = new double[size, testImageInputs.Length];
            for (int i = 0; i < size; i++)
            {
                var image = converter.Convert(images[i]);
                for (int j = 0; j < image.Length; j++)
                {
                    result[i, j] = image[j];
                }
            }
            return result;
        }
    }
}
