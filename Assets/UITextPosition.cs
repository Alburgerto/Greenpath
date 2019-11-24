using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UITextPosition : MonoBehaviour
{
    public GameObject m_object; // Object on top of which the interaction UI will be drawn
    public Vector3 m_offset = new Vector3(0, 0.5f, 0); // Offset for position relative to m_object

    private void Update()
    {
        ActivateUI();
    }

    public void ActivateUI()
    {
        if (m_object == null) { return; }
        Vector3 position = Camera.main.WorldToScreenPoint(m_object.transform.position + m_offset);

        if (position != transform.position)
        {
            transform.position = Camera.main.WorldToScreenPoint(m_object.transform.position + m_offset);
        }
    }

}
