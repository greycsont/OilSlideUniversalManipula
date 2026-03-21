using UnityEngine;

using UnityEngine.Audio;

namespace OilSlideUniversal;

public sealed class SoloAudioSource : MonoBehaviour
{
    private AudioSource _audioSource;
    
    private static SoloAudioSource _instance;

    public static SoloAudioSource Instance
    {
        get
        {
            if (_instance == null)
            {
                var obj = new GameObject("SoloAudioSource");
                DontDestroyOnLoad(obj);
                _instance = obj.AddComponent<SoloAudioSource>();
            }
            return _instance;
        }
    }

    public void Play()
    {
        Play(Plugin.clip);
    }

    public void Play(AudioClip clip)
    {
        if (_audioSource == null)
            _audioSource = gameObject.AddComponent<AudioSource>();

        _audioSource.Stop();
        
        _audioSource.SetSpatialBlend(0f);
        _audioSource.volume = 1f;
        _audioSource.PlayOneShot(clip, true);
    }
}        