using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour
{
    private Vector3 moveDirection = Vector3.forward;
    private bool turnLeftNext = true;

    private bool hasStartedMovement = false;
    private bool canTurn = false;
    private bool isTurnCooldown = false; // ✅ New flag

    void Update()
    {
        if (!GameManager.Instance.hasGameStarted)
            return;

        if (!hasStartedMovement)
        {
            hasStartedMovement = true;
            StartCoroutine(AllowTurningAfterDelay(1.5f));
        }

        float currentSpeed = GameManager.Instance.playerSpeed;


        transform.Translate(moveDirection * currentSpeed * Time.deltaTime, Space.World);

        // ✅ Tap + cooldown check
        if (canTurn && !isTurnCooldown && (Input.GetMouseButtonDown(0) || Input.touchCount > 0))
        {
            float angle = turnLeftNext ? -90f : 90f;
            transform.Rotate(0, angle, 0);
            moveDirection = Quaternion.Euler(0, angle, 0) * moveDirection;
            turnLeftNext = !turnLeftNext;

            // Start cooldown
            StartCoroutine(TurnCooldown());
        }

        if (transform.position.y < -2f)
        {
            GameManager.Instance.GameOver();
        }
    }

    IEnumerator AllowTurningAfterDelay(float delay)
    {
        Debug.Log("⏳ Waiting " + delay + " seconds to allow turning...");
        yield return new WaitForSeconds(delay);
        canTurn = true;
        Debug.Log("✅ Turning is now allowed!");
    }

    IEnumerator TurnCooldown()
    {
        isTurnCooldown = true;
        yield return new WaitForSeconds(0.25f); // Adjust as needed
        isTurnCooldown = false;
    }
}
