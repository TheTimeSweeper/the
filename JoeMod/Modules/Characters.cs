﻿using RoR2;
using UnityEngine;

namespace Modules.Characters {

    // for simplifying characterbody creation
    public class BodyInfo {
        public string bodyName = "";
        public string bodyNameToken = "";
        public string subtitleNameToken = "";
        /// <summary>
        /// body prefab you're cloning for your character- commando is the safest
        /// </summary>
        public string bodyNameToClone = "Commando";

        public Color bodyColor = Color.grey;

        public Texture characterPortrait = null;

        public GameObject crosshair = null;
        public GameObject podPrefab = null;

        //stats
        public float maxHealth = 100f;
        public float healthRegen = 1f;
        public float armor = 0f;
        /// <summary>
        /// base shield is a thing apparently. neat
        /// </summary>
        public float shield = 0f;

        public float damage = 12f;
        public float attackSpeed = 1f;
        public float crit = 1f;

        public float moveSpeed = 7f;
        public float jumpPower = 15f;

        //growth
        public bool autoCalculateLevelStats = true;

        public float healthGrowth = 30f;
        public float regenGrowth = 0.2f;
        public float shieldGrowth = 0f;
        public float armorGrowth = 0f;

        public float damageGrowth = 2.4f;
        public float attackSpeedGrowth = 0f;
        public float critGrowth = 0f;

        public float moveSpeedGrowth = 0f;
        public float jumpPowerGrowth = 0f;// jump power per level exists for some reason

        //other
        public float acceleration = 80f;
        public int jumpCount = 1;

        //camera
        public Vector3 modelBasePosition = new Vector3(0f, -0.92f, 0f);
        public Vector3 cameraPivotPosition = new Vector3(0f, 1.6f, 0f);
        public Vector3 aimOriginPosition = new Vector3(0f, 2.5f, 0f);

        public float cameraParamsVerticalOffset = 1f;
        public float cameraParamsDepth = -12;

        private CharacterCameraParams _cameraParams;
        public CharacterCameraParams cameraParams {
            get {
                if (_cameraParams == null) {
                    _cameraParams = ScriptableObject.CreateInstance<CharacterCameraParams>();
                    _cameraParams.minPitch = -70;
                    _cameraParams.maxPitch = 70;
                    _cameraParams.wallCushion = 0.1f;
                    _cameraParams.pivotVerticalOffset = cameraParamsVerticalOffset;
                    _cameraParams.standardLocalCameraPos = new Vector3(0, 0, cameraParamsDepth);
                }
                return _cameraParams;
            }
            set => _cameraParams = value;
        }

        public float SurvivorHeightToSet {
            //todo dynamically set good camera params based on a height that you want
            set {
                float height = value;
            }
        }
    }

    // for simplifying rendererinfo creation
    public class CustomRendererInfo {
        public string childName;
        public Material material;
        public bool ignoreOverlays;
    }
}