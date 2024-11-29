using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
    public float movementSpeed;
    public LayerMask solidObjectsLayer;
    public bool isMoving;
    public LayerMask grassLayer;
    public float encounterCooldown = 3f;
    private float encounterTimer = 0f;

    private Vector2 input;
    private Animator animator;
    private AchievementManager achievementManager; // Reference to AchievementManager

    private void Awake()
    {
        animator = GetComponent<Animator>();
        achievementManager = FindObjectOfType<AchievementManager>();
    }

    private void Update()
    {
        encounterTimer -= Time.deltaTime;

        if (!isMoving && encounterTimer <= 0)
        {
            input.x = Input.GetAxisRaw("Horizontal");
            input.y = Input.GetAxisRaw("Vertical");

            if (input.x != 0) input.y = 0;

            if (input != Vector2.zero)
            {
                animator.SetFloat("moveX", input.x);
                animator.SetFloat("moveY", input.y);

                var targetPos = transform.position;
                targetPos.x += input.x;
                targetPos.y += input.y;

                if (IsWalkable(targetPos))
                {
                    StartCoroutine(Move(targetPos));
                    if (achievementManager != null)
                    {
                        achievementManager.IncrementSteps(); // Increment steps for achievement
                    }
                }
            }
        }

        animator.SetBool("isMoving", isMoving);
    }

    IEnumerator Move(Vector3 targetPos)
    {
        isMoving = true;
        while ((targetPos - transform.position).sqrMagnitude > Mathf.Epsilon)
        {
            transform.position = Vector3.MoveTowards(transform.position, targetPos, movementSpeed * Time.deltaTime);
            yield return null;
        }

        transform.position = targetPos;
        isMoving = false;

        CheckForEncounters();
    }

    private bool IsWalkable(Vector3 targetPos)
    {
        return Physics2D.OverlapCircle(targetPos, 0.2f, solidObjectsLayer) == null;
    }

    private void CheckForEncounters()
    {
        if (Physics2D.OverlapCircle(transform.position, 0.2f, grassLayer) != null)
        {
            if (UnityEngine.Random.Range(1, 101) <= 10) // Explicitly use UnityEngine.Random
            {
                UnityEngine.Debug.Log("Battle Has Started"); // Explicitly use UnityEngine.Debug
                SceneManager.LoadScene("BattleScene");
            }
        }
    }

    public void ResetEncounterTimer()
    {
        encounterTimer = encounterCooldown;
    }
}
