using UnityEngine;

public class PlayerDeath : MonoBehaviour
{
    [Header("Death Settings")]
    [SerializeField] private GameObject deathVFXPrefab;
    [SerializeField] private AudioClip deathSFX;
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private float minVelocityToDie = 0.1f;
    [SerializeField] private float idleTimeToDie = 0.5f;

    [Header("Timing")]
    [SerializeField] private float gracePeriod = 1.5f;
    private float spawnTime;

    private Rigidbody2D rb;
    private float idleTimer = 0f;
    private bool isDead = false;
    public bool IsDead => isDead;
    PlatformGenerator platformGenerator;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
            audioSource = gameObject.AddComponent<AudioSource>();
    }

    private void Start()
    {
        spawnTime = Time.time;
    }

    private void Update()
    {
        if (isDead || Time.time - spawnTime < gracePeriod) return;

        if (Mathf.Abs(rb.velocity.y + 0.5f) < minVelocityToDie)
        {
            idleTimer += Time.deltaTime;
            if (idleTimer >= idleTimeToDie)
            {
                Die();
            }
        }
        else
        {
            idleTimer = 0f;
        }

        if (transform.position.y < -1f)
        {
            Die();
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (isDead || Time.time - spawnTime < gracePeriod) return;

        if (collision.collider.CompareTag("Explosion"))
        {
            Die();
        }
    }


    public void Die()
    {
        if (isDead) return;
        isDead = true;

        if (deathVFXPrefab != null)
            Instantiate(deathVFXPrefab, transform.position, Quaternion.identity);

        if (deathSFX != null)
            audioSource.volume = 0.1f;
            audioSource.PlayOneShot(deathSFX);

        foreach (Transform child in transform)
        {
            var sr = child.GetComponent<SpriteRenderer>();
            var col = child.GetComponent<Collider2D>();
            if (sr != null) sr.enabled = false;
            if (col != null) col.enabled = false;
        }

        rb.simulated = false;

        Debug.Log("[PlayerDeath] Je vais appeler le GameManager");
        GameManager.Instance?.OnPlayerDeath(gameObject);

        // ⚠️ On désactive juste après, PAS avant
        Invoke(nameof(DisableSelf), 0.1f); // Laisse un petit temps pour que le GameManager finisse son taf
    }

    public void HideVisuals()
    {
        foreach (Transform child in transform)
        {
            var sr = child.GetComponent<SpriteRenderer>();
            if (sr != null) sr.enabled = false;
        }
    }

    public void ShowVisuals()
    {
        foreach (Transform child in transform)
        {
            var sr = child.GetComponent<SpriteRenderer>();
            if (sr != null) sr.enabled = true;
        }
    }

    private void DisableSelf()
    {
        HideVisuals();
    }

}
