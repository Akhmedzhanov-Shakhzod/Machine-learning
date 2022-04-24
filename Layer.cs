namespace NeuralNetworks
{
    public class Layer
    {
        public List<Neuron> Neurons { get; }
        public int Count => Neurons?.Count ?? 0;

        public Layer(List<Neuron> neurons, NeuronType neuronType = NeuronType.Normal)
        {
            //TODO: Проверить все входные нейроны на соответствие типов
            Neurons = neurons;
        }
        public List<double> GetSignals()
        {
            var result = new List<double>();
            foreach (Neuron neuron in Neurons)
            {
                result.Add(neuron.Output);
            }
            return result;
        }
    }
}
