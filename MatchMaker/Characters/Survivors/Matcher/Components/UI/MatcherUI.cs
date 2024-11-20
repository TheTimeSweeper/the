using RoR2.UI;
using MatcherMod.Modules.UI;
using Matchmaker.MatchGrid;
using RoR2.Skills;
using UnityEngine;
using System;

namespace MatcherMod.Survivors.Matcher.Components.UI
{
    public class MatcherUI : MonoBehaviour, ICompanionUI<MatcherGridController>
    {
        [SerializeField]
        private MatchGrid matchGrid;
        public MatchGrid MatchGrid => matchGrid;

        [SerializeField]
        private MPEventSystemLocator eventSystemLocator;

        public MatcherGridController controller;

        public bool Created { get; private set; }
        public bool Showing { get; private set; } = true;

        public RectTransform notificationArea;

        public const float NOTIFICATION_SHIFT = 300f;
        private float notificationOriginalY = 98.9f;

        void ICompanionUI<MatcherGridController>.OnInitialize(MatcherGridController hasUIComponent, HUD hud)
        {
            controller = hasUIComponent;
            notificationArea = hud.transform.Find("MainContainer/NotificationArea")?.GetComponent<RectTransform>();
            if(notificationArea != null)
            {
                notificationOriginalY = notificationArea.anchoredPosition.y;
                if (Mathf.Abs(notificationArea.anchoredPosition.y - 98.9f) > 10) // it's been moved
                {
                    notificationArea = null;
                }
            }

            Show(false);
        }

        public void Init(MatchTileType[] tileTypes, SpecialTileInfo[] SpecialTileTypes, Vector2Int gridSize)
        {
            Created = true;

            matchGrid.OnMatchAwarded = controller.OnMatchAwarded;

            InitGrid(tileTypes, SpecialTileTypes, gridSize);
        }

        public void InitGrid(MatchTileType[] tileTypes, SpecialTileInfo[] SpecialTileTypes, Vector2Int gridSize)
        {
            matchGrid.Init(tileTypes, SpecialTileTypes, gridSize);
        }

        public void OnUIUpdate()
        {

        }

        public void Show() => Show(!Showing);
        public void Show(bool shouldShow, Action<float> extendAction = null)
        {
            if (shouldShow == Showing)
                return;
            eventSystemLocator.eventSystem.SetSelectedGameObject(shouldShow ? gameObject : null);
            Showing = shouldShow;
            gameObject.SetActive(shouldShow);
            if(notificationArea != null)
            {
                notificationArea.anchoredPosition = new Vector2(notificationArea.anchoredPosition.x, notificationOriginalY + (shouldShow ? NOTIFICATION_SHIFT : 0));
            }
            matchGrid.timeStopAction = shouldShow ? extendAction : null;
        }

        public void OnBodyLost()
        {
            Show(false);
            Destroy(gameObject);
        }

        public void OnBodyUnFocused()
        {
            Show(false);
        }
    }
}
