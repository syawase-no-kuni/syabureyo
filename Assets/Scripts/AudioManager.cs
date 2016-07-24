using UnityEngine;
using System.Collections.Generic;

public class AudioManager : SingletonMonoBehaviour<AudioManager>
{
    [SerializeField]
    [Range(0, 1)]
    private float volume = 0.5f;

    static readonly int SeChanels = 4;

    enum AudioType
    {
        Bgm,
        Se,
    }

    GameObject _object = null;

    AudioSource _sourceBgm = null;
    AudioSource _sourceSeDefault = null;
    AudioSource[] _sourceSeArray;
   
    Dictionary<string, AudioData> _poolBgm = new Dictionary<string, AudioData>();
    Dictionary<string, AudioData> _poolSe = new Dictionary<string, AudioData>();

    class AudioData
    {
        public string Key;
        public string ResName;
        public AudioClip Clip;

        public AudioData(string key, string res)
        {
            Key = key;
            ResName = "AudioData/" + res;
            Clip = Resources.Load(ResName) as AudioClip;
        }
    }

    public AudioManager()
    {
        _sourceSeArray = new AudioSource[SeChanels];
    }

    AudioSource _GetAudioSource(AudioType type, int channel = -1)
    {
        if(_object == null)
        {
            // GameObjectがなければ作る
            _object = new GameObject("AudioSources");
            _object.transform.SetParent(transform);
            
            // 破棄しないようにする
            DontDestroyOnLoad(_object);
            
            // AudioSourceを作成
            _sourceBgm = _object.AddComponent<AudioSource>();
            _sourceSeDefault = _object.AddComponent<AudioSource>();
            for(int i = 0; i < SeChanels; i++)
            {
                _sourceSeArray[i] = _object.AddComponent<AudioSource>();
            }
        }

        if(type == AudioType.Bgm)
        {
            // BGM
            return _sourceBgm;
        }
        else
        {
            // SE
            if(0 <= channel && channel < SeChanels)
            {
                // チャンネル指定
                return _sourceSeArray[channel];
            }
            else
            {
                // デフォルト
                return _sourceSeDefault;
            }
        }
    }

    #region LoadAudio

    public static void LoadBgm(string key, string resName)
    {
        Instance._LoadBgm(key, resName);
    }
    public static void LoadSe(string key, string resName)
    {
        Instance._LoadSe(key, resName);
    }
    void _LoadBgm(string key, string resName)
    {
        // すでに登録済みだった場合、上書きする
        if(_poolBgm.ContainsKey(key))
        {
            _poolBgm.Remove(key);
        }
        _poolBgm.Add(key, new AudioData(key, resName));
    }
    void _LoadSe(string key, string resName)
    {
        // すでに登録済みだった場合、上書きする
        if(_poolSe.ContainsKey(key))
        {
            _poolSe.Remove(key);
        }
        _poolSe.Add(key, new AudioData(key, resName));
    }

    #endregion

    #region PlayAudio

    public static bool PlayBgm(string key)
    {
        return Instance._PlayBgm(key);
    }
    bool _PlayBgm(string key)
    {
        if(_poolBgm.ContainsKey(key) == false)
        {
            Debug.LogWarning("PlayBgm key=" + key + "は読み込まれていません！");
            return false;
        }

        // いったん止める
        _StopBgm();

        // リソースの取得
        var _data = _poolBgm[key];

        // 再生
        var source = _GetAudioSource(AudioType.Bgm);
        source.loop = true;
        source.clip = _data.Clip;
        source.volume = volume;
        source.Play();

        return true;
    }
    public static bool StopBgm()
    {
        return Instance._StopBgm();
    }
    bool _StopBgm()
    {
        _GetAudioSource(AudioType.Bgm).Stop();
        return true;
    }

    public static bool PlaySe(string key, int channel = -1)
    {
        return Instance._PlaySe(key, channel);
    }
    bool _PlaySe(string key, int channel = -1)
    {
        if(_poolSe.ContainsKey(key) == false)
        {
            Debug.LogWarning("PlaySe key=" + key + "は読み込まれていません！");
            return false;
        }

        // リソースの取得
        var _data = _poolSe[key];

        if(0 <= channel && channel < SeChanels)
        {
            // チャンネル指定
            var source = _GetAudioSource(AudioType.Se, channel);
            source.clip = _data.Clip;
            source.volume = volume;
            source.Play();
        }
        else
        {
            // デフォルトで再生
            var source = _GetAudioSource(AudioType.Se);
            source.PlayOneShot(_data.Clip);
            source.volume = volume;
        }

        return true;
    }

    #endregion
}