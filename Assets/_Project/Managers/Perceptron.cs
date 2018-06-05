using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class TrainingSet
{
	public double[] input;
	public double output;
}

public class Perceptron : MonoBehaviour
{
    [SerializeField]
    GameObject _npc;
	List<TrainingSet> _trainingSets = new List<TrainingSet>();

	double[] _weights = {0,0};
	double _bias = 0;
	double _totalError = 0;

	double DotProductBias(double[] v1, double[] v2) 
	{
		if (v1 == null || v2 == null)
			return -1;
	 
		if (v1.Length != v2.Length)
			return -1;
	 
		double d = 0;
		for (int x = 0; x < v1.Length; x++)
		{
			d += v1[x] * v2[x];
		}

		d += _bias;
	 
		return d;
	}

	double CalcOutput(int i)
	{
		return(ActivationFunction(DotProductBias(_weights,_trainingSets[i].input)));
	}

	double CalcOutput(double i1, double i2)
	{
		double[] inp = new double[] {i1, i2};
		return(ActivationFunction(DotProductBias(_weights,inp)));
	}

	double ActivationFunction(double dp)
	{
		if(dp > 0) return (1);
		return(0);
	}

	void InitialiseWeights()
	{
		for(int i = 0; i < _weights.Length; i++)
		{
			_weights[i] = Random.Range(-1.0f,1.0f);
		}
		_bias = Random.Range(-1.0f,1.0f);
	}

	void UpdateWeights(int j)
	{
		double error = _trainingSets[j].output - CalcOutput(j);
		_totalError += Mathf.Abs((float)error);
		for(int i = 0; i < _weights.Length; i++)
		{			
			_weights[i] = _weights[i] + error*_trainingSets[j].input[i]; 
		}
		_bias += error;
	}

	void Train()
	{
		for(int t = 0; t < _trainingSets.Count; t++)
		{
			UpdateWeights(t);
		}
	}


	void Start ()
    {
        // Need to be randomized from the start.
        InitialiseWeights();
	}
	
	private void SendInput (double input1, double input2, double expectedOutput)
    {
        // React.
        double output = CalcOutput(input1, input2);
        Debug.Log(output);

        if (output == 0)
        {
            // Duck for cover.
            _npc.GetComponent<Animator>().SetTrigger("Crouch");
            _npc.GetComponent<Rigidbody>().isKinematic = false;
        }
        else
        {
            _npc.GetComponent<Rigidbody>().isKinematic = true;
        }

        // Learn from it for next time.
        var trainingSet = new TrainingSet
        {
            input = new double[2] { input1, input2 },
            output = expectedOutput
        };
        _trainingSets.Add(trainingSet);
        Train();
    }
}