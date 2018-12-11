using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectCollision : MonoBehaviour
{
    public RaycastHit downInfo, upInfo, leftInfo, rightInfo;
    public bool down, up, left, right;
    public bool isCircle, isBox;
    Vector2 origin;
    float radius;
    Vector2 size;
    public bool colliding;
    public LayerMask mask;

    void Start()
    {

        

    }

	void Update ()
    {
        origin = transform.position;
        if (isCircle)
        {
            CircleCasting();
            radius = GetComponent<CircleCollider2D>().radius;
        }
        else if (isBox)
        {
            BoxCasting();
            size = GetComponent<BoxCollider2D>().size;
        }
        else
        {

            if (GetComponent<CircleCollider2D>())
            {
                isCircle = true;
                CircleCasting();
                radius = GetComponent<CircleCollider2D>().radius;
            }
            else if (GetComponent<BoxCollider2D>())
            {
                isBox = true;
                BoxCasting();
                size = GetComponent<BoxCollider2D>().size;
            }
            else
            {
                Debug.LogWarning("Object collision type unable to be determined, please set in inspector.");
            }
        }
    }

    void CircleCasting()
    {
        down = Physics2D.CircleCast(origin, radius, Vector2.down, 0.5f, mask);
        up = Physics2D.CircleCast(origin, radius, Vector2.up, 0.5f, mask);
        left = Physics2D.CircleCast(origin, radius, Vector2.left, 0.5f, mask);
        right = Physics2D.CircleCast(origin, radius, Vector2.right, 0.5f, mask);
    }
    
    void BoxCasting()
    {
        down = Physics2D.BoxCast(origin, size, 0, Vector2.down, 0.5f, mask);
        up = Physics2D.BoxCast(origin, size, 0, Vector2.up, 0.5f, mask);
        left = Physics2D.BoxCast(origin, size, 0, Vector2.left, 0.5f, mask);
        right = Physics2D.BoxCast(origin, size, 0, Vector2.right, 0.5f, mask);
    }

    private void OnCollisionStay2D()
    {
        colliding = true;
    }
}
