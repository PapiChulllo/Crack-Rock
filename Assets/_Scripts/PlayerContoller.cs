using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float movementSpeed = 3f;  // Adjust the speed to control the character's movement
    public LayerMask solidObjectsLayer;
    public LayerMask grassLayer;

    private Vector2 input;
    private Animator animator;
    private MusicManager musicManager; // Reference to the MusicManager

    private bool isMoving = false;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        musicManager = FindObjectOfType<MusicManager>();
    }

    private void Update()
    {
        if (!isMoving)
        {
            input.x = Input.GetAxisRaw("Horizontal");
            input.y = Input.GetAxisRaw("Vertical");

            // Ensure only one axis is used at a time
            if (input.x != 0) input.y = 0;

            if (input != Vector2.zero)
            {
                animator.SetFloat("moveX", input.x);
                animator.SetFloat("moveY", input.y);

                // Calculate the target position
                Vector3 targetPos = transform.position;
                targetPos.x += input.x;
                targetPos.y += input.y;

                if (IsWalkable(targetPos))
                {
                    StartCoroutine(Move(targetPos));
                }
            }
        }

        animator.SetBool("isMoving", isMoving);
    }

    private System.Collections.IEnumerator Move(Vector3 targetPos)
    {
        isMoving = true;

        while ((targetPos - transform.position).sqrMagnitude > Mathf.Epsilon)
        {
            transform.position = Vector3.MoveTowards(transform.position, targetPos, movementSpeed * Time.deltaTime);
            yield return null;
        }

        transform.position = targetPos;
        isMoving = false;

        // Check for encounters after completing movement
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
                UnityEngine.Debug.Log("Encounter Started"); // Explicitly use UnityEngine.Debug
                StartEncounter();
            }
        }
    }

    private void StartEncounter()
    {
        if (musicManager != null)
        {
            musicManager.PlayBattleMusic(); // Play battle music
        }

        // Load the battle scene
        UnityEngine.SceneManagement.SceneManager.LoadScene("BattleScene");
    }
}
