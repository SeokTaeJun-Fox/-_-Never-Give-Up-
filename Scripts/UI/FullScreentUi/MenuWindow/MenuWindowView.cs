using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MenuWindowView : MonoBehaviour
{
    [SerializeField] private Button saveButton;
    [SerializeField] private Button settingButton;
    [SerializeField] private Button goLobbyButton;
    [SerializeField] private Button closeButton;

    public Button SaveButton { get => saveButton; }
    public Button SettingButton { get => settingButton; }
    public Button GoLobbyButton { get => goLobbyButton; }
    public Button CloseButton { get => closeButton; }
}
