namespace NeuralNetworks
{
    public class Neuron
    {
        public List<double> Weights { get; }
        public List<double> Inputs { get; }
        public NeuronType NeuronType { get; }
        public double Output { get; private set; }
        public double Delta { get; private set; }

        public Neuron(int inputCount, NeuronType type = NeuronType.Normal)
        {
            NeuronType = type;
            Weights = new List<double>();
            Inputs = new List<double>();

            InitWeightsWithRandomValue(inputCount);
        }

        private void InitWeightsWithRandomValue(int inputCount)
        {
            var random = new Random();

            for (int i = 0; i < inputCount; i++)
            {
                if(NeuronType == NeuronType.Input)
                {
                    Weights.Add(1);
                }
                else
                {
                    Weights.Add(random.NextDouble());
                }

                Inputs.Add(0);
            }
        }
        public double FeedForward(List<double> inputs)
        {
            /* TODO: Проверить количество входных сигналов и весов на соответсвие, они должны быть одинакового количество
            if (inputs != Weights)
                return 0;
            */

            for (int i = 0; i < inputs.Count; i++)
            {
                Inputs[i] = inputs[i];
            }

            var sum = 0.0;
            for (int i = 0; i < inputs.Count; i++)
            {
                sum += inputs[i] * Weights[i];
            }
            if (NeuronType != NeuronType.Input)
            {
                Output = Sigmoid(sum);
            }
            else
            {
                Output = sum;
            }
            return Output;
        }
        
        //public void SetWeights(params double[] weights)
        //{
        //    // TODO: Удалить после добавление возможности обучение сети.
        //    for (int i = 0; i < weights.Length; i++)
        //    {
        //        Weights[i] = weights[i];
        //    }
        //}

        public void Learn(double error, double learningRate)
        {
            if (NeuronType == NeuronType.Input) return;

            Delta = error * SigmoidDx(Output);
            
            for(int i = 0; i < Weights.Count; i++)
            {
                Weights[i] = Weights[i] - Inputs[i] * Delta * learningRate;
            }
        }

        private double Sigmoid(double x)
        {
            return 1.0 / (1.0 + Math.Pow(Math.E, -x));
        }
        private double SigmoidDx(double x)
        {
            return Sigmoid(x)/(1 - Sigmoid(x));
        }
        public override string ToString()
        {
            return Output.ToString();
        }

    }
}
