using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapInitial : MonoBehaviour
{
    [SerializeField] ActiveMainUIRequestEvent mainUIRequest;
    [SerializeField] LoadSceneRequestEvent loadSceneRequest;

    private void Awake()
    {
        if (mainUIRequest != null)
        {
            mainUIRequest.Raise(true);
        }

        GlobalData.isGameStart = true;
    }
}
