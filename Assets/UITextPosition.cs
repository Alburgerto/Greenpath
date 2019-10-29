using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UITextPosition : MonoBehaviour
{
    public Transform m_object; // Object on top of which the interaction UI will be drawn
    public Vector3 m_offset = new Vector3(0, 0.5f, 0);

    private void Update()
    {
        Vector3 position = Camera.main.WorldToScreenPoint(m_object.position + m_offset);

        if (position != transform.position)
        {
            transform.position = Camera.main.WorldToScreenPoint(m_object.position + m_offset);
        }
    }

}
