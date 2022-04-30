using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OnSelected : MonoBehaviour
{
    [SerializeField] Sprite icon;
    [SerializeField] Sprite iconWhenSelected;
    Image image;
    Button button;
    bool isRunning;

    void Start()
    {
        image = GetComponent<Image>();
        button = GetComponent<Button>();
        button.onClick.AddListener(WhenSelected);
    }

    public void WhenSelected()
    {
        if (!isRunning)
        {
            StartCoroutine(WhenSelectedCoroutine());
        }
    }

    IEnumerator WhenSelectedCoroutine()
    {
        isRunning = true;
        image.sprite = iconWhenSelected;
        PlayerActions.Instance.cursorRenderer.sprite = icon;
        while (PlayerActions.Instance.currentAction != PlayerActions.Action.None)
        {
            yield return null;
        }
        image.sprite = icon;
        PlayerActions.Instance.cursorRenderer.sprite = null;
        isRunning = false;
    }
}
