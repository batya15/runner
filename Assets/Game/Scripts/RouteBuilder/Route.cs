using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace RouteBuilder {
    namespace RouteModel {
        public class Route {

            Dictionary<string, Point> points = new Dictionary<string, Point>();

            string template(int x, int y) {
                return x + "-" + y;
            }

            public Point GetPoint(int x, int y) {
                string key = template(x, y);
                return points.ContainsKey(key) ? points[key] : null;
            }

            public int Lenght {
                get { return points.Count; }
            }

            public int minX = 0, minY = 0, maxX = 0, maxY = 0;

            public Point AddPoint(int x, int y) {
                string key = template(x, y);
                if (points.ContainsKey(key)) {
                    return null;
                } else {
                    points.Add(key, new Point(x, y, points.Count));
                    minX = Mathf.Min(minX, x);
                    minY = Mathf.Min(minY, y);
                    maxX = Mathf.Max(maxX, x);
                    maxY = Mathf.Max(maxY, y);
                    return GetPoint(x, y);
                }
            }

            public Point Last() {
                return points.Last().Value;
            }

            public Point Last(int skip) {
                return points[points.Keys.Reverse().Skip(skip).FirstOrDefault()];
            }
        }
    }
    
}


