using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Portal : MonoBehaviour
{
    private bool dissapear, grow;
    public float maxSize = 0.3f;
    public float growSpeed;
    public GameObject shape;
    public float minX, maxX, minZ, maxZ, roofY, floorY, roofOffset, respawnInterval;
    public Vector3 newPosition;

    // Start is called before the first frame update
    void Start()
    {
        transform.localScale = new Vector3(0.05f, 0.05f, 0.05f);
        transform.position = new Vector3(shape.transform.position.x, floorY, shape.transform.position.z);
        transform.localRotation = new Quaternion(shape.transform.localRotation.x, shape.transform.localRotation.y, 0, transform.localRotation.w);
        dissapear = true;
        grow = true;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (transform.localScale.x < maxSize)
        {
            if (grow)
                transform.localScale = new Vector3(transform.localScale.x * growSpeed * growSpeed, maxSize, transform.localScale.z * growSpeed * growSpeed);
        }
        else
        {
            if (dissapear)
                StartCoroutine(ShapeMovement(dissapear));

            if (!grow)
                transform.localScale = new Vector3(transform.localScale.x / growSpeed / growSpeed, maxSize, transform.localScale.z / growSpeed / growSpeed);
        }
    }

    public IEnumerator ShapeMovement(bool dissapear)
    {
        grow = !grow;
        shape.GetComponent<Rigidbody>().isKinematic = false;
        shape.GetComponent<Collider>().isTrigger = true;
        yield return new WaitForSeconds(1);
        shape.GetComponent<Collider>().isTrigger = false;

        if (dissapear) // Object goes through floor
        {
            newPosition = new Vector3(Random.Range(minX, maxX), roofY + roofOffset, Random.Range(minZ, maxZ));
            shape.transform.position = newPosition;
            shape.GetComponent<Rigidbody>().isKinematic = true;
            StartCoroutine(ResetPortal());
        }

        if (!dissapear)
        {
            yield break;
        }
    }

    public IEnumerator ResetPortal()
    {
        dissapear = false;
        transform.position = new Vector3(newPosition.x, roofY, newPosition.z);
        transform.localScale = new Vector3(0.05f, 0.05f, 0.05f);

        MeshRenderer[] renderers = GetComponentsInChildren<MeshRenderer>();

        for (int i = 0; i < renderers.Length; i++)
        {
            renderers[i].enabled = false;
        }
        
        yield return new WaitForSeconds(respawnInterval);

        StartCoroutine(ShapeMovement(dissapear));
    }
}
