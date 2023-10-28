using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundController : MonoBehaviour
{
    [SerializeField] private float skySpeed;
    [SerializeField] private Transform leftEdge;
    [SerializeField] private GameObject skySprite;

    private Transform skyObj;
    private float xBegin;

    // Start is called before the first frame update
    void Start()
    {
        // skySprite.SetActive(true);
        skyObj = transform.GetChild(1);
        xBegin = skyObj.transform.position.x;
    }

    // Update is called once per frame
    void Update()
    {
        float xPos = skyObj.transform.position.x;
        float xNext = xPos - skySpeed * Time.deltaTime;
        float xDiff = xNext - leftEdge.transform.position.x;
        if(xDiff <= 0)
            xNext = xBegin + xDiff;

        skyObj.transform.position = new Vector3(xNext, skyObj.transform.position.y, skyObj.transform.position.z);
    }
}
