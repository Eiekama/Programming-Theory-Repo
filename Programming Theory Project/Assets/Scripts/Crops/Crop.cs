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
    float growthSpeed = 1.0f;
    bool isGrowing;
    public bool IsReady { get; private set; }

    void Awake()
    {
        currentField = GetComponentInParent<Field>();
    }

    protected virtual void Start()
    {
        seedForm.SetActive(true);
        harvestForm.SetActive(false);
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
}
