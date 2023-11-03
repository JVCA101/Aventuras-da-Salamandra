using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MorceguinhoController : MonoBehaviour
{
    [SerializeField] private float flyingSpeed = 5f;
    [SerializeField] private float oscilationSpeed = 2.0f;
    [SerializeField] private bool isFacingLeft = true;
    // Start is called before the first frame update
    void Start()
    {
        GetComponent<SpriteRenderer>().flipX = isFacingLeft;
    }

    // Update is called once per frame
    void Update()
    {
        float y = Mathf.Sin(Time.time*oscilationSpeed)*flyingSpeed;
        Vector2 newPos = transform.position;
        newPos.y = y;
        transform.position = newPos;
    }
}
