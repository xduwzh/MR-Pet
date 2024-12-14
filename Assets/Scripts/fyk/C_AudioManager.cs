using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class C_AudioManager : MonoBehaviour
{
    public static C_AudioManager instance;

    [HideInInspector]
    public AudioSource audioSource;
    public AudioClip Eat;
    public AudioClip DogEat;
    public AudioClip Tool;
    public AudioClip CatBall;

    public AudioClip DogWoof;
    public AudioClip CatMeow;
    public AudioClip LizardSound;

    public AudioClip TreeGrow;

    public Dictionary<string, AudioClip> soundMap = new Dictionary<string, AudioClip>();


    public Transform[] Envs;
    private int envsIndex = 1;
    void Awake()
    {
        // 单例模式
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }

        // 添加 AudioSource 组件
        audioSource = gameObject.AddComponent<AudioSource>();
    }
    // Start is called before the first frame update
    void Start()
    {
        soundMap.Add("Eat", Eat);
        soundMap.Add("DogEat", DogEat);
        soundMap.Add("DogToy", Tool);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void playSoundOnce(string soundName)
    {
        audioSource.clip = soundMap[soundName];
        audioSource.Play();
    }

    public void PlaySound(AudioClip clip)
    {
        if (clip != null)
        {
            audioSource.PlayOneShot(clip); // 播放一次性音效
        }
    }
    public void ChangeEnv()
    {
        switch (envsIndex)
        {
            case (1): Envs[0].gameObject.SetActive(true); Envs[3].gameObject.SetActive(false); break;
            case (2): Envs[1].gameObject.SetActive(true); Envs[0].gameObject.SetActive(false); break;
            case (3): Envs[2].gameObject.SetActive(true); Envs[1].gameObject.SetActive(false); break;
            case (4): Envs[3].gameObject.SetActive(true); Envs[2].gameObject.SetActive(false); envsIndex = 0; break;
        }
        envsIndex++;
    }
}
