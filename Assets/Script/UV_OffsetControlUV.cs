using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UV_OffsetControlUV : MonoBehaviour
{
    bool b;
    Material m_Material;
    // Use this for initialization
    void Start()
    {
        m_Material = GetComponent<Renderer>().material;
    }

    // Update is called once per frame
    void Update()
    {
        float uv_x = m_Material.GetVector("_Offset").x;
        float uv_y = m_Material.GetVector("_Offset").y;
        if (uv_x > 0.2f)
            b = true;
        if (uv_x < 0)
            b = false;
        if(b)
        {
            uv_x -= Time.deltaTime * 0.1f;
            uv_y -= Time.deltaTime * 0.1f;
        }
        else
        {
            uv_x += Time.deltaTime * 0.1f;
            uv_y += Time.deltaTime * 0.1f;
        }
         

       
        m_Material.SetVector("_Offset", new Vector4(uv_x, uv_y, 0.0f, 0.0f));
    }
}


