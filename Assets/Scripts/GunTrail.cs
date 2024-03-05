using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public class GunTrail : MonoBehaviour
{
    [SerializeField] float lifeTime = 1.5f; 
    private IEnumerator Start()
    {
        //Almost a whipcracking sound. Whapascz.
        LineRenderer l = GetComponent<LineRenderer>();
        while (0 < lifeTime)
        {
            
            yield return null;
            lifeTime -= Time.deltaTime;
            //Is gonna linger on one until its one second left, do some mapping if I want it to be a different effect.
            l.endColor = new Color(1,1,1,lifeTime);
        }
        Destroy(gameObject);
    }

    public void SetPositions(Vector3 start, Vector3 end)
    {
        LineRenderer l = GetComponent<LineRenderer>();

        Vector3[] v = { start, end };

        l.SetPositions(v);
    }
}
