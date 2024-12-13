using System.Collections;
using UnityEngine;

public class MusicManager : MonoBehaviour
{
    public AudioSource overworldMusic; // Reference to the overworld music AudioSource
    public AudioSource battleMusic;   // Reference to the battle music AudioSource
    public float fadeDuration = 1.5f; // Duration of the fade in/out

    private AudioSource currentMusic; // Keeps track of the currently playing music

    private void Start()
    {
        // Ensure overworld music starts playing at the beginning
        currentMusic = overworldMusic;
        currentMusic.volume = 1f;
        currentMusic.Play();
    }

    public void PlayBattleMusic()
    {
        // Switch to battle music
        if (currentMusic != battleMusic)
        {
            StartCoroutine(FadeMusic(battleMusic));
        }
    }

    public void PlayOverworldMusic()
    {
        // Switch back to overworld music
        if (currentMusic != overworldMusic)
        {
            StartCoroutine(FadeMusic(overworldMusic));
        }
    }

    private IEnumerator FadeMusic(AudioSource newMusic)
    {
        if (currentMusic != null)
        {
            // Fade out the current music
            float startVolume = currentMusic.volume;
            for (float t = 0; t < fadeDuration; t += Time.deltaTime)
            {
                currentMusic.volume = Mathf.Lerp(startVolume, 0, t / fadeDuration);
                yield return null;
            }
            currentMusic.Stop();
            currentMusic.volume = startVolume; // Reset volume for later use
        }

        // Switch to the new music
        currentMusic = newMusic;
        currentMusic.volume = 0;
        currentMusic.Play();

        // Fade in the new music
        for (float t = 0; t < fadeDuration; t += Time.deltaTime)
        {
            currentMusic.volume = Mathf.Lerp(0, 1, t / fadeDuration);
            yield return null;
        }
        currentMusic.volume = 1; // Ensure volume is set to max
    }
}
