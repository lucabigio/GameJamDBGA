using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class fixepipe : MonoBehaviour
{
    [SerializeField]
    GameObject Full;
    [SerializeField]
    GameObject Empty;
    

    public void fill()
    {
        Empty.SetActive(false);
        Full.SetActive(true);
    }

    public void empty()
    {
        Empty.SetActive(true);
        Full.SetActive(false);
    }
}
