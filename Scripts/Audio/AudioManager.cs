using System.Collections.Generic;
using UnityEngine;

public class AudioManager : Singleton<AudioManager>
{
    [Header("Audio Sources")]
    public AudioSource bgmSource; // 배경 음악 전용
    public AudioSource sfxSource; // 효과음 전용

    [Header("BGM Clips")]
    public List<AudioClip> bgmClips; // 선택 가능한 BGM 리스트
    private int currentBGMIndex = 0;

    [Header("SFX Clips")]
    public AudioClip attackClip;
    public AudioClip playerHitClip;
    public AudioClip itemPickupClip;
    public AudioClip uiClickClip;
    public AudioClip buildingClip;
    public AudioClip gatherClip;
    public AudioClip footstepClip;

    [Header("Volume Settings")]
    [Range(0f, 1f)]
    public float masterVolume;
    [Range(0f, 1f)]
    public float bgmVolume;
    [Range(0f, 1f)]
    public float sfxVolume;

    public void PlayBGM(int index)
    {
        if (index < 0 || index >= bgmClips.Count)
        {
            return;
        }

        if (bgmSource.isPlaying)
        {
            bgmSource.Stop();
        }

        bgmSource.clip = bgmClips[index];
        bgmSource.loop = true;
        bgmSource.Play();
        currentBGMIndex = index;

        UpdateVolume();
    }

    public void PlaySFX(AudioClip clip)
    {
        if (clip != null)
        {
            sfxSource.PlayOneShot(clip, sfxVolume * masterVolume);
        }
    }
    public void SetBGMVolume(float volume)
    {
        bgmVolume = Mathf.Clamp01(volume);
        UpdateVolume();
    }

    public void SetSFXVolume(float volume)
    {
        sfxVolume = Mathf.Clamp01(volume);
        // sfxSource의 볼륨은 PlaySFX에서 처리
    }

    public void SetMasterVolume(float volume)
    {
        masterVolume = Mathf.Clamp01(volume);
        UpdateVolume();
    }

    private void UpdateVolume()
    {
        bgmSource.volume = bgmVolume * masterVolume;
    }

    public int GetCurrentBGMIndex()
    {
        return currentBGMIndex;
    }

    public float GetBGMVolume()
    {
        return bgmVolume;
    }

    public float GetSFXVolume()
    {
        return sfxVolume;
    }

    public float GetMasterVolume()
    {
        return masterVolume;
    }
}
