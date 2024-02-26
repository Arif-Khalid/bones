using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Audio Groups", menuName = "AudioScriptableObject", order = 2)]
public class AudioScriptableObject : ScriptableObject
{
    public List<AudioManager.AudioGroup> AudioGroups = new List<AudioManager.AudioGroup>();
}
