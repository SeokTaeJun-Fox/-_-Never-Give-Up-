using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageUIManager : MonoBehaviour
{
    [SerializeField] private string objectName; //데미지 유아이 오브젝트 이름 (오브젝트 풀 불러오기, 회수하기에 사용됩니다.)
    [SerializeField] private ObjectPoolEvent poolEvent;
    [SerializeField] private DamageUIGetRetriveEvent damageUIGetRetriveEvent;
    [SerializeField] private GetCanvasRequestEvent canvasRequestEvent;  //ui매니저에 캔버스 불러오기 이벤트
    [SerializeField] private List<DamageUI> curActiveDamageUI;    //디버그용 인스펙터에 수정x

    private void Awake()
    {
        damageUIGetRetriveEvent.OnGet += GetDamageUi;
        damageUIGetRetriveEvent.OnRetrieve += RetriveDamageUi;
    }

    //불러오기
    public DamageUI GetDamageUi()
    {
        DamageUI damageUi = poolEvent.RaiseGet(objectName).GetComponent<DamageUI>();
        damageUi.transform.SetParent(canvasRequestEvent.RaiseGet(CanvasType.WorldLayer).transform);
        curActiveDamageUI.Add(damageUi);

        return damageUi;
    }

    //회수
    public void RetriveDamageUi(DamageUI _damageUi)
    {
        _damageUi.Initial();
        poolEvent.RaiseRelease(objectName, _damageUi.gameObject);
    }

    private void OnDisable()
    {
        damageUIGetRetriveEvent.OnGet -= GetDamageUi;
        damageUIGetRetriveEvent.OnRetrieve -= RetriveDamageUi;
    }
}
