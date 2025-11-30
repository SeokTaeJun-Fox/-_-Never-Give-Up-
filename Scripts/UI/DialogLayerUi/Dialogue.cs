using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[CreateAssetMenu(fileName = "Dialogue_", menuName = "Scriptable Object/Dialogue/Dialogue")]
public class Dialogue : ScriptableObject
{
    [SerializeField] private List<DialogueLine> lines;
    //public event Action<DialogueLine> OnLineChanged;

    public IReadOnlyList<DialogueLine> Lines => lines;

    //private int curLineIndex = 0;
    
    //public void SetLine(int _lineIndex)
    //{ 
    //    if (_lineIndex < lines.Count)
    //    {
    //        curLineIndex = _lineIndex;
    //        OnLineChanged?.Invoke(lines[curLineIndex]);
    //    }
    //    else
    //    {
    //        Debug.LogWarning($"다이얼로그 라인 설정 {_lineIndex}값이 다이얼로그 리스트의 인덱스값을 넘어섰습니다.");
    //    }
    //}

    //public void NextLine()
    //{ 
    //    if (curLineIndex + 1 < lines.Count)
    //    {
    //        curLineIndex++;
    //        OnLineChanged?.Invoke(lines[curLineIndex]);
    //    }
    //    else
    //    {
    //        Debug.LogWarning($"다이얼로그 라인 설정 {curLineIndex}값이 다이얼로그 리스트의 인덱스값을 넘어섰습니다.");
    //    }
    //}

    //public int GetCurLineIndex()
    //{ 
    //    return curLineIndex;
    //}

    //public int GetDialogCount()
    //{ 
    //    return lines.Count;
    //}

    //public bool IsEndLine()
    //{ 
    //    return (curLineIndex + 1) == lines.Count;
    //}
}

[System.Serializable]
public class DialogueLine
{
    public Sprite talkerSpr;
    public string talker;
    [TextArea] public string content;
}
