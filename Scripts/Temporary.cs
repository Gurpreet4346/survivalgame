using UnityEngine;

public class DebugScarecrow : MonoBehaviour
{
    void FixedUpdate()
    {
        if (Input.GetKeyDown(KeyCode.P)) // Press "P" to push the scarecrow
        {
            Rigidbody rb = GetComponent<Rigidbody>();
            if (rb != null)
            {
                rb.AddForce(Vector3.forward * 10f, ForceMode.Impulse);
                Debug.Log("Scarecrow moved by script.");
            }
        }
    }
}
