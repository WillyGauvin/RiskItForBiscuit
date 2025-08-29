using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "TutorialManager", menuName = "Scriptable Objects/TutorialManager")]
public class TutorialManager : ScriptableObject
{
    Dictionary<string, bool> ids = new Dictionary<string, bool>();

    public bool CheckIfFirstTime(string id)
    {
        if (!ids.ContainsKey(id))
        {
            ids[id] = true;
            return true;
        }
        return false;
    }
}
