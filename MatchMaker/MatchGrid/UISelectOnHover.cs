using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine.EventSystems;
using UnityEngine;
using RoR2.UI;

namespace Matchmaker.MatchGrid
{
    public class UISelectOnHover : EventTrigger
    {
        private MPEventSystemLocator _eventSystemLocator;

        private void Awake()
        {
            _eventSystemLocator = GetComponent<MPEventSystemLocator>();
        }

        public override void OnPointerEnter(PointerEventData eventData)
        {
            base.OnPointerEnter(eventData);
            SetSelectedGameObject();
        }

        public void SetSelectedGameObject()
        {
            _eventSystemLocator.eventSystem?.SetSelectedGameObject(gameObject);
        }
    }
}
