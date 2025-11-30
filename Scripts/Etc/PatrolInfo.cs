using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//패트롤 정보가 들어있습니다.
public class PatrolInfo : MonoBehaviour
{
    [SerializeField] private Transform[] patrols;
    public Transform[] Patrols => patrols;
}
