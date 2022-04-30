using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
#if UNITY_EDITOR
using UnityEditor;
#endif

public class UIHandler : MonoBehaviour
{
    [SerializeField] GameObject mainUI;
    [SerializeField] GameObject plantUI;

    public void GoToPlantUI()
    {
        mainUI.SetActive(false);
        plantUI.SetActive(true);
    }

    public void IsWatering()
    {
        PlayerActions.Instance.currentAction = PlayerActions.Action.Water;
    }

    public void IsFertilising()
    {
        PlayerActions.Instance.currentAction = PlayerActions.Action.Fertilise;
    }

    public void IsRemoving()
    {
        PlayerActions.Instance.currentAction = PlayerActions.Action.Remove;
    }

    public void QuitGame()
    {
#if UNITY_EDITOR
        EditorApplication.ExitPlaymode();
#else
        Application.Quit();
#endif
    }

    public void IsPlanting(GameObject crop)
    {
        PlayerActions.Instance.currentAction = PlayerActions.Action.Plant;
        PlayerActions.Instance.CropPrefab = crop;
    }

    public void GoToMainUI()
    {
        plantUI.SetActive(false);
        mainUI.SetActive(true);
    }
}
