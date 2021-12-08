using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Xml;

public class PlayerController : MonoBehaviour
{
    private Rigidbody2D rigid;
    private BoxCollider2D boxCollider;
    private Animator animator;
    public Transform trans;
    private SpriteRenderer spriteRenderer;
    private ParticleSystem skillEffect;
    public PlayerInfo playerInfo;
    public Advent advent;
    public Effusio effusio;
    private bool isInputEnabled = true;
    private bool isGround = false;
    private bool isWall = false;
    private bool isJump = false;
    public bool isAttackable = true;
    public bool isEffusioUse = false;
    public bool isAdventUse = false;
    private int jumpCnt;
    private float myScale = 0.7f;
    public int effusioSelect = 0;
    public string[] effusioIdList = new string[3];
    public string selectEffusioId;
    public string selectAdventId;
    public float attackTimer;
    public int attackCnt;

    void Start()
    {
        DontDestroyOnLoad(this);
        rigid = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        trans = GetComponent<Transform>();
        playerInfo = GetComponentInChildren<PlayerInfo>();
        advent = GetComponentInChildren<Advent>();
        effusio = GetComponentInChildren<Effusio>();
        skillEffect = GetComponentInChildren<ParticleSystem>();

        try { playerInfo.loadPlayerInfo(); }
        catch (Exception)
        {
            playerInfo.savePlayerInfo();
            playerInfo.loadPlayerInfo();
        }

        try { advent.loadAdventInfo(playerInfo.adventId); }
        catch (Exception) { advent.loadAdventInfo("Empty"); }
        if (playerInfo.adventId.Equals("Empty")) { isAdventUse = false; }
        else { isAdventUse = true; }


        effusioIdList[0] = playerInfo.effusioId_a;
        effusioIdList[1] = playerInfo.effusioId_b;
        effusioIdList[2] = playerInfo.effusioId_c;

        try { effusio.loadEffusioInfo(effusioIdList[effusioSelect]); }
        catch (Exception) { effusio.loadEffusioInfo("Empty"); }
        selectEffusioId = effusioIdList[effusioSelect];
        if (selectEffusioId.Equals("Empty")) { isEffusioUse = false; }
        else { isEffusioUse = true; }

        jumpCnt = playerInfo.maxJumpCnt;
        trans.localScale = new Vector3(myScale, myScale, 1);

        attackTimer = 0.0f;
    }

    void Update()
    {
        updatePlayerState();
        if (playerInfo.hp == 0) { isInputEnabled = false; die(); }
        if (isInputEnabled)
        {
            move();
            jumpControl();
            attack();
            useEffusio();
            useAdvent();
            selectEffusio();
        }
        attackTimer += Time.deltaTime;
    }

    private void updatePlayerState()
    {
        isGround = checkGrounded(0.3f);
        isWall = checkWall();
        animator.SetBool("isGround", isGround);
        animator.SetFloat("AirSpeedY", rigid.velocity.y);
        float verticalVelocity = rigid.velocity.y;
        animator.SetBool("isSky", verticalVelocity != 0);

        if (!isWall)
        {
            animator.SetBool("isWall", false);
            rigid.gravityScale = 1;
        }

        if (isGround && verticalVelocity < 0)
        {
            animator.SetBool("isSky", false);

            jumpCnt = playerInfo.maxJumpCnt;
        }
        else if (isWall && !isGround && !isJump)
        {
            bool check;
            if (Input.GetKey(KeyCode.UpArrow))
            {
                if (Input.GetKey(KeyCode.LeftArrow) && trans.localScale.x < 0) { check = true; }
                else if (Input.GetKey(KeyCode.RightArrow) && trans.localScale.x > 0) { check = true; }
                else { check = false; }
            }
            else { check = false; }

            if (check)
            {
                jumpCnt = 1;

                rigid.gravityScale = 0;

                Vector2 newVelocity;
                newVelocity.x = 0;
                newVelocity.y = -1;

                rigid.velocity = newVelocity;

                isWall = true;
                animator.SetBool("isWall", true);
                animator.SetBool("isSky", false);
            }
        }
        else if (isWall && isGround)
        {
            isWall = false;
            animator.SetBool("isWall", false);
        }
    }

    private bool checkGrounded(float distance)
    {
        Vector2 origin = trans.position;

        float radius = 0.1f;

        Vector2 direction;
        direction.x = 0;
        direction.y = -1;

        LayerMask layerMask = LayerMask.GetMask("Ground");

        RaycastHit2D hitRec = Physics2D.CircleCast(origin, radius, direction, distance, layerMask);
        return hitRec.collider != null;
    }

    private bool checkWall()
    {
        Vector2 origin = trans.position;

        float radius = 0.1f;

        Vector2 direction;
        direction.x = trans.localScale.x;
        direction.y = 0;

        float distance = 0.18f;
        LayerMask layerMask = LayerMask.GetMask("Ground");

        RaycastHit2D hitRec = Physics2D.CircleCast(origin, radius, direction, distance, layerMask);
        return hitRec.collider != null;
    }

    public void hurt(int damage)
    {
        isInputEnabled = false;
        gameObject.layer = LayerMask.NameToLayer("PlayerAttacked");

        playerInfo.hp = Math.Max(playerInfo.hp - damage, 0);
        Debug.Log("hp : " + playerInfo.hp);
        if (playerInfo.hp == 0)
        {
            return;
        }
        animator.SetTrigger("Hurt");

        rigid.velocity = new Vector2(0, 0);

        // visual effect
        spriteRenderer.color = new Color(1, 1, 1, 0.4f);

        Vector2 hurtRecoil = new Vector2(5f, 1f);
        rigid.AddForce(new Vector2(-trans.localScale.x * hurtRecoil.x,hurtRecoil.y), ForceMode2D.Impulse);

        StartCoroutine(recoverFromHurtCoroutine());
    }

    private IEnumerator recoverFromHurtCoroutine()
    {
        yield return new WaitForSeconds(playerInfo.hurtDelay);
        isInputEnabled = true;
        yield return new WaitForSeconds(playerInfo.recoverTime);
        spriteRenderer.color = Color.white;
        gameObject.layer = LayerMask.NameToLayer("Player");
    }
    private void die()
    {
        animator.SetTrigger("Die");

        rigid.velocity = new Vector2(0,0);

        // visual effect
        spriteRenderer.color = new Color(1, 1, 1, 1f); ;

        StartCoroutine(deathCoroutine());
    }
    private IEnumerator deathCoroutine()
    {
        GameObject.Find("GameManager").GetComponent<GameManager>().die();

        yield return new WaitForSeconds(5f);

        // SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        Time.timeScale = 0;
    }

    public void attack()
    {
        if (Input.GetKeyDown(KeyCode.X) && attackTimer > playerInfo.attackDelay && !isWall && isAttackable)
        {
            attackCnt++;
            if (attackCnt > 3) { attackCnt = 1; }
            if (attackTimer > 1.0f) { attackCnt = 1; }
            animator.SetTrigger("Attack" + attackCnt);

            attackTimer = 0.0f;
            isAttackable = false;
            isInputEnabled = false;
            StartCoroutine(attackCoroutine(0.1f, playerInfo.attackDelay, new Vector2(trans.localScale.x, 0).normalized));
        }
    }

    private IEnumerator attackCoroutine(float effectDelay, float attackDelay, Vector2 detectDirection)
    {
        yield return new WaitForSeconds(effectDelay);

        float radius = 0.6f;
        float distance = 1f;
        LayerMask layerMask = LayerMask.GetMask("Enemy");

        RaycastHit2D[] hitRecList = Physics2D.CircleCastAll(trans.position, radius, detectDirection, distance, layerMask);

        foreach (RaycastHit2D hitRec in hitRecList)
        {
            GameObject obj = hitRec.collider.gameObject;

            string layerName = LayerMask.LayerToName(obj.layer);

            if (layerName == "Enemy")
            {
                Enemy enemyController = obj.GetComponent<Enemy>();
                if (enemyController != null)
                    enemyController.TakeDamage((int)(playerInfo.attackPoint));
            }
        }

        // attack cool down
        yield return new WaitForSeconds(attackDelay - effectDelay);
        isInputEnabled = true;
        isAttackable = true;
    }
    public void selectEffusio()
    {
        if (Input.GetKeyDown("1") && isEffusioUse) { effusioSelect = 0; }
        else if (Input.GetKeyDown("2") && isEffusioUse) { effusioSelect = 1; }
        else if (Input.GetKeyDown("3") && isEffusioUse) { effusioSelect = 2; }
        try { effusio.loadEffusioInfo(effusioIdList[effusioSelect]); }
        catch (Exception)
        {
            effusio.loadEffusioInfo(effusioIdList[effusioSelect]);
        }
        selectEffusioId = effusioIdList[effusioSelect];
    }
    public void useEffusio()
    {
        if (Input.GetKeyDown(KeyCode.V) && !isWall && isAttackable && isEffusioUse)
        {
            skillEffect.Play();
            animator.SetTrigger("Block");
            isAttackable = false;
            isEffusioUse = false;
            isInputEnabled = false;

            StartCoroutine(effusioCoroutine());
        }
    }
    private IEnumerator effusioCoroutine()
    {
        GameObject copy = Resources.Load("Boss/Prefabs/" + selectEffusioId) as GameObject;
        copy.tag = "Summon";
        Vector3 dir = new Vector3(transform.localScale.x, 0, 0).normalized;
        Vector3 copyDir = copy.transform.localScale;
        copy.transform.localScale = new Vector3(-dir.x * Math.Abs(copyDir.x), copyDir.y, copyDir.z);
        Destroy(Instantiate(copy, skillEffect.transform.position, trans.rotation), 5f);

        isAttackable = true;
        isInputEnabled = true;
        yield return new WaitForSeconds(effusio.coolTime);
        isEffusioUse = true;
    }

    public void useAdvent()
    {
        if (Input.GetKeyDown(KeyCode.Z) && !isWall && isAttackable && isAdventUse)
        {
            skillEffect.Play();
            animator.SetTrigger("Block");
            isAttackable = false;
            isAdventUse = false;
            isInputEnabled = false;

            StartCoroutine(adventCoroutine(advent.skillDelay, advent.skillCoolTime, new Vector2(trans.localScale.x, 0).normalized));
        }
    }
    public void selectAdvent(string select)
    {
        try
        {
            advent.loadAdventInfo(select);
            playerInfo.adventId = select;
            selectAdventId = select;
        }
        catch (Exception)
        {
            advent.loadAdventInfo("Empty");
            selectAdventId = "Empty";
        }
        if (playerInfo.adventId.Equals("Empty")) { isAdventUse = false; }
        else { isAdventUse = true; }
    }

    private IEnumerator adventCoroutine(float effectDelay, float coolTime, Vector2 detectDirection)
    {
        if (advent.skilltype == "Shot")
        {
            yield return new WaitForSeconds(effectDelay);

            GameObject shotClone = Resources.Load("Player/Advent/Prefabs/" + advent.id) as GameObject;
            Vector3 summonPosition = new Vector3(0, 0, -1) + skillEffect.transform.position;
            Instantiate(shotClone, summonPosition, trans.rotation);
        }
        if (advent.skilltype == "Guard")
        {
            yield return new WaitForSeconds(effectDelay);
            GameObject copy = Resources.Load("Player/Advent/Prefabs/" + advent.id) as GameObject;
            Instantiate(copy, trans.position, trans.rotation);
        }
        if (advent.skilltype == "Summon")
        {
            yield return new WaitForSeconds(effectDelay);
            GameObject copy = Resources.Load("Player/Advent/Prefabs/" + advent.id) as GameObject;
            Vector3 direction = new Vector3(trans.localScale.x, 0, 0).normalized;
            Vector3 summonPosition = new Vector3(0, 1, -1) + direction + trans.position;
            Instantiate(copy, summonPosition, trans.rotation);
        }
        if (advent.skilltype == "Heal")
        {
            yield return new WaitForSeconds(effectDelay);
            playerInfo.hp = Math.Min(playerInfo.hp + advent.point, playerInfo.maxHp);
        }

        isAttackable = true;
        isInputEnabled = true;
        yield return new WaitForSeconds(coolTime - effectDelay);
        isAdventUse = true;
    }

    public void move()
    {
        // calculate movement
        float horizontalMovement = Input.GetAxis("Horizontal") * playerInfo.movePower;

        // set velocity
        Vector2 newVelocity;
        newVelocity.x = horizontalMovement;
        newVelocity.y = rigid.velocity.y;
        rigid.velocity = newVelocity;

        if (!isWall && isInputEnabled)
        {
            // the sprite itself is inversed 
            float moveDirection = trans.localScale.x * horizontalMovement;

            if (moveDirection < 0)
            {
                // flip player sprite
                Vector3 newScale;
                newScale.x = horizontalMovement < 0 ? -myScale : myScale;
                newScale.y = myScale;
                newScale.z = 1;

                trans.localScale = newScale;
            }
        }

        // stop
        if (Input.GetAxis("Horizontal") == 0)
        {
            animator.SetInteger("animState", 0);
        }
        else
        {
            animator.SetInteger("animState", 1);
        }
    }

    private void jumpControl()
    {
        if (!Input.GetKeyDown(KeyCode.C)) { isJump = false; return; }
        else if (Input.GetKeyDown(KeyCode.C) && Input.GetKey(KeyCode.UpArrow) && !isWall && isGround && jumpCnt > 0) { superJump(); }
        else if (Input.GetKeyDown(KeyCode.C) && !isWall && jumpCnt > 0) { jump(); }
        else if (Input.GetKeyDown(KeyCode.C) && isWall && !isGround) { wallJump(); }
    }

    public void wallJump()
    {
        Vector2 zero;
        zero.x = 0; zero.y = 0;
        rigid.velocity = zero;

        Vector2 realClimbJumpForce;
        realClimbJumpForce.x = playerInfo.movePower * 2f * -trans.localScale.x;
        realClimbJumpForce.y = (playerInfo.movePower / 4) + playerInfo.jumpPower;
        rigid.AddForce(realClimbJumpForce, ForceMode2D.Impulse);

        animator.SetTrigger("Jump");

        isInputEnabled = false;
        StartCoroutine(wallJumpCoroutine(playerInfo.jumpDelay));
    }

    private IEnumerator wallJumpCoroutine(float delay)
    {
        yield return new WaitForSeconds(delay);

        isInputEnabled = true;

        // jump to the opposite direction
        Vector3 newScale;
        newScale.x = -trans.localScale.x;
        newScale.y = trans.localScale.y;
        newScale.z = trans.localScale.z;

        trans.localScale = newScale;
    }

    public void jump()
    {
        Vector2 newVelocity;
        newVelocity.x = rigid.velocity.x;
        newVelocity.y = playerInfo.jumpPower;

        rigid.velocity = newVelocity;
        isJump = true;
        animator.SetBool("isSky", true);
        jumpCnt -= 1;
        if (jumpCnt >= 0) { animator.SetTrigger("Jump"); }
    }

    public void superJump()
    {
        Vector2 newVelocity;
        newVelocity.x = rigid.velocity.x;
        newVelocity.y = playerInfo.jumpPower * 2;

        rigid.velocity = newVelocity;
        isJump = true;
        animator.SetBool("isSky", true);
        jumpCnt -= 1;
        if (jumpCnt >= 0) { animator.SetTrigger("Jump"); }
    }
}
