using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CollisionHandler : MonoBehaviour
{
    [SerializeField] private float delayBeforeRestartLevel = 2f;
    [SerializeField] private float delayBeforeNextLevel = 2f;
    [SerializeField] private AudioClip boomAudio;
    [SerializeField] private AudioClip winAudio;
    
    [SerializeField] private ParticleSystem boomParticle;
    [SerializeField] private ParticleSystem winParticle;

    private AudioSource _audioSource;

    private bool _isTransitioning = false;
    private bool _isCollisionDeisabled = false;

    private void Start()
    {
        _audioSource = GetComponent<AudioSource>();
    }

    private void Update()
    {
        HandelDebugKeys();
    }

    private void HandelDebugKeys()
    {
        if (Input.GetKeyDown(KeyCode.L))
        {
            LoadNextLevel();
        }

        if (Input.GetKeyDown(KeyCode.C))
        {
            _isCollisionDeisabled = !_isCollisionDeisabled;
        }
    }

    private void OnCollisionEnter(Collision other)
    {
        if (_isTransitioning || _isCollisionDeisabled)
        {
            return;
        }
        
        switch (other.gameObject.tag)
        {
            case "Friendly":
                Debug.Log("Let's go!");
                break;
            case "Finish":
                StartWinSequence();
                break;
            default:
                StartCrashSequence();
                break;
        }
    }

    private void StartWinSequence()
    {
        StartTransition();
        _audioSource.PlayOneShot(winAudio);
        GetComponent<Movement>().enabled = false;
        winParticle.Play();
        Invoke(nameof(LoadNextLevel), delayBeforeNextLevel);
    }

    private void LoadNextLevel()
    {
        var currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        var nextScene = (currentSceneIndex + 1) % SceneManager.sceneCountInBuildSettings;
        SceneManager.LoadScene(nextScene);
    }

    private void StartCrashSequence()
    {
        StartTransition();
        boomParticle.Play();
        _audioSource.PlayOneShot(boomAudio);
        GetComponent<Movement>().enabled = false;
        Invoke(nameof(ReloadLevel), delayBeforeRestartLevel);
    }

    private void StartTransition()
    {
        _isTransitioning = true;
        _audioSource.Stop();
    }

    private void ReloadLevel()
    {
        var currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(currentSceneIndex); 
    }
}
