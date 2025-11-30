using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DestryedWhenUnload : MonoBehaviour
{
    private void Awake()
    {
        SceneManager.sceneUnloaded += OLoadSceneEvent;
    }

    public void OLoadSceneEvent(Scene scene)
    {
        SceneManager.sceneUnloaded -= OLoadSceneEvent;
        Destroy(gameObject);
    }
}
