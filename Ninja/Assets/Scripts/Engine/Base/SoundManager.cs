using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SoundManager : SingletonMonoBehavior<SoundManager> {

    private AudioSource soundSource;
    private AudioSource bgSource;
    private Dictionary<string, float> playRecord = new Dictionary<string, float>();
    private float playCD = 0.1f;

	// Use this for initialization
	void Start () {
        GameObject bgGo = new GameObject();
        bgGo.name = "musicBg";
        bgGo.transform.SetParent(transform, false);
        bgSource = bgGo.AddMissingComponent<AudioSource>();
        bgSource.loop = true;
        DontDestroyOnLoad(bgGo);

        GameObject soundGo = new GameObject();
        soundGo.name = "soundGo";
        soundGo.transform.SetParent(transform, false);
        soundSource = soundGo.AddMissingComponent<AudioSource>();
        DontDestroyOnLoad(soundGo);
	}

    public void PlayMusic(string _name) {
        AudioClip clip = LocalAssetMgr.Instance.Load_Music(_name);
        if (clip == null)
            return;
        bgSource.clip = clip;
        bgSource.Play();
    }

    public void PlaySound(string _name) {
        if (playRecord.ContainsKey(_name)) {
            if (Time.time - playRecord[_name] < playCD)
                return;
            else
                playRecord[_name] = Time.time;
        }
        else {
            playRecord.Add(_name, Time.time);
        }

        AudioClip clip = LocalAssetMgr.Instance.Load_Music(_name);
        if (clip == null)
            return;
        soundSource.PlayOneShot(clip);
    }
}
