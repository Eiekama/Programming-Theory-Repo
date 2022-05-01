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

    public GameObject waterIcon;

    [SerializeField] Sprite harvestIcon;
    Sprite previousIcon;

    public GameObject fertilisedIndicator;
    public bool isFertilised;
    public float fertiliserTimer;
    public float fertiliserTimeLeft;

    void Start()
    {
        fertilisedIndicator.SetActive(false);
        fertiliserTimeLeft = fertiliserTimer;
    }

    void OnMouseDown()
    {
        PlayerActions.Instance.currentField = this;

        if (currentCrop != null && currentCrop.GetComponent<Crop>().IsReady)
        {
            currentCrop.GetComponent<Crop>().Harvest();
        } else if (PlayerActions.Instance.currentAction != PlayerActions.Action.None)
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
        }
    }

    void OnMouseOver()
    {
        if (currentCrop != null && currentCrop.GetComponent<Crop>().IsReady)
        {
            if (PlayerActions.Instance.cursorRenderer.sprite != harvestIcon)
            {
                previousIcon = PlayerActions.Instance.cursorRenderer.sprite;
                PlayerActions.Instance.cursorRenderer.sprite = harvestIcon;
            }
            PlayerActions.Instance.TrackCursor();
        } else if (PlayerActions.Instance.cursorRenderer.sprite == harvestIcon && (currentCrop == null || !currentCrop.GetComponent<Crop>().IsReady))
        {
            PlayerActions.Instance.cursorRenderer.sprite = previousIcon;
        }
    }

    void OnMouseExit()
    {
        if (PlayerActions.Instance.cursorRenderer.sprite == harvestIcon)
        {
            PlayerActions.Instance.cursorRenderer.sprite = previousIcon;
        }
    }
}
