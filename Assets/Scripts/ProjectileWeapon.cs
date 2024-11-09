using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileWeapon : Weapon
{
    public GameObject projectilePrefab;
    public float projectileAngle=30;
    public float projectileForce=10;
    public float projectileTime=5;


    protected override void Fire()
    {
        ProjectileFire();
    }

    void ProjectileFire()
    {
        Camera cam = Camera.main;

        Vector3 direction = cam.transform.forward;
        direction = Quaternion.AngleAxis(-projectileAngle, transform.right) * direction;
        direction.Normalize();
        direction *= projectileForce;

        GameObject obj = Instantiate(projectilePrefab);
        obj.GetComponent<Bomb>().time = projectileTime;
        obj.transform.position = firingPosition.position;

        obj.GetComponent<Rigidbody>().AddForce(direction, ForceMode.Impulse);
    }
}
