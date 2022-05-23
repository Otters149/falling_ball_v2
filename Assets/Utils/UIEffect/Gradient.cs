using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;

namespace utilpackages
{
    namespace uieffect
    {
        [AddComponentMenu("UI/Effects/Gradient")]
        public class Gradient : BaseMeshEffect
        {
            public Color32 topColor = Color.white;
            //public Color32 midColor1 = Color.blue;
            //public Color32 midColor2 = Color.grey;
            public Color32 bottomColor = Color.black;
            //public float percentage = 0.1f; 
            public override void ModifyMesh(VertexHelper helper)
            {
                if (!IsActive() || helper.currentVertCount == 0)
                    return;

                List<UIVertex> vertices = new List<UIVertex>();
                helper.GetUIVertexStream(vertices);

                float bottomY = vertices[0].position.y;
                float topY = vertices[0].position.y;

                for (int i = 1; i < vertices.Count; i++)
                {
                    float y = vertices[i].position.y;
                    if (y > topY)
                    {
                        topY = y;
                    }
                    else if (y < bottomY)
                    {
                        bottomY = y;
                    }
                }
                // Debug.Log("Top Y :" + topY);
                // Debug.Log("Bottom Y" + bottomY);

                float uiElementHeight = topY - bottomY;

                UIVertex v = new UIVertex();
                /* var temp = uiElementHeight * percentage;
                    var temp2 = uiElementHeight * (1-2*percentage);
                    var borderPositionY = topY - temp;
                    var borderPositionY2 = bottomY + temp;
                    Debug.Log("borderPosition Y :" + borderPositionY);
                     Debug.Log("borderPosition Y2" + borderPositionY2); */

                for (int i = 0; i < helper.currentVertCount; i++)
                {
                    helper.PopulateUIVertex(ref v, i);
                    /*if (v.position.y >= borderPositionY)
                       v.color = Color32.Lerp(midColor1, topColor, (v.position.y - borderPositionY) / temp);
                    else
                       if (v.position.y >= borderPositionY2)
                           v.color = Color32.Lerp(midColor2, midColor1, (v.position.y- borderPositionY2) / temp2);
                       else
                           v.color = Color32.Lerp(bottomColor, midColor2, (v.position.y - bottomY) / temp);
                    */
                    v.color = Color32.Lerp(bottomColor, topColor, (v.position.y - bottomY) / uiElementHeight);
                    helper.SetUIVertex(v, i);
                }
            }
        }
    }
}