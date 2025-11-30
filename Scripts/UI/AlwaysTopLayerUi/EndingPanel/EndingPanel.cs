using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BitWave_Labs.AnimatedTextReveal;
using TMPro;

public class EndingPanel : MonoBehaviour, IUIWindow
{
    [SerializeField] private List<EndingTextContainer> textContainerList;
    [SerializeField] private ImageAnimator nguImageAnimator;
    private int curTextContainerIndex;
    private bool canNextShow;
    private bool canClose;
    private bool isClosing;

    private EndingPanelModel model;

    public void InjectDependencies(object[] _dependencies)
    {

    }

    public void Initial(object[] _datas)
    {
        foreach (object data in _datas)
        {
            if (data is EndingPanelModel _model)
                model = _model;
        }

        curTextContainerIndex = 0;
        canNextShow = false;
        canClose = false;
        isClosing = false;
    }

    public void Open()
    {
        gameObject.SetActive(true);

        if (textContainerList.Count > 0)
            textContainerList[curTextContainerIndex].Show();
    }

    private void Awake()
    {
        foreach (var textContainer in textContainerList)
            textContainer.OnComplete += CanNextShow;
        nguImageAnimator.OnComplete = CanClose;

        canNextShow = false;
    }

    private void Update()
    {
        if (isClosing)
            return;

        if (Input.GetMouseButtonDown(0) && canNextShow && curTextContainerIndex < textContainerList.Count)
        {
            textContainerList[curTextContainerIndex - 1].gameObject.SetActive(false);
            canNextShow = false;
            textContainerList[curTextContainerIndex].Show();
        }
        else if (Input.GetMouseButtonDown(0) && canNextShow && curTextContainerIndex == textContainerList.Count)
        {
            textContainerList[curTextContainerIndex - 1].gameObject.SetActive(false);
            canNextShow = false;
            nguImageAnimator.Show();
        }
        else if (Input.GetMouseButtonDown(0) && canClose)
        {
            nguImageAnimator.gameObject.SetActive(false);
            Close();
        }
    }

    private void CanNextShow()
    {
        canNextShow = true;
        curTextContainerIndex++;
    }

    private void CanClose()
    {
        canClose = true;
    }

    public void Close()
    {
        isClosing = true;
        gameObject.SetActive(false);

        if (model != null)
            model.OnComplete?.Invoke();
    }
}
