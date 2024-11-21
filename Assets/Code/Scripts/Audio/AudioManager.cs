// /*-------------------------------------------
// ---------------------------------------------
// Creation Date: 05.11.24
// Author: melonanas1@gmail.com
// Origin Project: MadEscape
// ---------------------------------------------
// -------------------------------------------*/

using UnityEngine.Audio;
using UnityEngine;
using System;
using Audio;

namespace Audio
{
    public class AudioManager : MonoBehaviour
    {
        public Sound[] sounds;
        void Awake()
        {
            foreach (Sound s in sounds){
                s.source = gameObject.AddComponent<AudioSource>();
                s.source.clip = s.clip;
                s.source.volume = s.volume;
            }
        }

        public void Play(string soundName)
        {
            Sound s = Array.Find(sounds, sound => sound.name == soundName);
            s.source.Play();
        }
    }
}