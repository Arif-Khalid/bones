using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public enum AudioID
{
    Button,
}

public class AudioManager : MonoBehaviour
{
    [System.Serializable]
    public class AudioGroup
    {
        public AudioID id;
        public float volume;
        public float pitch;
        public bool loop;
        public AudioClip clip;
    }
    public static AudioManager instance = null;
    [SerializeField] private AudioScriptableObject _audioScriptableObject = null;
    private Dictionary<AudioID, AudioSource> _audioDictionary = new Dictionary<AudioID, AudioSource>();
    private void Awake() {
        if(instance != null) {
            Destroy(this.gameObject);
            return;
        }
        
        instance = this;
        DontDestroyOnLoad(gameObject);

        foreach(AudioGroup audioGroup in _audioScriptableObject.AudioGroups) {
            AudioSource source = gameObject.AddComponent<AudioSource>();
            source.volume = audioGroup.volume;
            source.pitch = audioGroup.pitch;
            source.loop = audioGroup.loop;
            source.clip = audioGroup.clip;
            source.reverbZoneMix = 0;
            if (_audioDictionary.ContainsKey(audioGroup.id)) {
                Debug.LogWarning("Multiple instances of id " + audioGroup.id + " in audio scriptable object.");
            }
            _audioDictionary.Add(audioGroup.id, source);
        }
    }

    public void Play(AudioID id) {
        if(!_audioDictionary.ContainsKey(id)) {
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
