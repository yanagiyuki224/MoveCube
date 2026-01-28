using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Audio;
using System.Linq;
public enum SEType
{
    Acquisition,
    Damage,
}
public enum BGMType
{
    Playing,
    Start,
}
public class SoundManager : SingletonMonoBehaviour<SoundManager>
{
    public Dictionary<SEType, AudioClip> seClipMap = new Dictionary<SEType, AudioClip>();
    public Dictionary<BGMType, AudioClip> bgmClipMap = new Dictionary<BGMType, AudioClip>();
    public AudioSource seAudioSource;
    public AudioSource bgmAudioSource;
    public AudioMixer bgmAudioMixer;
    private const string SePath = "_SE/";
    private const string BgmPath = "_BGM/";
    public float seVolume = 1;
    public float bgmVolume = 1;
    protected override void Awake()
    {
        base.Awake();
        if (seAudioSource == null)
        {
            seAudioSource = gameObject.AddComponent<AudioSource>();
        }
        if (bgmAudioSource == null)
        {
            bgmAudioSource = gameObject.AddComponent<AudioSource>();
            bgmAudioSource.loop = true;
        }
        if (bgmAudioMixer == null)
        {
            bgmAudioMixer = Resources.Load<AudioMixer>("_BGM/BGMMixer");
        }
        int count = System.Enum.GetValues(typeof(SEType)).Length;
        for (int i = 0; i < count; i++)
        {
            SEType type = (SEType)i;
            string clipName = type.ToString(); // enum名そのまま

            AudioClip clip = Resources.Load<AudioClip>(SePath + clipName);
            seClipMap[type] = clip;

#if UNITY_EDITOR || DEVELOPMENT_BUILD
            if (clip == null)
            {
                Debug.LogWarning($"SE not found: {SePath}{clipName}");
            }
#endif
        }
        count = System.Enum.GetValues(typeof(BGMType)).Length;
        for (int i = 0; i < count; i++)
        {
            BGMType type = (BGMType)i;
            string clipName = type.ToString(); // enum名そのまま
            AudioClip clip = Resources.Load<AudioClip>(BgmPath + clipName);
            bgmClipMap[type] = clip;
#if UNITY_EDITOR || DEVELOPMENT_BUILD
            if (clip == null)
            {
                Debug.LogWarning($"BGM not found: {BgmPath}{clipName}");
            }
#endif
        }

    }
    public void PlaySE(SEType type)
    {
        if (!seClipMap.ContainsKey(type))
            return;

        AudioClip clip = seClipMap[type];
        seAudioSource.PlayOneShot(clip);
    }
    public void PlayBGM(BGMType type)
    {
        if (!bgmClipMap.ContainsKey(type)) return;

        AudioClip clip = bgmClipMap[type];
        if (bgmAudioSource.clip == clip && bgmAudioSource.isPlaying) return;

        // --- ここがポイント：enum名でミキサーグループを検索 ---
        string groupName = type.ToString();
        AudioMixerGroup[] groups = bgmAudioMixer.FindMatchingGroups(groupName);

        if (groups.Length > 0)
        {
            bgmAudioSource.outputAudioMixerGroup = groups[0];
        }
        else
        {
            Debug.LogWarning($"AudioMixerGroup not found: {groupName}");
            return; // ミキサーグループが見つからない場合は何もしない
        }
        // --------------------------------------------------

        bgmAudioSource.clip = clip;
        bgmAudioSource.Play();
    }
    public void UpdateVolume()
    {
        bgmAudioSource.volume = bgmVolume;
        seAudioSource.volume = seVolume;
    }
}
