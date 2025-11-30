using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class SettingWindowController : MonoBehaviour, IUIWindow
{
    [SerializeField] private string playerPrebName_Sfx;
    [SerializeField] private string playerPrebName_bgm;
    [SerializeField] private string playerPrebName_cameraSensitivity;

    [SerializeField] private SettingWindowView view;

    [SerializeField] private UnityEvent onOpen;
    [SerializeField] private UnityEvent onClose;

    //이 클래스에 필요한 이벤트스크립터블오브젝트들
    [SerializeField] private ReportOpenWindowEvent openEvent;
    [SerializeField] private CinemachineCameraData cameraData;

    //이 클래스에 필요한 인터페이스들
    private ISoundManager soundManager;

    public void Initial(object[] _datas)
    {
        view.SfxSlider.onValueChanged.AddListener(ApplySfxValue);
        view.BgmSlider.onValueChanged.AddListener(ApplyBgmValue);
        view.SensitivitySlider.onValueChanged.AddListener(ApplySensitivityValue);
    }

    public void InjectDependencies(object[] _dependencies)
    {
        foreach (object dependency in _dependencies)
        {
            if (dependency is ISoundManager sm)
                soundManager = sm;
        }
    }

    public void Open()
    {
        gameObject.SetActive(true);
        transform.SetAsLastSibling();   //윈도우를 맨 앞에 배치

        if (openEvent != null)
            openEvent.RaiseOpenWindow(this);

        UpdateUI();

        Debug.Log($"sfx : {PlayerPrefs.GetFloat(playerPrebName_Sfx, 0)} \n" +
    $"bgm : {PlayerPrefs.GetFloat(playerPrebName_bgm, 0)} \n" +
    $"민감도 : {PlayerPrefs.GetFloat(playerPrebName_cameraSensitivity, 0)}");

        onOpen?.Invoke();
    }

    private void UpdateUI()
    {
        float sfxValue = SoundManager.SfxVolume;
        view.SfxSlider.value = sfxValue;

        float bgmValue = SoundManager.BgmVolume;
        view.BgmSlider.value = bgmValue;

        float sensitivityValue = cameraData.Sensitivity;
        view.SensitivitySlider.value = sensitivityValue;
    }

    public void Close()
    {
        SaveData();
        gameObject.SetActive(false);
        onClose?.Invoke();
    }

    //이벤트

    private void ApplySfxValue(float _value)
    {
        view.SfxTmp.text = ((int)(_value * 100)).ToString() + "%";
        soundManager?.SetSFXVolume(_value);
    }

    private void ApplyBgmValue(float _value)
    {
        view.BgmTmp.text = ((int)(_value * 100)).ToString() + "%";
        soundManager?.SetBGMVolume(_value);
    }

    private void ApplySensitivityValue(float _value)
    {
        view.SensitivityTmp.text = ((int)(_value)).ToString();
        cameraData.Sensitivity = _value;
    }

    private void SaveData()
    {
        PlayerPrefs.SetFloat(playerPrebName_Sfx, SoundManager.SfxVolume);
        PlayerPrefs.SetFloat(playerPrebName_bgm, SoundManager.BgmVolume);
        PlayerPrefs.SetFloat(playerPrebName_cameraSensitivity, cameraData.Sensitivity);
    }

    public void OnClickExitButton()
    {
        openEvent?.RaiseCloseWindow();
        Close();
    }
}
