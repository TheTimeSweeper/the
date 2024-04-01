using HellDiverMod.General.Components.UI;
using System.Collections.Generic;
using UnityEngine;

namespace HellDiverMod.Survivors.HellDiver.Components.UI
{
    public class HellDiverUI : MonoBehaviour, ICompanionUI<StratagemInputController>
    {
        [SerializeField]
        private CanvasGroup stratagemContainer;
        [SerializeField]
        private Transform stratagemGrid;
        public StratagemUIEntry entryPrefab;

        private List<StratagemUIEntry> entries = new List<StratagemUIEntry>();

        private StratagemInputController inputController;

        public void OnInitialize(StratagemInputController companionComponent)
        {
            inputController = companionComponent;
            int i = 0;
            for (; i < inputController.sequences.Count; i++)
            {
                if(i >= entries.Count)
                {
                    entries.Add(Instantiate(entryPrefab, stratagemGrid));
                }
                entries[i].Init(inputController.sequences[i]);
            }
            for (; i < entries.Count; i++)
            {
                entries[i].Show(false);
            }
        }

        public void OnUIUpdate() { }

        public void Show(bool shouldShow)
        {
            stratagemContainer.alpha = shouldShow? 1: 0;
        }

        public void UpdateSequence(int i, bool inputSuccess, int progress)
        {
            entries[i].UpdateInput(inputSuccess, progress);
        }

        public void UpdateComplete(int completed)
        {
            for (int i = 0; i < entries.Count; i++)
            {
                entries[i].UpdateComplete(i == completed);
            }
        }

        public void Reset()
        {
            for (int i = 0; i < entries.Count; i++)
            {
                entries[i].Reset();
            }
        }
    }
}
