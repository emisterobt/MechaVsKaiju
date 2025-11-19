using UnityEngine;
using UnityEngine.Audio;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;

    public Sonidos[] sonidos;

    public AudioMixer mixer;
    public AudioMixerGroup sfxs;

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
        foreach (Sonidos s in sonidos)
        {
            s.audioSource = gameObject.AddComponent<AudioSource>();
            s.audioSource.clip = s.audioClip;
            s.audioSource.loop = s.loop;
            s.audioSource.volume = s.volume;
            s.audioSource.outputAudioMixerGroup = sfxs;
        }
    }

    public void Play(string nombre)
    {
        foreach (Sonidos s in sonidos)
        {
            if (s.audioName == nombre)
            {
                s.audioSource.Play();
                return;
            }
        }
    }

    public void Stop(string nombre)
    {
        foreach (Sonidos s in sonidos)
        {
            if (s.audioName == nombre)
            {
                s.audioSource.Stop();
                return;
            }
        }
    }
}
