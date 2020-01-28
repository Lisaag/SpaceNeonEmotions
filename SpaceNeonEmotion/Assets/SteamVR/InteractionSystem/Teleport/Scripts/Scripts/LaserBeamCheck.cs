using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserBeamCheck : MonoBehaviour
{
    public GameObject collided;
    public Color regularColor;
    public Color highLightedColor;
    bool onObject = false;

    private void OnTriggerStay(Collider collision)
    {
        if (collision.CompareTag("MenuButton"))
        {
            collided = collision.gameObject;

            onObject = true;

            StopAllCoroutines();

            // You can re-use this block between calls rather than constructing a new one each time.
            var block = new MaterialPropertyBlock();

            // You can look up the property by ID instead of the string to be more efficient.
            block.SetColor("_BaseColor", highLightedColor);

            // You can cache a reference to the renderer to avoid searching for it.
            collided.GetComponent<Renderer>().SetPropertyBlock(block);
        }
    }

    private void FixedUpdate()
    {
        if (onObject)
        {
            StartCoroutine(BoolSwap());
        }

        if (!onObject)
        {
            // You can re-use this block between calls rather than constructing a new one each time.
            var block = new MaterialPropertyBlock();

            // You can look up the property by ID instead of the string to be more efficient.
            block.SetColor("_BaseColor", regularColor);

            if (collided)
                collided.GetComponent<Renderer>().SetPropertyBlock(block);

            collided = null;
        }
    }     

    IEnumerator BoolSwap()
    {
        yield return new WaitForSeconds(0.08f);
        onObject = !onObject;
    }
}
