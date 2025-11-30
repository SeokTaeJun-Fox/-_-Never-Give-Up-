using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class SkillSlotWindowController : MonoBehaviour, IUIWindow
{
    [SerializeField] private UnityEvent onClickButton;
    [SerializeField] private UnityEvent onOpen;
    [SerializeField] private UnityEvent onClose;

    //이 클래스에 필요한 이벤트스크립터블오브젝트들
    [SerializeField] private ReportOpenWindowEvent openEvent;

    //이 클래스에 필요한 스크립터블오브젝트들
    [SerializeField] private SkillHotKeyInfo keyInfo;

    [SerializeField] private SkillHotKeyPanelElementView[] hotKeyPanels;
    [SerializeField] private Button closeButton;
    [SerializeField] private Button uiBlockButton;

    private Skill target;

    private void Awake()
    {
        closeButton.onClick.AddListener(OnClickCloseButton);
        uiBlockButton.onClick.AddListener(OnClickCloseButton);

        for (int i = 0; i < hotKeyPanels.Length; i++)
        {
            int index = i;
            hotKeyPanels[i].GetButton.onClick.AddListener(() =>
            {
                if (Input.GetKey(KeyCode.LeftControl))
                {
                    hotKeyPanels[index].Hide();
                    keyInfo?.SetSkill(index, null);
                }
                else
                {
                    if (target == null)
                    {
                        Debug.LogError($"타겟 스킬이 없습니다.");
                        return;
                    }

                    if (keyInfo != null)
                    {
                        //스킬 단축키 항목 제거
                        int hidePanelIndex = keyInfo.Infos.FindIndex((x) => (x.skill != null) && (x.skill.SkillId == target.SkillId));
                        if (hidePanelIndex != -1)
                        {
                            hotKeyPanels[hidePanelIndex].Hide();
                            keyInfo.SetSkill(hidePanelIndex, null);
                        }

                        //스킬 단축키 항목 등록
                        keyInfo?.SetSkill(index, target);
                    }

                    hotKeyPanels[index].ShowView(target.SkillSprite);
                }

                onClickButton?.Invoke();
            });
        }
    }

    public void InjectDependencies(object[] _dependencies)
    {

    }

    public void Initial(object[] _datas)
    {
        foreach (object data in _datas)
        {
            if (data is Skill skill)
                target = skill;
        }

        for (int i = 0; i < hotKeyPanels.Length; i++)
        {
            Skill skill = keyInfo.GetSkill(i);

            if (skill == null)
            {
                hotKeyPanels[i].Hide();
            }
            else
            {
                hotKeyPanels[i].ShowView(skill.SkillSprite);
            }
        }
    }

    public void Open()
    {
        gameObject.SetActive(true);
        transform.SetAsLastSibling();   //윈도우를 맨 앞에 배치

        if (openEvent != null)
            openEvent.RaiseOpenWindow(this);

        onOpen?.Invoke();
    }

    public void Close()
    {
        gameObject.SetActive(false);
        onClose?.Invoke();
    }

    //이벤트
    private void OnClickCloseButton()
    {
        openEvent?.RaiseCloseWindow();
        Close();
    }
}
