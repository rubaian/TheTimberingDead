using UnityEngine;

public class SoundsManager : MonoBehaviour
{
    public static SoundsManager instance { get; private set; }
    private AudioSource source;

    private void Awake()
    {
        source = GetComponent<AudioSource>();

        // Keep this object even when we go to a new scene
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        // Destroy duplicate gameobjects
        else if (instance != null && instance != this)
            Destroy(gameObject);
    }

    public void PlaySound(AudioClip _sound)
    {
        if (_sound != null && !source.isPlaying) // Check if the sound is not already playing
        {
            source.PlayOneShot(_sound); // Play the sound
        }
    }
}