using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using cakeslice;

namespace cakeslice
{
    public class OutlineAnimation : MonoBehaviour
    {
        public OutlineEffect outline;
        bool pingPong = false;

        // Use this for initialization
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {
            Color c = outline.lineColor0;
            Color c2 = outline.lineColor2;

            if (pingPong)
            {
                c.a += Time.deltaTime;
                c2.a += Time.deltaTime;
                if (c.a >= 1)
                    pingPong = false;
                if (c2.a >= 1)
                    pingPong = false;
            }
            else
            {
                c.a -= Time.deltaTime;
                c2.a -= Time.deltaTime;
                if (c.a <= 0)
                    pingPong = true;
                if (c2.a <= 0)
                    pingPong = true;
            }

            c.a = Mathf.Clamp01(c.a);
            c2.a = Mathf.Clamp01(c2.a);
            outline.lineColor0 = c;
            outline.lineColor2 = c2;
            outline.UpdateMaterialsPublicProperties();
        }
    }
}