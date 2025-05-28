using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float normalSpeed = 5f;
    private float currentSpeed;
    private CharacterController controller;

    void Start()
    {
        controller = GetComponent<CharacterController>();
        currentSpeed = normalSpeed;
    }

    void Update()
    {
        float moveX = Input.GetAxis("Horizontal");
        float moveZ = Input.GetAxis("Vertical");
        Vector3 move = transform.right * moveX + transform.forward * moveZ;

        controller.Move(move * currentSpeed * Time.deltaTime);
    }

    public void ActivateSpeedBoost(float boostMultiplier, float duration)
    {
        StopAllCoroutines(); // voorkomt meerdere boosts tegelijk
        StartCoroutine(SpeedBoostRoutine(boostMultiplier, duration));
    }

    private System.Collections.IEnumerator SpeedBoostRoutine(float multiplier, float duration)
    {
        currentSpeed = normalSpeed * multiplier;
        yield return new WaitForSeconds(duration);
        currentSpeed = normalSpeed;
    }
}