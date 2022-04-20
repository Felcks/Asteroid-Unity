using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public Player player;
    public ParticleSystem explosion;
    public Text text;

    public int lives = 3;
    public float respawnTime = 3f;

    public int score = 0;

    public void AsteroidDestroyed(Asteroid asteroid)
    {
        this.explosion.transform.position = asteroid.transform.position;
        this.explosion.Play();

        //TODO: increase Score
        if (asteroid.size < 0.75f)
        {
            this.score += 100;
        } else if(asteroid.size < 1.2f)
        {
            this.score += 50;
        } else
        {
            this.score += 25;
        }
    }


    public void PlayerDies()
    {
        this.explosion.transform.position = this.player.transform.position;
        this.explosion.Play();
        this.lives--;

        if(this.lives <= 0)
        {
            GameOver();
        } else
        {
            Invoke(nameof(Respawn), this.respawnTime);
        }
    }

    private void Respawn()
    {
        this.player.gameObject.transform.position = Vector3.zero;
        this.player.gameObject.SetActive(true);
    }

    private void GameOver()
    {
        this.lives = 3;
        this.score = 0;
        Invoke(nameof(Respawn), this.respawnTime);
    }

    public void Update()
    {
        text.text = "Score: " + this.score;
    }
}
