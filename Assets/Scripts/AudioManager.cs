using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/// サウンド管理
public class AudioManager : SingletonMonoBehaviour<AudioManager>
{
    [SerializeField]
    private float volume;

    /// SEチャンネル数
    const int SE_CHANNEL = 4;

    /// サウンド種別
    enum eType
    {
        Bgm, // BGM
        Se,  // SE
    }


    // サウンド再生のためのゲームオブジェクト
    [SerializeField]
    GameObject _object = null;

    // サウンドリソース
    AudioSource _sourceBgm = null; // BGM
    AudioSource _sourceSeDefault = null; // SE (デフォルト)
    AudioSource[] _sourceSeArray; // SE (チャンネル)
   
    // BGMにアクセスするためのテーブル
    Dictionary<string, AudioData> _poolBgm = new Dictionary<string, AudioData>();
    // SEにアクセスするためのテーブル 
    Dictionary<string, AudioData> _poolSe = new Dictionary<string, AudioData>();

    /// 保持するデータ
    class AudioData
    {
        /// アクセス用のキー
        public string Key;
        /// リソース名
        public string ResName;
        /// AudioClip
        public AudioClip Clip;

        /// コンストラクタ
        public AudioData(string key, string res)
        {
            Key = key;
            ResName = "AudioData/" + res;
            // AudioClipの取得
            Clip = Resources.Load(ResName) as AudioClip;
        }
    }

    /// コンストラクタ
    public AudioManager()
    {
        // チャンネル確保
        _sourceSeArray = new AudioSource[SE_CHANNEL];
    }

    /// AudioSourceを取得する
    AudioSource _GetAudioSource(eType type, int channel = -1)
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
            for(int i = 0; i < SE_CHANNEL; i++)
            {
                _sourceSeArray[i] = _object.AddComponent<AudioSource>();
            }
        }

        if(type == eType.Bgm)
        {
            // BGM
            return _sourceBgm;
        }
        else
        {
            // SE
            if(0 <= channel && channel < SE_CHANNEL)
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

    // サウンドのロード
    // ※Resources/Soundsフォルダに配置すること
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
        if(_poolBgm.ContainsKey(key))
        {
            // すでに登録済みなのでいったん消す
            _poolBgm.Remove(key);
        }
        _poolBgm.Add(key, new AudioData(key, resName));
    }
    void _LoadSe(string key, string resName)
    {
        if(_poolSe.ContainsKey(key))
        {
            // すでに登録済みなのでいったん消す
            _poolSe.Remove(key);
        }
        _poolSe.Add(key, new AudioData(key, resName));
    }

    #endregion

    #region PlayAudio

    /// BGMの再生
    /// ※事前にLoadBgmでロードしておくこと
    public static bool PlayBgm(string key)
    {
        return Instance._PlayBgm(key);
    }
    bool _PlayBgm(string key)
    {
        if(_poolBgm.ContainsKey(key) == false)
        {
            // 対応するキーがない
            return false;
        }

        // いったん止める
        _StopBgm();

        // リソースの取得
        var _data = _poolBgm[key];

        // 再生
        var source = _GetAudioSource(eType.Bgm);
        source.loop = true;
        source.clip = _data.Clip;
        source.Play();

        return true;
    }
    /// BGMの停止
    public static bool StopBgm()
    {
        return Instance._StopBgm();
    }
    bool _StopBgm()
    {
        _GetAudioSource(eType.Bgm).Stop();

        return true;
    }

    /// SEの再生
    /// ※事前にLoadSeでロードしておくこと
    public static bool PlaySe(string key, int channel = -1)
    {
        return Instance._PlaySe(key, channel);
    }
    bool _PlaySe(string key, int channel = -1)
    {
        if(_poolSe.ContainsKey(key) == false)
        {
            // 対応するキーがない
            return false;
        }

        // リソースの取得
        var _data = _poolSe[key];

        if(0 <= channel && channel < SE_CHANNEL)
        {
            // チャンネル指定
            var source = _GetAudioSource(eType.Se, channel);
            source.clip = _data.Clip;
            source.Play();
        }
        else
        {
            // デフォルトで再生
            var source = _GetAudioSource(eType.Se);
            source.PlayOneShot(_data.Clip);
        }

        return true;
    }

    #endregion
}