using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class QuestWindowListPanel : MonoBehaviour
{
    [SerializeField] private QuestWindowListElementPanel preb;
    [SerializeField] private Transform prebParent;
    private Dictionary<string, QuestWindowListElementPanel> elementsByQuest = new Dictionary<string, QuestWindowListElementPanel>();
    [SerializeField] private ToggleGroup toggleGroup;

    private void OnDisable()
    {
        AllTogglesOff();
    }

    //퀘스트 목록을 추가합니다.
    public void AddElement(Quest quest, UnityAction<bool> onClicked)
    {
        var element = Instantiate(preb, prebParent);
        element.Setting(quest.Icon, quest.DisplayName);

        var toggle = element.GetComponent<Toggle>();
        element.Toggle.group = toggleGroup;
        toggle.onValueChanged.AddListener(onClicked);

        elementsByQuest.Add(quest.CodeName, element);
    }

    //퀘스트 목록을 제거합니다.
    public void RemoveElement(Quest quest)
    {
        Destroy(elementsByQuest[quest.CodeName].gameObject);
        elementsByQuest.Remove(quest.CodeName);
    }

    public void RemoveAllElement()
    {
        foreach (var element in elementsByQuest.Values)
            Destroy(element.gameObject);

        elementsByQuest.Clear();
    }

    public bool CheckElement(Quest _quest)
    { 
        return elementsByQuest.ContainsKey(_quest.CodeName);
    }

    public void AllTogglesOff()
    {
        foreach (var element in elementsByQuest.Values)
        {
            if (element.Toggle.isOn)
            {
                element.Toggle.isOn = false;
            }
        }
    }
}
