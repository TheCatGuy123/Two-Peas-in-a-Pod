using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private int player, health = 3;
    [SerializeField] private float jumpHeight, speed, castDist;
    private Rigidbody2D rb;
    [SerializeField] private RuntimeAnimatorController walkAnim, jumpAnim, idleAnim;
    [SerializeField] private Vector3 respawnPoint;
    [SerializeField] private Sprite flower, heart, deadHeart;
    [SerializeField] private SpriteRenderer[] hearts;
    [SerializeField] private Vector2 boxSize;
    [SerializeField] private string groundLayer, otherPlayerLayer;
    private bool onGround;
    [SerializeField] public AudioSource collectSound, deathSound, checkPoint, jumpSound, buttonSound;

    // Start is called before the first frame update
    void Start()
    {
        // set rigidbody
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        // boxcast (groundcheck)
        LayerMask layers = LayerMask.GetMask(groundLayer, otherPlayerLayer);
        RaycastHit2D boxCast = Physics2D.BoxCast(transform.position, boxSize, 0, -transform.up, castDist, layers);

        // set onGround based on boxcast
        if(boxCast.transform != gameObject.transform && boxCast) 
        {
            onGround = true;
        }
        else
        {
            onGround = false;
        }
        // P1 movement
        if (player == 0)
        {
            // P1 jumpAnim
            if (onGround == false)
            {
                GetComponent<Animator>().runtimeAnimatorController = jumpAnim;
            }

            // P1 horizontal movement
            if (Input.GetKey("a"))
            {
                if (onGround == true)
                {
                    GetComponent<Animator>().runtimeAnimatorController = walkAnim;
                }
                GetComponent<SpriteRenderer>().flipX = true;
                rb.velocity = new Vector2(-speed, rb.velocity.y);
            }
            else if (Input.GetKey("d"))
            {
                if (onGround == true)
                {
                    GetComponent<Animator>().runtimeAnimatorController = walkAnim;
                }
                GetComponent<SpriteRenderer>().flipX = false;
                rb.velocity = new Vector2(speed, rb.velocity.y);
            }
            else
            {
                if (onGround == true)
                {
                    GetComponent<Animator>().runtimeAnimatorController = idleAnim;
                }
                rb.velocity = new Vector2(0, rb.velocity.y);
            }
            // P1 jumping
            if (Input.GetKey("w") && onGround == true)
            {
                rb.velocity = new Vector2(rb.velocity.x, jumpHeight);
                jumpSound.Play();
            }
        }
        // P2 movement
        else if (player == 1)
        {
            // P2 jump anim
            if (onGround == false)
            {
                GetComponent<Animator>().runtimeAnimatorController = jumpAnim;
            }

            // P2 horizontal movement
            if (Input.GetKey("left"))
            {
                if (onGround == true)
                {
                    GetComponent<Animator>().runtimeAnimatorController = walkAnim;
                }
                GetComponent<SpriteRenderer>().flipX = true;
                rb.velocity = new Vector2(-speed, rb.velocity.y);
            }
            else if (Input.GetKey("right"))
            {
                if (onGround == true)
                {
                    GetComponent<Animator>().runtimeAnimatorController = walkAnim;
                }
                GetComponent<SpriteRenderer>().flipX = false;
                rb.velocity = new Vector2(speed, rb.velocity.y);
            }
            else
            {
                if (onGround == true)
                {
                    GetComponent<Animator>().runtimeAnimatorController = idleAnim;
                }
                rb.velocity = new Vector2(0, rb.velocity.y);
            }
            // P2 jumping
            if (Input.GetKey("up") && onGround == true)
            {
                rb.velocity = new Vector2(rb.velocity.x, jumpHeight);
                jumpSound.Play();
            }
        }
    }

    private void OnDrawGizmos()
    {
        // draw groundcheck box
        Gizmos.DrawWireCube(transform.position - transform.up * castDist, boxSize);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        // take damage
        if(other.tag == "-1HP")
        {
            Damage(1);
        }
        else if(other.tag == "-2HP")
        {
            Damage(2);
        }
        else if(other.tag == "-3HP")
        {
            Damage(3);
        }
        else if(other.tag == "Respawn")
        {
            // set checkpoint
            health = 3;
            if (other.gameObject.transform.position != respawnPoint)
            {
                other.gameObject.GetComponent<SpriteRenderer>().sprite = flower;
                respawnPoint = other.gameObject.transform.position;
                checkPoint.Play();
            }

            // refill hearts
            foreach(SpriteRenderer i in hearts)
            {
                i.sprite = heart;
            }
        }
    }

    private void Damage(int damageNum)
    {
        // damage and death
        health -= damageNum;
        deathSound.Play();
        if (health <= 0)
        {
            health = 3;
            transform.position = respawnPoint;
        }

        // update hearts
        int heartsFilled = 0;
        foreach(SpriteRenderer i in hearts)
        {
            if (heartsFilled < health)
            {
                heartsFilled += 1;
                i.sprite = heart;
            }
            else
            {
                i.sprite = deadHeart;
            }
        }
    }
}
