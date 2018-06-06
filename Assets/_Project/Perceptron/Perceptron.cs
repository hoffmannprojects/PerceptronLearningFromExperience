using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Assertions;

#region Helper Classes
[System.Serializable]
public class TrainingSet
{
    public double[] input;
    public double output;
} 
#endregion

public class Perceptron : MonoBehaviour
{
    #region Fields
    [SerializeField]
    private GameObject _npc;
    private List<TrainingSet> _trainingSets = new List<TrainingSet>();
    private string _dataPath;

    private double[] _weights = { 0, 0 }; // Store the actual learned behaviour.
    private double _bias = 0;
    private double _totalError = 0;
    #endregion

    #region Monobehaviour Methods
    private void Start ()
    {
        // Need to be randomized from the start.
        InitialiseWeights();
        _dataPath = Application.dataPath + "/weights.txt";
    }

    private void Update ()
    {
        if (Input.GetButtonDown("Cancel"))
        {
            // Reset training.
            InitialiseWeights();
            _trainingSets.Clear();
            Debug.Log("Training data has been reset.");
        }
        else if (Input.GetKeyDown("s"))
        {
            SaveWeightsAndBias();
        }
        else if (Input.GetKeyDown("l"))
        {
            LoadWeightsAndBias();
        }
    } 
    #endregion

    #region Private Methods
    double DotProductBias (double[] v1, double[] v2)
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

    double CalcOutput (int i)
    {
        return (ActivationFunction(DotProductBias(_weights, _trainingSets[i].input)));
    }

    double CalcOutput (double i1, double i2)
    {
        double[] inp = new double[] { i1, i2 };
        return (ActivationFunction(DotProductBias(_weights, inp)));
    }

    double ActivationFunction (double dp)
    {
        if (dp > 0) return (1);
        return (0);
    }

    void InitialiseWeights ()
    {
        for (int i = 0; i < _weights.Length; i++)
        {
            _weights[i] = Random.Range(-1.0f, 1.0f);
        }
        _bias = Random.Range(-1.0f, 1.0f);
    }

    void UpdateWeights (int j)
    {
        double error = _trainingSets[j].output - CalcOutput(j);
        _totalError += Mathf.Abs((float)error);
        for (int i = 0; i < _weights.Length; i++)
        {
            _weights[i] = _weights[i] + error * _trainingSets[j].input[i];
        }
        _bias += error;
    }

    void Train ()
    {
        for (int t = 0; t < _trainingSets.Count; t++)
        {
            UpdateWeights(t);
        }
    }

    public void SendInput (double input1, double input2, double desiredOutput )
    {
        // React.
        double output = CalcOutput(input1, input2);
        Debug.Log(output);

        if (output == 0)
        {
            // Desired casse: red sphere.
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
            output = desiredOutput
        };
        _trainingSets.Add(trainingSet);
        // As more trainingSets get added to the list, Train() will have more data for calculations.
        Train();
    } 

    private void SaveWeightsAndBias ()
    {
        StreamWriter streamWriter = File.CreateText(_dataPath);
        streamWriter.WriteLine(_weights[0] + "," + _weights[1] + "," + _bias);
        streamWriter.Close();
        Debug.Log("Saved weights and bias to disk.");
    }

    private void LoadWeightsAndBias ()
    {
        Assert.IsTrue(File.Exists(_dataPath));
        if (File.Exists(_dataPath))
        {
            StreamReader streamReader = File.OpenText(_dataPath);
            string line = streamReader.ReadLine();
            string[] words = line.Split(',');
            _weights[0] = System.Convert.ToDouble(words[0]);
            _weights[1] = System.Convert.ToDouble(words[1]);
            _bias = System.Convert.ToDouble(words[2]);
            Debug.Log("Loaded weights and bias to disk.");
        }
    }
    #endregion
}