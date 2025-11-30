using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using DG.Tweening;

public class DamageUI : MonoBehaviour
{
    [SerializeField] private DamageUIGetRetriveEvent damageUIGetRetriveEvent;   //인스턴스 반환용 변수
    [SerializeField] private Transform followObject;
    [SerializeField] private TextMeshProUGUI damageTmp;
    private Sequence damageAction;

    private void Update()
    {
        if (Camera.main != null && followObject != null)
        {
            transform.position = Camera.main.WorldToScreenPoint(followObject.position);
        }
    }

    public void Setting(Transform _followObject)
    { 
        followObject = _followObject;
        if (Camera.main != null && followObject != null)
        {
            transform.position = Camera.main.WorldToScreenPoint(followObject.position);
        }
    }

    public void ShowDamageText(int _damage)
    {
        damageTmp.text = _damage.ToString();

        if (damageAction == null)
        {
            damageAction = DOTween.Sequence();
            damageAction.SetAutoKill(false)
                        .Append(damageTmp.rectTransform.DOScale(1.2f, 0.2f))
                        .Join(damageTmp.rectTransform.DOLocalMoveY(350, 0.6f))
                        .Insert(0.2f, damageTmp.rectTransform.DOScale(0.8f, 0.4f)).OnComplete(() =>
                        {
                            damageTmp.rectTransform.localPosition = Vector3.zero;
                            damageTmp.rectTransform.localScale = Vector3.zero;
                            damageUIGetRetriveEvent?.RaiseRetrieve(this);
                        });
        }
        else
        {
            damageAction.Restart();
        }
    }

    public void Initial()
    {
        followObject = null;
    }
}
