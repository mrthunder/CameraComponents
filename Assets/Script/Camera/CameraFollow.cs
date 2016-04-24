using UnityEngine;
using System.Collections;

public class CameraFollow : MonoBehaviour {

    [SerializeField]
    private string m_sTargetTag = string.Empty;

    private GameObject m_Target;
    [SerializeField,Range(5,100)]
    private int m_iDistanceFromTarget = 10;

    private float m_fSpeed = 2;
	// Use this for initialization
	void Start () {
        if (!string.IsNullOrEmpty(m_sTargetTag))
        {
            m_Target = GameObject.FindWithTag(m_sTargetTag);
        }
        transform.position = m_Target.transform.position;
       m_fSpeed = 10;
    }
	
	// Update is called once per frame
	void Update () {
        if(m_Target!= null)
        {
            Follow();
        }
	
	}

    void Follow()
    {
        Vector3 distance = m_Target.transform.position - m_Target.transform.forward * m_iDistanceFromTarget;
        transform.position = Vector3.Lerp(transform.position, distance, Time.deltaTime * m_fSpeed);

        float angle = m_Target.transform.rotation.eulerAngles.y;
        if (Mathf.Round(transform.rotation.eulerAngles.y) != Mathf.Round(angle))
        {
            float myAngle = transform.rotation.eulerAngles.y;
            float newAngle = Mathf.LerpAngle(myAngle, angle, Time.deltaTime * 5);
            transform.rotation = Quaternion.Euler(0, newAngle, 0);
        }

    }
    
}
