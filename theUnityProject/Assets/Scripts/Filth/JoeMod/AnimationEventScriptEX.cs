using UnityEngine;

    public class AnimationEventScriptEX : MonoBehaviour
    {
        //public PlayerControllerEX guy;
        public GameObject beem, light;
        public Color color;

        void Start()
        {
            //guy = transform.parent.GetComponent<PlayerControllerEX>();
        }

        public int colorState;
        public float colorSpeed;
        void Update()
        {
            color = beem.GetComponent<Xft.XWeaponTrail>().MyColor;

            if (colorState == 0)
            {
                if (color.g < 1.0f) { color.g += colorSpeed * Time.deltaTime; }
                else { color.g = 1.0f; colorState = 1; }
                if (color.b > 0.0f) { color.b -= colorSpeed * Time.deltaTime; }
            }
            if (colorState == 1)
            {
                if (color.b < 1.0f) { color.b += colorSpeed * Time.deltaTime; }
                else { color.b = 1.0f; colorState = 2; }
                if (color.r > 0.0f) { color.r -= colorSpeed * Time.deltaTime; }
            }
            if (colorState == 2)
            {
                if (color.r < 1.0f) { color.r += colorSpeed * Time.deltaTime; }
                else { color.r = 1.0f; colorState = 0; }
                if (color.g > 0.0f) { color.g -= colorSpeed * Time.deltaTime; }
            }

            beem.GetComponent<Xft.XWeaponTrail>().MyColor = color;
            light.GetComponent<Light>().color = color;

            if (on)
            {
                if (ON)
                {
                    light.GetComponent<Light>().intensity = 6.0f;

                }
                else
                {
                    light.GetComponent<Light>().intensity = 2.0f;
                }
            }
            else
            {
                if (light.GetComponent<Light>().intensity > 2)
                {
                    light.GetComponent<Light>().intensity -= 12.0f * Time.deltaTime;
                }
                if (light.GetComponent<Light>().intensity > 0.0f)
                {
                    light.GetComponent<Light>().intensity -= 4.0f * Time.deltaTime;
                }
            }
        }

        void swing1(int set)
        {
            //if (set == 1) guy.secondswing = true; else guy.secondswing = false;
            if (set == 69) print("interesting");
        }

        bool on;
        bool ON;
        public void swingEffect(int set)
        {
            //		beem = transform.Find ("Char1_pelivs").GetComponentInChildren <ParticleSystem>();
            if (set == 0)
            {
                colorSpeed = 5;
                on = true;
            }
            if (set == 69)
            {
                colorSpeed = 12;
                on = true;
            }
            if (set == 169)
            {
            //ew gameplay on animation events
                //guy.jumpHit = true;
                ON = true;
            }

            if (set == 1)
            {
                colorSpeed = 0;
                on = false;
                ON = false;
            }
        }

        void jump(int set)
        {
            if (set == 0)
            {
                //guy.animFind.SetInteger("jumpState", 0);
                //guy.anim.SetFloat("jumpSwing", 0);
                //guy.jumpSwing = false;
            }
            if (set == 1)
            {
                //guy.direction2.y = guy.jSpeed;
            }
        }

        void chargeAttack(int ev)
        {
            //guy.animFind.SetInteger("chargeAttack", ev);

            if (ev == 0)
            {
                //guy.chargeAttack = false;
            }


        }

    }
