using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// UI 윈도우의 생성 정보를 담고 있는 ScriptableObject입니다.
/// UIWindowFactory에서 윈도우 생성 시 참조되며, UI 타입, 캔버스 종류, 프리팹,
/// 의존성 인터페이스 정보를 포함합니다.
/// </summary>
[CreateAssetMenu(fileName = "UIEntry", menuName = "Scriptable Object/UI/UIEntry")]
public class UIEntry : ScriptableObject
{
    //UI 윈도우의 고유 타입
    //EX : INVENTORY, SKILLWINDOW
    [SerializeField] private UIType uiType;

    //캔버스 배치타입
    //EX : WorldLayer, MainLayer
    [SerializeField] private CanvasType canvasType; 

    [SerializeField] private GameObject prefab; //윈도우 프리팹

    //의존성 인터페이스 이름 목록 (예: IItemManager, IPlayerStatManager)
    [SerializeField] private string[] dependencyInterfaceTypeNames;

    public UIType UiType { get => uiType; }
    public CanvasType CanvasType { get => canvasType; }
    public GameObject Prefab { get => prefab; }
    public string[] DependencyInterfaceTypeNames { get => dependencyInterfaceTypeNames; }
}
