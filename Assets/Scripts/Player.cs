using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public Bullet bulletPrefab;

    public float thrustSpeed = 1.0f;
    public float turnSpeed = 1.0f;

    private bool _thrusting;
    private float _turnDirection;

    private Rigidbody2D _rigidbody;

    public void Awake()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
    }

    public void Update()
    {
        _thrusting = (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow));

        if(Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow))
        {
            _turnDirection = 1f;
        } else if(Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow))
        {
            _turnDirection = -1f;
        } else
        {
            _turnDirection = 0f;
        }


        if (Input.GetKeyDown(KeyCode.Space) || Input.GetMouseButtonDown(0))
        {
            Shoot();
        }
    }

    public void FixedUpdate()
    {
        if (_thrusting)
        {
            _rigidbody.AddForce(this.transform.up * this.thrustSpeed);
        }

        if(_turnDirection != 0)
        {
            _rigidbody.AddTorque(this.turnSpeed * _turnDirection);
        }
    }

    private void Shoot()
    {
        Bullet bullet = Instantiate(this.bulletPrefab, this.transform.position, this.transform.rotation);
        bullet.Project(this.transform.up);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.tag == "Asteroid")
        {
            _rigidbody.velocity = Vector3.zero;
            _rigidbody.angularVelocity = 0f;

            this.gameObject.SetActive(false);

            FindObjectOfType<GameManager>().PlayerDies();
        }
    }

    private void OnDisable()
    {
        this.gameObject.layer = LayerMask.NameToLayer("IgnoresCollision");
    }

    private void OnEnable()
    {
        Invoke(nameof(TurnOnCollisions), 2f);
    }
   
    private void TurnOnCollisions()
    {
        this.gameObject.layer = LayerMask.NameToLayer("Player");
    }
}
