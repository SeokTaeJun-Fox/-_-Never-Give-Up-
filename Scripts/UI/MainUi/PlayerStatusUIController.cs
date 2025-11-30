using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStatusUIController : MainUIElement
{
    [SerializeField] private PlayerStatusUIView view;

    [SerializeField] private OnPlayerDamagedEvent onPlayerDamagedEvent;

    private IPlayerAbilityManager playerAbilityManager;

    public override void InjectDependencies(object[] _dependencies)
    {
        foreach (object dependency in _dependencies)
        {
            if (dependency is IPlayerAbilityManager paManager)
                playerAbilityManager = paManager;
        }
    }

    public override void Initial()
    {
        if (playerAbilityManager != null)
        {
            playerAbilityManager.OnAbilityChanged += OnAbilityChanged;
            playerAbilityManager.OnExpChanged += OnExpChanged;
            Setting();
        }

        if(onPlayerDamagedEvent != null)
            onPlayerDamagedEvent.OnPlayerDamaged += OnPlayerDamaged;
    }

    private void OnAbilityChanged(IReadOnlyDictionary<PlayerStat, object> _playerAbilities)
    {
        SettingAboutAbility(_playerAbilities);
    }

    private void OnExpChanged(int _currentExp, int _maxExp)
    {
        view.SetExpImgFilled((float) _currentExp / _maxExp);
    }

    private void Setting()
    {
        view.SetExpImgFilled((float)playerAbilityManager.GetCurrentExp / playerAbilityManager.MaxExp);
        SettingAboutAbility(playerAbilityManager.PlayerAbilities);
    }

    private void SettingAboutAbility(IReadOnlyDictionary<PlayerStat, object> _playerAbilities)
    {
        view.SetLevelText(_playerAbilities[PlayerStat.LEVEL].ToString());

        float hpRate = (float)((int)_playerAbilities[PlayerStat.HP]) / (int)_playerAbilities[PlayerStat.TOTAL_MAXHP];
        view.SetHpSlider(hpRate);
        string hpText = $"{_playerAbilities[PlayerStat.HP].ToString()} / {_playerAbilities[PlayerStat.TOTAL_MAXHP].ToString()}";
        view.SetHpText(hpText);

        float mpRate = (float)((int)_playerAbilities[PlayerStat.MP]) / (int)_playerAbilities[PlayerStat.TOTAL_MAXMP];
        view.SetMpSlider(mpRate);
        string mpText = $"{_playerAbilities[PlayerStat.MP].ToString()} / {_playerAbilities[PlayerStat.TOTAL_MAXMP].ToString()}";
        view.SetMpText(mpText);
    }

    //¿Ã∫•∆Æ
    private void OnDestroy()
    {
        if (playerAbilityManager != null)
        {
            playerAbilityManager.OnAbilityChanged -= OnAbilityChanged;
            playerAbilityManager.OnExpChanged -= OnExpChanged;
        }

        if (onPlayerDamagedEvent != null)
            onPlayerDamagedEvent.OnPlayerDamaged -= OnPlayerDamaged;
    }

    private void OnPlayerDamaged(int _damage)
    {
        view.ShakeHpBar();
    }
}
