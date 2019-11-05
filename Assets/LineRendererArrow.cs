using UnityEngine;
 [RequireComponent(typeof(LineRenderer))]
 public class LineRendererArrow : MonoBehaviour
 {
     [Tooltip("The percent of the line that is consumed by the arrowhead")]
     [Range(0, 1)]
     public float PercentHead = 0.4f;
     public Vector3 ArrowOrigin;
     public Vector3 ArrowTarget;
     public LineRenderer cachedLineRenderer;
    
     void Update()
     {
        float AdaptiveSize = (float)(PercentHead / Vector3.Distance(ArrowOrigin, ArrowTarget));

        if (cachedLineRenderer == null)
            cachedLineRenderer = this.GetComponent<LineRenderer>();
     
        cachedLineRenderer.SetPositions(new Vector3[] {
                ArrowOrigin
                , Vector3.Lerp(ArrowOrigin, ArrowTarget, 0.999f - AdaptiveSize)
                , Vector3.Lerp(ArrowOrigin, ArrowTarget, 1 - AdaptiveSize)
                , ArrowTarget });
     }
 }