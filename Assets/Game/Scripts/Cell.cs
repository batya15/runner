using UnityEngine;
using System.Collections;

namespace Runner {
    public class Cell : MonoBehaviour {

        Grid grid;
        void Awake() {
            grid = GetComponentInParent<Grid>();
        }
        
        void Update() {
            float y = transform.localPosition.y - 0.02f;
            if (y < -9.5f) {
                y = 11.5f;
            }
            transform.localPosition = new Vector3(transform.localPosition.x, y);
        }
    }
}
