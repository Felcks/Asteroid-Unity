using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Asteroid : MonoBehaviour
{
    public Sprite[] sprites;
    public float size = 1.0f;
    public float minSize = 0.5f;
    public float maxSize = 1.5f;
    public float speed = 50f;

    private SpriteRenderer _spriteRenderer;
    private Rigidbody2D _rigidbody;

    public float maxLifetime = 50.0f;

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
        _spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void Start()
    {
        _spriteRenderer.sprite = sprites[Random.Range(0, sprites.Length)];

        this.transform.eulerAngles = new Vector3(0.0f, 0.0f, Random.value * 360f);
        this.transform.localScale = Vector3.one * this.size;

        _rigidbody.mass = this.size;
    }

    public void SetTrajectory(Vector2 direction)
    {
        _rigidbody.AddForce(direction * this.speed);

        Destroy(this.gameObject, this.maxLifetime);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.tag == "Bullet")
        {
            if (this.size * 0.5f >= this.minSize)
            {
                CreateSplit();
                CreateSplit();
            }

            FindObjectOfType<GameManager>().AsteroidDestroyed(this);
            Destroy(this.gameObject);
        }
    }

    private void CreateSplit()
    {
        Vector2 positon = this.transform.position;
        positon += Random.insideUnitCircle * 0.5f;
        Asteroid half = Instantiate(this, positon, this.transform.rotation);
        half.size = this.size * 0.5f;
        half.SetTrajectory(Random.insideUnitCircle.normalized * this.speed);
    }
}
