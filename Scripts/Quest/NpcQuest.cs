using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Quest/NpcQuest", fileName = "NpcQuest_")]
public class NpcQuest : Quest
{
    [Header("NPC QUEST")]
    [SerializeField] private string startTargetNpcCode;
    [SerializeField] private string endTargetNpcCode;
    [SerializeField] private Sprite startTargetNpcSpr;
    [SerializeField] private string startTargetNpcName;

    [SerializeField] private Dialogue requestDialog;    //요청 대화
    [SerializeField] private Dialogue progressDialog;   //진행중일때 대화
    [SerializeField] private Dialogue completeDialog;   //완료시 대화

    public string StartTargetNpcCode { get => startTargetNpcCode; }
    public string EndTargetNpcCode { get => endTargetNpcCode; }
    public Sprite StartTargetNpcSpr { get => startTargetNpcSpr; }
    public string StartTargetNpcName { get => startTargetNpcName; }
    public Dialogue RequestDialog { get => requestDialog; }
    public Dialogue ProgressDialog { get => progressDialog; }
    public Dialogue CompleteDialog { get => completeDialog; }
}
