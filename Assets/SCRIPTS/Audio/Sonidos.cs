using UnityEngine;
[System.Serializable]
public class Sonidos
{
    public string audioName;

    public AudioClip audioClip;

    [Range (0f,1f)]
    public float volume;

    public bool loop;

    [HideInInspector]
    public AudioSource audioSource;

}
