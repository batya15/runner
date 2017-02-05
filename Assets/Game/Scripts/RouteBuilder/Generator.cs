using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using RouteBuilder.RouteModel;

namespace RouteBuilder {
    public class Generator {
        Route route;

        List<int[]> direction = new List<int[]> {
            new int[2] { 0, 1 },
            new int[2] { 1, 0 },
            new int[2] { 0, -1 },
            new int[2] { -1, 0 }
        };

        int lastIndex = 0;

        public Generator() {
            route = new Route();
            route.AddPoint(0, 0);
            route.AddPoint(0, 1);
        }

        public Point GetNext() {
            Point point = route.Last();
            
            List<int[]> possible = GetPossibleDirection(point);
            
            int indexDirection = GenerateIndex(lastIndex, possible);

            while (possible.Count > 1 && !DirectionTrue(point, possible[indexDirection])) {
                possible.RemoveAt(indexDirection);
                indexDirection = GenerateIndex(lastIndex, possible);
            }
            lastIndex = indexDirection > 1 ? lastIndex + indexDirection : lastIndex - indexDirection;
                                                                                                             
            return route.AddPoint(point.x + possible[indexDirection][0], point.y + possible[indexDirection][1]);
        }

        int GenerateIndex(int lastIndex, List<int[]> possible) {
            int result = 0;

            int ran = UserPrefs.key[((route.Lenght - lastIndex) % UserPrefs.key.Length)];
            if (ran % Mathf.Max(1, Mathf.Abs(route.Last().x + route.Last().y)) > ran % Mathf.Max(1, Mathf.Abs(route.Last().x - route.Last().y))) {
                ran += ran % UserPrefs.key[(ran + (route.Last().x % route.Last().y)) % UserPrefs.key.Length];
            } else {
                ran += ran % Mathf.Max(1, Mathf.Abs(UserPrefs.key[Mathf.Abs((ran - (route.Last().y * route.Last().x)) % UserPrefs.key.Length)]));
            }
            ran = Mathf.Abs(ran * route.Lenght % (Mathf.Max(1, Mathf.Max(route.Last().x, route.Last().y))));

            result = ran % possible.Count;
            return result;
        }

        bool DirectionTrue(Point point, int[] dir) {
            Point last = route.Last(1);
            bool needBuildWaves = false;
 
            Point indexPoint = null;
            int indexCirclePoint = route.Lenght + 1;
            for (int x = -1; x <= 1; x++) {
                for (int y = -1; y <= 1; y++) {
                    int nX = point.x + x;
                    int nY = point.y + y;

                    if ((x == 0 && y == 0) || (nX == last.x && nY == last.y)
                            || ((nX == last.x && last.x != point.x) || (nY == last.y && last.y != point.y))) {
                        //gos.Last().GetComponent<SpriteRenderer>().color = new Color(1, 1, 0, 0.5f);
                    } else {
                        //gos.Last().GetComponent<SpriteRenderer>().color = Color.blue;

                        if (route.GetPoint(nX, nY) != null) {
                            needBuildWaves = true;
                            int index = route.GetPoint(nX, nY).index;
                            indexCirclePoint = Mathf.Min(indexCirclePoint, index);
                            if (indexCirclePoint == index) {
                                indexPoint = route.GetPoint(nX, nY);
                            }
                        }
                    }
                }
            }


            int lengthSeach = (route.Lenght - indexCirclePoint);

            if (!needBuildWaves || (route.Lenght - indexCirclePoint) < 6) {
                return true;
            } else {
                List<int[]> possible = GetPossibleDirection(new Point(point.x + dir[0], point.y + dir[1]));

                bool findPath = false;
                foreach (int[] pos in possible) {
                    List<Point> currentPath = new List<Point>();

                    Point ignore = new Point(point.x + dir[0], point.y + dir[1]);


                    currentPath.Add(new Point(point.x + dir[0] + pos[0], point.y + dir[1] + pos[1]));

                    Point lastlastPoint = ignore;
                    Point lastPoint = currentPath.First();
                    Point currentPoint = null;



                    while (currentPath.Count <= lengthSeach && !findPath) {
                        int[] d = GetDirectuonByPointsLastCurrent(lastlastPoint, lastPoint, new int[2] { ignore.x, ignore.y });

                        if (d == null) {
                            break;
                        }

                        currentPoint = new Point(lastPoint.x + d[0], lastPoint.y + d[1]);

                        if (currentPoint.x < route.minX || currentPoint.x > route.maxX || currentPoint.y < route.minY || currentPoint.y > route.maxY) {
                            findPath = true;
                            break;
                        }

                        if (currentPath.First().x == currentPoint.x && currentPath.First().y == currentPoint.y) {
                            int[] w = GetDirectuonByPointsLastCurrent(lastPoint, currentPoint, new int[2] { ignore.x, ignore.y });
                            if (w == null || currentPath.Where(p => p.x == currentPoint.x + w[0] && p.y == currentPoint.y + w[1]).Count() != 0) {
                                break;
                            }
                        }

                        if (currentPath.Where(p => p.x == currentPoint.x && p.y == currentPoint.y).Count() == 0) {
                            currentPath.Add(currentPoint);
                        }

                        findPath = currentPath.Count >= lengthSeach;
                        lastlastPoint = lastPoint;
                        lastPoint = currentPoint;
                    }
                    if (findPath) {
                        break;
                    }
                };

                return findPath;     

            }

        }

        int[] GetDirectuonByPointsLastCurrent(Point lastPoint, Point currentPoint, int[] ignorePos) {
            int[] result = null;

            // на право от ячейки
            if (lastPoint.x == currentPoint.x && lastPoint.y > currentPoint.y && route.GetPoint(currentPoint.x - 1, currentPoint.y) == null && (currentPoint.x - 1 != ignorePos[0] || currentPoint.y != ignorePos[1])) {
                result = new int[2] { -1, 0 };
            } else if (lastPoint.y == currentPoint.y && lastPoint.x > currentPoint.x && route.GetPoint(currentPoint.x, currentPoint.y + 1) == null && (currentPoint.x != ignorePos[0] || currentPoint.y + 1 != ignorePos[1])) {
                result = new int[2] { 0, 1 };
            } else if (lastPoint.x == currentPoint.x && lastPoint.y < currentPoint.y && route.GetPoint(currentPoint.x + 1, currentPoint.y) == null && (currentPoint.x + 1 != ignorePos[0] || currentPoint.y != ignorePos[1])) {
                result = new int[2] { 1, 0 };
            } else if (lastPoint.y == currentPoint.y && lastPoint.x < currentPoint.x && route.GetPoint(currentPoint.x, currentPoint.y - 1) == null && (currentPoint.x != ignorePos[0] || currentPoint.y - 1 != ignorePos[1])) {
                result = new int[2] { 0, -1 };
            }

            // прямо от ячейки
            if (result == null) {
                if (lastPoint.x == currentPoint.x && lastPoint.y > currentPoint.y && route.GetPoint(currentPoint.x, currentPoint.y - 1) == null && (currentPoint.x != ignorePos[0] || currentPoint.y - 1 != ignorePos[1])) {
                    result = new int[2] { 0, -1 };
                } else if (lastPoint.y == currentPoint.y && lastPoint.x > currentPoint.x && route.GetPoint(currentPoint.x - 1, currentPoint.y) == null && (currentPoint.x - 1 != ignorePos[0] || currentPoint.y != ignorePos[1])) {
                    result = new int[2] { -1, 0 };
                } else if (lastPoint.x == currentPoint.x && lastPoint.y < currentPoint.y && route.GetPoint(currentPoint.x, currentPoint.y + 1) == null && (currentPoint.x != ignorePos[0] || currentPoint.y + 1 != ignorePos[1])) {
                    result = new int[2] { 0, 1 };
                } else if (lastPoint.y == currentPoint.y && lastPoint.x < currentPoint.x && route.GetPoint(currentPoint.x + 1, currentPoint.y) == null && (currentPoint.x + 1 != ignorePos[0] || currentPoint.y != ignorePos[1])) {
                    result = new int[2] { 1, 0 };
                }
            }

            // на лево
            if (result == null) {
                if (lastPoint.x == currentPoint.x && lastPoint.y > currentPoint.y && route.GetPoint(currentPoint.x + 1, currentPoint.y) == null && (currentPoint.x + 1 != ignorePos[0] || currentPoint.y != ignorePos[1])) {
                    result = new int[2] { 1, 0 };
                } else if (lastPoint.y == currentPoint.y && lastPoint.x > currentPoint.x && route.GetPoint(currentPoint.x, currentPoint.y - 1) == null && (currentPoint.x != ignorePos[0] || currentPoint.y - 1 != ignorePos[1])) {
                    result = new int[2] { 0, -1 };
                } else if (lastPoint.x == currentPoint.x && lastPoint.y < currentPoint.y && route.GetPoint(currentPoint.x - 1, currentPoint.y) == null && (currentPoint.x - 1 != ignorePos[0] || currentPoint.y != ignorePos[1])) {
                    result = new int[2] { -1, 0 };
                } else if (lastPoint.y == currentPoint.y && lastPoint.x < currentPoint.x && route.GetPoint(currentPoint.x, currentPoint.y + 1) == null && (currentPoint.x != ignorePos[0] || currentPoint.y + 1 != ignorePos[1])) {
                    result = new int[2] { 0, 1 };
                }
            }

            //назад
            if (result == null && route.GetPoint(lastPoint.x, lastPoint.y) == null && (lastPoint.x != ignorePos[0] || lastPoint.y != ignorePos[1])) {
                result = new int[2] { lastPoint.x - currentPoint.x, lastPoint.y - currentPoint.y };
            }
            return result;
        }

        List<Point> GetAllNeighbors(Point point, List<List<Point>> waves, List<Point> newWave) {
            List<int[]> d = GetPossibleDirection(point);
            return d.Select(i => new Point(point.x + i[0], point.y + i[1]))
                .Where(p => !waves.Where(w => w.Where(r => r.x == p.x && r.y == p.y).Any()).Any())
                .Where(p => !newWave.Where(r => r.x == p.x && r.y == p.y).Any())
                .ToList();
        }

        List<int[]> GetPossibleDirection(Point point) {
            List<int[]> possible = new List<int[]>();
            var r = point.x;
            for (int i = 0; i < direction.Count; i++) {
                if (route.GetPoint(point.x + direction[i][0], point.y + direction[i][1]) == null) {
                    possible.Add(direction[i]);
                }
            }
                                                                                          
            return possible;
        }

    }
}

