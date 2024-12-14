using UnityEngine;

public class MusicManager : MonoBehaviour
{
    public AudioSource overworldMusic; // Assign AudioSource for overworld music
    public AudioSource battleMusic;   // Assign AudioSource for battle music

    private void Start()
    {
        PlayOverworldMusic(); // Start with overworld music
    }

    public void PlayOverworldMusic()
    {
        if (battleMusic.isPlaying)
        {
            StartCoroutine(FadeOut(battleMusic));
        }
        if (!overworldMusic.isPlaying)
        {
            overworldMusic.volume = 0;
            overworldMusic.Play();
            StartCoroutine(FadeIn(overworldMusic));
        }
    }

    public void PlayBattleMusic()
    {
        if (overworldMusic.isPlaying)
        {
            StartCoroutine(FadeOut(overworldMusic));
        }
        if (!battleMusic.isPlaying)
        {
            battleMusic.volume = 0;
            battleMusic.Play();
            StartCoroutine(FadeIn(battleMusic));
        }
    }

    private System.Collections.IEnumerator FadeIn(AudioSource audioSource, float duration = 1.5f)
    {
        float targetVolume = 1f;
        audioSource.volume = 0;

        while (audioSource.volume < targetVolume)
        {
            audioSource.volume += Time.deltaTime / duration;
            yield return null;
        }
    }

    private System.Collections.IEnumerator FadeOut(AudioSource audioSource, float duration = 1.5f)
    {
        float startVolume = audioSource.volume;

        while (audioSource.volume > 0)
        {
            audioSource.volume -= Time.deltaTime / duration;
            yield return null;
        }

        audioSource.Stop();
        audioSource.volume = startVolume; // Reset volume for future use
    }
}
