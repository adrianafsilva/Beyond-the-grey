//(Recorri ao Google/Youtube)

using UnityEngine;

public class Parallax : MonoBehaviour
{
    private float length, startpos;
    public Transform cam;
    public float parallaxEffect; 

    void Start()
    {
        startpos = transform.position.x;
        length = GetComponent<SpriteRenderer>().bounds.size.x;
    }

    void FixedUpdate()
    {
        if (cam == null) cam = Camera.main.transform;

        float dist = (cam.position.x * (1 - parallaxEffect));
        transform.position = new Vector3(startpos + dist, transform.position.y, transform.position.z);
    }
}
