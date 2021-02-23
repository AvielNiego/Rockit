using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class Movement : MonoBehaviour
{
    [SerializeField] private float mainThrust = 1000f;
    [SerializeField] private float rotationThrust = 100f;
    [SerializeField] private AudioClip engineAudio;
    
    [SerializeField] private ParticleSystem mainBoosterParticle;
    [SerializeField] private ParticleSystem leftBoosterParticle;
    [SerializeField] private ParticleSystem rightBoosterParticle;

    private Rigidbody _rigidbody;
    private AudioSource _audioSource;
    private void Start()
    {
        _rigidbody = GetComponent<Rigidbody>();
        _audioSource = GetComponent<AudioSource>();
    }

    private void Update()
    {
        ProcessThrust();
        ProcessRotation();
    }

    private void ProcessThrust()
    {
        if (Input.GetKey(KeyCode.Space))
        {
            StartThrusting();
        }
        else
        {
            StopThrusting();
        }
    }

    private void StartThrusting()
    {
        _rigidbody.AddRelativeForce(Vector3.up * (mainThrust * Time.deltaTime));
        if (!mainBoosterParticle.isPlaying)
        {
            mainBoosterParticle.Play();
        }

        if (!_audioSource.isPlaying)
        {
            _audioSource.PlayOneShot(engineAudio);
        }
    }

    private void StopThrusting()
    {
        if (_audioSource.isPlaying)
        {
            _audioSource.Stop();
        }

        mainBoosterParticle.Stop();
    }

    private void ProcessRotation()
    {
        if (Input.GetKey(KeyCode.A))
        {
            RotateRight();
        }
        else if (Input.GetKey(KeyCode.D))
        {
            RotateLeft();
        }
        else
        {
            StopRotating();
        }
    }

    private void StopRotating()
    {
        rightBoosterParticle.Stop();
        leftBoosterParticle.Stop();
    }

    private void RotateLeft()
    {
        ApplyRotation(-rotationThrust);
        if (!rightBoosterParticle.isPlaying)
        {
            rightBoosterParticle.Play();
        }
    }

    private void RotateRight()
    {
        ApplyRotation(rotationThrust);
        if (!leftBoosterParticle.isPlaying)
        {
            leftBoosterParticle.Play();
        }
    }

    private void ApplyRotation(float thrust)
    {
        _rigidbody.freezeRotation = true;
        transform.Rotate(Vector3.forward * (thrust * Time.deltaTime));
        _rigidbody.freezeRotation = false;
    }
}
