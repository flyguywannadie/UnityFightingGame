using System.Collections.Generic;
using UnityEngine;

public class UIInputBuffer : MonoBehaviour
{
    [SerializeField] private UIBufferedInput[] inputs;
    public bool Activated = false;

    private void Start()
    {
        ClearUI();
        if (Activated)
        {
            TurnOn();
        } else
        {
            TurnOff();
        }
    }

    public void Toggle()
    {
        Activated = !Activated;
        if (Activated)
        {
            TurnOn();
        }
        else
        {
            TurnOff();
        }
    }

    public void TurnOn()
    {
        gameObject.SetActive(true);
        Activated = true;
    }

    public void TurnOff()
    {
        gameObject.SetActive(false);
        Activated = false;
    }

    public void UpdateUI(List<BufferedInput> bis)
    {
        if (!Activated)
        {
            return;
        }

        for (int i = 0; i < bis.Count; i++)
        {
            if (i >= inputs.Length)
            {
                return;
            }
            inputs[i].VisualizeInput(bis[i]);
        }
    }

    public void ClearUI()
    {
        foreach (var input in inputs)
        {
            input.Clear();
        }
    }
}
