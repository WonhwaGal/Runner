using System.Collections;
using UnityEngine;
using UnityEngine.Audio;

public class AudioManager : MonoBehaviour
{
    [SerializeField] private AudioSource _musicSource;
    [SerializeField] private AudioSource _effectsSource;
    [SerializeField] private AudioMixer _mixer;

    [SerializeField] private string _masterGroup = "Master";
    [SerializeField] private string _musicSourceGroup = "Music";
    [SerializeField] private string _effectsSourceGroup = "Effects";

    private void Start()
    {
        DontDestroyOnLoad(this);
    }
}