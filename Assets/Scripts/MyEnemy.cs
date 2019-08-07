using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MyEnemy : MonoBehaviour
{
    public float MoveForce = 30f;
    public int HP; // Сколько раз противника можно ударить, прежде чем он умрет.
    public int Damage;
    public float RaycastDistance;
    public float MinDistance;
    public float MaxSpeed;
    public float Cooldown = 1f;

    public GameObject Target;
    Vector3 targetPos;
    public bool IsAngry;
    public float AngryTime = 3f;
    float lastAngryTime;
    float lastAttackTime;
    public bool IsForward = true; // Направление трансформа
    Vector3 Direction = new Vector3(1, 0, 0); // Vector right
    private Rigidbody2D rb;

    public LayerMask LayerMask;

    Vector3 startpos;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        InvokeRepeating("InvFlip", 2f, 2f);
        startpos = transform.position;
    }

    private void Update()
    {
        if (Time.time - lastAngryTime > AngryTime)
        {
            IsAngry = false;
            Target = null;
        }
    }

    void InvFlip()
    {
        if (!IsAngry && Mathf.Abs(rb.velocity.x) <= 0.1f)
            Flip();
    }

    private void FixedUpdate()
    {
        RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.right * Direction.x, RaycastDistance, LayerMask);

        if (hit.collider != null)
        {
            IsAngry = true;
            lastAngryTime = Time.time;
            Target = hit.collider.gameObject;
            targetPos = Target.transform.position;
        }

        Debug.DrawLine(transform.position, (Vector2)transform.position + Vector2.right * Direction.x * RaycastDistance, Color.red);

        if (IsAngry)
        {
            MoveToPosition(targetPos);

            if (Vector3.Distance(transform.position, Target.transform.position) <= MinDistance)
            {
                Attack();
            }
        }
        else if ((startpos - transform.position).sqrMagnitude > 0.25f)
        {
            MoveToPosition(startpos);
        }
    }

    void MoveToPosition(Vector3 pos)
    {
        float x = pos.x - transform.position.x;

        if (x < 0 && IsForward) // Поворот
            Flip();
        if (x > 0 && !IsForward)
            Flip();

        rb.AddForce(Vector2.right * Direction.x * MoveForce);
        if (Mathf.Abs(rb.velocity.x) > MaxSpeed)
        {
            rb.velocity = new Vector2(Direction.x * MaxSpeed, rb.velocity.y);
        }
    }

    void Attack()
    {
        if (Time.time - lastAttackTime < Cooldown)
        {
            return;
        }
        lastAttackTime = Time.time;

        PlayerController mpc = Target.GetComponent<PlayerController>();
        if (mpc != null)
        {
            mpc.TakeDamage(Damage);
        }
    }

    void Flip()
    {
        IsForward = !IsForward;
        Direction.x *= -1f;

        Vector3 v = transform.localScale;
        v.x *= -1;
        transform.localScale = v;
    }

    public void Hurt(int Damage)
    {
        HP--;
        if (HP <= 0)
        {
            Destroy(gameObject);
        }
    }
}