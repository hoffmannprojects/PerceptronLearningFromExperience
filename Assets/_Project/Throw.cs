using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

[RequireComponent(typeof(Perceptron))]
public class Throw : MonoBehaviour
{
    [SerializeField]
    private readonly GameObject _spherePrefab;
    [SerializeField]
    private readonly GameObject _cubePrefab;
    private readonly Color _green = Color.green; // Trying to set material color instead of setting a new material.
    private readonly Color _red = Color.red;
    private static Perceptron _perceptron;

    // Use this for initialization
    private void Start ()
    {
        _perceptron = GetComponent<Perceptron>();
        Assert.IsNotNull(_perceptron);
	}
	
	// Update is called once per frame
	private void Update ()
    {

	}
}
