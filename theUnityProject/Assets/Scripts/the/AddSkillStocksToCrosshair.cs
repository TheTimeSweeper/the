using RoR2.UI;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using static RoR2.UI.CrosshairController;
using RoR2;

public class AddSkillStocksToCrosshair : MonoBehaviour
{
    [SerializeField]
    private GameObject[] stonks;
    [SerializeField]
    private SkillSlot skillSlot;
    [SerializeField]
    private int interval = 1;
    [SerializeField]
    private int max = 100;

    [ContextMenu("add to crosshairController")]
    public void transfer() {
        CrosshairController crosshairController = GetComponent<CrosshairController>();
        List<SkillStockSpriteDisplay> displays = crosshairController.skillStockSpriteDisplays.ToList();
#if UNITY_EDITOR
        UnityEditor.Undo.RecordObject(crosshairController, "crosshair stonks");
#endif
        for (int i = 0; i < stonks.Length; i++) {

            displays.Add(new SkillStockSpriteDisplay {
                minimumStockCountToBeValid = (i + 1) * interval,
                target = stonks[i],
                maximumStockCountToBeValid = max,
                skillSlot = skillSlot,
            });
        }

        crosshairController.skillStockSpriteDisplays = displays.ToArray();
    }

}
