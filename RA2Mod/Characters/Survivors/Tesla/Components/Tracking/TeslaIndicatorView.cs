using RA2Mod.Survivors.Tesla;
using System;
using UnityEngine;

public class TeslaIndicatorView : MonoBehaviour {

    public static Sprite[] rangeSprites => TeslaAssets.rangeSprites;
    public static Sprite allySprite => TeslaAssets.allySprite;
    public static Sprite towerSprite => TeslaAssets.towerSprite;

    public static Color[] targetColors = new Color[] { Color.cyan,
                                                       Color.red,
                                                       Color.green };

    public SpriteRenderer indicatorRenderer;
    public GameObject towerIndicator;

    public void SetColor(int currentTarget) {

        indicatorRenderer.color = targetColors[currentTarget];
    }

    public void SetSpriteAlly() {
        indicatorRenderer.sprite = allySprite;
    }
    public void SetSpriteTower() {
        indicatorRenderer.sprite = towerSprite;
    }
    public void SetSpriteRange(int currentRange) {
        indicatorRenderer.sprite = rangeSprites[currentRange];
    }

    public void SetTowerIndicator(bool hasTower) {
        towerIndicator.SetActive(hasTower);
    }
}
