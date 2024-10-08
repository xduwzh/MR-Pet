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
    /// �����ͣʱ�Ļص�����
    /// </summary>
    /// <param name="eventData">�¼�����</param>
    public void OnPointerEnter(PointerEventData eventData)
    {
        // ��ǰ��ť������
        Button button = GetComponent<Button>();
        // û�в��ҵ���ť���õ������

        Hover.SetActive(true);

        if (button == null)
        {
            // ����
            return;
        }

        // ��ǰ��ťδ�����õ������
        if (button.interactable)
        {
            // ��ƵԴ������
            AudioSource audioSource = GetComponent<AudioSource>();
            // û�в��ҵ���ƵԴ�������
            if (audioSource == null)
            {
                // ������ƵԴ
                audioSource = gameObject.AddComponent<AudioSource>();
            }

            // ��ƵԴû�����ڲ��ŵ������
            if (!audioSource.isPlaying)
            {
                // ������ƵԴ����
                // 2D��Ч
                audioSource.spatialBlend = 0;
                // ���Ѳ���
                audioSource.playOnAwake = false;
                // ��Ƶ����
                audioSource.clip = Sound;
                // ������Ƶ
                audioSource.Play();
                // TODO �����ô��룬��ɾ��
                print("�Ѳ�����Ƶ");
            }
        }
    }
    public void OnPointerExit(PointerEventData eventData)
    {
        Hover.SetActive(false);
    }

}
