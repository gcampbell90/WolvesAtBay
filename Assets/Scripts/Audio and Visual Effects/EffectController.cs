using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class EffectController : MonoBehaviour
{
    private static EffectController _instance;
    public static EffectController Instance { get { return _instance; } }

    [SerializeField] private AudioClip[] swordSwingClips;
    [SerializeField] private AudioClip[] deathSoundClips;

    public float minPitch = 0.95f;
    public float maxPitch = 1.05f;

    AudioSource source;

    public AudioMixerGroup output;

    private void Awake()
    {
        //Dont know if this is actually needed...
        if (_instance != null && _instance != this)
        {
            Destroy(this.gameObject);
            //Debug.Log("EffectController instance already exists...Destroying");

        }
        else
        {
            _instance = this;
            //Debug.Log("Setting EffectController to this instance...");
        }

        source = gameObject.AddComponent<AudioSource>(); //Create AudioSource
    }

    void PlayRandomSound(AudioClip[] RandomClips)
    {
        int randomNumber = Random.Range(0, RandomClips.Length); //randomise
        source.clip = RandomClips[randomNumber]; //Load Clip to AudioSource
       
        source.volume = 0.25f; //Sets volume
        source.pitch = Random.Range(minPitch, maxPitch); //Sets pitch

        //source.PlayOneShot(source.clip);
    }

    public void PlaySwordSound()
    {
        //PlayRandomSound(swordSwingClips);
        //RandomSwordSound();
    }

    public void PlayDeathSound()
    {
        //PlayRandomSound(deathSoundClips);
        //RandomDeathSound();
    }
}
