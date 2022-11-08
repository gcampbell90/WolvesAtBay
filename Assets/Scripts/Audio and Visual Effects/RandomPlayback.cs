using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class RandomPlayback : MonoBehaviour
{
    public AudioClip[] RandomClips;
    public AudioMixerGroup output;

    public float minPitch = 0.95f;
    public float maxPitch = 1.05f;

    public void PlaySound()
    {
        PlayRandomSound();
    }

    void PlayRandomSound()
    {
        int randomNumber = Random.Range(0, RandomClips.Length); //randomise
        AudioSource source = gameObject.AddComponent<AudioSource>(); //Create AudioSource
        source.clip = RandomClips[randomNumber]; //Load Clip to AudioSource
        source.outputAudioMixerGroup = output; //Set output for AudioSource

        source.volume = 0.8f; //Sets volume
        source.pitch = Random.Range(minPitch, maxPitch); //Sets pitch

        source.Play(); //Play clip
        Destroy(source, RandomClips[randomNumber].length); //Destroy when done
    }
}