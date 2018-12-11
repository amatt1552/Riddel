using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Telephathy : MonoBehaviour
{
    //public static GameObject targetA, targetB;
    //ObjectCollision targetCollisionA, targetCollisionB;
   // ObjectSettings targetSettingsA, targetSettingsB;
   // Rigidbody2D rbTargetA, rbTargetB;

    public static GameObject[] targets = new GameObject[10];
    ObjectCollision[] targetsCollisions = new ObjectCollision[10];
    ObjectSettings[] targetsSettings = new ObjectSettings[10];
    Rigidbody2D[] rbTargets = new Rigidbody2D[10];
    int maxTouches = 2;

    TouchInputs touch;
    InGameDebug debug;
    //Vector3 tapPosA, tapPosB;

    //float extraDistance = 1.5f; 
    public int strength;
    public int maxSpeed, maxRotateSpeed;
    Int32 speedReduction, speedIncrease;

    public int massTolerance = 1;

    public bool collecting;
    GameObject floater;

    bool crushOneShot;

    BasicCollision basicCollision;

    /* NOTES
     * target is the object you selected
     * the ObjectCollision helps to detect the direction of the collisions
     * strength is how fast you can move the object.
     * need to set up boxSettings
     * speedReduction and speedIncrease should be inputed as positive
     */

    void Start()
    {
        basicCollision = new BasicCollision();
        touch = TouchInputs.GC;
        debug = InGameDebug.GC;
        
    }
    void Update()
    {
        
        for (int i = 0; i < maxTouches; i++)
        {
            float distance = 0;
            float extraDistance = 0;
            if (targets[i] != null)
            {
                extraDistance = 2f - targetsSettings[i].radius;
                //makes static objects rotate/move properly

                if (targetsSettings[i].isStatic)
                {
                    distance = targetsSettings[i].radius * 2;
                }
                else
                {
                    if (rbTargets[i].mass <= massTolerance)
                    {
                        distance = targetsSettings[i].radius + extraDistance;

                    }
                    else
                    {
                        distance = targetsSettings[i].radius;
                        
                    }
                }

                //removes target

                if ((Input.touchCount < i || Vector2.Distance(touch.TapPosition(i), targets[i].transform.position) > distance) && !collecting)
                {
                    targetsSettings[i].selected = false;
                    rbTargets[i].gravityScale = 1;

                    rbTargets[i] = null;
                    targetsSettings[i] = null;
                    targetsCollisions[i] = null;
                    targets[i] = null;
                }

            }
            
        }

        if (Input.touchCount == 0)
        {
            float distance = 0;
            float extraDistance = 0;
            if (targets[0] != null)
            {
                extraDistance = 2f - targetsSettings[0].radius;
                //makes static objects rotate/move properly

                if (targetsSettings[0].isStatic)
                {
                    distance = targetsSettings[0].radius * 2;
                }
                else
                {
                    if (rbTargets[0].mass <= massTolerance)
                    {
                        distance = targetsSettings[0].radius + extraDistance;

                    }
                    else
                    {
                        distance = targetsSettings[0].radius;

                    }
                }

                //removes target

                if ((touch.TapEnded(0) || Vector2.Distance(touch.TapPosition(0), targets[0].transform.position) > distance) && !collecting)
                {
                    targetsSettings[0].selected = false;
                    rbTargets[0].gravityScale = 1;

                    rbTargets[0] = null;
                    targetsSettings[0] = null;
                    targetsCollisions[0] = null;
                    targets[0] = null;
                }
            }
        }
    }

    void FixedUpdate()
    { 
        Selection();

        for(int i = 0; i < Input.touchCount; i++)
        {
            if (i < maxTouches && targets[i] != null)
            {
                //Movement(targets[i], rbTargets[i], targetsSettings[i], targetsCollisions[i], i);
                NewMovement(targets[i], rbTargets[i], targetsSettings[i], i);
                if (Input.touchCount > 0)
                {
                    Rotation();
                    if(GameObject.Find("Inventory"))
                        Collect();
                    
                }
            }
        }
        if (Input.touchCount == 0)
        {
            if (targets[0] != null)
            {
                //Movement(targets[i], rbTargets[i], targetsSettings[i], targetsCollisions[i], i);
                NewMovement(targets[0], rbTargets[0], targetsSettings[0], 0);
                if (Input.touchCount > 0)
                {
                    Rotation();
                    if (GameObject.Find("Inventory"))
                        Collect();

                }
            }
        }

        if (targets[0] != null)
        Crush(targets[0].GetComponent<Collider2D>());
    }

    void Movement(GameObject target, Rigidbody2D rbTarget, ObjectSettings targetSettings, ObjectCollision targetCollision, int index)
    {
        float speedReduction = SlowDownValue(index);
        float speedIncrease = 0;
        float actualSpeed = maxSpeed - speedReduction + speedIncrease;

        rbTarget.gravityScale = 0;

        if (!targetSettings.isStatic)
        {
            
            #region dynamic telephathy
            if ((touch.TapPosition(index).x - target.transform.position.x >= 0 && !targetCollision.right))
            {
                if (rbTarget.velocity.x < actualSpeed)
                {
                        
                    rbTarget.AddForce(Vector2.right * (strength - rbTarget.mass));
                }
                else
                {
                    rbTarget.AddForce(Vector2.left * (strength * 1.5f - rbTarget.mass));
                }
            }

            if ((touch.TapPosition(index).x - target.transform.position.x <= 0 && !targetCollision.left))
            {
                if (rbTarget.velocity.x > -actualSpeed)
                {
                       
                    rbTarget.AddForce(Vector2.left * (strength - rbTarget.mass));
                }
                else
                {
                    rbTarget.AddForce(Vector2.right * (strength * 1.5f - rbTarget.mass));
                }
            }

            if ((touch.TapPosition(index).y - target.transform.position.y >= 0 && !targetCollision.up))
            {
                if (rbTarget.velocity.y < actualSpeed)
                {
                       
                    rbTarget.AddForce(Vector2.up * (strength - rbTarget.mass));
                }
                else
                {
                    rbTarget.AddForce(Vector2.down * (strength * 1.5f - rbTarget.mass));
                }
            }

            if ((touch.TapPosition(0).y - target.transform.position.y <= 0 && !targetCollision.down))
            {
                if (rbTarget.velocity.y > -actualSpeed)
                {
                        
                    rbTarget.AddForce(Vector2.down * (strength - rbTarget.mass));
                }
                else
                {
                    rbTarget.AddForce(Vector2.up * (strength * 1.5f - rbTarget.mass));
                }
            }
            #endregion
            
        }
        else
        {
            #region static telepathy
            /*
            if (TouchInputs.GC.TapBegan(0))
            {
                targetSettingsA.pivot.transform.position = TouchInputs.GC.TapPosition(0);
            }

            Vector3 oldDiff = targetSettingsA.pivot.transform.position - targetA.transform.position; 
            Vector3 newDiff = TouchInputs.GC.TapPosition(0) - targetA.transform.position;

            float oldRot = Mathf.Atan2(oldDiff.y, oldDiff.x) * Mathf.Rad2Deg;
            float newRot = Mathf.Atan2(newDiff.y, newDiff.x) * Mathf.Rad2Deg;

            if (newRot < oldRot)
            {
                targetA.transform.Rotate(Vector3.forward * 30 * Time.deltaTime);
            }
            if (newRot > oldRot)
            {
                targetA.transform.Rotate(Vector3.forward * -30 * Time.deltaTime);
            }
            // targetA.transform.rotation = Quaternion.Euler(0f, 0f, rot_z - pivot);

            InGameDebug.GC.NewDebug("" + oldRot, "old");
            InGameDebug.GC.NewDebug("" + newRot, "new");
            */
            #endregion
        }
    }

    void NewMovement(GameObject target, Rigidbody2D rbTarget, ObjectSettings targetSettings, int index)
    {
        float speedReduction = SlowDownValue(index);
        float speedIncrease = 0;
        float actualSpeed = maxSpeed - speedReduction + speedIncrease;
        Vector2 direction = Vector2.zero;
        Vector3 origin = new Vector3(target.transform.position.x, target.transform.position.y, 0);
        
        direction = touch.TapPosition(index) - origin;
        InGameDebug.GC.NewDebug(actualSpeed + "", "maxSpeed");
        rbTarget.gravityScale = 0;
        print(rbTarget.velocity.magnitude);
        if (!targetSettings.isStatic)
        {
            Converging(rbTarget);
            #region dynamic telephathy
            if (Vector3.Distance(touch.TapPosition(index), target.transform.position) >= 0)
            {
                if (rbTarget.velocity.magnitude < actualSpeed)
                {

                    rbTarget.AddForce(direction * (strength - rbTarget.mass));
                }

            }
            else
            {
                Hover();
            }
            
            #endregion

        }
        else
        {
            #region static telepathy
            /*
            if (TouchInputs.GC.TapBegan(0))
            {
                targetSettingsA.pivot.transform.position = TouchInputs.GC.TapPosition(0);
            }

            Vector3 oldDiff = targetSettingsA.pivot.transform.position - targetA.transform.position; 
            Vector3 newDiff = TouchInputs.GC.TapPosition(0) - targetA.transform.position;

            float oldRot = Mathf.Atan2(oldDiff.y, oldDiff.x) * Mathf.Rad2Deg;
            float newRot = Mathf.Atan2(newDiff.y, newDiff.x) * Mathf.Rad2Deg;

            if (newRot < oldRot)
            {
                targetA.transform.Rotate(Vector3.forward * 30 * Time.deltaTime);
            }
            if (newRot > oldRot)
            {
                targetA.transform.Rotate(Vector3.forward * -30 * Time.deltaTime);
            }
            // targetA.transform.rotation = Quaternion.Euler(0f, 0f, rot_z - pivot);

            InGameDebug.GC.NewDebug("" + oldRot, "old");
            InGameDebug.GC.NewDebug("" + newRot, "new");
            */
            #endregion
        }
    }

    void Selection()
    {
        for (int i = 0; i < Input.touchCount; i++)
        {
            if (touch.TapBegan(i) && touch.ObjectOnTap(i) != null && !Inventory.open && i < maxTouches)
            {
                if (touch.ObjectOnTap(i).tag == "moveable")
                {
                    print("selecting");
                    targets[i] = touch.ObjectOnTap(i);
                    rbTargets[i] = targets[i].GetComponent<Rigidbody2D>();
                    targetsCollisions[i] = targets[i].GetComponent<ObjectCollision>();
                    targetsSettings[i] = targets[i].GetComponent<ObjectSettings>();

                    targetsSettings[i].selected = true;
                }
            }
        }
        if (Input.touchCount == 0)
        {
            if (touch.TapBegan(0) && touch.ObjectOnTap(0) != null && !Inventory.open)
            {
                if (touch.ObjectOnTap(0).tag == "moveable")
                {
                    print("selecting");
                    targets[0] = touch.ObjectOnTap(0);
                    rbTargets[0] = targets[0].GetComponent<Rigidbody2D>();
                    targetsCollisions[0] = targets[0].GetComponent<ObjectCollision>();
                    targetsSettings[0] = targets[0].GetComponent<ObjectSettings>();

                    targetsSettings[0].selected = true;
                }
            }
        }
    }

    void Rotation()
    {
        
            if (targets[1] == null && (TouchInputs.GC.TapMoved(1) || TouchInputs.GC.TapStationary(1)))
            {
                
                rbTargets[0].AddTorque(touch.RotatingDirection() * 2);
            }
       
    }

    void Collect()
    {

        if (targetsSettings[0].collectable && Inventory.open)
        {
            collecting = true;
            

            if (floater == null )
            {
                floater = (GameObject)Instantiate(Resources.Load("Images/" + targets[0].name));
            }
            else
            {
                floater.transform.position = touch.TapPosition(0);
            }

            targets[0].SetActive(false);

            if (touch.TapEnded(0) && Camera.main.WorldToViewportPoint(touch.TapPosition(0)).y < 0.2f)
            {
                Inventory.GC.TryCollect(targets[0]);
                Destroy(targets[0]);
                Destroy(floater);
                collecting = false;
                targets[0] = null;
            }
            else if (touch.TapEnded(0))
            {
                Destroy(floater);
                targets[0].SetActive(true);
                collecting = false;
            }
        }
        else
        {
            print("spast..");
            targets[0].SetActive(true);
            collecting = false;
            Destroy(floater);
        }
  
    }

    void Crush(Collider2D target)
    {
        ContactFilter2D filter = new ContactFilter2D();
        Collider2D[] results = new Collider2D[0];
        if (targets[1] != null)
        {
            if (target.OverlapCollider(filter, results) > 0)
            {
                for (int i = 0; i < results.Length; i++)
                {
                    if (results[i].name == targets[1].name && !crushOneShot)
                    {
                        
                        if (targetsSettings[0].crushable)
                        {
                            targetsSettings[0].ReduceDamage(1);
                            print(targetsSettings[0].GetDamage());
                        }
                        if (targetsSettings[1].crushable)
                        {
                            targetsSettings[1].ReduceDamage(1);
                        }
                        crushOneShot = true;
                    }
                }
            }
            else
            {
                crushOneShot = false;
            }
        }

        
    }

    void Converging(Rigidbody2D rb)
    {
        
    }

    void Hover()
    {

    }

    float SlowDownValue(int index)
    {
        if (touch.TapStationary(index))
        {
            //return maxSpeed / 2;
        }
        
        return 0;
    }
}
