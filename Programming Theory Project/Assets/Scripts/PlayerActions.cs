using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerActions : MonoBehaviour
{
    public static PlayerActions Instance { get; private set; }

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
    }

    [SerializeField] Camera mainCamera;
    [SerializeField] GameObject cursor;
    public SpriteRenderer cursorRenderer { get; private set; }


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


    public void TrackCursor()
    {
        Vector2 pos = mainCamera.ScreenToWorldPoint(Input.mousePosition);
        cursor.transform.position = pos;
    }


    public void Plant()
    {
        Debug.Log("Planted " + cropPrefab);
        Instantiate(cropPrefab, currentField.transform);
        currentField.CurrentCrop = currentField.GetComponentInChildren<Crop>().gameObject;
    }

    public void Water()
    {
        Debug.Log("Watered field");
        currentField.CurrentCrop.GetComponent<Crop>().WaterLevel += 8;
    }

    public void Fertilise()
    {
        Debug.Log("Fertilised field");
        currentField.isFertilised = true;
        currentField.fertilisedIndicator.SetActive(true);
    }

    public void Remove()
    {
        Debug.Log("Removed crop");
        Destroy(currentField.CurrentCrop);
        currentField.CurrentCrop = null;
        if (currentField.waterIcon.activeInHierarchy)
        {
            currentField.waterIcon.SetActive(false);
        }
    }

    public void ClearAction()
    {
        currentAction = Action.None;
        currentField = null;
        cropPrefab = null;
    }
}
