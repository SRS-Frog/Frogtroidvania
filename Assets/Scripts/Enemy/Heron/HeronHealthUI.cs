using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeronHealthUI : MonoBehaviour
{
    [SerializeField] private Health h;

    [SerializeField] private float maxWidth;
    private float height;
    [SerializeField] private RectTransform fluid;
    [SerializeField] private RectTransform flash;
    
    // Start is called before the first frame update
    void Start()
    {
        height = fluid.sizeDelta.y;
    }

    // Update is called once per frame
    void Update()
    {
        fluid.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, maxWidth * h.health/400);
        if (flash.rect.width >= fluid.rect.width)
        {
            flash.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, flash.rect.width - Time.deltaTime * 40f);
        }
    }
}
