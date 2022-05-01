using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Crop : MonoBehaviour
{
    [SerializeField] protected Sprite[] growthStages;
    protected SpriteRenderer spriteRenderer;
    protected Field currentField;

    [SerializeField] protected float growTime;
    [SerializeField] protected float growthSpeed = 1.0f;

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
    }
    [SerializeField] float waterLossRate = 1.0f;

    public bool IsReady { get; protected set; }
    protected bool isGrowing;

    void Awake()
    {
        currentField = GetComponentInParent<Field>();
    }

    protected virtual void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        spriteRenderer.sprite = growthStages[0];
        StartCoroutine(ManageFertiliser());
        StartCoroutine(ManageWater());
        StartCoroutine(Growing());
    }

    protected virtual IEnumerator Growing()
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
                growTimeLeft -= growthSpeed * Time.deltaTime;

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



    IEnumerator ManageFertiliser()
    {
        bool changedSpeed = false;
        while (true)
        {
            if (currentField.isFertilised && isGrowing)
            {
                if (currentField.fertiliserTimeLeft > 0)
                {
                    if (!changedSpeed)
                    {
                        growthSpeed *= 2;
                        changedSpeed = true;
                    }
                    currentField.fertiliserTimeLeft -= Time.deltaTime;
                    yield return null;
                } else
                {
                    growthSpeed /= 2;
                    currentField.fertiliserTimeLeft = currentField.fertiliserTimer;
                    changedSpeed = false;
                    currentField.isFertilised = false;
                    currentField.fertilisedIndicator.SetActive(false);
                    yield return null;
                }
            } else
            {
                yield return null;
            }
        }
    }

    IEnumerator ManageWater()
    {
        bool changedSpeed = false;
        while (true)
        {
            if (waterLevel > 0)
            {
                if (!IsReady)
                {
                    if (!isGrowing)
                    {
                        isGrowing = true;
                        currentField.waterIcon.SetActive(false);
                    }
                    if (waterLevel >= 4 && changedSpeed)
                    {
                        growthSpeed *= 2;
                        changedSpeed = false;
                        spriteRenderer.color = Color.white;
                    }
                    else if (waterLevel < 4 && !changedSpeed)
                    {
                        growthSpeed /= 2;
                        changedSpeed = true;
                        spriteRenderer.color = new Color(0.89f, 0.68f, 0, 1); // some shade of orange
                    }
                    WaterLevel -= waterLossRate * Time.deltaTime;
                }
                yield return null;
            } else
            {
                if (isGrowing)
                {
                    isGrowing = false;
                    currentField.waterIcon.SetActive(true);
                }
                yield return null;
            }
        }
    }
}
