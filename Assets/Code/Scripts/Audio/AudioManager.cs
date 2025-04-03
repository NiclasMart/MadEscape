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
    public enum Priority
    {
        Low,
        High,
        Critical
    }

    public class AudioManager : ObjectPool
    {
        [SerializeField] private AudioDataSet _audioSet;
        [SerializeField][Min(0)] private int _maxAudioChannels;

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

        public void Play(AudioActionType type, Priority prio)
        {
            // use object pool instead

            var audioSource = gameObject.AddComponent<AudioSource>();
            AudioData data = _audioSet.AudioDataList.Find(elem => elem.AudioActionType == type);
            audioSource.clip = data.Clip;
            audioSource.volume = data.volume;
            audioSource.Play();

        }
    }
}