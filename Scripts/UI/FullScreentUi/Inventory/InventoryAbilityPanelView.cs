using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class InventoryAbilityPanelView : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI totalMaxHpTmp;
    [SerializeField] TextMeshProUGUI addMaxHpTmp;
    [SerializeField] TextMeshProUGUI totalMaxMpTmp;
    [SerializeField] TextMeshProUGUI addMaxMpTmp;
    [SerializeField] TextMeshProUGUI totalAtkTmp;
    [SerializeField] TextMeshProUGUI addAtkTmp;
    [SerializeField] TextMeshProUGUI totalDefTmp;
    [SerializeField] TextMeshProUGUI addDefTmp;
    [SerializeField] TextMeshProUGUI attackRangeTmp;

    public void Setting(IReadOnlyDictionary<PlayerStat, object> _playerAbilities)
    {
        totalMaxHpTmp.text = _playerAbilities[PlayerStat.TOTAL_MAXHP].ToString();
        addMaxHpTmp.text = $"({_playerAbilities[PlayerStat.MAXHP].ToString()}+{_playerAbilities[PlayerStat.ADD_MAXHP].ToString()})";
        totalMaxMpTmp.text = _playerAbilities[PlayerStat.TOTAL_MAXMP].ToString();
        addMaxMpTmp.text = $"({_playerAbilities[PlayerStat.MAXMP].ToString()}+{_playerAbilities[PlayerStat.ADD_MAXMP].ToString()})";
        totalAtkTmp.text = _playerAbilities[PlayerStat.TOTAL_ATK].ToString();
        addAtkTmp.text = $"({_playerAbilities[PlayerStat.ATK].ToString()}+{_playerAbilities[PlayerStat.ADD_ATK].ToString()})";
        totalDefTmp.text = _playerAbilities[PlayerStat.TOTAL_DEF].ToString();
        addDefTmp.text = $"({_playerAbilities[PlayerStat.DEF].ToString()}+{_playerAbilities[PlayerStat.ADD_DEF].ToString()})";
        attackRangeTmp.text = _playerAbilities[PlayerStat.ATTACKRANGE].ToString();
    }
}
