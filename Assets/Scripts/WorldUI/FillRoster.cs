using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FillRoster : MonoBehaviour
{
    public static Transform trans;

    private void Awake()
    {
        trans = this.transform;
    }
}