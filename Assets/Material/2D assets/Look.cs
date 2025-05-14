using UnityEngine;

public class Look : MonoBehaviour
{
    GameObject target;

    private void Start()
    {
        target = GameObject.FindGameObjectWithTag("MainCamera");
    }

    void Update()
    {

        transform.LookAt(target.transform.position);


    }
}
