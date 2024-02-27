using System.Collections.Generic;
using UnityEngine;

// Each AudioID represents a 2D audio
public enum AudioID
{
    Button,
    Background,
    BombKick,
}

/**
 * Responsible for playing all 2D audio
 */
public class AudioManager : MonoBehaviour
{
    // Represents audio data to be used for any one clip
    [System.Serializable]
    public class AudioGroup
    {
        public AudioID id;
        public float volume;
        public float pitch;
        public bool loop;
        public AudioClip clip;
    }

    [SerializeField] private AudioScriptableObject _audioScriptableObject = null;
    private Dictionary<AudioID, AudioSource> _audioDictionary = new Dictionary<AudioID, AudioSource>();

    public static AudioManager instance = null;
    private void Awake() {
        if (instance != null) {
            Destroy(this.gameObject);
            return;
        }

        instance = this;
        DontDestroyOnLoad(gameObject);

        // Create an audio source for each audio group
        foreach (AudioGroup audioGroup in _audioScriptableObject.AudioGroups) {
            AudioSource source = gameObject.AddComponent<AudioSource>();
            source.volume = audioGroup.volume;
            source.pitch = audioGroup.pitch;
            source.loop = audioGroup.loop;
            source.clip = audioGroup.clip;
            source.spatialBlend = 0;
            if (_audioDictionary.ContainsKey(audioGroup.id)) {
                Debug.LogWarning("Multiple instances of id " + audioGroup.id + " in audio scriptable object.");
            }
            _audioDictionary.Add(audioGroup.id, source);
        }

        // Play the looping background music from the start
        Play(AudioID.Background);
    }

    public void Play(AudioID id) {
        if (!_audioDictionary.ContainsKey(id)) {
            Debug.LogWarning("Audio of id " + id + " does not exist.");
            return;
        }
        _audioDictionary[id].Play();
    }

    public void Stop(AudioID id) {
        if (!_audioDictionary.ContainsKey(id)) {
            Debug.LogWarning("Audio of id " + id + " does not exist.");
            return;
        }
        _audioDictionary[id].Stop();
    }
}
