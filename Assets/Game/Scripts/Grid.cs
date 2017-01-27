using UnityEngine;
using System.Collections;


namespace Runner {
    public class Grid : MonoBehaviour {

        const float SIZE_CELL = 3;
        const int SIZE_GRID = 7;
        int _screenWidth;

        [SerializeField]
        GameObject cell;

        void Awake() {
            /*for (int x = 0; x < SIZE_GRID; x++) {
                for (int y = 0; y < SIZE_GRID; y++) {
                    GameObject obj = Instantiate(cell);
                    obj.transform.localPosition = new Vector2(x * SIZE_CELL - (SIZE_GRID / 2 * SIZE_CELL), y * SIZE_CELL - (SIZE_GRID / 2 * SIZE_CELL));
                    obj.transform.SetParent(transform, false);
                }
            }*/
            setCameraSize();
        }
        
        void setCameraSize() {
            _screenWidth = Screen.width;
            if (Screen.width > Screen.height) {
                Camera.main.orthographicSize = (SIZE_GRID - 2) * SIZE_CELL / 2  * (float)Screen.height / (float)Screen.width;
            } else {
                Camera.main.orthographicSize = (SIZE_GRID - 2) * SIZE_CELL / 2;
            }
        }

        void Update() {
            if (_screenWidth != Screen.width) {
                setCameraSize();
            }
        }
        
    }
}

