public class NeuralNetwork
{
    private int inputSize;
    private int hiddenSize;
    private int outputSize;
    private float[,] weights1;
    private float[,] weights2;
    private float[] biases1;
    private float[] biases2;

    public NeuralNetwork(int inputSize, int hiddenSize, int outputSize)
    {
        this.inputSize = inputSize;
        this.hiddenSize = hiddenSize;
        this.outputSize = outputSize;

        weights1 = new float[inputSize, hiddenSize];
        weights2 = new float[hiddenSize, outputSize];
        biases1 = new float[hiddenSize];
        biases2 = new float[outputSize];

        InitialiseWeights();
    }

    private void InitialiseWeights()
    {
        System.Random rand = new System.Random();
        for (int i = 0; i < inputSize; i++)
        {
            for (int j = 0; j < hiddenSize; j++)
            {
                weights1[i, j] = (float)(rand.NextDouble() * 2 - 1);
            }
        }
        for (int i = 0; i < hiddenSize; i++)
        {
            for (int j = 0; j < outputSize; j++)
            {
                weights2[i, j] = (float)(rand.NextDouble() * 2 - 1);
            }
        }
    }

    private float[] Activate(float[] inputs, float[,] weights, float[] biases)
    {
        int layerSize = biases.Length;
        float[] outputs = new float[layerSize];

        for (int i = 0; i < layerSize; i++)
        {
            float sum = biases[i];
            for (int j = 0; j < inputs.Length; j++)
            {
                sum += inputs[j] * weights[j, i];
            }
            outputs[i] = (float)System.Math.Tanh(sum);
        }

        return outputs;
    }

    public float[] Predict(float[] inputs)
    {
        float[] hiddenLayer = Activate(inputs, weights1, biases1);
        float[] outputLayer = Activate(hiddenLayer, weights2, biases2);
        return outputLayer;
    }

    public void Mutate(float mutationRate)
    {
        System.Random rand = new System.Random();
        for (int i = 0; i < inputSize; i++)
        {
            for (int j = 0; j < hiddenSize; j++)
            {
                if (rand.NextDouble() < mutationRate)
                {
                    weights1[i, j] += (float)(rand.NextDouble() * 2 - 1) * 0.1f;
                }
            }
        }
        for (int i = 0; i < hiddenSize; i++)
        {
            for (int j = 0; j < outputSize; j++)
            {
                if (rand.NextDouble() < mutationRate)
                {
                    weights2[i, j] += (float)(rand.NextDouble() * 2 - 1) * 0.1f;
                }
            }
        }
    }
}
