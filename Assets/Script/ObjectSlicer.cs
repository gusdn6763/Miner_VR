using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EzySlice;

public class ObjectSlicer : MonoBehaviour
{
    public float slicedObjectInitialVelocity = 100;
    public Material slicedMaterial;
    public Transform startSlicingPoint;
    public Transform endSlicingPoint;
    public LayerMask sliceableLayer;
    public VelocityEstimator velocityEstimator;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        RaycastHit hit;
        Vector3 slicingDirection = endSlicingPoint.position - startSlicingPoint.position;
        bool hasHit = Physics.Raycast(startSlicingPoint.position, slicingDirection, out hit, slicingDirection.magnitude + 0.5f, sliceableLayer);
        if (hasHit)
        {
            BrokenOre ore = hit.transform.gameObject.GetComponent<BrokenOre>();
            if(ore.possible)
            {
                Slice(hit.transform.gameObject, hit.point, velocityEstimator.GetVelocityEstimate(), ore);
            }
        }
    }

    void Slice(GameObject target, Vector3 planePosition, Vector3 slicerVelocity, BrokenOre ore)
    {
        Debug.Log("WE SLICE THE OBJECT");
        Vector3 slicingDirection = endSlicingPoint.position - startSlicingPoint.position;
        Vector3 planeNormal = Vector3.Cross(slicerVelocity, slicingDirection);

        SlicedHull hull = target.Slice(planePosition, planeNormal, slicedMaterial);
        print(hull);
        if(hull != null)
        {
            ore.BrokenObject();
            GameObject upperHull = hull.CreateUpperHull(target, slicedMaterial);
            GameObject lowerHull = hull.CreateLowerHull(target, slicedMaterial);

            CreateSlicedComponent(upperHull);
            CreateSlicedComponent(lowerHull);

            Destroy(target);
        }
    }

    void CreateSlicedComponent(GameObject slicedHull)
    {
        Rigidbody rb = slicedHull.AddComponent<Rigidbody>();
        MeshCollider collider = slicedHull.AddComponent<MeshCollider>();
        collider.convex = true;

        rb.AddExplosionForce(slicedObjectInitialVelocity, slicedHull.transform.position, 1);

        Destroy(slicedHull, 4);
    }
}
