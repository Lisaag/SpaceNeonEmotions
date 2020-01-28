using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonoBehaviourExtensionCoroutines : MonoBehaviour
{
    public static Coroutine StartCoroutine(MonoBehaviour behaviour, Action action, float delay)
    {
        return behaviour.StartCoroutine(WaitAndDo(delay, action));
    }

    private static IEnumerator WaitAndDo(float time, Action action)
    {
        yield return new WaitForSeconds(time);
        action();
    }
}
