using RoR2.UI;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using static RoR2.UI.CrosshairController;

public class AddSkillStocksToCrosshair : MonoBehaviour
{
    [SerializeField]
    private Transform stonkGrid;
    [SerializeField]
    private CrosshairController crosshairController;

    [ContextMenu("add to crosshairController")]
    public void transfer() {

        List<SkillStockSpriteDisplay> displays = crosshairController.skillStockSpriteDisplays.ToList();
        SkillStockSpriteDisplay firstDisplay = crosshairController.skillStockSpriteDisplays[0];

        for (int i = 0; i < stonkGrid.childCount; i++) {
            SkillStockSpriteDisplay display = firstDisplay;
            display.minimumStockCountToBeValid = i + 1;
            display.target = stonkGrid.GetChild(i).gameObject;
            displays.Add(display);
        }

        crosshairController.skillStockSpriteDisplays = displays.ToArray();
    }

}
