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
using Core;

namespace Audio
{
    public class AudioManager : MonoBehaviour
    {
        public Sound[] sounds;
        void Awake()
        {
            if (ServiceProvider.Get<AudioManager>() != null)
            {
                Destroy(gameObject);
                return;
            }

            ServiceProvider.Register(this);
            DontDestroyOnLoad(gameObject);
        }

        public void Play(string soundName)
        {
            Sound s = Array.Find(sounds, sound => sound.name == soundName);
            s.source.Play();
        }
    }
}