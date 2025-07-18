using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerManager : MonoBehaviour
{
    private MovementManager movementManager;

    [SerializeField] InputActionReference Player_Movement, Player_Jump;

    private Vector2 playerMovementInput;
    private bool controlsInverted = false;
    private float invertTimer = 0f;


    private void Awake()
    {
        movementManager = GetComponent<MovementManager>();
    }

    private void Update()
    {
        // Gestion du timer d'inversion
        if (controlsInverted)
        {
            invertTimer -= Time.deltaTime;
            if (invertTimer <= 0f)
                controlsInverted = false;
        }

        // Lecture de l'input
        playerMovementInput = Player_Movement.action.ReadValue<Vector2>().normalized;

        // Inversion conditionnelle de l'axe horizontal
        if (controlsInverted)
            playerMovementInput.x *= -1;

        movementManager.MovementInput = playerMovementInput;

        // Gestion du saut
        if (Player_Jump.action.triggered)
        {
            movementManager.JumpBuffered = true;
        }
    }

    public void InvertControlsTemporarily(float duration = 2f)
    {
        Debug.Log("Inversion activée !");
        controlsInverted = true;
        invertTimer = duration;
    }

}
