using System.Collections.Generic;
using UnityEngine;

public class BaseUnattachedAnimator : MonoBehaviour
{

    [System.Serializable]
    public class UnattachedAnimation
    {
        public string animationState;
        public string layerName;
    }

    [System.Serializable]
    public class UnattachedAnimations
    {
        public List<UnattachedAnimation> animations;
    }

    [System.Serializable]
    public class UnattachedAnimationCombo
    {
        public string name;
        public KeyCode keyCode;
        public List<UnattachedAnimations> comboAnimations;
        [HideInInspector]
        public float comboTimer;
        [HideInInspector]
        public int comboStep;
    }

    [SerializeField]
    protected Animator[] animators;

    [SerializeField, Space]
    List<UnattachedAnimationCombo> animationCombos;

    [Header("whyt he fuck aren't these in the animator")]
    [SerializeField, Range(0, 0.999f)]
    protected float aimPitch = 0.5f;
    [SerializeField, Range(0, 0.999f)]
    protected float aimYaw = 0.5f;

    protected float _combatTim;
    protected float _jumpTim;

    protected virtual void Update()
    {
        _combatTim -= Time.deltaTime;

        for (int i = 0; i < animators.Length; i++)
        {
            Moob(animators[i]);
            Jumb(animators[i]);
            Shooting(animators[i]);
            Aiming(animators[i]);
            Combat(animators[i]);
        }
    }

    private void Moob(Animator animator)
    {
        //man it's been so long since I've written a moob function

        float hori = Input.GetAxis("Horizontal");
        float veri = Input.GetAxis("Vertical");

        animator.SetBool("isMoving", Mathf.Abs(Input.GetAxisRaw("Horizontal")) + Mathf.Abs(Input.GetAxisRaw("Vertical")) > 0.01f);
        animator.SetFloat("forwardSpeed", veri);
        animator.SetFloat("rightSpeed", hori);

        animator.SetBool("isSprinting", Input.GetKey(KeyCode.LeftControl));
    }

    private void Jumb(Animator animator)
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            animator.Play("Jump");
            animator.SetBool("isGrounded", false);
            _jumpTim = 1.5f;
        }

        _jumpTim -= Time.deltaTime;

        animator.SetFloat("upSpeed", Mathf.Lerp(-48, 16, _jumpTim / 2f));

        if (_jumpTim <= 0)
        {
            if (!animator.GetBool("isGrounded"))
            {
                animator.Play("LightImpact", 1);
            }
            animator.SetBool("isGrounded", true);
        }
    }

    private void Combat(Animator animator)
    {
        animator.SetBool("inCombat", _combatTim > 0);
    }

    private void Aiming(Animator animator)
    {
        if (Input.GetKeyDown(KeyCode.Q))
            aimYaw += 0.2f;

        if (Input.GetKeyDown(KeyCode.E))
            aimYaw -= 0.2f;

        aimYaw = Mathf.Clamp(aimYaw, 0, 0.999f);

        animator.SetFloat("aimYawCycle", aimYaw);
        animator.SetFloat("aimPitchCycle", aimPitch);
    }

    protected virtual void Shooting(Animator animator)
    {
        for (int i = 0; i < animationCombos.Count; i++)
        {
            RunCombo(animator, animationCombos[i]);
        }
    }

    protected virtual void RunCombo(Animator animator, UnattachedAnimationCombo combo)
    {
        if (Input.GetKeyDown(combo.keyCode))
        {
            List<UnattachedAnimation> animations = combo.comboAnimations[combo.comboStep].animations;
            for (int i = 0; i < animations.Count; i++)
            {
                animator.Play(animations[i].animationState, animator.GetLayerIndex(animations[i].layerName));
            }

            combo.comboTimer = 2;

            combo.comboStep++;
            if (combo.comboStep >= combo.comboAnimations.Count)
            {
                combo.comboStep = 0;
            }

            _combatTim = 2;
        }

        combo.comboTimer -= Time.deltaTime;

        if (combo.comboTimer <= 0)
        {
            combo.comboStep = 0;
        }
    }
}
