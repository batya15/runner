using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Generator : MonoBehaviour {

    [SerializeField]
    GameObject sprite;
    GridState gridState;

    enum DIRECTION { UP, DOWN, LEFT, RIGHT }

    List<int[]> direction = new List<int[]> { 
        new int[2] { 0, 1 },
        new int[2] { 1, 0 },      
        new int[2] { 0, -1 },   
        new int[2] { -1, 0 } 
    };

    void Awake() {
        gridState = GetComponent<GridState>();
    }

    IEnumerator Start() {
        GridState.Point prevPoint = gridState.AddPoint(0, -1);           
        CreatePoint(prevPoint, Color.green);
                                        
       /* CreatePoint(gridState.AddPoint(0, 0), Color.green);
        CreatePoint(gridState.AddPoint(1, 0), Color.green);
        CreatePoint(gridState.AddPoint(2, 0), Color.green);
        CreatePoint(gridState.AddPoint(3, 0), Color.green);
        CreatePoint(gridState.AddPoint(3, 1), Color.green);    
        CreatePoint(gridState.AddPoint(3, 2), Color.green);
        CreatePoint(gridState.AddPoint(2, 2), Color.green);  */

        int x = gridState.Last().x;
        int y = gridState.Last().y + 1;              

        for (int i = 0; i < 1000; i++) {
            GridState.Point point = gridState.AddPoint(x, y);
            CreatePoint(point, Color.green);
            yield return new WaitForEndOfFrame();

            List<int[]> possible = GetPossibleDirection(point);
            int indexDirection = Random.Range(0, possible.Count);

            bool result = false;
            yield return DirectionTrue(point, possible[indexDirection], b => result = b);

            while (possible.Count > 1 && !result) {    
                yield return new WaitForSeconds(3.0f);
                possible.RemoveAt(indexDirection);
                indexDirection = Random.Range(0, possible.Count);
                yield return DirectionTrue(point, possible[indexDirection], b => result = b);
            }
            
            x += possible[indexDirection][0];
            y += possible[indexDirection][1];

           // yield return new WaitForSeconds(0.2f);
            Debug.Log(gridState.Lenght);
        }
        Debug.Log("Finish");
    }

    List<GameObject> gos = new List<GameObject>();

    bool breakPoint = true;

    public void Run() {
        breakPoint = false;
    }

    IEnumerator DirectionTrue(GridState.Point point, int[] dir, System.Action<bool> cb) {
        foreach (GameObject g in gos) { Destroy(g);}
        gos.Clear();

        GridState.Point last = gridState.Last(1);
        bool needBuildWaves = false;

        //gos.Add(CreatePoint(new GridState.Point(){ x = point.x, y = point.y }, Color.red, 50));
        gos.Add(CreatePoint(new GridState.Point() { x = point.x + dir[0], y = point.y + dir[1] }, Color.cyan, 56));
       // yield return new WaitForEndOfFrame();
        GridState.Point indexPoint = null;
        int indexCirclePoint = gridState.Lenght + 1;
        for (int x = -1; x <= 1; x++) {
            for (int y = -1; y <= 1; y++) {
                int nX = point.x + x;
                int nY = point.y + y;
                gos.Add(CreatePoint(new GridState.Point() { x = nX, y = nY }, new Color(1,1,0,0.5f), 100));
                gos.Last().transform.localScale = Vector3.one / 2;
                yield return new WaitForEndOfFrame();

                if ((x == 0 && y == 0) || (nX == last.x && nY == last.y)
                        || ((nX == last.x && last.x != point.x) || (nY == last.y && last.y != point.y))) {
                    gos.Last().GetComponent<SpriteRenderer>().color = new Color(1, 1, 0, 0.5f);
                } else {
                    gos.Last().GetComponent<SpriteRenderer>().color = Color.blue;

                    if (gridState.GetPoint(nX, nY) != null) {
                        needBuildWaves = true;
                        int index = gridState.GetPoint(nX, nY).index;
                        indexCirclePoint = Mathf.Min(indexCirclePoint, index);
                        if (indexCirclePoint == index) {
                            indexPoint = gridState.GetPoint(nX, nY);
                        }
                    }
                }
                //yield return new WaitForEndOfFrame();
            }
        }
        breakPoint = true;
        yield return new WaitUntil(() => !breakPoint);
        // yield return new WaitForSeconds(2);               
        
        if (!needBuildWaves || (gridState.Lenght - indexCirclePoint) < 6) {
            cb(true);
        } else {
            cb(false);
            /*List<int[]> possible = GetPossibleDirection(new GridState.Point() { x = point.x + dir[0], y = point.y + dir[1]} );
            yield return new WaitForSeconds(2);
            bool findPath = false;
            foreach (int[] pos in possible) {
                List<GridState.Point> path = new List<GridState.Point>();
                path.Add(new GridState.Point() {x = point.x + dir[0]+ pos[0], y = point.y + dir[1] + pos[1]});
                gos.Add(CreatePoint(path.Last(), Color.yellow, 55));
                GridState.Point lastPoint = path.First();
                GridState.Point currentPoint = path.Last();
                while (path.Count < gridState.Lenght - indexCirclePoint) {
                    int [] d = null;
                    while (d == null) {
                        if (lastPoint.x == currentPoint.x && lastPoint.y > currentPoint.y && gridState.GetPoint(currentPoint.x - 1, currentPoint.y) == null) {
                            d = new int[2] { -1, 0 };
                        } else if (lastPoint.y == currentPoint.y && lastPoint.x > currentPoint.x && gridState.GetPoint(currentPoint.x, currentPoint.y + 1) == null) {
                            d = new int[2] { 0, 1 };
                        } else if (lastPoint.x == currentPoint.x && lastPoint.y < currentPoint.y && gridState.GetPoint(currentPoint.x + 1, currentPoint.y) == null) {
                            d = new int[2] { 1, 0 };
                        } else if (lastPoint.y == currentPoint.y && lastPoint.x < currentPoint.x && gridState.GetPoint(currentPoint.x, currentPoint.y - 1) == null) {
                            d = new int[2] { 0, -1 };
                        }
                    }
                    if (path.Where(p => p.x == currentPoint.x + d[0] && p.y == currentPoint.y + d[1]).Count() == 0) {
                        path.Add(new GridState.Point() { x = currentPoint.x + d[0], y = currentPoint.y + d[1]});
                        gos.Add(CreatePoint(path.Last(), Color.yellow, 55));
                        yield return new WaitForEndOfFrame();
                        yield return new WaitForSeconds(2);
                    }
                    findPath = path.Count > gridState.Lenght - indexCirclePoint;
                    if (path.Last().x == currentPoint.x && path.Last().y == currentPoint.y) {
                        break;
                    }
                    lastPoint = currentPoint;
                    currentPoint = new GridState.Point() { x = currentPoint.x + d[0], y = currentPoint.y + d[1] };
                }
                if (findPath) {
                    break;
                }
            };

            cb(findPath);*/


            //waves algoritm
            //gos.Add(CreatePoint(indexPoint, Color.yellow, 500));
            /*List<List<GridState.Point>> waves = new List<List<GridState.Point>>();
            waves.Add(new List<GridState.Point>() { new GridState.Point() { x = point.x + dir[0], y = point.y + dir[1] } });
            Debug.Log("Waves Allgoritm" + (((gridState.Lenght - indexCirclePoint) / 2)));
            while (waves.Count <= (gridState.Lenght - indexCirclePoint) / 2 && waves.Last().Count() > 0) {
                List<GridState.Point> newWave = new List<GridState.Point>();
                foreach (GridState.Point p in waves.Last()) {
                    List<GridState.Point> w = GetAllNeighbors(p, waves, newWave);
                    foreach (GridState.Point r in w) {
                        newWave.Add(r);
                    }
                }

                  
                Color color = Random.ColorHSV(0.7f, 1f, 1f, 1f, 0.5f, 1f);

                 foreach (GridState.Point p in newWave) {
                     gos.Add(CreatePoint(p, color, 150));
                 }
                yield return new WaitForEndOfFrame();   
                waves.Add(newWave);
            }

            return (waves.Count >= (gridState.Lenght - indexCirclePoint) / 2);  */  

        }    
        
    }

    private List<GridState.Point> GetAllNeighbors(GridState.Point point, List<List<GridState.Point>> waves, List<GridState.Point> newWave) {
        List<int[]> d = GetPossibleDirection(point);
        return d.Select(i => new GridState.Point() { x = point.x + i[0], y = point.y + i[1] })
            .Where(p => !waves.Where(w => w.Where(r => r.x == p.x && r.y == p.y).Any()).Any())
            .Where(p => !newWave.Where(r => r.x == p.x && r.y == p.y).Any())
            .ToList();
    }

    private List<int[]> GetPossibleDirection(GridState.Point point) {
        List<int[]> possible = new List<int[]>();
        for (int i = 0; i < direction.Count; i++) {
            if (gridState.GetPoint(point.x + direction[i][0], point.y + direction[i][1]) == null) {
                possible.Add(direction[i]);
            }
        }

        if (possible.Count == 0) { Debug.LogError("Fail possible direction!!!!");}
        return possible;
    }

    GameObject CreatePoint(GridState.Point point, Color c, int order = 1) {
        GameObject go = Instantiate(sprite);
        go.GetComponent<SpriteRenderer>().color = c;
        go.GetComponent<SpriteRenderer>().sortingOrder = order;
        go.name = point.x + "-" + point.y;
        go.transform.SetParent(transform);
        go.transform.position = new Vector2(point.x, point.y);
        return go;
    }
}
