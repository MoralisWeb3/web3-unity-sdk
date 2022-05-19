using UnityEngine;
using UnityEngine.UI;

namespace MoralisUnity.Examples.Sdk.Shared
{
    /// <summary>
    /// Helper methods for this class
    /// </summary>
    public static class ScrollRectExtensions
    {
        //  Fields   ---------------------------------------------
        private static Vector3[] _Corners = new Vector3[4];

        //  General Methods  --------------------------------------
        public static void ScrollToTop(this ScrollRect scrollRect)
        {
            scrollRect.normalizedPosition = new Vector2(0, 1);
        }
        
        public static void ScrollToBottom(this ScrollRect scrollRect)
        {
            scrollRect.normalizedPosition = new Vector2(0, 0);
        }
        
        public static void ScrollToCenter(this ScrollRect scrollRect, RectTransform target)
        {
            // The scroll rect's view's space is used to calculate scroll position
            var view = scrollRect.viewport != null ? scrollRect.viewport : scrollRect.GetComponent<RectTransform>();

            // Calcualte the scroll offset in the view's space
            var viewRect = view.rect;
            var elementBounds = target.TransformBoundsTo(view);
            var offset = viewRect.center.y - elementBounds.center.y;

            // Normalize and apply the calculated offset
            var scrollPos = scrollRect.verticalNormalizedPosition - scrollRect.NormalizeScrollDistance(1, offset);
            scrollRect.verticalNormalizedPosition = Mathf.Clamp(scrollPos, 0f, 1f);
        }



        //  Advanced Methods  -------------------------------------
        private static Bounds TransformBoundsTo(this RectTransform source, Transform target)
        {
            // Based on code in ScrollRect's internal GetBounds and InternalGetBounds methods
            var bounds = new Bounds();
            if (source != null) {
                source.GetWorldCorners(_Corners);

                var vMin = new Vector3(float.MaxValue, float.MaxValue, float.MaxValue);
                var vMax = new Vector3(float.MinValue, float.MinValue, float.MinValue);

                var matrix = target.worldToLocalMatrix;
                for (int j = 0; j < 4; j++) {
                    Vector3 v = matrix.MultiplyPoint3x4(_Corners[j]);
                    vMin = Vector3.Min(v, vMin);
                    vMax = Vector3.Max(v, vMax);
                }

                bounds = new Bounds(vMin, Vector3.zero);
                bounds.Encapsulate(vMax);
            }
            return bounds;
        }
        
        public static float NormalizeScrollDistance(this ScrollRect scrollRect, int axis, float distance)
        {
            // Based on code in ScrollRect's internal SetNormalizedPosition method
            var viewport = scrollRect.viewport;
            var viewRect = viewport != null ? viewport : scrollRect.GetComponent<RectTransform>();
            var viewBounds = new Bounds(viewRect.rect.center, viewRect.rect.size);

            var content = scrollRect.content;
            var contentBounds = content != null ? content.TransformBoundsTo(viewRect) : new Bounds();

            var hiddenLength = contentBounds.size[axis] - viewBounds.size[axis];
            return distance / hiddenLength;
        }
    }
}