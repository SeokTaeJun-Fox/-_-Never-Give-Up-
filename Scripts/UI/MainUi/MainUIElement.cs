using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class MainUIElement : MonoBehaviour
{
    public abstract void InjectDependencies(object[] _dependencies);
    public abstract void Initial();
}
