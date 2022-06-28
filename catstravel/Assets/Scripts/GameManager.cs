using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    Button xbtn;
    public GameObject Inventory = null;
    void Start()
    {

    }

    void Update()
    {
        if (Input.GetKey(KeyCode.I))
        {
            Inventory.SetActive(true);
        }
        if (Input.GetKey(KeyCode.Tab))
        {

        }
    }
}
