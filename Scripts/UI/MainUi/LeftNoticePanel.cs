using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LeftNoticePanel : MonoBehaviour
{
    [SerializeField] private LeftNoticeText[] leftNoticeTexts;
    int noticeIndex;    //공지오브젝트는 순서대로 사용되고,
                        //모든 공지 오브젝트가 다 사용되면 
                        //공지 오브젝트의 인덱스 0번부터 순차적으로 공지 오브젝트가 사용됩니다.

    private void Awake()
    {
        noticeIndex = 0;
    }

    public void ShowText(string _content)
    {
        for (int i = 0; i < leftNoticeTexts.Length; i++)
        {
            if (!leftNoticeTexts[i].gameObject.activeInHierarchy)
            {
                leftNoticeTexts[i].ShowText(_content);
                noticeIndex = 0;
                return;
            }
        }

        if (leftNoticeTexts.Length > noticeIndex)
            leftNoticeTexts[noticeIndex].ShowText(_content);
        else
        {
            noticeIndex = 0;
            leftNoticeTexts[noticeIndex].ShowText(_content);
        }

        noticeIndex++;
    }
}
