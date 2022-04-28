using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Field : MonoBehaviour
{
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

    public GameObject[] icons; // make sure icons are placed in order
    public bool isFertilised;

    public float fertiliserTimer;
    public float fertiliserTimeLeft;

    void Start()
    {
        fertiliserTimeLeft = fertiliserTimer;
    }

    void OnMouseDown()
    {
        PlayerActions.Instance.currentField = this;

        if (currentCrop != null && currentCrop.GetComponent<Crop>().IsReady)
        {
            currentCrop.GetComponent<Crop>().Harvest();
        }

        if (PlayerActions.Instance.currentAction != PlayerActions.Action.None)
        {
            switch (PlayerActions.Instance.currentAction)
            {
                case PlayerActions.Action.Plant:
                    if (currentCrop == null)
                    {
                        PlayerActions.Instance.Plant();
                    }
                    break;

                case PlayerActions.Action.Water:
                    if (currentCrop != null)
                    {
                        PlayerActions.Instance.Water();
                    }
                    break;

                case PlayerActions.Action.Fertilise:
                    if (!isFertilised)
                    {
                        PlayerActions.Instance.Fertilise();
                    }
                    break;

                case PlayerActions.Action.Remove:
                    if (currentCrop != null)
                    {
                        PlayerActions.Instance.Remove();
                    }
                    break;
            }
            PlayerActions.Instance.ClearAction();
        }
    }
}
