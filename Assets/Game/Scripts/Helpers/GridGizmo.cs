using UnityEngine;
using System.Collections;

namespace Helpers {
    public class GridGizmo : MonoBehaviour {

        public Color color = new Color(0, 1, 0);
        public float offsetDelta = 0;

        void OnDrawGizmos() {
            Gizmos.color = new Color(0, 1, 0, 0.2F);
            Vector3 size = new Vector3(3, 3, 1);
            size.Scale(this.transform.lossyScale);
            Gizmos.DrawCube(this.transform.position, size);


            Gizmos.color = color;

            Gizmos.DrawLine(
                transform.position + new Vector3(-1.5f, -1.5f, 0),
                transform.position + new Vector3(-1.5f, 1.5f, 0)
            );

            Gizmos.DrawLine(
                transform.position + new Vector3(-1.5f, -1.5f, 0),
                transform.position + new Vector3(1.5f, -1.5f, 0)
            );
        }
    }
}


