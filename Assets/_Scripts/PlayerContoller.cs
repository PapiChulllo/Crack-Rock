using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
    public float movementSpeed = 5f;
    public LayerMask solidObjectsLayer;
    public LayerMask grassLayer;

    private Vector2 input;
    private bool isMoving;
    private Animator animator;
    private AchievementManager achievementManager;
    private AudioSource audioSource;

    [Header("Footstep Settings")]
    public AudioClip[] footstepSounds; // Array of footstep sounds
    public float footstepInterval = 0.3f; // Time between footsteps

    private float footstepTimer;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        achievementManager = FindObjectOfType<AchievementManager>();
        audioSource = GetComponent<AudioSource>();
    }

    private void Update()
    {
        HandleMovement();
    }

    private void HandleMovement()
    {
        if (isMoving)
        {
            footstepTimer -= Time.deltaTime;
            if (footstepTimer <= 0)
            {
                PlayFootstep();
                footstepTimer = footstepInterval;
            }
        }

        if (!isMoving)
        {
            input.x = Input.GetAxisRaw("Horizontal");
            input.y = Input.GetAxisRaw("Vertical");

            if (input.x != 0) input.y = 0; // Prioritize horizontal movement

            if (input != Vector2.zero)
            {
                animator.SetFloat("moveX", input.x);
                animator.SetFloat("moveY", input.y);

                Vector3 targetPosition = transform.position + new Vector3(input.x, input.y, 0);
                if (IsWalkable(targetPosition))
                {
                    StartCoroutine(Move(targetPosition));
                }
            }
        }

        animator.SetBool("isMoving", isMoving);
    }

    private IEnumerator Move(Vector3 targetPosition)
    {
        isMoving = true;
        while ((targetPosition - transform.position).sqrMagnitude > Mathf.Epsilon)
        {
            transform.position = Vector3.MoveTowards(transform.position, targetPosition, movementSpeed * Time.deltaTime);
            yield return null;
        }
        transform.position = targetPosition;
        isMoving = false;

        CheckForEncounters();
    }

    private bool IsWalkable(Vector3 targetPosition)
    {
        return Physics2D.OverlapCircle(targetPosition, 0.2f, solidObjectsLayer) == null;
    }

    private void CheckForEncounters()
    {
        if (Physics2D.OverlapCircle(transform.position, 0.2f, grassLayer) != null)
        {
            if (UnityEngine.Random.Range(1, 101) <= 10) // Explicitly use UnityEngine.Random
            {
                UnityEngine.Debug.Log("Battle has started!"); // Explicitly use UnityEngine.Debug
                SceneManager.LoadScene("BattleScene");
            }
        }
    }

    private void PlayFootstep()
    {
        if (footstepSounds.Length > 0 && audioSource != null)
        {
            AudioClip clip = footstepSounds[UnityEngine.Random.Range(0, footstepSounds.Length)];
            audioSource.PlayOneShot(clip);
        }
    }
}
