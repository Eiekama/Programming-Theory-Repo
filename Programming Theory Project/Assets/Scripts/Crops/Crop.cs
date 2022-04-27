using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Crop : MonoBehaviour
{
    [SerializeField] protected GameObject seedForm;
    [SerializeField] protected GameObject harvestForm;
    protected Field currentField;

    public enum Hydration
    {
        Hydrated,
        NeedWater,
        Dehydrated
    }
    public Hydration hydration;

    [SerializeField] float growTime;
    [SerializeField] float waterLossRate;
    [SerializeField] float growthSpeed = 1.0f;

    public bool IsReady { get; private set; }
    bool isGrowing;

    void Awake()
    {
        currentField = GetComponentInParent<Field>();
    }

    protected virtual void Start()
    {
        seedForm.SetActive(true);
        harvestForm.SetActive(false);
        StartCoroutine(WhenFertilised());
        StartCoroutine(Growing());
    }

    protected IEnumerator Growing()
    {
        isGrowing = true;
        IsReady = false;

        var growTimeLeft = growTime;
        while (growTimeLeft > 0)
        {
            if (isGrowing)
            {
                growTimeLeft -= growthSpeed * Time.deltaTime;
                yield return null;
            }
        }

        seedForm.SetActive(false);
        harvestForm.SetActive(true);
        isGrowing = false;
        IsReady = true;
        currentField.icons[0].SetActive(true);
    }

    public virtual void Harvest()
    {
        Debug.Log("Crop was harvested");
        currentField.icons[0].SetActive(false);
    }

    IEnumerator WhenFertilised()
    {
        var originalGrowthSpeed = growthSpeed;
        while (true)
        {
            if (currentField.isFertilised && isGrowing)
            {
                if (currentField.fertiliserTimeLeft > 0)
                {
                    if (originalGrowthSpeed == growthSpeed)
                    {
                        growthSpeed *= 2;
                    }
                    currentField.fertiliserTimeLeft -= Time.deltaTime;
                    yield return null;
                } else
                {
                    growthSpeed = originalGrowthSpeed;
                    currentField.fertiliserTimeLeft = currentField.fertiliserTimer;
                    currentField.isFertilised = false;
                    yield return null;
                }
            } else
            {
                yield return null;
            }
        }
    }
}
