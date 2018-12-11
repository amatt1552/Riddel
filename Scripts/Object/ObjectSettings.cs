using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectSettings : MonoBehaviour
{
    Rigidbody2D rbObject;

    public bool isKey;
    public string keyCode;

    public bool hasShadow;
    public string shadowName;
    public GameObject shadowObj;

    public bool collectable;
    public bool selected;
    Animator lightningFront, lightningBack;
    ParticleSystem distortion;

    //static vars

    public bool isStatic;

    //floating vars

    Vector3 floatingPos;
    Vector3 hover;
    public float hoverSpeed = 0.1f;
    bool flip;
    float inc;
    float floatDistance = 0.1f;

    public bool crushable;
    public int maxDamage;
    float recoverDamage;
    int damage;
    float reset;

    [HideInInspector]
    public float radius;
    [HideInInspector]
    public Vector2 boxSize;
    [HideInInspector]
    public GameObject pivot;
    [HideInInspector]
    public bool locked;

    private void Start()
    {
        if (GetComponent<CircleCollider2D>())
        {
            radius = GetComponent<CircleCollider2D>().radius;
        }
        else if (GetComponent<BoxCollider2D>())
        {
            boxSize = GetComponent<BoxCollider2D>().size;
            radius = boxSize.x;
        }
        rbObject = GetComponent<Rigidbody2D>();
        if (transform.Find("distorting"))
            distortion = transform.Find("distorting").GetComponent<ParticleSystem>();
        pivot = new GameObject("pivot");
        pivot.transform.parent = transform;
        damage = maxDamage;
        

    }

    private void Update()
    {
        if (hasShadow)
            Shadow();

        if (distortion != null)
            Distortion();

        if (!Resources.Load("Images/" + gameObject.name))
            collectable = false;

        if(crushable)
            Damage();
    }

    private void OnTriggerStay2D(Collider2D col)
    {
       
        if (isKey && !selected)
        {
            if (col.tag == keyCode)
            {
                floatingPos = col.transform.position;
                if (col.transform.Find("lightningFront"))
                {
                    lightningFront = col.transform.Find("lightningFront").GetComponent<Animator>();
                }
                if (col.transform.Find("lightningBack"))
                {
                    lightningBack = col.transform.Find("lightningBack").GetComponent<Animator>();
                }
                
                if (Mathf.Abs(transform.position.x - col.transform.position.x) < radius/2)
                {
                    if (lightningFront != null && lightningBack != null)
                    {
                        lightningFront.gameObject.SetActive(true);
                        lightningBack.gameObject.SetActive(true);
                    }
                    locked = true;
                    Hover();
                }
                
            }
        }
        else
        {
            if (lightningFront != null && lightningBack != null)
            {
                lightningFront.gameObject.SetActive(false);
                lightningBack.gameObject.SetActive(false);
            }
            locked = false;
            
        }

    }

    void Hover()
    {
        if (flip)
        {
            inc += hoverSpeed * Time.deltaTime;
        }
        else
        {
            inc -= hoverSpeed * Time.deltaTime;
        }

        if (inc > floatDistance || inc < -floatDistance)
        {
            flip = !flip;
        }

        hover = new Vector2(0, inc);
        rbObject.velocity = Vector2.zero;
        rbObject.MovePosition(Vector2.MoveTowards(transform.position, floatingPos + hover, radius * Time.deltaTime));
    }

    void Shadow()
    {
        if (shadowName != null && shadowObj == null)
        {
            shadowObj = (GameObject)Instantiate(Resources.Load("Shadows/" + shadowName));
            shadowObj.transform.position = transform.position;
        }
        else if (shadowObj != null)
        {
            shadowObj.transform.position = transform.position;
        }
        else
        {
            Debug.LogWarning("Need to set shadow.");
        }
    }

    void Distortion()
    {
        if (selected)
        {
            if (!distortion.isPlaying) distortion.Play();
            
        }
        else
        {
            if (distortion.isPlaying) distortion.Stop();
        }
    }

    void Damage()
    {
        if (damage <= 0)
        {
            gameObject.SetActive(false);
        }
        else if (damage < maxDamage && recoverDamage <= 0 && reset <= 0)
        {
            damage++;
            recoverDamage = 1;
        }

        if (recoverDamage > 0)
        {
            recoverDamage -= 1 * Time.deltaTime;
        }

        if (reset > 0)
        {
            reset -= 1 * Time.deltaTime;
        }
    }
    public void ReduceDamage(int amount)
    {
        damage -= amount;
        reset = 1;
    }
    public int GetDamage()
    {
        return damage;
    }
}
