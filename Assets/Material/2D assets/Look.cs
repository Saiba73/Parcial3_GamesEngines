using UnityEngine;

public class Look : MonoBehaviour
{
    public GameObject target;
  
    void Update()
    {
        transform.LookAt(target.transform.position);
    }
}
