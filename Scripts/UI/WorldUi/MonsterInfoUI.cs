using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class MonsterInfoUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI nameTMP;
    [SerializeField] private Slider hpBar;
    [SerializeField] private GameObject uiParent;
    [SerializeField] private MonsterBuffPanel monsterBuffPanel;
    [SerializeField] private Transform followObject;    //디버그 표시용 인스펙터 수정x

    private MonsterInfo targetMonsterInfo;
    private IReadOnlyDictionary<PlayerStat, object> targetMonsterAbilities;
    private IBuffController buffController;

    private void Update()
    {
        if (Camera.main != null && followObject != null)
        { 
            transform.position = Camera.main.WorldToScreenPoint(followObject.position);
        }
    }

    public void Setting(Transform _followObject, MonsterInfo _monsterInfo, IReadOnlyDictionary<PlayerStat, object> _monsterAbilities, IBuffController _buffController)
    { 
        followObject = _followObject;
        targetMonsterInfo = _monsterInfo;
        targetMonsterAbilities = _monsterAbilities;
        buffController = _buffController;

        nameTMP.text = $"LV.{_monsterInfo.MonsterAbility.Level} {_monsterInfo.MonsterName}";
        UpdateHpBar();

        monsterBuffPanel.SetBuffController(buffController);
        monsterBuffPanel.ActivePanel();
    }

    public void UpdateHpBar()
    {
        hpBar.value = (float)((int)targetMonsterAbilities[PlayerStat.HP]) / (int)targetMonsterAbilities[PlayerStat.TOTAL_MAXHP];
    }

    public void Hide(bool _isHide)
    { 
        uiParent.SetActive(!_isHide);
    }

    //초기화 (오브젝트 풀로 돌려놓고싶다면 반드시 실행합니다.)
    public void Initial()
    {
        followObject = null;
        targetMonsterInfo = null;
        targetMonsterAbilities = null;
        nameTMP.text = "";
        hpBar.value = 1;
        uiParent.SetActive(true);

        monsterBuffPanel.Initial();
    }
}
