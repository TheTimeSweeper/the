using System;
using UnityEngine;

public class TeslaIndicatorView : MonoBehaviour {

    public static Sprite[] rangeSprites = new Sprite[] { Modules.Assets.LoadAsset<Sprite>("texIndicator1Close"),
                                                         Modules.Assets.LoadAsset<Sprite>("texIndicator2Med"),
                                                         Modules.Assets.LoadAsset<Sprite>("texIndicator3Far") };
    public static Sprite allySprite = Modules.Assets.LoadAsset<Sprite>("texIndicatorAlly");

    public static Color[] targetColors = new Color[] { Color.cyan,
                                                       Color.red,
                                                       Color.green };

    public SpriteRenderer indicatorRenderer;
    public GameObject towerIndicator;

    public void SetColor(int currentTarget) {

        indicatorRenderer.color = targetColors[currentTarget];
    }

    public void SetSprite(int currentRange) {
        indicatorRenderer.sprite = rangeSprites[currentRange];
    }
    public void SetSpriteAlly() {
        indicatorRenderer.sprite = allySprite;
    }

    public void SetTowerSprite(bool hasTower) {
        towerIndicator.SetActive(hasTower);
    }
}
