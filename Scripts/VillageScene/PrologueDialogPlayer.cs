using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PrologueDialogPlayer : MonoBehaviour
{
    [SerializeField] private OpenDialogPanelRequestEvent dialogRequestEvent;
    [SerializeField] private Dialogue prologue;

    private void Awake()
    {
        if(!GlobalData.isSeePrologue)
            StartCoroutine(CoPlayPrologueDialog());   
    }

    private IEnumerator CoPlayPrologueDialog()
    {
        GlobalData.isSeePrologue = true;

        yield return new WaitForSeconds(1f);

        dialogRequestEvent.Raise(prologue);
    }
}
