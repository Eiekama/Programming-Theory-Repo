using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Crop : MonoBehaviour
{
    [SerializeField] protected Sprite[] growthStages;
    protected SpriteRenderer spriteRenderer;
    protected Field currentField;

    [SerializeField] protected float growTime;
    [SerializeField] float growthSpeed = 1.0f;
    protected float GrowthSpeed
    {
        get { return growthSpeed; }
        set
        {
            if (value < 0)
            {
                Debug.LogError("growth speed was set to negative");
            } else
            {
                growthSpeed = value;
            }
        }
    } // ENCAPSULATION

    [SerializeField] float waterLevel = 10;
    public float WaterLevel
    {
        get { return waterLevel; }
        set
        {
            if (value > 10)
            {
                waterLevel = 10;
            } else if (value < 0)
            {
                waterLevel = 0;
            } else
            {
                waterLevel = value;
            }
        }
    } // ENCAPSULATION

    [SerializeField] float waterLossRate = 1.0f;

    public bool IsReady { get; protected set; } // ENCAPSULATION
    protected bool isGrowing;

    bool isRunningF;
    bool isRunningW;

    void Awake()
    {
        currentField = GetComponentInParent<Field>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        spriteRenderer.sprite = growthStages[0];
    }

    void Start()
    {
        StartCoroutine(Growing());
    }

    void Update()
    {
        if (currentField.isFertilised && !isRunningF)
        {
            StartCoroutine(ManageFertiliser());
        }
        if (!isRunningW && waterLevel > 0)
        {
            StartCoroutine(ManageWater());
        }
    }

    void OnDestroy()
    {
        if (currentField.waterIcon.activeInHierarchy)
        {
            currentField.waterIcon.SetActive(false);
        }
    }

    protected virtual IEnumerator Growing() // ABSTRACTION
    {
        isGrowing = true;
        IsReady = false;

        var growTimeLeft = growTime;
        var interval = growTime / (growthStages.Length - 1);
        var currentStage = 0;

        while (growTimeLeft > 0)
        {
            if (isGrowing)
            {
                growTimeLeft -= GrowthSpeed * Time.deltaTime;
                var stage = growthStages.Length - 1 - Mathf.CeilToInt(growTimeLeft / interval);

                if (currentStage != stage)
                {
                    currentStage = stage;
                    spriteRenderer.sprite = growthStages[currentStage];
                }
                yield return null;
            } else
            {
                yield return null;
            }
        }

        isGrowing = false;
        IsReady = true;
    }

    public virtual void Harvest()
    {
        Debug.Log("Crop was harvested");
    }

    IEnumerator ManageFertiliser() // ABSTRACTION
    {
        isRunningF = true;
        bool changedSpeed = false;

        while (currentField.fertiliserTimeLeft > 0)
        {
            if (isGrowing)
            {
                if (!changedSpeed)
                {
                    GrowthSpeed *= 2;
                    changedSpeed = true;
                }
                currentField.fertiliserTimeLeft -= Time.deltaTime;
                yield return null;
            } else
            {
                yield return null;
            }
        }

        GrowthSpeed /= 2;
        currentField.ResetFertiliserTimer();
        currentField.isFertilised = false;
        currentField.SetFertilisedIndicatorActive(false);
        isRunningF = false;
    }

    IEnumerator ManageWater() // ABSTRACTION
    {
        isRunningW = true;
        bool changedSpeed = false;
        spriteRenderer.color = Color.white;
        currentField.waterIcon.SetActive(false);
        if (!isGrowing && !IsReady)
        {
            isGrowing = true;
        }

        while (waterLevel > 0)
        {
            if (isGrowing)
            {
                if (changedSpeed && waterLevel >= 4)
                {
                    GrowthSpeed *= 2;
                    changedSpeed = false;
                    spriteRenderer.color = Color.white;
                }
                else if (!changedSpeed && waterLevel < 4)
                {
                    GrowthSpeed /= 2;
                    changedSpeed = true;
                    spriteRenderer.color = new Color(0.89f, 0.68f, 0, 1); // some shade of orange
                }

                WaterLevel -= waterLossRate * Time.deltaTime;
                yield return null;
            } else
            {
                yield return null;
            }
        }

        if (changedSpeed)
        {
            GrowthSpeed *= 2;
        }
        isGrowing = false;
        currentField.waterIcon.SetActive(true);
        isRunningW = false;
    }
}
