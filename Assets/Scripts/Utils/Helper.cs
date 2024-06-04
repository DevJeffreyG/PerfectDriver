using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Helper : MonoBehaviour
{
    public static GameObject FindChildByTag(Transform parent, string tag)
    {
        for(int i = 0; i < parent.childCount; i++)
        {
            Transform child = parent.GetChild(i);
            if (child.gameObject.CompareTag(tag)) return child.gameObject;
        }

        return null;
    }
}
