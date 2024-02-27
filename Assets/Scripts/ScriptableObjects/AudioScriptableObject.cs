using System.Collections.Generic;
using UnityEngine;

/**
 * Responsible for storing audio groups
 * These groups are used for creating audio sources in AudioManager
 */
[CreateAssetMenu(fileName = "Audio Groups", menuName = "AudioScriptableObject", order = 2)]
public class AudioScriptableObject : ScriptableObject
{
    public List<AudioManager.AudioGroup> AudioGroups = new List<AudioManager.AudioGroup>();
}
