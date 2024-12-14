using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class C_Tree : MonoBehaviour
{
    public float startScale;
    public float targetScale;
    public float duration;

    public AudioClip growSound;
    // Start is called before the first frame update
    void Start()
    {
        AudioSource.PlayClipAtPoint(growSound, transform.position);
        StartCoroutine(ScaleObject(Vector3.one * startScale, Vector3.one * targetScale, duration));  
    }

    private IEnumerator ScaleObject(Vector3 startScale, Vector3 endScale, float time)
    {
        float elapsedTime = 0f;
        while (elapsedTime < time)
        {
            // ���Բ�ֵ��Lerp�����㵱ǰ������
            this.transform.localScale = Vector3.Lerp(startScale, endScale, elapsedTime / time);
            elapsedTime += Time.deltaTime;
            yield return null; // �ȴ���һ֡
        }
        // ȷ����������ֵ�Ǿ�ȷ��Ŀ��ֵ
        this.transform.localScale = endScale;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
