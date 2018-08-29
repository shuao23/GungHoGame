using UnityEngine;
using UnityEngine.UI;

public class DisplayRoboHealth : MonoBehaviour {

    [SerializeField]
    private Character character;
    [SerializeField]
    private Text text;

    public void Update()
    {
        text.text = character.Health.CurrentHealth.ToString();
    }
}

