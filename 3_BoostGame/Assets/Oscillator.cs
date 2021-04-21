using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[DisallowMultipleComponent] //only allows one components
public class Oscillator : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] Vector3 movmentVector = new Vector3(10f, 0f, 0f);
    [SerializeField] float period = 2f;

    [Range(0,1)][SerializeField] float movmentFactor; // 0 for not moved , 1 for fully moved
    Vector3 startingPos;

    void Start()
    {
        startingPos = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        //Set movment factor
        if(period <= Mathf.Epsilon) { return; }
        float cycles = Time.time / period;

        const float tau = Mathf.PI * 2f ;
        float rawSinWave  = Mathf.Sin(cycles * tau);

        movmentFactor = rawSinWave / 2f + 0.5f;
        Vector3 offset = movmentFactor * movmentVector;
        transform.position = startingPos + offset;
    }
}
