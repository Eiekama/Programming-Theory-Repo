using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MultiHarvest : Crop
{
    [SerializeField] GameObject grownForm;

    protected override void Start()
    {
        grownForm.SetActive(false);
        base.Start();
    }

    public override void Harvest()
    {
        base.Harvest();
        harvestForm.SetActive(false);
        grownForm.SetActive(true);
        StartCoroutine(Growing());
    }
}
