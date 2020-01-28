using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Portal : MonoBehaviour
{
    public bool dissapear;
    public float maxSize = 0.3f;
    public float growSpeed;
    public GameObject shape;
    public float minX, maxX, minZ, maxZ, roofY, floorY, roofOffset;
    public int respawnInterval, minRespawnTime, maxRespawnTime;
    public Vector3 newPosition;
    public AnimationStart animationHandler;
    public AudioSource increaseSound, decreaseSound;

    // Start is called before the first frame update
    void Start()
    {
        transform.localScale = new Vector3(0.05f, 0.05f, 0.05f);
        transform.position = new Vector3(shape.transform.position.x, floorY, shape.transform.position.z);
        transform.localRotation = new Quaternion(0, shape.transform.localRotation.y, 0, transform.localRotation.w);
        dissapear = true;
        respawnInterval = Random.Range(minRespawnTime, maxRespawnTime);
        animationHandler.StartAnimation();
    }

    public void ShapeMovementCaller()
    {
        StartCoroutine(ShapeMovement(dissapear));
    }

    public IEnumerator ShapeMovement(bool dissapear)
    {
        shape.GetComponent<Rigidbody>().isKinematic = false;
        shape.GetComponent<Collider>().isTrigger = true;
        yield return new WaitForSeconds(1);
        shape.GetComponent<Collider>().isTrigger = false;

        if (dissapear) // Object goes through floor
        {
            newPosition = new Vector3(Random.Range(minX, maxX), roofY + roofOffset, Random.Range(minZ, maxZ));
            shape.transform.position = newPosition;
            shape.GetComponent<Rigidbody>().isKinematic = true;
        }
    }

    public void ResetPortalCaller()
    {
        if (dissapear)
            StartCoroutine(ResetPortal());

        else
            Destroy(gameObject);
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

        for (int i = 0; i < renderers.Length; i++)
        {
            renderers[i].enabled = true;
        }

        animationHandler.StartAnimation();
    }

    public void PlayIncreaseSound()
    {
        SoundManager.instance.PlaySound(increaseSound, gameObject, false, 0);
    }

    public void PlayDecreaseSound()
    {
        SoundManager.instance.PlaySound(decreaseSound, gameObject, false, 0);
    }
}
