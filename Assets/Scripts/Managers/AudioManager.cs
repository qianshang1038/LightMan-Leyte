using System.Collections;
using UnityEngine;
using UnityEngine.Rendering;

public class AudioManager : Singleton<AudioManager>
{
    private const int MAX_AUDIO_CHANNEL_NUM = 6; // ������Ƶͨ������

    // �Զ�����Ƶͨ������¼��һ����Ƶ �Լ��ϴβ���ʱ��
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
        // ��ʼ��������Ƶͨ����ͬʱ���������Դ������
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

    // ����������Ч��Ƭ�Σ�����������ƽ�⣬����
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
        return -1; // û���ҵ�����Ƶ��
    }

    // ���ڿռ�λ�ò���
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

    // �ڿռ�λ�ò���(������)
    public Vector3 GetPlayerPos()
    {
        return Player.Instance.transform.position;
    }



    // ѭ�����ű�������
    public void PlayBackgroundMusic(AudioClip clip)
    {
        musicAudioSource.clip = clip;
        musicAudioSource.volume = baseMusicVolume * musicVolumeMultiplier;
        musicAudioSource.loop = true;
        musicAudioSource.Play();
    }

    // ֹͣ������Ч
    public void StopAllSoundEffects()
    {
        foreach (var channel in soundChannels)
        {
            channel.audioSource.Stop();
        }
    }

    // ������������
    public void SetMusicVolume(float volume)
    {
        musicVolumeMultiplier = Mathf.Clamp(volume, 0f, 1f);
        musicAudioSource.volume = musicVolumeMultiplier * baseMusicVolume;
        PlayerPrefs.SetFloat(PLAYER_PREFS_MUSIC_VOLUME, musicVolumeMultiplier);
    }

    // ������Ч����
    public void SetSoundEffectsVolume(float volume)
    {
        soundVolumeMultiplier = Mathf.Clamp(volume, 0f, 1f);
        foreach (var channel in soundChannels)
        {
            channel.audioSource.volume = soundVolumeMultiplier;
        }
        PlayerPrefs.SetFloat(PLAYER_PREFS_SOUND_EFFECTS_VOLUME, soundVolumeMultiplier);
    }

    // ���뵭���л�����
    public void ChangeMusic(AudioClip newClip, float fadeDuration = 1.0f)
    {
        StartCoroutine(ChangeMusicWithDelay(newClip, fadeDuration));
    }

    private IEnumerator ChangeMusicWithDelay(AudioClip newClip, float fadeDuration)
    {
        float startVolume = musicAudioSource.volume;

        // ������ǰ����
        for (float t = 0; t < fadeDuration; t += Time.deltaTime)
        {
            musicAudioSource.volume = Mathf.Lerp(startVolume, 0, t / fadeDuration);
            yield return null;
        }
        musicAudioSource.Stop();

        // �л�����
        musicAudioSource.clip = newClip;
        musicAudioSource.Play();

        // ����������
        for (float t = 0; t < fadeDuration; t += Time.deltaTime)
        {
            musicAudioSource.volume = Mathf.Lerp(0, startVolume, t / fadeDuration);
            yield return null;
        }
    }

    // ������������Ч����
    private void OnDestroy()
    {
        PlayerPrefs.SetFloat(PLAYER_PREFS_SOUND_EFFECTS_VOLUME, soundVolumeMultiplier);
        PlayerPrefs.SetFloat(PLAYER_PREFS_MUSIC_VOLUME, musicVolumeMultiplier);
    }

    // ÿһ��Ĭ�Ͽ�������
    protected override bool ShouldDestroyOnLoad() => true;
}
