using System.Collections;
using UnityEngine;
using UnityEngine.Rendering;

public class AudioManager : Singleton<AudioManager>
{
    private const int MAX_AUDIO_CHANNEL_NUM = 6; // 增加音频通道数量

    // 自定义音频通道，记录了一段音频 以及上次播放时间
    private class AudioChannel
    {
        public AudioSource audioSource;
        public float lastPlayTime;
    }

    private AudioChannel[] soundChannels;
    private AudioSource musicAudioSource;

    private float baseMusicVolume = 1.0f;
    private float musicVolumeMultiplier = 1.0f;
    private float soundVolumeMultiplier = 1.0f;

    private const string PLAYER_PREFS_SOUND_EFFECTS_VOLUME = "SoundEffectsVolume";
    private const string PLAYER_PREFS_MUSIC_VOLUME = "MusicVolume";

    private void Awake()
    {
        // 初始化所有音频通道，同时加载相关资源和设置
        InitializeAudioChannels();
        InitializeMusicSource();
        LoadVolumeSettings();
    }

    private void InitializeAudioChannels()
    {
        soundChannels = new AudioChannel[MAX_AUDIO_CHANNEL_NUM];
        for (int i = 0; i < MAX_AUDIO_CHANNEL_NUM; i++)
        {
            soundChannels[i] = new AudioChannel
            {
                audioSource = gameObject.AddComponent<AudioSource>(),
                lastPlayTime = 0
            };
        }
    }

    private void InitializeMusicSource()
    {
        musicAudioSource = gameObject.AddComponent<AudioSource>();
        baseMusicVolume = musicAudioSource.volume;
    }

    private void LoadVolumeSettings()
    {
        soundVolumeMultiplier = PlayerPrefs.GetFloat(PLAYER_PREFS_SOUND_EFFECTS_VOLUME, 1.0f);
        musicVolumeMultiplier = PlayerPrefs.GetFloat(PLAYER_PREFS_MUSIC_VOLUME, 1.0f);
        SetMusicVolume(musicVolumeMultiplier);
    }

    // 播放所有音效：片段，音量，声道平衡，声调
    public int PlayAllSound(AudioClip clip, float volumeControl, float pan = 0.0f, float pitch = 1.0f)
    {
        for (int i = 0; i < soundChannels.Length; i++)
        {
            if (!soundChannels[i].audioSource.isPlaying)
            {
                soundChannels[i].audioSource.clip = clip;
                soundChannels[i].audioSource.volume = volumeControl * soundVolumeMultiplier;
                soundChannels[i].audioSource.pitch = pitch;
                soundChannels[i].audioSource.panStereo = pan;
                soundChannels[i].audioSource.Play();
                soundChannels[i].lastPlayTime = Time.time;
                return i;
            }
        }
        return -1; // 没有找到可用频道
    }

    // 不在空间位置播放
    public void PlaySound(AudioClip[] audioClipArray, float volumeControl = 1.0f)
    {
        if (audioClipArray.Length == 0)
            return;

        AudioClip thisClip = audioClipArray[Random.Range(0, audioClipArray.Length)];
        musicAudioSource.PlayOneShot(thisClip, volumeControl * soundVolumeMultiplier);
    }

    public void PlaySound(AudioClip audioClip, float volumeControl = 1.0f)
    {
        musicAudioSource.PlayOneShot(audioClip, volumeControl * soundVolumeMultiplier);
    }

    // 在空间位置播放(待完善)
    public Vector3 GetPlayerPos()
    {
        return Player.Instance.transform.position;
    }



    // 循环播放背景音乐
    public void PlayBackgroundMusic(AudioClip clip)
    {
        musicAudioSource.clip = clip;
        musicAudioSource.volume = baseMusicVolume * musicVolumeMultiplier;
        musicAudioSource.loop = true;
        musicAudioSource.Play();
    }

    // 停止所有音效
    public void StopAllSoundEffects()
    {
        foreach (var channel in soundChannels)
        {
            channel.audioSource.Stop();
        }
    }

    // 设置音乐音量
    public void SetMusicVolume(float volume)
    {
        musicVolumeMultiplier = Mathf.Clamp(volume, 0f, 1f);
        musicAudioSource.volume = musicVolumeMultiplier * baseMusicVolume;
        PlayerPrefs.SetFloat(PLAYER_PREFS_MUSIC_VOLUME, musicVolumeMultiplier);
    }

    // 设置音效音量
    public void SetSoundEffectsVolume(float volume)
    {
        soundVolumeMultiplier = Mathf.Clamp(volume, 0f, 1f);
        foreach (var channel in soundChannels)
        {
            channel.audioSource.volume = soundVolumeMultiplier;
        }
        PlayerPrefs.SetFloat(PLAYER_PREFS_SOUND_EFFECTS_VOLUME, soundVolumeMultiplier);
    }

    // 淡入淡出切换音乐
    public void ChangeMusic(AudioClip newClip, float fadeDuration = 1.0f)
    {
        StartCoroutine(ChangeMusicWithDelay(newClip, fadeDuration));
    }

    private IEnumerator ChangeMusicWithDelay(AudioClip newClip, float fadeDuration)
    {
        float startVolume = musicAudioSource.volume;

        // 淡出当前音乐
        for (float t = 0; t < fadeDuration; t += Time.deltaTime)
        {
            musicAudioSource.volume = Mathf.Lerp(startVolume, 0, t / fadeDuration);
            yield return null;
        }
        musicAudioSource.Stop();

        // 切换音乐
        musicAudioSource.clip = newClip;
        musicAudioSource.Play();

        // 淡入新音乐
        for (float t = 0; t < fadeDuration; t += Time.deltaTime)
        {
            musicAudioSource.volume = Mathf.Lerp(0, startVolume, t / fadeDuration);
            yield return null;
        }
    }

    // 保存音乐与音效设置
    private void OnDestroy()
    {
        PlayerPrefs.SetFloat(PLAYER_PREFS_SOUND_EFFECTS_VOLUME, soundVolumeMultiplier);
        PlayerPrefs.SetFloat(PLAYER_PREFS_MUSIC_VOLUME, musicVolumeMultiplier);
    }

    // 每一关默认可以销毁
    protected override bool ShouldDestroyOnLoad() => true;
}
