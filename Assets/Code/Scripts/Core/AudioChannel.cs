// /*-------------------------------------------
// ---------------------------------------------
// Creation Date: 03.04.25
// Author: salcintram07@web.de
// Origin Project: MadEscape
// ---------------------------------------------
// -------------------------------------------*/

using System;
using System.Collections;
using System.Collections.Generic;
using Audio;
using Generation;
using UnityEngine;

namespace Core
{
    [RequireComponent(typeof(AudioSource))]
    public class AudioChannel : MonoBehaviour, IPoolable
    {
        private AudioSource _source;
        private Coroutine _playCoroutine;
        public event Action<IPoolable> OnDestroy;

        void Awake()
        {
            _source = gameObject.GetComponent<AudioSource>();
        }

        public void Play(AudioData data)
        {
            _source.clip = data.Clip;
            _source.volume = data.volume;
            _source.outputAudioMixerGroup = data.MixerGroup;

            _playCoroutine = StartCoroutine(PlayingAudioClip(data.Clip.length));
        }

        public void Stop()
        {
            OnDestroy?.Invoke(this);
        }

        public void Reset()
        {
            StopCoroutine(_playCoroutine);

            _source.Stop();
            _source.time = 0;

        }

        IEnumerator PlayingAudioClip(float time)
        {
            _source.Play();
            yield return new WaitForSeconds(time);
            OnDestroy?.Invoke(this);
        }

        public GameObject GetAttachedGameobject()
        {
            return gameObject;
        }
    }
}