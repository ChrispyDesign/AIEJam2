using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PulseWithBeat : MonoBehaviour
{
    public enum changeStyle { Raw, Clamped, None}
    public changeStyle sizeChangeStyle;
    public changeStyle colourChangeStyle;

    public enum colourStyle { albedo, emission, both}
    public colourStyle colourChangeType;

    public float sizeMin;
    public float sizeMax;

    public Color One;
    public Color Two;

    private bool beat1;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        SizeChange();
        ColourChange();
    }

    void SizeChange()
    {
        if (sizeChangeStyle == changeStyle.Raw)
        {
            float sizeDif = sizeMax - sizeMin;
            float size = sizeMin + (sizeDif * AudioManager.GetInstance().GetOpportunityScalarRaw());
            transform.localScale = Vector3.one * size;
        }
        if (sizeChangeStyle == changeStyle.Clamped)
        {
            float sizeDif = sizeMax - sizeMin;
            float size = sizeMin + (sizeDif * AudioManager.GetInstance().GetOpportunityScalarClamped());
            transform.localScale = Vector3.one * size;
        }
    }
    void ColourChange()
    {
        if (colourChangeStyle == changeStyle.Raw)
        {
            if (colourChangeType != colourStyle.emission)
                GetComponent<Renderer>().material.color = Color.Lerp(One, Two, AudioManager.GetInstance().GetOpportunityScalarRaw());
            if (colourChangeType != colourStyle.albedo)
                GetComponent<Renderer>().material.SetColor("_EmissionColor", Color.Lerp(One, Two, AudioManager.GetInstance().GetOpportunityScalarRaw()));
        }
        if (colourChangeStyle == changeStyle.Clamped)
        {
            if (colourChangeType != colourStyle.emission)
                GetComponent<Renderer>().material.color = Color.Lerp(One, Two, AudioManager.GetInstance().GetOpportunityScalarClamped());
            if (colourChangeType != colourStyle.albedo)
                GetComponent<Renderer>().material.SetColor("_EmissionColor", Color.Lerp(One, Two, AudioManager.GetInstance().GetOpportunityScalarClamped()));
        }
    }
}
