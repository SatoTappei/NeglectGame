using DG.Tweening;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

/// <summary>
/// ゲーム中の音を管理するコンポーネント
/// </summary>
public class AudioManager : MonoBehaviour
{
    /// <summary>
    /// 再生するサウンドのデータ
    /// </summary>
    [Serializable]
    class AudioData
    {
        [SerializeField] string _key;
        [SerializeField] AudioClip _clip;
        [SerializeField] float _volume;

        public string Key => _key;
        public AudioClip Clip => _clip;
        public float Volume => _volume;
    }

    static readonly int AudioSourceLength = 10;

    static AudioManager _instance;

    [Header("再生する音のデータ")]
    [SerializeField] AudioData[] _soundDatas;
    [Header("連続で再生出来る間隔")]
    [SerializeField] float _interval;

    float _lastTime;
    AudioSource[] _audioSources = new AudioSource[AudioSourceLength];
    Dictionary<string, AudioData> _soundDic;

    public static AudioManager Instance => _instance;

    void Awake()
    {
        if (_instance == null)
        {
            _instance = this;
        }
        else
        {
            Destroy(this);
        }

        Init();
    }

    void Init()
    {
        _soundDic = new(_soundDatas.Length);
        _soundDic = _soundDatas.ToDictionary(s => s.Key, s => s);

        for (int i = 0; i < _audioSources.Length; i++)
        {
            _audioSources[i] = gameObject.AddComponent<AudioSource>();
        }
    }

    public void PlaySE(string key)
    {
        if (!AtInterval()) return;

        AudioData audioData = GetAudio(key);
        if (audioData == null)
        {
            Debug.LogWarning("音が登録されていません:" + key);
            return;
        }

        AudioSource audioSource = GetAudioSource();
        if(audioSource == null)
        {
            Debug.LogWarning("音を鳴らせませんでした。AudioSourceが足りません");
            return;
        }

        audioSource.clip = audioData.Clip;
        audioSource.volume = audioData.Volume;
        audioSource.Play();
    }

    public void PlayBGM(string key)
    {
        if (!AtInterval()) return;

        AudioData audioData = GetAudio(key);
        if (audioData == null)
        {
            Debug.LogWarning("音が登録されていません:" + key);
            return;
        }

        AudioSource audiSource = GetBGMAudioSource();
        audiSource.clip = audioData.Clip;
        audiSource.volume = audioData.Volume;
        audiSource.loop = true;
        audiSource.Play();
    }

    AudioData GetAudio(string key)
    {
        if(_soundDic.TryGetValue(key, out AudioData data))
        {
            return data;
        }
        else
        {
            return null;
        }
    }

    bool AtInterval()
    {
        bool isOk = Time.realtimeSinceStartup - _lastTime > _interval;
        if (isOk)
        {
            _lastTime = Time.realtimeSinceStartup;
            return true;
        }
        else
        {
            return false;
        }
    }

    AudioSource GetAudioSource()
    {
        // 一番後ろのAudioSourceはBGM再生用に取っておく
        for (int i = 0; i < _audioSources.Length - 1; i++)
        {
            if (!_audioSources[i].isPlaying)
            {
                return _audioSources[i];
            }
        }
        return null;
    }

    AudioSource GetBGMAudioSource() => _audioSources[_audioSources.Length - 1];

    /// <summary>再生中の効果音を全て止める</summary>
    public void StopSEAll()
    {
        foreach (AudioSource source in _audioSources)
        {
            if (source.isPlaying)
            {
                source.Stop();
            }       
        }
    }

    public void StopBGM() => GetBGMAudioSource().Stop();
    public void FadeOutBGM(float duration = 0.5f) => GetBGMAudioSource().DOFade(0, duration);

    void OnDestroy()
    {
        _instance = null;
    }
}