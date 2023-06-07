using EntityStates;
using System.Collections.Generic;
using UnityEngine;

namespace JoeModForReal.Components {
    public class ComboRecipeCooker : MonoBehaviour { 

        [System.Serializable]
        public class ComboRecipe {
            public SerializableEntityStateType resultState;
            public List<int> combo;
            public bool resetComboHistory = false;
            public Sprite sprite = null;
        }
        public List<ComboRecipe> comboRecipes = new List<ComboRecipe>();
        private Queue<int> recipeReader = new Queue<int>();
        private Queue<int> historyReader = new Queue<int>();

        void Start () { 

            //initialize setting shit through numbers
        }

        internal ComboRecipe GetCombo(List<int> comboHistory, params int[] additionalHistory) {
            for (int i = 0; i < comboRecipes.Count; i++) {
                if(matchCombo(comboRecipes[i].combo, comboHistory, additionalHistory)) {
                    return comboRecipes[i];
                }
            }
            return null;
        }

        private bool matchCombo(List<int> comboRecipe, List<int> comboHistory, int[] additionalHistory) {

            recipeReader.Clear();
            historyReader.Clear();

            for (int i = additionalHistory.Length - 1; i >= 0; i--) {
                historyReader.Enqueue(additionalHistory[i]);
            }

            for (int i = comboHistory.Count - 1; i >= 0; i--) {
                historyReader.Enqueue(comboHistory[i]);
            }

            for (int i = comboRecipe.Count - 1; i >= 0; i--) {
                recipeReader.Enqueue(comboRecipe[i]);
            }

            bool comboMatched = false;
            while(recipeReader.Count > 0) {
                if (historyReader.Count <= 0) 
                    break;

                if (recipeReader.Dequeue() != historyReader.Dequeue())
                    break;

                if (recipeReader.Count <= 0) {
                    comboMatched = true;
                    break;
                }
            }
            return comboMatched;
        }
    }
}
