using System;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class Configuration : MonoBehaviour
{
    [SerializeField] private AudioMixer audioMixer;

    [SerializeField] private Slider master;
    [SerializeField] private Slider music;
    [SerializeField] private Slider sfxs;


    private void Start()
    {
        //master = GameObject.Find("SliderMainAudio").GetComponent<Slider>();
        //music = GameObject.Find("SliderMusic").GetComponent<Slider>();
        //sfxs = GameObject.Find("SliderSFXs").GetComponent<Slider>();
        music.value = AudioManager.Instance.musicVolume;
        sfxs.value = AudioManager.Instance.sfxsVolume;
        master.value = AudioManager.Instance.masterVolume;
    }

    public void SetMusicValue()
    {
        
        audioMixer.SetFloat("Musica", MathF.Log10(music.value) * 20);
        AudioManager.Instance.musicVolume = music.value;
    }

    public void SetSfxsValue()
    {
        audioMixer.SetFloat("SFXs", MathF.Log10(sfxs.value) * 20);
        AudioManager.Instance.sfxsVolume = sfxs.value;
    }

    public void SetMasterValue()
    {
        audioMixer.SetFloat("Master", MathF.Log10(master.value) * 20);
        AudioManager.Instance.masterVolume = master.value;

    }

}
