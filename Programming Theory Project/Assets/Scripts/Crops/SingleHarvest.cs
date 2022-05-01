using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SingleHarvest : Crop
{
    public override void Harvest()
    {
        base.Harvest();
        currentField.CurrentCrop = null;
        if (currentField.waterIcon.activeInHierarchy)
        {
            currentField.waterIcon.SetActive(false);
        }
        Destroy(gameObject);
    }
}
