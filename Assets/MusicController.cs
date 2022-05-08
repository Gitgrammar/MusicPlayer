using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;

public class MusicController : MonoBehaviour
{
    //アタッチされたAudioSource をキャッシュするための変数
    AudioSource myAudio;

    //スライダーの値をもとにボリュームを調整する
    public Slider volSlider;
    //選択項目をもとに読み込むファイルを決定する
    public Dropdown musicSelect;
    //番号とファイルURLのdictinonary
    Dictionary<int, string> dict = new Dictionary<int, string>();
    void Start()
    {
        dict.Add(0, "https://joytas.net/php/futta-fly3t.wav");
        dict.Add(1, "https://joytas.net/php/futta-rainbow3t.wav");
        dict.Add(2, "https://joytas.net/php/futta-snowman3t.wav");

        myAudio = GetComponent<AudioSource>();
        myAudio.volume = volSlider.value;
        //1曲目をセット
        StartCoroutine(GetAudioClip(dict[0]));
    }
    IEnumerator GetAudioClip(string musicUrl)
    {
        //音楽ファイルURLをもとにUWRインスタンスを生成（今回ファイルタイプはWAV）
        using(var uwr = UnityWebRequestMultimedia.GetAudioClip(musicUrl, AudioType.WAV))
        {
            yield return uwr.SendWebRequest();
            if (uwr.isNetworkError || uwr.isHttpError)
            {
                Debug.LogError(uwr.error);
                yield break;

            }
            //無事音楽ファイルが取得できたらオーディオソースにセット
            myAudio.clip = DownloadHandlerAudioClip.GetContent(uwr);

        }
    }
    public void play()
    {
        myAudio.Play();
    }
    public void pause()
    {
        myAudio.Pause();
    }
    public void stop()
    {
        myAudio.Stop();
    }
    public void OnVolChange()
    {
        myAudio.volume = volSlider.value;
    }
    public void ChangeMusic()
    {
        //ドロップダウンの情報を引数にこルーチン
        StartCoroutine(GetAudioClip(dict[musicSelect.value]));
    }
}
