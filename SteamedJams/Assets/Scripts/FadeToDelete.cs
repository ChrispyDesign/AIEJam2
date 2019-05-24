using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FadeToDelete : MonoBehaviour
{
    public float fadeWait;
    public float fadeRate;

    private float fadeTimer;

    private Color matColor;
    // Start is called before the first frame update
    void Start()
    {
        if (GetComponent<Renderer>().material.color != null)
            matColor = GetComponent<Renderer>().material.color;
    }

    // Update is called once per frame
    void Update()
    {
        if (fadeTimer >= fadeWait)
            Fade();
        else
            fadeTimer += Time.deltaTime;
    }

    void Fade()
    {
        matColor = new Color(matColor.r, matColor.g, matColor.b, matColor.a - (Time.deltaTime * fadeRate));
        if (GetComponent<Renderer>().material.color != null)
            GetComponent<Renderer>().material.color = matColor;
        if (matColor.a < 0.01)
            Destroy(this.gameObject);
    }
}
