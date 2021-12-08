using System.Collections;
using UnityEngine;


public class Enemy : MonoBehaviour
{
    public int currentHp = 10;
    public float moveSpeed = 5f;
    public float jumpPower = 10;
    public float atkCoolTime = 3f;
    public float atkCoolTimeCalc = 300f;
    public float destroyDelay = 1;
    public int damage = 1;
    public int bullet_dmg = 0;
    public string bossName;

    public bool isHit = false;
    public bool isGround = true;
    public bool canAtk = true;
    public bool MonsterDirRight;
    public bool summoner = false;

    protected Rigidbody2D rb;
    protected BoxCollider2D boxCollider;
    protected SpriteRenderer spriteRenderer;
    public GameObject hitBoxCollider;
    public Animator Anim;
    public LayerMask layerMask;
    protected float ackDistance;
    public float distance;

    public GameObject target;//플레이어
    public GameObject Life;//라이프
    public GameObject Soul;//소울


    protected void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        boxCollider = GetComponent<BoxCollider2D>();
        Anim = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        if (this.tag == "Summon")
        {
            this.gameObject.layer = LayerMask.NameToLayer("Summon");
        }
        else
        {
            target = GameObject.FindWithTag("Player");
        }

        StartCoroutine(CalcCoolTime());
        StartCoroutine(ResetCollider());
    }

    IEnumerator ResetCollider()
    {
        while (true)
        {
            yield return null;
            if (!hitBoxCollider.activeInHierarchy)
            {
                yield return new WaitForSeconds(0.5f);
                hitBoxCollider.SetActive(true);
                isHit = false;
            }
        }
    }
    IEnumerator CalcCoolTime()
    {
        while (true)
        {
            yield return null;
            if (!canAtk)
            {
                atkCoolTimeCalc -= Time.deltaTime;
                if (atkCoolTimeCalc <= 0)
                {
                    atkCoolTimeCalc = atkCoolTime;
                    canAtk = true;
                }
            }
        }
    }
    IEnumerator fadeCoroutine()
    {
        while (destroyDelay > 0)
        {
            destroyDelay -= Time.deltaTime;

            if (spriteRenderer.color.a > 0)
            {
                Color newColor = spriteRenderer.color;
                newColor.a -= Time.deltaTime / destroyDelay;
                spriteRenderer.color = newColor;
                yield return null;
            }
        }
        Debug.Log("적 처치");

        if (!(this.bossName.Equals("") || this.bossName == null))
        {
            DropSoul(this.bossName);
            DropLife();
        }
        else
        {
            if (Random.value > 0.66f)
            {
                DropLife();
            }
        }
        Destroy(gameObject);
    }

    public bool IsPlayingAnim(string AnimName)
    {
        if (Anim.GetCurrentAnimatorStateInfo(0).IsName(AnimName))
        {
            return true;
        }
        return false;
    }
    public void MyAnimSetTrigger(string AnimName)
    {
        if (!IsPlayingAnim(AnimName))
        {
            Anim.SetTrigger(AnimName);
        }
    }

    protected void MonsterFlip()
    {
        MonsterDirRight = !MonsterDirRight;

        Vector3 thisScale = transform.localScale;
        if (MonsterDirRight)
        {
            thisScale.x = -Mathf.Abs(thisScale.x);
        }
        else
        {
            thisScale.x = Mathf.Abs(thisScale.x);
        }
        transform.localScale = thisScale;
        rb.velocity = Vector2.zero;
    }
    protected bool IsTargetDir()
    {
        if (transform.position.x < target.transform.position.x ? MonsterDirRight : !MonsterDirRight)
        {
            return true;
        }
        return false;
    }

    protected void GroundCheck()
    {
        if (Physics2D.BoxCast(boxCollider.bounds.center, boxCollider.size, 0, Vector2.down, 0.05f, layerMask))
        {
            isGround = true;
        }
        else
        {
            isGround = false;
        }
    }

    public void TakeDamage(int dam)
    {
        currentHp -= dam;
        Debug.Log("적 체력: " + currentHp);
        isHit = true;
        // Knock Back or Dead
        if (currentHp <= 0)
        {
            this.gameObject.layer = 0;
            StartCoroutine(fadeCoroutine());
        }
        else
        {
            MyAnimSetTrigger("Hit");
            rb.velocity = Vector2.zero;
            if (transform.position.x > target.transform.position.x)
            {
                rb.velocity = new Vector2(5f, 0);
            }
            else
            {
                rb.velocity = new Vector2(-5f, 0);
            }
            hitBoxCollider.SetActive(false);
        }
    }

    protected void OnTriggerEnter2D(Collider2D collision)
    {
        // if(this.tag.Equals("Summon")){

        //     if(target.tag.Equals("Enemy")||target.tag.Equals("EnemyHitBox")){
        //         print(collision.gameObject.name);
        //         Enemy enemy = collision.gameObject.GetComponent<Enemy>();
        //         enemy.TakeDamage(damage);
        //         //print(enemy.gameObject.name);
        //     }
        // }
        if (target.tag.Equals(collision.tag))
        {
            print(collision.tag);
            if (target.tag.Equals("Player"))
            {
                PlayerController player = collision.gameObject.GetComponent<PlayerController>();
                player.hurt(damage);
            }
            else
            {
                Enemy enemy = collision.gameObject.GetComponent<Enemy>();
                enemy.TakeDamage(damage);
            }
        }
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
            if (collision.gameObject.CompareTag("Summon"))
            {
                Enemy summon = collision.gameObject.GetComponent<Enemy>();
                
                this.TakeDamage(summon.damage);
            }
       


    }

    void DropLife()
    {
        GameObject LifeClone = Instantiate(Life, new Vector3(transform.position.x, transform.position.y + 2.1f, transform.position.z), transform.rotation);
        Rigidbody2D LifeClone_rb = LifeClone.GetComponent<Rigidbody2D>();
        LifeClone_rb.velocity = transform.up * 3f;
    }
    void DropSoul(string boName)
    {
        GameObject SoulClone = Instantiate(Soul, new Vector3(transform.position.x, transform.position.y + 2f, transform.position.z), transform.rotation);
        SoulClone.GetComponent<Soul>().bossName = boName;
        Rigidbody2D SoulClone_rb = SoulClone.GetComponent<Rigidbody2D>();
        SoulClone_rb.velocity = transform.up * 3f;
    }
}