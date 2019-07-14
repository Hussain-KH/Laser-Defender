using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour {
    
    // configuration parameters

    [Header("Player")]
    [SerializeField] float moveSpeed = 10f;
    [SerializeField] float padding = 0.5f;
    [SerializeField] int health = 200;
    [SerializeField] AudioClip deathSound;
    [SerializeField] [Range(0, 1)] float deathSoundVolume = 0.75f;
    [SerializeField] AudioClip shootSound;
    [SerializeField] [Range(0, 1)] float shootSoundVolume = 0.75f;

    [Header("Projectile")]
    [SerializeField] GameObject playerLaserPrefab;
    [SerializeField] float projectileSpeed = 10f;
    [SerializeField] float projectileFiringPeriod = 0.1f;

    Coroutine firingCoroutine;
    Level level;

    float MinX;
    float MaxX;
    float MinY;
    float MaxY;

    // Use this for initialization
    void Start () {

        level = FindObjectOfType<Level>();
        SetUpmoveBoumdaries();
    }


    // Update is called once per frame
    void Update () {

        Move();
        Fire();        
	}

    private void OnTriggerEnter2D(Collider2D other)
    {
        DamageDealer damageDealer = other.GetComponent<DamageDealer>();
        if (!damageDealer) { return; }
        ProcessHit(damageDealer);
    }

    private void ProcessHit(DamageDealer damageDealer)
    {
        health -= damageDealer.GetDamage();
        damageDealer.Hit();

        if (health <= 0)
        {
            Die();
        }
    }

    public int GetHealth()
    {
        return health;
    }

    private void Die()
    {
        level.GameOver();
        Destroy(gameObject);
        AudioSource.PlayClipAtPoint(deathSound, Camera.main.transform.position, deathSoundVolume);
    }

    private void Fire()
    {
        if (Input.GetButtonDown("Fire1"))
        {
            firingCoroutine = StartCoroutine(FireContinuously());
        }
        if (Input.GetButtonUp("Fire1"))
        {
            StopCoroutine(firingCoroutine);
        }
    }

    IEnumerator FireContinuously()
    {
        while (true)
        {
            GameObject playerLaser = Instantiate(
            playerLaserPrefab,
            transform.position,
            Quaternion.identity) as GameObject;
            playerLaser.GetComponent<Rigidbody2D>().velocity = new Vector2(0, projectileSpeed);
            AudioSource.PlayClipAtPoint(shootSound, Camera.main.transform.position, shootSoundVolume);

            yield return new WaitForSeconds(projectileFiringPeriod);
        }
    }

    private void Move()
    {
        var deltaX = Input.GetAxis("Horizontal") * Time.deltaTime * moveSpeed;
        var deltaY = Input.GetAxis("Vertical") * Time.deltaTime * moveSpeed;

        var newXpos = Mathf.Clamp(transform.position.x + deltaX, MinX, MaxX);
        var newYpos = Mathf.Clamp(transform.position.y + deltaY, MinY, MaxY);
        transform.position = new Vector2(newXpos, newYpos);
    }

    private void SetUpmoveBoumdaries()
    {
        Camera gameCamera = Camera.main;

        MinX = gameCamera.ViewportToWorldPoint(new Vector3(0, 0, 0)).x + padding;
        MaxX = gameCamera.ViewportToWorldPoint(new Vector3(1, 0, 0)).x - padding;

        MinY = gameCamera.ViewportToWorldPoint(new Vector3(0, 0, 0)).y + padding;
        MaxY = gameCamera.ViewportToWorldPoint(new Vector3(0, 1, 0)).y - padding;
    }
}
