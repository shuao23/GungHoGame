using UnityEngine;
using UnityEngine.UI;

public class DisplayBaseHealth : MonoBehaviour
{

    [SerializeField]
    private MainBase mainBase;
    [SerializeField]
    private Text text;

    public void Update()
    {
        text.text = mainBase.Health.CurrentHealth.ToString();
    }
}

