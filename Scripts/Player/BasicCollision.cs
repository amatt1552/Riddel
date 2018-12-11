using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicCollision
{
  
  /// <summary>
  /// gets all directions between origin and point of collision
  /// </summary>
  /// <param name="collider"></param>
  /// <param name="distance"></param>
  /// <param name="direction"></param>
  /// <returns></returns>
  
    public List<Vector3> CollisionDirections(SphereCollider collider, float distance, Vector3 direction)
    {
        float radius = collider.radius;
        List<Vector3> vectorList = new List<Vector3>();
        RaycastHit[] hitAll;

        hitAll = Physics.SphereCastAll(collider.transform.position, radius, direction, distance);
        for (int i = 0; i < hitAll.Length; i++)
        {
            Vector3 pos = Vector3.zero;
            if (hitAll[i].point != Vector3.zero)
            {
                pos = Vector3.Normalize(hitAll[i].point - collider.transform.position);
            }
            vectorList.Add(pos);
        }
        return vectorList;

    }

    /// <summary>
    /// gets direction between origin and point of collision
    /// </summary>
    /// <param name="collider"></param>
    /// <param name="distance"></param>
    /// <param name="direction"></param>
    /// <returns></returns>

    public Vector3 CollisionDirection(SphereCollider collider, float distance, Vector3 direction)
    {
        float radius = collider.radius;
        Ray ray = new Ray();
        ray.direction = direction;
        ray.origin = collider.transform.position;
        RaycastHit hit;

        Physics.SphereCast(ray, radius/2, out hit, distance);
        
        Vector3 pos = Vector3.zero;
            
        pos = Vector3.Normalize(hit.point - collider.transform.position);
            
        return pos;

    }

    public bool Colliding(SphereCollider collider, float distance, Vector3 direction)
    {
        float radius = collider.radius;
        Ray ray = new Ray();
        ray.direction = direction;
        ray.origin = collider.transform.position;
        RaycastHit hit;

        return Physics.SphereCast(ray, radius/2, out hit, distance);
    }

    public bool Colliding(CircleCollider2D collider, float distance, Vector3 direction)
    {
        float radius = collider.radius;
        return Physics2D.CircleCast(collider.transform.position, radius / 2, direction, distance);
    }
}
