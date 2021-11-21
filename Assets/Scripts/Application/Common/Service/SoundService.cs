using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SoundService : SingletonGameObject<SoundService> {
    private Dictionary<string, FxClip> clips = new Dictionary<string, FxClip>();
    private AudioSource effectSource;
    private AudioSource bgmSource;
    private AudioListener listener;
    private float effectVolume = 0.5f;
    private float bgmVolume = 0.5f;
    private bool bgmVolumUpdate;
    private float bgmVolumnFrom;
    private float bgmVolumnTo;
    private float bgmVolumnProgress;
    private float bgmVolumnDuration = 0.3f;
    
    public string currentBgm { get; private set; }
    public bool isBgmPlaying => bgmSource.isPlaying;

    void Awake() {
        DontDestroyOnLoad(gameObject);
        effectSource = gameObject.AddComponent<AudioSource>();
        bgmSource = gameObject.AddComponent<AudioSource>();
        listener = gameObject.AddComponent<AudioListener>();
    }

    void Update() {
        if (bgmVolumUpdate) {
            bgmVolumnProgress += Time.deltaTime;
            bgmVolumnProgress = Mathf.Min(bgmVolumnProgress, bgmVolumnDuration);
            float progress = bgmVolumnProgress / bgmVolumnDuration;
            if (bgmSource != null) {
                bgmSource.volume = Mathf.Lerp(bgmVolumnFrom, bgmVolumnTo, progress);
            }

            if (progress >= 1f) {
                bgmVolumnProgress = 0;
                bgmVolumUpdate = false;
            }
        }
    }

    public void PlayBGM(string path, float pitch, float volume = 0.5f) {
        AudioClip clip = Resources.Load<AudioClip>("Sound/" + path);
        bgmSource.clip = clip;
        currentBgm = path;
        
        bgmSource.pitch = pitch;
        bgmSource.loop = true;
        bgmSource.volume = volume;
        bgmSource.Play();
    }

    public void SetBgmPitch(float pitch) {
        bgmSource.pitch = pitch;
        bgmSource.Play();
    }

    public void StopBGM() {
        currentBgm = "";
        bgmSource.Stop();
    }

    public async void PlayEffect(string name, float delay, float volume = 0.5f) {
        if (delay > 0) {
            await new WaitForSeconds(delay);
        }
        
        PlayEffect(name, false, volume);
    }

    public AudioClip PlayEffect(string name, bool loop = false, float volume = 0.5f) {
        if (Service.setting.value == null) {
            return null;
        }
        
        if (Service.setting.value.effectSound == false) {
            return null;
        }

        FxClip res = null;
        if (clips.ContainsKey(name)) {
            res = clips[name];
        } else {
            res = new FxClip();
            res.clip = Resources.Load<AudioClip>("Sound/" + name);
            if (res.clip == null) {
                Debug.LogError("can not load effect sound:" + name);
                return null;
            }
            
            clips.Add(name, res);
        }

        var now = Time.realtimeSinceStartup;
        if (now - res.fired < res.length * 0.3f) {
            return res.clip;
        }

        res.fired = now;
        effectSource.clip = res.clip;
        effectSource.volume = volume;

        if (loop) {
            effectSource.loop = true;
            effectSource.Play();
        } else {
            effectSource.loop = false;
            effectSource.PlayOneShot(res.clip, effectVolume);
        }

        return res.clip;
    }

    public void StopEffect() {
        effectSource.Stop();
    }

    public AudioClip PlayEffectAndMuteBGM(string name, float lengthSaler) {
        AudioClip clip = PlayEffect(name);
        if (clip != null) {
            StartCoroutine(EaseMuteBGMRoutine(clip.length * lengthSaler));
        }

        return clip;
    }

    public void EaseMuteBGM() {
        bgmSource.loop = false;
        StartCoroutine(EaseMuteRoutine(10.0f));
        
    }

    private IEnumerator EaseMuteRoutine(float time) {
        yield return this.Ease(EaseType.linear, time, (v) => {
            bgmSource.volume = Mathf.Lerp(bgmSource.volume, 0f, v);
        });
        bgmSource.volume = 0;
    }

    private IEnumerator EaseMuteBGMRoutine(float length) {
        bgmVolumnFrom = bgmSource.volume;
        bgmVolumnTo = 0.1f;
        bgmVolumUpdate = true;

        float duration = length - bgmVolumnDuration;
        duration = Mathf.Max(0.1f, duration);
        yield return new WaitForSeconds(duration);

        bgmVolumnFrom = bgmSource.volume;
        bgmVolumnTo = 1f;
        bgmVolumUpdate = true;
    }
}
