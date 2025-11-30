using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class PlayerAbilityView : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI level;
    [SerializeField] private TextMeshProUGUI hp;
    [SerializeField] private TextMeshProUGUI mp;
    [SerializeField] private TextMeshProUGUI atk;
    [SerializeField] private TextMeshProUGUI atkInfo;
    [SerializeField] private TextMeshProUGUI def;
    [SerializeField] private TextMeshProUGUI defInfo;
    [SerializeField] private TextMeshProUGUI attackRange;

    [SerializeField] private Button closeButton;
    [SerializeField] private Button uiBlockButton;

    public Button CloseButton { get => closeButton; }
    public Button UiBlockButton { get => uiBlockButton; }

    public void Setting(IReadOnlyDictionary<PlayerStat, object> _abilityDic)
    {
        level.text = _abilityDic[PlayerStat.LEVEL].ToString();
        hp.text = _abilityDic[PlayerStat.HP].ToString() + " / " + _abilityDic[PlayerStat.TOTAL_MAXHP].ToString();
        mp.text = _abilityDic[PlayerStat.MP].ToString() + " / " + _abilityDic[PlayerStat.TOTAL_MAXMP].ToString();
        
        atk.text = _abilityDic[PlayerStat.TOTAL_ATK].ToString();
        atkInfo.text = $"({_abilityDic[PlayerStat.ATK]}+{_abilityDic[PlayerStat.ADD_ATK]})";

        def.text = _abilityDic[PlayerStat.TOTAL_DEF].ToString();
        defInfo.text = $"({_abilityDic[PlayerStat.DEF]}+{_abilityDic[PlayerStat.ADD_DEF]})";

        attackRange.text = _abilityDic[PlayerStat.ATTACKRANGE].ToString();
    }
}
