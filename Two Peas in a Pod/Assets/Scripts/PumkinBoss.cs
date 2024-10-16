using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PumkinBoss : MonoBehaviour
{
    [SerializeField] private Transform wall;
    [SerializeField] private RedButton button, button1;
    [SerializeField] private GameObject player, player1, seed, hitBox, hpBar, hpBarParent, deathParticle, deathParticle1;
    private bool isActive;
    [SerializeField] private RuntimeAnimatorController startAnim, idleAnim, deathAnim;
    private Rigidbody2D rb;
    [SerializeField] private float jumpHeight, hp, maxHp, hpBarSize;
    public AudioSource damageSound, deathSound, music;
    [SerializeField] private AudioClip bossMusic, victoryMusic;
    [SerializeField] private ChangeScene sceneChanger;
    // Start is called before the first frame update
    void Start()
    {
        // set variables
        music = GameObject.FindGameObjectWithTag("Music").GetComponent<AudioSource>();
        hpBarSize = hpBar.transform.localScale.x;
        hp = maxHp;
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        // start boss if wall reaches top (if both players are pressing the buttons)
        if(wall.localPosition.y >= 13.6f && button.isSpringed == true)
        {
            StartCoroutine(StartBoss());
        }
    }

    IEnumerator StartBoss()
    {
        // a bunch of stuff when starting the boss
        button.isSpringed = false;
        button1.isSpringed = false;
        GetComponent<Animator>().runtimeAnimatorController = startAnim;
        yield return new WaitForSeconds(3f);
        // after animation is done, set music and start attacking
        GetComponent<Animator>().runtimeAnimatorController = idleAnim;
        music.clip = bossMusic;
        music.Play();
        isActive = true;
        hpBarParent.SetActive(true);
        hitBox.GetComponent<Collider2D>().tag = "-2HP";
        StartCoroutine(Attack(0));
    }

    IEnumerator Attack(int attackNum)
    {
        // attack 0, jumping
        if(attackNum == 0)
        {
            rb.velocity = new Vector2(player.transform.position.x - transform.position.x, jumpHeight);
            yield return new WaitForSeconds(3f);
            rb.velocity = new Vector2(player1.transform.position.x - transform.position.x, jumpHeight);
            yield return new WaitForSeconds(5f);
            StartCoroutine(Attack(1));
        }
        // attack 1, spitting seeds
        else if(attackNum == 1)
        {
            GameObject currentSeed = Instantiate(seed, transform.position, Quaternion.identity);
            Vector2 diff = player.transform.position - transform.position;
            diff.Normalize();
            currentSeed.GetComponent<Rigidbody2D>().velocity = 5 * diff;
            yield return new WaitForSeconds(0.5f);

            currentSeed = Instantiate(seed, transform.position, Quaternion.identity);
            diff = player.transform.position - transform.position;
            diff.Normalize();
            currentSeed.GetComponent<Rigidbody2D>().velocity = 5 * diff;
            yield return new WaitForSeconds(1f);

            currentSeed = Instantiate(seed, transform.position, Quaternion.identity);
            diff = player1.transform.position - transform.position;
            diff.Normalize();
            currentSeed.GetComponent<Rigidbody2D>().velocity = 5 * diff;
            yield return new WaitForSeconds(0.5f);

            currentSeed = Instantiate(seed, transform.position, Quaternion.identity);
            diff = player1.transform.position - transform.position;
            diff.Normalize();
            currentSeed.GetComponent<Rigidbody2D>().velocity = 5 * diff;
            yield return new WaitForSeconds(2f);
            StartCoroutine(Attack(2));
        }
        // attack 2, dashing
        else if(attackNum == 2)
        {
            rb.velocity = new Vector2((player.transform.position.x - transform.position.x) * 3, 0);
            yield return new WaitForSeconds(2f);
            rb.velocity = new Vector2((player1.transform.position.x - transform.position.x) * 3, 0);
            yield return new WaitForSeconds(5f);
            StartCoroutine(Attack(0));
        }
    }

    IEnumerator Death()
    {
        // stuff when player beat boss
        yield return new WaitForSeconds(3f);
        hitBox.GetComponent<Collider2D>().tag = "Untagged";
        GetComponent<Animator>().runtimeAnimatorController = deathAnim;
        deathSound.Play();
        yield return new WaitForSeconds(1.2f);
        for(int i = 0; i < 15; i++)
        {
            Instantiate(deathParticle, transform.position, transform.rotation);
        }
        yield return new WaitForSeconds(0.05f);
        for(int i = 0; i < 15; i++)
        {
            Instantiate(deathParticle1, transform.position, transform.rotation);
        }
        yield return new WaitForSeconds(0.05f);
        GetComponent<SpriteRenderer>().enabled = false;
        yield return new WaitForSeconds(0.5f);
        music.clip = victoryMusic;
        music.Play();
        yield return new WaitForSeconds(02f);
        music.volume = 0.5f;
        sceneChanger.MoveScenes(); 
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        // taking damage from spikes
        if (other.tag == "-2HP" && isActive)
        {
            damageSound.Play();
            hp -= 2;
            if (isActive)
            {
                hpBar.transform.localScale = new Vector3(hpBarSize/(maxHp + 0.001f) * hp, hpBar.transform.localScale.y, hpBar.transform.localScale.z);
            }

            if (hp <= 0)
            {
                hp = 0;
                if (isActive)
                {
                    hpBar.transform.localScale = new Vector3(hpBarSize/(maxHp + 0.001f) * hp, hpBar.transform.localScale.y, hpBar.transform.localScale.z);
                }
                StopAllCoroutines();
                StartCoroutine(Death());
            }
        }
    }
}
