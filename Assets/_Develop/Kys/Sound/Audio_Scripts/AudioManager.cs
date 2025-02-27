using InnerSight_Seti;
using UnityEngine;
using UnityEngine.Audio;

namespace InnerSight_Kys
{
    public class AudioManager : Singleton<AudioManager>
    {
        [SerializeField] private AudioMixer audioMixer;
        [SerializeField] private AudioMixerGroup audioMixerGroupBgm;
        [SerializeField] private AudioMixerGroup audioMixerGroupSfx;
        public Sounds[] sounds;

        private string bgmSound = ""; // Currently playing BGM
        public string BgmSound => bgmSound;

        protected override void Awake()
        {
            base.Awake();

            if (audioMixer == null)
            {
                return;
            }

            foreach (var sound in sounds)
            {
                // Initialize the AudioSource for each sound
                var outputGroup = sound.loop ? audioMixerGroupBgm : audioMixerGroupSfx;
                sound.Initialize(this.gameObject, outputGroup);
            }
        }

        public void Play(string name)
        {
            Sounds sound = System.Array.Find(sounds, s => s.name == name);
            if (sound == null)
            {
                return;
            }
            sound.source.Play();
        }

        public void Stop(string name)
        {
            Sounds sound = System.Array.Find(sounds, s => s.name == name);
            if (sound == null)
            {
                return;
            }
            sound.source.Stop();
            if (name == bgmSound)
            {
                bgmSound = "";
            }
        }

        public void PlayBgm(string name)
        {
            if (bgmSound == name) return;

            StopBgm();

            Sounds sound = System.Array.Find(sounds, s => s.name == name);
            if (sound == null)
            {
                return;
            }

            bgmSound = name;
            sound.source.Play();
        }

        public void StopBgm()
        {
            Stop(bgmSound);
        }
    }
}
