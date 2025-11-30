using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[CreateAssetMenu(fileName = "IFC_ClearBuff_Category_", menuName = "Scriptable Object/Item/ItemFunctionElement/IFC_ClearBuff_Category")]
public class IFC_ClearBuff_Category : ItemFunctionElement
{
    [SerializeField] private List<Category> clearBuffList;

    public override void Use(IItemUser _user)
    {
        _user.ClearBuff(clearBuffList);
    }

    public override void UnUse(IItemUser _user)
    {

    }
}
