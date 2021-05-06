using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Stone : MonoBehaviour
{
    [SerializeField] private Ore ore;

    private void OnCollisionEnter(Collision collision)
    {
        print(collision.gameObject);
        if (collision.gameObject.CompareTag(Constant.pick))
        {
            Vector3 pos = collision.contacts[0].point;
            Vector3 _normal = collision.contacts[0].normal;
            Quaternion rot = Quaternion.FromToRotation(-Vector3.forward, _normal);
            Instantiate(ore, pos, rot);
        }
    }


}
