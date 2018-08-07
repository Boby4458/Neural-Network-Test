using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public sealed class neuralNetwork : MonoBehaviour {

    //Backprop equation: change = (learningRate * delta * value) + (momentum * pastChange)



    //public int hiddenLayerRows;

    public GameObject runner;

    private List<layer> Layers;
    public int[] neuronsPerLayer;
    private layer inputLayer = new layer();
    private layer outputLayer;
    public float initWeightMin, initWeightMax;
    public int layerCount;

    private void Start()
    {
        //print(Sigmoid(0.5f).ToString());
        
        neuron inputNeuron = new neuron();
        inputNeuron.curValue = 1;
        inputLayer.neurons = new List<neuron>();
        neuron inputNeuron1 = new neuron();
        inputNeuron1.curValue = 3;
        neuron inputNeuron2 = new neuron();
        inputNeuron2.curValue = 7;
        neuron inputNeuron3 = new neuron();
        inputNeuron3.curValue = 2;
        neuron inputNeuron4 = new neuron();
        inputNeuron4.curValue =  0.5f;
       

        inputLayer.neurons = new List<neuron>() { inputNeuron, inputNeuron1, inputNeuron2, inputNeuron3, inputNeuron4 };
        
    }

    private void displayNetwork()
    {
        for (int x = 0; x < Layers.Count; x++)
        {
            for (int y = 0; y < Layers[x].neurons.Count; y++)
            {
                GameObject.Find("(" + x.ToString() + ", " + y.ToString() + ")").GetComponent<Text>().text = Layers[x].neurons[y].curValue.ToString();
                
              
            }
        }
        for (int x = 1; x < Layers.Count; x++)
        {

            //print(x);
            for (int y = 0; y < Layers[x].neurons.Count; y++)
            {
               // print(y);
                for (int z = 0; z < Layers[x].neurons[y].backwardWeights.Count; z++)
                {
                  //  print(z);
                    GameObject.Find("(" + (x-1).ToString() + ", " + (y).ToString() + ")" + "C (" + z.ToString() + ")").GetComponent<Text>().text = Layers[x].neurons[y].backwardWeights[z].ToString();
                }
            }
        }

    }

    private void generateNetwork ()
    {
        inputLayer = new layer();

        inputLayer.neurons = new List<neuron>() { new neuron() };
        Layers = new List<layer>(layerCount);
        for (int i = 0; i < layerCount; i++)
        {
            layer newLayer = new layer();

            for (int x = 0; x < neuronsPerLayer [i]; x++)
            {

                neuron newNeuron = new neuron();
                if (i != 0)
                {
                    newNeuron.backwardConnections = Layers[i - 1].neurons;

                }
                else
                {
                    print("works");
                    newNeuron.backwardConnections = inputLayer.neurons;
                }
                newLayer.neurons.Add(newNeuron);
                

            }
            
            
            foreach (neuron _neuron in (i != 0) ? Layers[i - 1].neurons : inputLayer.neurons)
            {
                _neuron.forwardConnections = newLayer.neurons;
            }
            Layers.Add(newLayer);
        }

        Debug.Log("Layer count" + Layers.Count);
        Debug.Log("Layer 1 neuron count " + Layers[0].neurons.Count);


    }

    private void initWeights()
    {
        for (int l = 0; l < Layers.Count; l++)
        {
            for (int n = 0; n < neuronsPerLayer [l]; n++)
            {
                Layers[l].neurons[n].backwardWeights = new List<float>(Layers[l].neurons[n].backwardConnections.Count);
                for (int w = 0; w < Layers[l].neurons[n].backwardConnections.Count; w++)
                {
                    Layers[l].neurons[n].backwardWeights.Add((float)Random.Range(initWeightMin, (float)initWeightMax) /1);
                }
            }
        }
    }

    private void initBiases()
    {
        for (int l = 0; l < layerCount; l++)
        {
            for (int n = 0; n < Layers [l].neurons.Count; n++)
            {
                Layers [l].neurons [n].bias = Random.Range ((float)-1, (float)1);
            }
        }
    }

    private float Sigmoid(float x)
    {
        return 1 / (1f + Mathf.Exp(0.5f * -x));
    }

    public float run()
    {

        for (int l = 0; l < Layers.Count; l++)
        {
            for (int n = 0; n < Layers [l].neurons.Count; n++)
            {
                float sum = 0;
                for (int conn = 0; conn < Layers[l].neurons[n].backwardConnections.Count; conn++)
                {
                    float curValue = ((l != 0) ? (Layers[l - 1].neurons[conn].curValue) : (inputLayer.neurons[conn].curValue));
                    sum += curValue * Layers[l].neurons[n].backwardWeights[conn];
                }
                sum += Layers[l].neurons[n].bias;
                sum = Sigmoid(sum);
                Layers[l].neurons[n].curValue = sum;
            }
        }
        return Layers[Layers.Count].neurons[0].curValue;
    }

    private void evolve()
    {

    }
    private void populate()
    {
        if (Layers.Count == 0)
        {
            generateNetwork();
        }
        
        initWeights();
        initBiases();
        run();
        displayNetwork();
        Instantiate(runner, GameObject.Find("spawnpoint").transform.position, GameObject.Find("spawnpoint").transform.rotation);

    }

}
sealed class layer
{
    public List<neuron> neurons = new List<neuron>();
  

}
sealed class neuron
{

    public neuronType myType;
    public float bias;
    public float curValue;
    public List<neuron> forwardConnections = new List<neuron>();
    public List<neuron> backwardConnections = new List<neuron>();
    //public List<float> forwardWeights = new List<float>();
    public List<float> backwardWeights = new List<float>();

}

enum neuronType
{
    inputNeuron,
    hiddenNeuron,
    outPutNeuron
};