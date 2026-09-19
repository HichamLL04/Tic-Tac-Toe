using System;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    private static AudioManager _instance;
    AudioSource audioSource;
    [SerializeField] AudioClip backgroundOST;
    [SerializeField] AudioClip[] plays;

    public static AudioManager instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindAnyObjectByType<AudioManager>();
                if (_instance == null)
                {
                    GameObject go = new GameObject("MenuManager");
                    _instance = go.AddComponent<AudioManager>();
                }
            }
            return _instance;
        }
    }

    public void Awake()
    {
        if (_instance == null)
        {
            _instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else if (_instance != this)
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        PlayOst();
    }

    void PlayClip(AudioClip clip)
    {
        if (clip == null) return;
        if (audioSource == null)
        {
            audioSource = GetComponent<AudioSource>();
            if (audioSource == null)
            {
                audioSource = gameObject.AddComponent<AudioSource>();
            }
        }
        audioSource.PlayOneShot(clip);
    }

    public void PlayRandomPlaySound()
    {
        if (plays != null && plays.Length > 0)
        {
            int randomIndex = UnityEngine.Random.Range(0, plays.Length);
            if (plays[randomIndex] != null)
            {
                PlayClip(plays[randomIndex]);
            }
        }
    }

    void PlayOst()
    {
        if (backgroundOST != null)
        {
            if (audioSource == null) audioSource = GetComponent<AudioSource>();
            if (audioSource == null) audioSource = gameObject.AddComponent<AudioSource>();
            audioSource.loop = true;
            audioSource.clip = backgroundOST;
            audioSource.Play();
        }
    }
}

