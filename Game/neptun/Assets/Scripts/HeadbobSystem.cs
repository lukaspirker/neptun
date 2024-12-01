using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.ProBuilder;

public class HeadbobSystem : MonoBehaviour
{
    static HeadbobSystem instance;

    public static HeadbobSystem Instance
    {
        get { return instance; }
    }

    public float Amount = 0.05f;

    public float Frequency = 10.0f;

    public float Smooth = 10.0f;

    Vector3 StartPos;

    void Awake()
    {
        if (instance != null && instance == this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            instance = this;
        }
    }
        // Start is called before the first frame update
        void Start()
    {
        StartPos = transform.localPosition;
    }

    // Update is called once per frame
    void Update()
    {
        CheckForHeadbobTrigger();   
    }

    private void CheckForHeadbobTrigger()
    {
        float inputMagnitude = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical")).magnitude;

        if (inputMagnitude > 0)
        {
            StartHeadBob();
        }
    }

    private Vector3 StartHeadBob()
    {
        Vector3 pos = Vector3.zero;
        pos.y += Mathf.Lerp(pos.y, Mathf.Sin(Time.time * Frequency) * Amount * 1.4f, Smooth * Time.deltaTime);
        pos.x += Mathf.Lerp(pos.x, Mathf.Cos(Time.time * Frequency / 2f) * Amount * 1.6f, Smooth * Time.deltaTime);
        transform.localPosition += pos;

        return pos;
    }

    private void StopHeadbob()
    {
        if(transform.localPosition == StartPos) return;
        transform.localPosition = Vector3.Lerp(transform.localPosition,StartPos, 1* Time.deltaTime);
    }
}
