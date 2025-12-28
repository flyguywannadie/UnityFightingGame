using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIBufferedInput : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI A;
    [SerializeField] private TextMeshProUGUI B;
    [SerializeField] private TextMeshProUGUI C;
    [SerializeField] private TextMeshProUGUI time;
    [SerializeField] private Image direction;

    [SerializeField] private Sprite[] directions;

    public void VisualizeInput(BufferedInput bi)
    {
        if (bi.Light())
        {
            A.gameObject.SetActive(true);
        } else
        {
            A.gameObject.SetActive(false);
        }

        if (bi.Heavy())
        {
            B.gameObject.SetActive(true);
        }
        else
        {
            B.gameObject.SetActive(false);
        }

        if (bi.Special())
        {
            C.gameObject.SetActive(true);
        }
        else
        {
            C.gameObject.SetActive(false);
        }

        time.text = bi.frames.ToString();

        direction.gameObject.SetActive(true);
        direction.sprite = directions[bi.GetDirection()];
    }

    public void Clear()
    {
        A.gameObject.SetActive(false);
        B.gameObject.SetActive(false);
        C.gameObject.SetActive(false);

        time.text = "";

        direction.gameObject.SetActive(false);
    }
}
