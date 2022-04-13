using System;
using UnityEngine;

public class TeslaIndicatorView : MonoBehaviour {

    public static Sprite[] rangeSprites = new Sprite[] { Modules.Assets.LoadAsset<Sprite>("texIndicator1Close"),
                                                         Modules.Assets.LoadAsset<Sprite>("texIndicator2Med"),
                                                         Modules.Assets.LoadAsset<Sprite>("texIndicator3Far") };
    public static Sprite allySprite = Modules.Assets.LoadAsset<Sprite>("texIndicatorAlly");

    public static Color[] targetcolors = new Color[] { Color.cyan,
                                                       Color.red,
                                                       Color.green };

    public SpriteRenderer indicatorRenderer;
    public GameObject towerIndicator;

    internal void UpdateColor(int currentTarget) {

        indicatorRenderer.color = targetcolors[currentTarget];
    }

    internal void setSprite(int currentRange) {
        indicatorRenderer.sprite = rangeSprites[currentRange];
    }

    internal void setSpriteAlly() {
        indicatorRenderer.sprite = allySprite;
    }

    internal void setTowerSprite(bool hasTower) {
        towerIndicator.SetActive(hasTower);
    }
}
