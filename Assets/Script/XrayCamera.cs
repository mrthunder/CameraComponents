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


    void FixedUpdate()
    {
        GetGameObjects();
        Xray();
    }

    /// <summary>
    /// Check if the object in the parameter, is inside the <see cref="m_fAngle"/>
    /// </summary>
    /// <param name="obj">Object for check</param>
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
        


    }

    /// <summary>
    /// Get all objects around the camera and <see cref="ScanView(GameObject)"/>. Also, if there is any object inside the <seealso cref="m_ObjectsInRange"/>, this object will be remove
    /// and return to a normal state.
    /// </summary>
    void GetGameObjects()
    {
        var hits = Physics.SphereCastAll(this.transform.position, m_iDistance, Vector3.forward,m_iDistance);

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

    /// <summary>
    /// If the <see cref="m_bTransparent"/> is true,the Xray will use the <seealso cref="Transparent"/>, otherwise <seealso cref="ToggleMesh"/>
    /// </summary>
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
    /// <summary>
    /// If the <see cref="m_bTransparent"/> is true,the Xray will use the <seealso cref="Transparent(GameObject, bool)"/>, otherwise <seealso cref="ToggleMesh(GameObject, bool)"/>
    /// </summary>
    /// <param name="obj">A single object</param>
    /// <param name="invert">Invert the action</param>
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
    /// <summary>
    /// Make the material alpha color 0.5f.
    /// </summary>
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
    /// <summary>
    /// Make the material alpha color 0.5f.
    /// </summary>
    /// <param name="obj">A single object</param>
    /// <param name="invert">invert the action</param>
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

    /// <summary>
    /// Toggle the mesh render.
    /// </summary>
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
    /// <summary>
    /// Toggle the mesh render.
    /// </summary>
    /// <param name="obj">A single object</param>
    /// <param name="invert">invert the action</param>
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

        Vector3 PointA = ((transform.forward * m_iDistance));
        PointA = Quaternion.Euler(0, m_fAngle, 0) * PointA;
        PointA += transform.position;
        Gizmos.DrawLine(transform.position, PointA);

        Vector3 PointB = (transform.forward * m_iDistance);
        PointB = Quaternion.Euler(0, -m_fAngle, 0) * PointB;
        PointB += transform.position;
        Gizmos.DrawLine(transform.position, PointB);

    }

}
