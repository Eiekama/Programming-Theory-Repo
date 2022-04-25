using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Field : MonoBehaviour
{
    bool isFertilised;
    GameObject currentCrop;
    public GameObject CurrentCrop
    {
        get { return currentCrop; }
        set
        {
            if (value != null && value.GetComponent<Crop>() == null)
            {
                Debug.LogError("Prefab provided does not have Crop class");
            }
            else
            {
                currentCrop = value;
            }
        }
    }

    void OnMouseDown()
    {
        PlayerActions.Instance.currentField = this;
        switch (PlayerActions.Instance.currentAction)
        {
            case PlayerActions.Action.Plant:
                if (currentCrop == null)
                {
                    PlayerActions.Instance.Plant();
                }
                break;
            case PlayerActions.Action.Water:
                PlayerActions.Instance.Water();
                break;
            case PlayerActions.Action.Fertilise:
                PlayerActions.Instance.Fertilise();
                break;
            case PlayerActions.Action.Remove:
                if (currentCrop != null)
                {
                    PlayerActions.Instance.Remove();
                }
                break;
        }
    }
}
