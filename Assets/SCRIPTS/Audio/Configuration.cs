using System;
using UnityEngine;
using UnityEngine.Audio;

public class Configuration : MonoBehaviour
{
    [SerializeField] private AudioMixer audioMixer;

    [Range(0.0001f,1f)]
    [SerializeField] private float sfxsVolume;
    [Range(0.0001f,1f)]
    [SerializeField] private float musicVolume;
    [Range(0.0001f,1f)]
    [SerializeField] private float masterVolume;

    public static Configuration Instance;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
        else
        {
            Destroy(this.gameObject);
        }

    }

    private void Update()
    {
        audioMixer.SetFloat("SFXs", MathF.Log10(sfxsVolume)*20);
        audioMixer.SetFloat("Musica", MathF.Log10(musicVolume) * 20);
        audioMixer.SetFloat("Master", MathF.Log10(masterVolume) * 20);
    }

}
