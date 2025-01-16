using UnityEngine;

namespace Seti
{
    /// <summary>
    /// 임의의 AudioClip을 재생
    /// </summary>
    [RequireComponent(typeof(AudioSource))]
    public class RandomAudioPlayer : MonoBehaviour
    {
        [SerializeField]
        public class SoundBank
        {
            public string name;
            public AudioClip[] clips;
        }

        // 필드
        #region Variables
        // 재생속도 랜덤
        public bool randomizePitch = true;
        public float pitchRandomRange = 0.2f;

        public float playDelay = 0;
        public SoundBank defaultBank = new();

        protected AudioSource m_AudioSource;
#pragma warning disable IDE1006 // 명명 스타일
        public AudioSource audioSource { get { return m_AudioSource; } }
#pragma warning restore IDE1006 // 명명 스타일
        public AudioClip Clip { get; private set; }
        #endregion

        // 라이프 사이클
        #region Life Cycle
        private void Awake()
        {
            // 참조
            m_AudioSource = GetComponent<AudioSource>();
        }
        #endregion

        // 메서드
        #region Methods
        public AudioClip PlayRandomClip(int bankID = 0)
        {
            return InternalPlayRandomClip(bankID);
        }

        private AudioClip InternalPlayRandomClip(int bankID)
        {
            var bank = defaultBank;
            if (bank.clips == null || bank.clips.Length == 0)
                return null;

            // 클립 중 하나를 임의로 선택
            var clip = bank.clips[Random.Range(0, bank.clips.Length)];
            if (clip == null)
                return null;

            m_AudioSource.pitch = randomizePitch ? Random.Range(1.0f - pitchRandomRange, 1.0f + pitchRandomRange) : 1.0f;
            m_AudioSource.clip = clip;
            m_AudioSource.PlayDelayed(playDelay);

            return clip;
        }
        #endregion
    }
}