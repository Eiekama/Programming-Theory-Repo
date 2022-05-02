using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerActions : MonoBehaviour
{
    public static PlayerActions Instance { get; private set; } // ENCAPSULATION

    public enum Action
    {
        None,
        Plant,
        Water,
        Fertilise,
        Remove
    }
    public Action currentAction;
    public Field currentField;

    GameObject cropPrefab;
    public GameObject CropPrefab
    {
        get { return cropPrefab; }
        set
        {
            if (value != null && value.GetComponent<Crop>() == null)
            {
                Debug.LogError("Prefab provided does not have Crop class");
            } else
            {
                cropPrefab = value;
            }
        }
    } // ENCAPSULATION

    [SerializeField] Camera mainCamera;
    [SerializeField] GameObject cursor;
    public SpriteRenderer cursorRenderer { get; private set; } // ENCAPSULATION


    void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    void Start()
    {
        cursorRenderer = cursor.GetComponent<SpriteRenderer>();
        ClearAction();
    }

    void Update()
    {
        if (currentAction != Action.None)
        {
            TrackCursor();
        }
    }

    void OnMouseDown()
    {
        if (currentAction != Action.None)
        {
            ClearAction();
        }
    }


    public void TrackCursor() // ABSTRACTION
    {
        Vector2 pos = mainCamera.ScreenToWorldPoint(Input.mousePosition);
        cursor.transform.position = pos;
    }


    public void Plant() // ABSTRACTION
    {
        Debug.Log("Planted " + cropPrefab);
        Instantiate(cropPrefab, currentField.transform);
        currentField.CurrentCrop = currentField.GetComponentInChildren<Crop>().gameObject;
    }

    public void Water() // ABSTRACTION
    {
        Debug.Log("Watered field");
        currentField.CurrentCrop.GetComponent<Crop>().WaterLevel += 8;
    }

    public void Fertilise() // ABSTRACTION
    {
        Debug.Log("Fertilised field");
        currentField.isFertilised = true;
        currentField.SetFertilisedIndicatorActive(true);
    }

    public void Remove() // ABSTRACTION
    {
        Debug.Log("Removed crop");
        Destroy(currentField.CurrentCrop);
        currentField.CurrentCrop = null;
    }

    public void ClearAction() // ABSTRACTION
    {
        currentAction = Action.None;
        currentField = null;
        cropPrefab = null;
    }
}
