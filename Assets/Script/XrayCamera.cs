using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class XrayCamera : MonoBehaviour
{

    /// <summary>
    /// This bolean will tell if the camera will just make the object transparent or will toggle the mesh render.
    /// </summary>
    [SerializeField, Tooltip("Do you want to see the object transparent or don't want to see anything. True - Transparent; False - don't see.")]
    private bool m_bTransparent = false;

    /// <summary>
    /// This is the angle that the xray will affect the objects.
    /// </summary>
    [SerializeField, Tooltip("This is the angle, that the Xray will affect"), Range(0, 180)]
    private float m_fAngle = 30;

    /// <summary>
    /// The object inside the <see cref="m_fAngle"/> and in the range of that distance will be affect of the Xray.
    /// </summary>
    [SerializeField, Tooltip("Distance that the Xray will affect"), Range(1, 10)]
    private int m_iDistance = 5;

    /// <summary>
    /// This is the list of GameObjects that are caught by the Xray
    /// </summary>
    private List<GameObject> m_ObjectsInRange = new List<GameObject>();



    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    void FixedUpdate()
    {
        GetGameObjects();
        Xray();
    }

    void ScanView(GameObject obj)
    {
        Vector3 cameraFoward = this.gameObject.transform.forward;
        Vector3 ToObject = obj.transform.position - this.gameObject.transform.position;

        float dotProduct = Vector3.Dot(cameraFoward, ToObject);

        float lenght = (cameraFoward.magnitude * ToObject.magnitude);

        float rad = Mathf.Acos((dotProduct / lenght));

        float angle = rad * Mathf.Rad2Deg;

        if (angle < m_fAngle)
        {
            if (!m_ObjectsInRange.Exists(x => x == obj))
            {
                m_ObjectsInRange.Add(obj);
            }

        }
        else
        {
            if (m_ObjectsInRange.Exists(x=>x == obj))
            {
                Xray(obj, true);
                m_ObjectsInRange.Remove(obj);
            }
        }


    }

    void GetGameObjects()
    {
        var hits = Physics.SphereCastAll(this.transform.position, m_iDistance, Vector3.forward,m_iDistance/2);

        foreach (RaycastHit hit in hits)
        {
            ScanView(hit.collider.gameObject);
        }
        var Objects = m_ObjectsInRange.FindAll(x => !System.Array.Exists(hits, y => y.collider.gameObject == x));
        
        for(int i = 0; i < Objects.Count; i++)
        {
            Xray(Objects[i], true);
            m_ObjectsInRange.Remove(Objects[i]);
        }
    }

    void Xray()
    {
        if (m_bTransparent)
        {
            Transparent();
        }
        else
        {
            ToggleMesh();
        }
    }
    void Xray(GameObject obj, bool invert)
    {
        if (m_bTransparent)
        {
            Transparent(obj, invert);
        }
        else
        {
            ToggleMesh(obj, invert);
        }
    }

    void Transparent()
    {
        foreach (GameObject gameobjectInRange in m_ObjectsInRange)
        {
            Renderer mat = gameobjectInRange.GetComponent<Renderer>();

            if (mat.material.color.a > 0.5f)
            {
                mat.material.color = new Color(mat.material.color.r, mat.material.color.g, mat.material.color.b, 0.5f);
            }

        }
    }
    void Transparent(GameObject obj, bool invert)
    {

        Renderer mat = obj.GetComponent<Renderer>();
        if(invert)
        {
            if (mat.material.color.a == 0.5f)
            {
                mat.material.color = new Color(mat.material.color.r, mat.material.color.g, mat.material.color.b, 1f);
            }
        }
        else
        {
            if (mat.material.color.a > 0.5f)
            {
                mat.material.color = new Color(mat.material.color.r, mat.material.color.g, mat.material.color.b, 0.5f);
            }
        }
        


    }

    void ToggleMesh()
    {
        foreach (GameObject gameobjectInRange in m_ObjectsInRange)
        {
            MeshRenderer mesh = gameobjectInRange.GetComponent<MeshRenderer>();

            if (mesh.enabled)
            {
                mesh.enabled = false;
            }

        }
    }

    void ToggleMesh(GameObject obj, bool invert)
    {

        MeshRenderer mesh = obj.GetComponent<MeshRenderer>();
        if(invert)
        {
            if (!mesh.enabled)
            {
                mesh.enabled = true;
            }
        }
        else
        {
            if (mesh.enabled)
            {
                mesh.enabled = false;
            }
        }
        

    }


    //Gizmo

    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        //Gizmos.DrawFrustum(this.gameObject.transform.position, m_fAngle,m_iDistance,0, 16/9);
        Gizmos.DrawWireSphere(transform.position, m_iDistance);
    }

}
