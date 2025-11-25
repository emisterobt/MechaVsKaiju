using System;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class Configuration : MonoBehaviour
{
    [SerializeField] private AudioMixer audioMixer;

    [Range(0.0001f,1f)]
    [SerializeField] private float sfxsVolume;
    [Range(0.0001f,1f)]
    [SerializeField] private float musicVolume;
    [Range(0.0001f,1f)]
    [SerializeField] private float masterVolume;

    [SerializeField] private Slider master;
    [SerializeField] private Slider music;
    [SerializeField] private Slider sfxs;


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

    private void Start()
    {
        master = GameObject.Find("SliderMainAudio").GetComponent<Slider>();
        music = GameObject.Find("SliderMusic").GetComponent<Slider>();
        sfxs = GameObject.Find("SliderSFXs").GetComponent<Slider>();
        music.value = musicVolume;
        sfxs.value = sfxsVolume;
        master.value = masterVolume;
    }

    public void SetMusicValue()
    {
        
        audioMixer.SetFloat("Musica", MathF.Log10(music.value) * 20);
        musicVolume = music.value;
    }

    public void SetSfxsValue()
    {
        audioMixer.SetFloat("SFXs", MathF.Log10(sfxs.value) * 20);
        sfxsVolume = sfxs.value;
    }

    public void SetMasterValue()
    {
        audioMixer.SetFloat("Master", MathF.Log10(master.value) * 20);
        masterVolume = master.value;

    }

}
