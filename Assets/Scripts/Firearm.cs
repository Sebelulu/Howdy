using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Firearm : MonoBehaviour, IGun
{
    [SerializeField] GameObject projectile;
    [SerializeField] float fireDelta = 0.5F;
    [SerializeField] Transform origin;


    private float nextFire = 0.5F;
    private float myTime = 0.0F;

    bool canShoot;

    void Start()
    {
        nextFire = fireDelta;
    }

    public void Fire(RaycastHit hit)
    {
        if (canShoot)
        {
            canShoot = false;
            //GameObject newProjectile = Instantiate(bullet, transform.position, transform.rotation) as GameObject;

            
            

            //Vector3 targetDir = target - origin.position;
            //targetDir = targetDir.normalized;

            //float dot = Vector3.Dot(targetDir, transform.forward);
            //float angle = Mathf.Acos(dot) * Mathf.Rad2Deg;


            Vector3 hitPoint = Vector3.zero;

            if (hit.collider != null)
            {
                Debug.Log(hit.collider.gameObject.name);
                hitPoint = hit.point;
                IHittable h = hit.collider.GetComponent<IHittable>();
                if(h != null)
                {
                    h.Hit();
                }
            }
            else
            {
                //Figure this shit out...
                hitPoint = Vector3.forward * 100;
            }

            GameObject bulletTrail = Instantiate(projectile, transform.position, transform.rotation);
            bulletTrail.GetComponent<GunTrail>().SetPositions(origin.position, hitPoint);

            //newProjectile.transform.LookAt(target);

            

            myTime = 0.0F;

        }

    }

    private void Update()
    {
        myTime += Time.deltaTime;

        if (myTime > nextFire)
        {
            canShoot = true;
        }
    }
}

