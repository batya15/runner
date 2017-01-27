using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GridState : MonoBehaviour {

    public class Point {
        public int x;
        public int y;
        public int index;
    }

    Dictionary<string, Point> points = new Dictionary<string, Point>();

    string template(int x, int y) {
        return x + "-" + y;
    }

    public Point GetPoint(int x, int y) {
        string key = template(x, y);
        return points.ContainsKey(key)? points[key]: null;
    }

    public int Lenght {
        get { return points.Count; }
    }

    public Point AddPoint(int x, int y) {
        string key = template(x, y);
        if (points.ContainsKey(key)) {
            return null;
        } else {
            points.Add(key, new Point() { x = x, y = y, index = points.Count });
            return GetPoint(x, y);
        }
    }

    public Point Last() {
        return points.Last().Value;
    }
}
