using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Oscillator : MonoBehaviour
{
    [SerializeField] private Vector3 movementVector; 
    [SerializeField] private float period = 2f;
    
    private Vector3 _startingPosition;
    private float _movementFactor;

    private void Start()
    {
        _startingPosition = transform.position;
    }

    private void Update()
    {
        if (period < Mathf.Epsilon)
        {
            return;
        }
        
        var cycles = Time.time / period;
        
        const float tau = Mathf.PI * 2;
        var rawSinWave = Mathf.Sin(cycles * tau);

        _movementFactor = (rawSinWave + 1) / 2;

        var offset = _movementFactor * movementVector;
        transform.position = _startingPosition + offset;
    }
}
