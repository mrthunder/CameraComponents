using UnityEngine;
using System.Collections;

public class Walk : MonoBehaviour {

    private Vector3 m_v3Velocity = new Vector3(0, 0, 0);

    private Rigidbody m_Ridybody;

    public Vector3 Velocity { get { return m_v3Velocity; } }

    public int accel = 1;


	// Use this for initialization
	void Start () {
        m_Ridybody = GetComponent<Rigidbody>();
        if(m_Ridybody == null)
        {
            m_Ridybody = gameObject.AddComponent<Rigidbody>();
        }
        m_Ridybody.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationZ ;
	}
	
	// Update is called once per frame
	void Update () {
        if(Input.GetAxisRaw("Horizontal") != 0)
        {
            transform.Rotate(0, Input.GetAxisRaw("Horizontal") * accel, 0);
        }
       
        
    }
    void FixedUpdate()
    {
        Vector3 vel = transform.forward * (Input.GetAxis("Vertical"));
        m_Ridybody.AddForce(vel*100);
    }
}
