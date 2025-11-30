using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Unity.VisualScripting;
using System.Linq;

public class ItemInfoPanelCreator : MonoBehaviour
{
    [SerializeField] private ItemInfoPanelRegistry registry;
    [SerializeField] private ReportOpenWindowEvent openEvent;
    [SerializeField] private RectTransform rectTransform;
    private List<(ItemType itemType, GameObject panel)> cache = new List<(ItemType, GameObject)>();

    public void OpenPanel(ItemStack _itemStack, Vector2 _anchorPos)
    {
        OpenPanel(_itemStack);
        rectTransform.anchoredPosition = _anchorPos;
    }

    public void OpenPanel(ItemStack _itemStack)
    {
        closeAllPanel();

        ItemType itemtype = _itemStack.item.ItemType;
        GameObject panel = cache.Find(x => x.itemType.HasFlag(itemtype)).panel;
        IUIWindow uIPanel = null;

        //첫 오픈시
        if (panel == null)
        {
            //_itemType에 해당하는 entry 찾기
            var entry = registry.Entries.Find(x => x.ItemType.HasFlag(itemtype));
            if (entry != null)
            {
                //entry를 이용하여 프리팹생성 및 의존주입
                object[] dependencies = ResolveDependencies(entry);

                panel = Instantiate(entry.Prefab, transform);
                uIPanel = panel.GetComponent<IUIWindow>();
                uIPanel?.InjectDependencies(dependencies);
                uIPanel?.Initial(new object[] { _itemStack });
                uIPanel?.Open();
                cache.Add((entry.ItemType, panel));

                if (uIPanel == null)
                {
                    Debug.LogWarning($"{itemtype}에 해당하는 패널 프리팹에 uipanel컴포넌트를 부착하지 않았습니다.");
                }
            }
            else
            {
                Debug.LogWarning($"{itemtype}에 해당하는 패널을 등록하지 않았습니다.");
            }
        }
        //재 오픈시
        else
        {
            panel.SetActive(true);
            uIPanel = panel.GetComponent<IUIWindow>();
            uIPanel.Initial(new object[] { _itemStack });
            uIPanel.Open();
        }
    }

    public void closeAllPanel()
    {
        foreach (var tuple in cache)
        {
            if (tuple.panel.activeSelf)
            {
                openEvent.RaiseCloseWindow();
                tuple.panel.GetComponent<IUIWindow>()?.Close();
            }
        }
    }

    private object[] ResolveDependencies(ItemInfoPanelEntry _entry)
    {
        List<object> resolved = new List<object>();

        foreach (var typeName in _entry.DependencyInterfaceTypeNames)
        {
            var type = GetInterfaceType(typeName);
            if (type == null)
            {
                Debug.LogError($"{typeName}타입형의 클래스는 존재하지 않습니다.");
                continue;
            }

            var service = ServiceLocator.Get(type);
            if (service == null)
            {
                Debug.LogError($"서비스로케이터에 {typeName}타입을 등록하지 않았습니다.");
            }

            resolved.Add(service);
        }

        return resolved.ToArray();
    }

    private Type GetInterfaceType(string _typeName)
    {
        return AppDomain.CurrentDomain.GetAssemblies()
            .SelectMany(a => a.GetTypes())
            .FirstOrDefault(t => t.IsInterface && t.Name == _typeName);
    }
}
