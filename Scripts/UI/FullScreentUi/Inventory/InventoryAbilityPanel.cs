using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryAbilityPanel : MonoBehaviour
{
    [SerializeField] private InventoryAbilityPanelView view;

    //이 클래스에 필요한 인터페이스
    private IPlayerAbilityManager playerAbilityManager;

    public void Initial(IPlayerAbilityManager _playerAbilityManager)
    {
       playerAbilityManager = _playerAbilityManager;

        if (playerAbilityManager != null)
        {
            playerAbilityManager.OnAbilityChanged += OnAbilityChanged;
            Setting(playerAbilityManager.PlayerAbilities);
        }
    }

    private void Setting(IReadOnlyDictionary<PlayerStat, object> _playerAbilities)
    {
        view.Setting(_playerAbilities);
    }

    public void OnCloseInven()
    {
        if (playerAbilityManager != null)
            playerAbilityManager.OnAbilityChanged -= OnAbilityChanged;
    }

    private void OnDestroy()
    {
        if (playerAbilityManager != null)
            playerAbilityManager.OnAbilityChanged -= OnAbilityChanged;
    }

    //이벤트
    private void OnAbilityChanged(IReadOnlyDictionary<PlayerStat, object> _playerAbilities)
    {
        Setting(_playerAbilities);
    }
}
