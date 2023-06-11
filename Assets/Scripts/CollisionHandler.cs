using System;
using UnityEngine;
using UnityEngine.SceneManagement;


public class CollisionHandler : MonoBehaviour
{

    [SerializeField] float delay = 2f;
    [SerializeField] AudioClip crashSound;
    [SerializeField] AudioClip successSound;
    [SerializeField] ParticleSystem crashParticles;
    [SerializeField] ParticleSystem successParticles;

    AudioSource audioSource;

    bool isTransitioning = false;
    bool collisionsActive = true;
    
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    void Update ()
    {
        RespondToDebugKeys();
    }

    void RespondToDebugKeys()
    {
        if (Input.GetKeyDown(KeyCode.C))
        {
            collisionsActive = !collisionsActive; //toggles a bool
        }
        else if (Input.GetKeyDown(KeyCode.L))
        {
            LoadNextLevel();
        }
    }
    
    void OnCollisionEnter(Collision other) {
        
        if (isTransitioning||!collisionsActive){return;}

        switch (other.gameObject.tag)
        {
            case "Friendly":
                Debug.Log("This thing is friendly");
                break;
            case "Finish":
                StartLevelUp();
                Debug.Log("Yo you have landed");
                break;   
            default:
                StartCrashSequence();
                Debug.Log("Ooops You Crashed!!");
                break;
        }    
    }

    void StartCrashSequence()
    {
        isTransitioning = true;

        audioSource.Stop();
        GetComponent<Movement>().enabled = false;
        audioSource.PlayOneShot(crashSound);
        GetComponent<ParticleSystem>().Play(crashParticles);
        Invoke("ReloadLevel", delay);
    }

    void StartLevelUp()
    {
        audioSource.Stop();
        isTransitioning = true;
        GetComponent<Movement>().enabled = false;
        audioSource.PlayOneShot(successSound);
        GetComponent<ParticleSystem>().Play(successParticles);
        Invoke("LoadNextLevel", delay);
    }

    void ReloadLevel()
    {
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(currentSceneIndex);
    }

    void LoadNextLevel()
    {
        int nextSceneIndex = SceneManager.GetActiveScene().buildIndex + 1;
        if (nextSceneIndex == SceneManager.sceneCountInBuildSettings)
        {
            nextSceneIndex = 0;
        }
        SceneManager.LoadScene(nextSceneIndex);
            
    }
}