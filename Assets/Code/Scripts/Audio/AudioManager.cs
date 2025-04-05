// /*-------------------------------------------
// ---------------------------------------------
// Creation Date: 05.11.24
// Author: melonanas1@gmail.com
// Origin Project: MadEscape
// ---------------------------------------------
// -------------------------------------------*/

using UnityEngine;
using Core;
using UnityEngine.Audio;

namespace Audio
{
    public class AudioManager : ObjectPool
    {
        [SerializeField] private AudioDataSet _audioSet;
        [SerializeField] private AudioMixer _audioMixer;
        [SerializeField, Range(0, 1)] private float _highPriorityBufferSize = 0.25f; // this amount determines how much of the object pool is blocked for high priority audio

        void Awake()
        {
            ServiceProvider.Register(this, gameObject);
            base.Initialize();
        }

        public void Play(AudioActionType type, bool highPriority = false)
        {
            AudioChannel newAudioChannel;

            // check for free capacity according to priority
            if (CurrentCapacity >= MaxCapacity - MaxCapacity * _highPriorityBufferSize && !highPriority) return;

            // get audio source
            if (!TryGetObject(out GameObject pooledObject)) return;
            pooledObject.SetActive(true);
            newAudioChannel = pooledObject.GetComponent<AudioChannel>();

            AudioData data = _audioSet.AudioDataList.Find(elem => elem.AudioActionType == type);
            newAudioChannel.Play(data);
        }
    }
}