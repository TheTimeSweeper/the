using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnattachedAnimator : MonoBehaviour
{
    [SerializeField]
    private Animator teslinator;

    [Header("whyt he fuck aren't these in the animator")]
    [SerializeField, Range(0, 0.999f)]
    private float aimPitch = 0.5f;
    [SerializeField, Range(0, 0.999f)]
    private float aimYaw = 0.5f;

    private bool placingCoil;
    private float combatTim;
    private float jumpTim;

    void Update()
    {
        if (!teslinator)
            return;

        Moob();
        Jumb();
        Shooting();
        Aiming();
        Timers();

        Tim();
    }

    private void Moob() {
        //man it's been so long since I've written a moob function


        Debug.DrawLine(Vector3.zero, Vector3.one * 100);

        float hori = Input.GetAxis("Horizontal");
        float veri = Input.GetAxis("Vertical");

        teslinator.SetBool("isMoving", Mathf.Abs(hori) + Mathf.Abs(veri) > 0.01f);
        teslinator.SetFloat("forwardSpeed", veri);
        teslinator.SetFloat("rightSpeed", hori);
        
        teslinator.SetBool("isSprinting", Input.GetKey(KeyCode.LeftShift));
    }

    private void Jumb() {

        if (Input.GetKeyDown(KeyCode.Space)) {
            teslinator.Play("Jump");
            teslinator.SetBool("isGrounded", false);
            jumpTim = 1.5f;
        }

        jumpTim -= Time.deltaTime;

        teslinator.SetFloat("upSpeed", Mathf.Lerp(-48, 16, jumpTim / 2f));

        if(jumpTim <= 0) {
            teslinator.SetBool("isGrounded", true);
        }
    }

    private void Timers() {
        combatTim -= Time.deltaTime;
        teslinator.SetBool("inCombat", combatTim > 0);
    }

    private void Aiming() {

        if (Input.GetKeyDown(KeyCode.Q))
            aimYaw += 0.2f;

        if (Input.GetKeyDown(KeyCode.E))
            aimYaw -= 0.2f;

        aimYaw = Mathf.Clamp(aimYaw, 0, 0.999f);

        teslinator.SetFloat("aimYawCycle", aimYaw);
        teslinator.SetFloat("aimPitchCycle", aimPitch);
    }

    private void Shooting() {
        if (Input.GetMouseButtonDown(0)) {

            teslinator.Play("HandOut", 2);
            teslinator.Play("Shock", 2);
            combatTim = 2;
        }

        if (Input.GetMouseButtonDown(1)) {

            teslinator.SetBool("isHandOut", true);
        }

        if (Input.GetMouseButtonUp(1)) {

            teslinator.Play("Shock", 2);
            teslinator.SetBool("isHandOut", false);
            combatTim = 2;
        }

        if (!placingCoil) {

            if (Input.GetKeyDown(KeyCode.R)) {

                teslinator.Play("Placing");
                placingCoil = true;
                combatTim = 2;
            }
        } else {

            if (Input.GetKeyDown(KeyCode.R)) {

                teslinator.Play("DoPlace");
                placingCoil = false;
            }
            if (Input.GetKeyDown(KeyCode.LeftShift)) {

                teslinator.Play("CancelPlace");
                placingCoil = false;
            }
        }
    }

    private void setTimeScale(float tim)
    {
        Time.timeScale = tim;

        Debug.Log($"set tim: {Time.timeScale}");
    }

    private void Tim()
    {
        //time keys
        if (Input.GetKeyDown(KeyCode.I))
        {
            if (Time.timeScale == 0)
            {
                setTimeScale(Time.timeScale + 0.1f);
            }
            else
            {
                setTimeScale(Time.timeScale + 0.5f);
            }
        }
        if (Input.GetKeyDown(KeyCode.K))
        {

            setTimeScale(Time.timeScale - 0.1f);
        }
        if (Input.GetKeyDown(KeyCode.O))
        {
            setTimeScale(1);
        }
        if (Input.GetKeyDown(KeyCode.L))
        {
            setTimeScale(0);
        }
    }

}
