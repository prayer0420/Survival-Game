using System.Collections.Generic;
using UnityEngine;

public class AudioManager : Singleton<AudioManager>
{
    [Header("Audio Sources")]
    public AudioSource bgmSource; // ��� ���� ����
    public AudioSource sfxSource; // ȿ���� ����

    [Header("BGM Clips")]
    public List<AudioClip> bgmClips; // ���� ������ BGM ����Ʈ
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
        // sfxSource�� ������ PlaySFX���� ó��
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
