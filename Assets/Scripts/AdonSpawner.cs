using UnityEngine;
using System.Collections;

public class AdonSpawner : MonoBehaviour {

    [SerializeField]
    private GameObject adonPrefab = null;

    public GameObject Spawn()
    {
        if(adonPrefab != null)
        {
            var adon = Instantiate(adonPrefab);
            adon.transform.position = transform.position;
            adon.transform.SetParent(transform);
            return adon;
        }
        return null;
    }
}
