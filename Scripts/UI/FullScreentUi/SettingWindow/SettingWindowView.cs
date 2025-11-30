using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SettingWindowView : MonoBehaviour
{
    [SerializeField] private Slider sfxSlider;
    [SerializeField] private TextMeshProUGUI sfxTmp;
    [SerializeField] private Slider bgmSlider;
    [SerializeField] private TextMeshProUGUI bgmTmp;
    [SerializeField] private Slider sensitivitySlider;
    [SerializeField] private TextMeshProUGUI sensitivityTmp;

    public Slider SfxSlider { get => sfxSlider; }
    public TextMeshProUGUI SfxTmp { get => sfxTmp; }
    public Slider BgmSlider { get => bgmSlider; }
    public TextMeshProUGUI BgmTmp { get => bgmTmp; }
    public Slider SensitivitySlider { get => sensitivitySlider; }
    public TextMeshProUGUI SensitivityTmp { get => sensitivityTmp; }
}
