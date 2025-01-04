using UnityEngine;

public class DestroyWhenOffScreen : MonoBehaviour
{
    void Update()
    {
        if (transform.position.y < -Camera.main.orthographicSize - 1.0f)
        {
            Destroy(gameObject);
        }
    }
}
