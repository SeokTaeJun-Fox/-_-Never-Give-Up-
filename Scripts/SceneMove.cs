using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneMove : MonoBehaviour
{
    [SerializeField] private int moveSceneNum;

    private void Start()
    {
        SceneManager.LoadScene(moveSceneNum);
    }
}
