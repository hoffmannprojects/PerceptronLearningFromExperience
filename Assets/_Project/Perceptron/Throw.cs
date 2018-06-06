using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

[RequireComponent(typeof(Perceptron))]
public class Throw : MonoBehaviour
{
    [SerializeField]
    private GameObject _spherePrefab;
    [SerializeField]
    private GameObject _cubePrefab;
    private readonly Color _green = Color.green; // Trying to set material color instead of setting a new material.
    private readonly Color _red = Color.red;
    private Perceptron _perceptron;

    // Use this for initialization
    private void Start ()
    {
        _perceptron = GetComponent<Perceptron>();
        Assert.IsNotNull(_perceptron);
	}
	
	// Update is called once per frame
	private void Update ()
    {
        var force = new Vector3(0f, 0f, 500f);
        var sphere = 0d;
        var cube = 1d;
        var red = 0d;
        var green = 1d;

        if (Input.GetKeyDown("1"))
        {
            // Shoot red sphere.
            GameObject gameObject = Instantiate(_spherePrefab, Camera.main.transform.position, Camera.main.transform.rotation);
            gameObject.GetComponent<MeshRenderer>().material.color = _red;
            gameObject.GetComponent<Rigidbody>().AddForce(force);
            _perceptron.SendInput(sphere, red, 0d);
        }
        else if (Input.GetKeyDown("2"))
        {
            // Shoot green sphere.
            GameObject gameObject = Instantiate(_spherePrefab, Camera.main.transform.position, Camera.main.transform.rotation);
            gameObject.GetComponent<MeshRenderer>().material.color = _green;
            gameObject.GetComponent<Rigidbody>().AddForce(force);
            _perceptron.SendInput(sphere, green, 1d);
        }
        else if (Input.GetKeyDown("3"))
        {
            // Shoot red cube.
            GameObject gameObject = Instantiate(_cubePrefab, Camera.main.transform.position, Camera.main.transform.rotation);
            gameObject.GetComponent<MeshRenderer>().material.color = _red;
            gameObject.GetComponent<Rigidbody>().AddForce(force);
            _perceptron.SendInput(cube, red, 1d);
        }
        else if (Input.GetKeyDown("4"))
        {
            // Shoot green cube.
            GameObject gameObject = Instantiate(_cubePrefab, Camera.main.transform.position, Camera.main.transform.rotation);
            gameObject.GetComponent<MeshRenderer>().material.color = _green;
            gameObject.GetComponent<Rigidbody>().AddForce(force);
            _perceptron.SendInput(cube, green, 1d);
        }
    }
}
