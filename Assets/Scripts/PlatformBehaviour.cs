using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformBehaviour : MonoBehaviour
{
    public enum PlatformType
    {
        Normal,
        Blinking,
        InvertControls,
        Explosive
    }

    public PlatformType platformType = PlatformType.Normal;

    // ===== Explosive =====
    [Header("Explosive Settings")]
    public float explosionDelay = 3f;
    private bool isExploding = false;
    private float explosionTimer;
    public GameObject ExploPrefab;
    public AudioClip boomClip;
    public AudioSource audioSource;

    // ===== Blinking =====
    [Header("Blinking Settings")]
    public float visibleDuration = 4f;
    public float invisibleDuration = 2f;
    private float blinkTimer;
    private bool isVisible = true;

    private SpriteRenderer sr;
    private Collider2D col;

    void Awake()
    {
        sr = GetComponent<SpriteRenderer>();
        col = GetComponent<Collider2D>();
    }

    void Update()
    {
        switch (platformType)
        {
            case PlatformType.Blinking:
                HandleBlinking();
                break;
            case PlatformType.Explosive:
                if (isExploding)
                    HandleExplosion();
                break;
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        PlayerManager pm = collision.collider.GetComponentInParent<PlayerManager>();
        if (pm == null) return;

        switch (platformType)
        {
            case PlatformType.InvertControls:
                pm.InvertControlsTemporarily();
                break;

            case PlatformType.Explosive:
                if (!isExploding)
                {
                    isExploding = true;
                    explosionTimer = explosionDelay;
                }
                break;
        }
    }


    private void HandleExplosion()
    {
        explosionTimer -= Time.deltaTime;
        if (explosionTimer <= 0f)
        {
            GameObject explosion = Instantiate(ExploPrefab, transform.position, Quaternion.identity);
            Destroy(explosion, 1.5f);
            Destroy(gameObject); // BOOM.
        }
    }


    private void HandleBlinking()
    {
        blinkTimer -= Time.deltaTime;
        if (isVisible && blinkTimer <= 0f)
        {
            SetVisible(false);
            blinkTimer = invisibleDuration;
        }
        else if (!isVisible && blinkTimer <= 0f)
        {
            SetVisible(true);
            blinkTimer = visibleDuration;
        }
    }

    private void SetVisible(bool visible)
    {
        isVisible = visible;
        sr.enabled = visible;
        col.enabled = visible;
    }

    void OnEnable()
    {
        if (platformType == PlatformType.Blinking)
        {
            isVisible = true;
            blinkTimer = visibleDuration;
        }
    }
}
