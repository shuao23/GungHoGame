using UnityEngine;
using UnityEngine.UI;

public class DisplayRoboRecharge : MonoBehaviour {

    [SerializeField]
    private Character character;
    [SerializeField]
    private Image image;

    public void Update()
    {
        image.fillAmount = character.RocketRechargePercentage;
    }
}

