using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class BeginUIBtn : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    // Start is called before the first frame update
    public AudioClip Sound;
    public GameObject Hover;

    /// <summary>
    /// 鼠标悬停时的回调函数
    /// </summary>
    /// <param name="eventData">事件数据</param>
    public void OnPointerEnter(PointerEventData eventData)
    {
        // 当前按钮的引用
        Button button = GetComponent<Button>();
        // 没有查找到按钮引用的情况下

        Hover.SetActive(true);

        if (button == null)
        {
            // 结束
            return;
        }

        // 当前按钮未被禁用的情况下
        if (button.interactable)
        {
            // 音频源的引用
            AudioSource audioSource = GetComponent<AudioSource>();
            // 没有查找到音频源的情况下
            if (audioSource == null)
            {
                // 创建音频源
                audioSource = gameObject.AddComponent<AudioSource>();
            }

            // 音频源没有正在播放的情况下
            if (!audioSource.isPlaying)
            {
                // 设置音频源属性
                // 2D音效
                audioSource.spatialBlend = 0;
                // 唤醒播放
                audioSource.playOnAwake = false;
                // 音频剪辑
                audioSource.clip = Sound;
                // 播放音频
                audioSource.Play();
                // TODO 测试用代码，可删除
                print("已播放音频");
            }
        }
    }
    public void OnPointerExit(PointerEventData eventData)
    {
        Hover.SetActive(false);
    }

}
