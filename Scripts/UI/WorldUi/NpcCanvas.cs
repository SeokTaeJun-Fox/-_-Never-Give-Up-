using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class NpcCanvas : MonoBehaviour
{
    [SerializeField] private Image questStateImage;
    [SerializeField] private Sprite exclamationMark;
    [SerializeField] private Sprite questionMark;
    [SerializeField] private Sprite questProgressMark;

    [SerializeField] private GameObject pressZImg;

    [SerializeField] private TextMeshProUGUI nameText;

    private Camera realCamera;

    private void Update()
    {
        if (realCamera == null)
        {
            realCamera = Camera.main;
        }

        if (realCamera != null)
        {
            transform.LookAt(realCamera.transform);
        }
    }

    public void SettingQuestState(NpcQuestState _state)
    {
        switch (_state)
        {
            case NpcQuestState.NONE:
                questStateImage.gameObject.SetActive(false);
                break;
            case NpcQuestState.QUEST_READY:
                {
                    questStateImage.gameObject.SetActive(true);
                    questStateImage.sprite = exclamationMark;
                }
                break;
            case NpcQuestState.QUEST_IN_PROGRESS:
                {
                    questStateImage.gameObject.SetActive(true);
                    questStateImage.sprite = questProgressMark;
                }
                break;
            case NpcQuestState.QUEST_COMPLETE_READY:
                {
                    questStateImage.gameObject.SetActive(true);
                    questStateImage.sprite = questionMark;
                }
                break;
        }
    }

    public void SettingName(string _name)
    { 
        nameText.text = _name;
    }

    public void ActivePressZImg(bool _active)
    { 
        pressZImg.SetActive(_active);
    }
}
