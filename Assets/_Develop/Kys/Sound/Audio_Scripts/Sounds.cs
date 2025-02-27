using UnityEngine;
using UnityEngine.Audio;

namespace InnerSight_Kys
{
    /// <summary>
    /// Individual sound properties and associated AudioSource.
    /// </summary>
    [System.Serializable]
    public class Sounds
    {
        public string name; // Name of the sound.

        public AudioClip clip; // Audio clip for the sound.

        [Range(0f, 1f)]
        public float volume = 1f; // Default volume (0.0 - 1.0).

        [Range(0.1f, 3f)]
        public float pitch = 1f; // Default pitch (0.1 - 3.0).


        public bool loop; // Whether the sound should loop.

        [HideInInspector]
        public AudioSource source; // Associated AudioSource for playback.

        /// <summary>
        /// Initializes the AudioSource for this sound.
        /// </summary>
        /// <param name="parent">Parent GameObject to attach the AudioSource.</param>
        /// <param name="outputGroup">Optional: Assign AudioMixerGroup to the AudioSource.</param>
        public void Initialize(GameObject parent, AudioMixerGroup outputGroup = null)
        {
            if (clip == null)
            {
                Debug.LogError($"Sound {name} has no AudioClip assigned!");
                return;
            }

            source = parent.AddComponent<AudioSource>();
            source.clip = clip;
            source.volume = volume;
            source.pitch = pitch;
            source.loop = loop;
            source.playOnAwake = false;

            if (outputGroup != null)
            {
                source.outputAudioMixerGroup = outputGroup;
            }
        }
    }
}
