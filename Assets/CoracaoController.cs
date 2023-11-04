using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoracaoController : MonoBehaviour
{
    [SerializeField] private float vSpeed = 0.3f;
    [SerializeField] private float oscilationSpeed = 2.0f;
    private float initialPos;
    private bool isOffCam = false;
    // Start is called before the first frame update
    void Start()
    {
        initialPos = transform.position.y;        
    }

    // Update is called once per frame
    void Update()
    {
        if(isOffCam)
            return;
        Vector2 newPos = transform.position;
        float y = Mathf.Sin(Time.time*oscilationSpeed)*vSpeed;
        newPos.y = initialPos + y;
        transform.position = newPos;
    }

    void LateUpdate()
    {
        isOffCam = Mathf.Abs(transform.position.x - Camera.main.transform.position.x) > 15f;
    }
}
