using System.Collections.Generic;
using UnityEngine;

public class DiChuyen : MonoBehaviour
{
    public float tocdo = 10f; 
    public ContactFilter2D movementFilter;
    public float collisionOffset = 0.05f;

    private Rigidbody2D rb2d;
    private Vector2 HuongDiChuyen;
    private List<RaycastHit2D> castCollisions = new List<RaycastHit2D>();
    private SpriteRenderer spr;
    private Animator anim;

    void Start()
    {
        rb2d = GetComponent<Rigidbody2D>();
        spr = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();
    }

    void Update()
    {
        HuongDiChuyen.x = Input.GetAxisRaw("Horizontal"); 
        HuongDiChuyen.y = Input.GetAxisRaw("Vertical");  

        if (HuongDiChuyen != Vector2.zero)
        {
            HuongDiChuyen = HuongDiChuyen.normalized;
        }

        anim.SetFloat("isMove", HuongDiChuyen.sqrMagnitude);

        MoveCharacter();
        FlipSprite();
    }

    bool TryMove(Vector2 direction)
    {
        int count = rb2d.Cast(
            direction,
            movementFilter,
            castCollisions,
            tocdo * Time.deltaTime + collisionOffset
        );

        if (count == 0)
        {
            rb2d.MovePosition(rb2d.position + direction * tocdo * Time.deltaTime);
            return true;
        }
        return false;
    }

    void MoveCharacter()
    {
        if (HuongDiChuyen != Vector2.zero)
        {
            bool canMove = TryMove(HuongDiChuyen);

            if (!canMove)
            {
                canMove = TryMove(new Vector2(HuongDiChuyen.x, 0));
                if (!canMove)
                {
                    canMove = TryMove(new Vector2(0, HuongDiChuyen.y));
                }
            }
        }
    }

    void FlipSprite()
    {
        if (HuongDiChuyen.x < 0)
        {
            spr.flipX = true;
        }
        else if (HuongDiChuyen.x > 0)
        {
            spr.flipX = false;
        }
    }
}