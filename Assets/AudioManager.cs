using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;

    [System.Serializable]
    public struct SFX
    {
        public string name; // Nama SFX
        public AudioClip clip; // Klip SFX
    }

    [Header("Sound Effects")]
    public SFX[] sfxArray; // Array pasangan nama dan klip

    [Header("Music")]
    public AudioClip[] musicClips; // Array Music
    public AudioSource sfxSource; // Audio source untuk SFX
    public AudioSource musicSource; // Audio source untuk musik

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            //DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    // Play SFX by name
    public void PlaySFX(string name)
    {
        foreach (SFX sfx in sfxArray)
        {
            if (sfx.name == name)
            {
                sfxSource.PlayOneShot(sfx.clip);
                return;
            }
        }
        Debug.LogWarning($"SFX with name '{name}' not found!");
    }

    // Play SFX by index
    public void PlaySFX(int index)
    {
        if (index >= 0 && index < sfxArray.Length)
        {
            sfxSource.PlayOneShot(sfxArray[index].clip);
        }
        else
        {
            Debug.LogWarning($"SFX index '{index}' out of range!");
        }
    }

    public void PlayMusic(int index)
    {
        if (index >= 0 && index < musicClips.Length)
        {
            musicSource.clip = musicClips[index];
            musicSource.Play();
        }
        else
        {
            Debug.LogWarning("Music index out of range!");
        }
    }

    public void StopMusic()
    {
        musicSource.Stop();
    }
}
