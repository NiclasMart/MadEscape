// /*-------------------------------------------
// ---------------------------------------------
// Creation Date: 05.11.24
// Author: melonanas1@gmail.com
// Origin Project: MadEscape
// ---------------------------------------------
// -------------------------------------------*/

using UnityEngine.Audio;
using UnityEngine;

namespace Audio
{   
    [System.Serializable]
    public class Sound
    {   
        public string name;
        public AudioClip clip;
        [Range(0f, 1f)]
        public float volume;
        [HideInInspector]
        public AudioSource source;
    }
}