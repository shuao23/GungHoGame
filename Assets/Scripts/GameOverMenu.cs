using UnityEngine;
using UnityEngine.UI;

public class GameOverMenu : MonoBehaviour {

    [SerializeField]
    private Character character;
    [SerializeField]
    private Canvas canvas;

    private void Start()
    {
        character.Health.OnDeath += Health_OnDeath;
    }

    private void Health_OnDeath(object sender, System.EventArgs e)
    {
        canvas.enabled = true;
    }
}
