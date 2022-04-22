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

    public Field selectedField;

    GameObject cropPrefab;
    public GameObject CropPrefab
    {
        get { return cropPrefab; }
        set
        {
            if (value.GetComponent<Crop>() == null)
            {
                Debug.LogError("Prefab provided does not have Crop class");
            } else
            {
                cropPrefab = value;
            }
        }
    }

    [SerializeField] Camera mainCamera;
    [SerializeField] Transform cursor;
    [SerializeField] SpriteRenderer[] icons; // make sure icons are placed in order


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
        currentAction = Action.None;
        selectedField = null;
        cropPrefab = null;
    }

    void Update()
    {
        if (currentAction != Action.None)
        {
            TrackCursor();
            
            switch (currentAction)
            {
                case Action.Water:
                    SetIcon(0);
                    break;
                case Action.Fertilise:
                    SetIcon(1);
                    break;
                case Action.Remove:
                    SetIcon(2);
                    break;
            }
        } else
        {
            SetIcon();
        }
    }


    void TrackCursor()
    {
        Vector2 pos = mainCamera.ScreenToWorldPoint(Input.mousePosition);
        cursor.position = pos;
    }

    void SetIcon(int index)
    {
        for (int i = 0; i < icons.Length; i++)
        {
            if (i == index && !icons[i].enabled)
            {
                icons[i].enabled = true;
            } else if (i != index && icons[i].enabled)
            {
                icons[i].enabled = false;
            }
        }
    }

    void SetIcon()
    { // disables all icons
        for (int i = 0; i < icons.Length; i++)
        {
            if(icons[i].enabled)
            {
                icons[i].enabled = false;
            }
        }
    }


    void Plant()
    {

    }

    void Water()
    {

    }

    void Fertilise()
    {

    }

    void Remove()
    {

    }
}
