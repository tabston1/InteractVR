using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BillboardExit : MonoBehaviour
{

    private GameObject parent;


    void onClick()
    {
        parent = this.transform.parent.gameObject;
        if (parent != null)
        {
            Destroy(parent);
        }

    }

}
