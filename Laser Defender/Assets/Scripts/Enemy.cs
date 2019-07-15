using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour {

    [Header("Enemy Stats")]
    [SerializeField] float health = 100f;
    [SerializeField] int scoreValue = 150;

    [Header("Shooting")]
    [SerializeField] float shotCounter;
    [SerializeField] float minTimeBetweenShots = 0.2f;
    [SerializeField] float maxTimeBetweenShots = 3f;
    [SerializeField] GameObject enemyLaserPrefab;
    [SerializeField] float enemyShootSpeed = 10f;
    [SerializeField] float bossRotate = 10f;

    [Header("Sound Effects")]
    [SerializeField] GameObject deathVFX;
    [SerializeField] float durationOfExplosion = 1f;
    [SerializeField] AudioClip deathSound;
    [SerializeField] [Range(0, 1)] float deathSoundVolume = 0.75f;
    [SerializeField] AudioClip shootSound;
    [SerializeField] [Range(0, 1)] float shootSoundVolume = 0.75f;

    string objectName;
    Player player;
    Vector2 shootDiraction;
    GameSession gameSession;
    EnemySpawner enemySpawner;

    // Use this for initialization
    void Start () {

        enemySpawner = FindObjectOfType<EnemySpawner>();
        gameSession = FindObjectOfType<GameSession>();
        player = FindObjectOfType<Player>();
        objectName = gameObject.name.Replace("(Clone)", "");
        shotCounter = UnityEngine.Random.Range(minTimeBetweenShots, maxTimeBetweenShots);
	}
	
	// Update is called once per frame
	void Update ()
    {
        CountDownAndShoot();
        if (objectName == "Boss")
        {
            transform.Rotate(0, 0, bossRotate);
            scoreValue = gameSession.GetScore();
            enemySpawner.bossAlive = true;
        }
    }

    private void CountDownAndShoot()
    {
        shotCounter -= Time.deltaTime;

        if(shotCounter <= 0)
        {
            Fire();
            shotCounter = UnityEngine.Random.Range(minTimeBetweenShots, maxTimeBetweenShots);
        }
    }
    private void Fire()
    {


        if (objectName == "Boss")
        {
            StartCoroutine(BossDelay());
            if (!player) { return; }
            
        }
        else
        {
            GameObject enemyLaser = Instantiate(
            enemyLaserPrefab,
            transform.position,
            Quaternion.identity) as GameObject;
            enemyLaser.GetComponent<Rigidbody2D>().velocity = new Vector2(0, -enemyShootSpeed);
            AudioSource.PlayClipAtPoint(shootSound, Camera.main.transform.position, shootSoundVolume);
        }

    }
    IEnumerator BossDelay()
    {
        yield return new WaitForSeconds(5);

        GameObject enemyLaser = Instantiate(
        enemyLaserPrefab,
        transform.position,
        Quaternion.identity) as GameObject;
        if (player)
        {
            shootDiraction = (player.transform.position - transform.position).normalized * enemyShootSpeed;
            enemyLaser.GetComponent<Rigidbody2D>().velocity = new Vector2(shootDiraction.x, shootDiraction.y);
            AudioSource.PlayClipAtPoint(shootSound, Camera.main.transform.position, shootSoundVolume);
        }
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

    private void Die()
    {
        enemySpawner.bossAlive = false;
        FindObjectOfType<GameSession>().AddToScore(scoreValue);
        Destroy(gameObject);
        GameObject particleInstance = Instantiate(deathVFX, transform.position, Quaternion.identity);
        Destroy(particleInstance, durationOfExplosion);
        AudioSource.PlayClipAtPoint(deathSound, Camera.main.transform.position, deathSoundVolume);
    }
}
