using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Field : MonoBehaviour
{
    bool isFertilised;
    GameObject currentCrop;

    void OnMouseDown()
    {
        PlayerActions.Instance.selectedField = this;
    }
}
