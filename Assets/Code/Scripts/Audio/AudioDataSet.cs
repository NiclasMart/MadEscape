// /*-------------------------------------------
// ---------------------------------------------
// Creation Date: 03.04.25
// Author: salcintram07@web.de
// Origin Project: MadEscape
// ---------------------------------------------
// -------------------------------------------*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

namespace Audio
{
    [System.Serializable]
    public class AudioData
    {
        public AudioActionType AudioActionType = AudioActionType.None;
        public AudioClip Clip;
        public AudioMixerGroup MixerGroup;
        [Range(0f, 1f)] public float volume = 1f;
    }

    [CreateAssetMenu(fileName = "AudioDataSet", menuName = "Scriptable Objects/AudioDataSet", order = 0)]
    public class AudioDataSet : ScriptableObject
    {
        public List<AudioData> AudioDataList = new();
    }
}