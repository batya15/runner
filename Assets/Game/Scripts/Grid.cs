using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace Runner {
    public class Grid : MonoBehaviour {

        const float SIZE_CELL = 3;
        const int SIZE_GRID = 7;
        int _screenWidth;

        [SerializeField]
        GameObject cell_empty;
        [SerializeField]
        GameObject cell_intersection;
        [SerializeField]
        GameObject cell_road;

        enum DIRECTION {
            UP, DOWN, LEFT, RIGHT
        }

        List<Cell> cells = new List<Cell>();

        void Awake() {
            for (int x = 0; x < SIZE_GRID; x++) {
                for (int y = 0; y < SIZE_GRID; y++) {
                    GameObject obj = Instantiate(x == 3? cell_road : cell_empty);
                    obj.transform.localPosition = new Vector2(x * SIZE_CELL - (SIZE_GRID / 2 * SIZE_CELL), y * SIZE_CELL - (SIZE_GRID / 2 * SIZE_CELL));
                    obj.transform.SetParent(transform, false);
                    cells.Add(obj.GetComponent<Cell>());
                }
            }
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
            
        IEnumerator Start() {
            while (true) {
                Vector3 delta = new Vector3(0, -Time.deltaTime, 0);
                foreach (Cell c in cells) {
                    c.transform.position += delta;
                    if (c.transform.position.y < -9.5f) {
                        c.transform.position = new Vector3(c.transform.position.x, 11.0f, 0); ;
                    }
                }
                yield return null;
            }
            
        }


        void Update() {
            if (_screenWidth != Screen.width) {
                setCameraSize();
            }
        }
        
    }
}

