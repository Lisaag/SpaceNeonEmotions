using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Button Presses", menuName = "buttn Presses")]
public class ButtonPresses : ScriptableObject
{
    public List<int> surveyResults = new List<int>();
}
