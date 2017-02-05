using RouteBuilder;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Helpers {
    public class TestRouteBuilder : MonoBehaviour {

        [SerializeField]
        GameObject sprite;

        Generator generator;

        void Awake() {
            generator = new Generator();
        }

        IEnumerator Start() {
            for (int i = 0; i < 1000; i++) {
                CreatePoint(generator.GetNext(), Color.green);
                yield return null; 
            }    
        }


        GameObject CreatePoint(Point point, Color c, int order = 1) {
            GameObject go = Instantiate(sprite);
            go.GetComponent<SpriteRenderer>().color = c;
            go.GetComponent<SpriteRenderer>().sortingOrder = order;
            go.name = point.x + "-" + point.y;
            go.transform.SetParent(transform);
            go.transform.position = new Vector2(point.x, point.y);
            return go;
        }
    }
}
