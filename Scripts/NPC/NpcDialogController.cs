using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

//3D NPC오브젝트의 다이얼로그 처리 컨트롤러입니다.
public class NpcDialogController : MonoBehaviour
{
    [SerializeField] private PlayerSensor sensor;
    [SerializeField] private Dialogue npcDialog;
    [SerializeField] private NpcQuestController npcQuestController;

    [SerializeField] private OpenDialogPanelRequestEvent openDialogRequestEvent;

    private bool isInteraction = false;

    private void Awake()
    {
        if (sensor != null)
        {
            sensor.OnEnter += OnEnterPlayer;
            sensor.OnExit += OnExitPlayer;
        }
    }

    private void Update()
    {
        if (isInteraction && Input.GetKeyDown(KeyCode.Z))
        {
            switch (npcQuestController.CurState)
            {
                case NpcQuestState.NONE:
                    openDialogRequestEvent?.Raise(npcDialog);
                    break;
                case NpcQuestState.QUEST_READY:
                    openDialogRequestEvent?.Raise(npcDialog, npcQuestController.CurReadyQuests.ToList());
                    break;
                case NpcQuestState.QUEST_IN_PROGRESS:
                    openDialogRequestEvent?.Raise(npcQuestController.InProgressQuest.ProgressDialog);
                    break;
                case NpcQuestState.QUEST_COMPLETE_READY:
                    openDialogRequestEvent?.Raise(npcQuestController.CompleteReadyTakeQuest.CompleteDialog, npcQuestController.CompleteReadyTakeQuest);
                    break;
            }
        }
    }

    //이벤트
    private void OnDestroy()
    {
        if (sensor != null)
        {
            sensor.OnEnter -= OnEnterPlayer;
            sensor.OnExit -= OnExitPlayer;
        }
    }

    private void OnEnterPlayer(Transform _player)
    {
        isInteraction = true;
    }

    private void OnExitPlayer()
    {
        isInteraction = false;
    }
}
