
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    [Header("Audio Sources")]
    [SerializeField] AudioSource musicSource;
    [SerializeField] AudioSource SFXSource;

    [Header("Audio Clips")]
    public AudioClip background;
    public AudioClip cat;
    public AudioClip collect;
    public AudioClip chodzenie;
    public AudioClip star;
   
    private void Start()
    {
        musicSource.clip = background;
        musicSource.Play();
    }

    public void PlaySFX (AudioClip clip)
    {
        SFXSource.PlayOneShot(clip);
    }
}
