using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CollectedItemUI : MonoBehaviour
{
    public float m_textDisplayTime;
    public float m_panelAlpha;
    public float m_fadeTime;
    
    private TextMeshProUGUI m_text;
    private Image m_panelImage;

    // Start is called before the first frame update
    void Start()
    {
        m_text = GetComponentInChildren<TextMeshProUGUI>();
        m_panelImage = GetComponent<Image>();
    }

    public void SetText(string l_text)
    {
        StartCoroutine(InitializePanel(l_text));
    }

    private IEnumerator InitializePanel(string l_text)
    {
        m_text.text = l_text;

        // fade in
        float time = 0;
        Color panelColor = m_panelImage.color;
        while (time < m_fadeTime)
        {
            panelColor.a = Mathf.Lerp(0, m_panelAlpha, time / m_fadeTime);
            m_panelImage.color = panelColor;
            time += Time.deltaTime;
            yield return null;
        }

        yield return new WaitForSeconds(m_textDisplayTime);

        // fade out
        time = 0;
        while (time < m_fadeTime)
        {
            panelColor.a = Mathf.Lerp(m_panelAlpha, 0, time / m_fadeTime);
            m_panelImage.color = panelColor;
            time += Time.deltaTime;
            yield return null;
        }

        m_text.text = "";
    }
}
