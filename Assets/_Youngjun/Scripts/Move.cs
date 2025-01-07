using UnityEngine;

public class Move : MonoBehaviour
{
    float moveSpeed = 4f;
    Vector3 forward;
    Vector3 right;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        forward = Camera.main.transform.forward;
        forward.y = 0;
        forward = Vector3.Normalize(forward);

        right = Quaternion.Euler(new Vector3(0f, 90f, 0f)) * forward;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.anyKey)
        {
            MoveObject();
        }
    }

    void MoveObject()
    {
        Vector3 rightMovement = right * moveSpeed * Time.smoothDeltaTime * Input.GetAxis("Horizontal");
        Vector3 forwardMovement = forward * moveSpeed * Time.smoothDeltaTime * Input.GetAxis("Vertical");

        Vector3 finalMovement = forwardMovement + rightMovement;
        Vector3 dir = Vector3.Normalize(finalMovement);

        if (dir != Vector3.zero)
        {
            transform.forward = dir;
            transform.position += finalMovement;
        }
    }
}
