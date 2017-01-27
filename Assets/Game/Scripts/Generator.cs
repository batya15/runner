using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Generator : MonoBehaviour {

    [SerializeField]
    GameObject sprite;
    GridState gridState;

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
        GameObject g = CreatePoint(prevPoint, Color.blue);
                                        
       /* CreatePoint(gridState.AddPoint(0, 0), Color.green);
        CreatePoint(gridState.AddPoint(1, 0), Color.green);
        CreatePoint(gridState.AddPoint(2, 0), Color.green);
        CreatePoint(gridState.AddPoint(3, 0), Color.green);
        CreatePoint(gridState.AddPoint(3, 1), Color.green);    
        CreatePoint(gridState.AddPoint(3, 2), Color.green);
        CreatePoint(gridState.AddPoint(2, 2), Color.green);  */

        int x = gridState.Last().x;
        int y = gridState.Last().y + 1;

        //yield return new WaitForSeconds(30);

        for (int i = 0; i < 100; i++) {
            GridState.Point point = gridState.AddPoint(x, y);
            yield return new WaitForSeconds(0.2f);
            GameObject go = CreatePoint(point, Color.green);

            List<int[]> possible = GetPossibleDirection(point);
            int indexDirection = Random.Range(0, possible.Count);
            if (possible.Count == 0) {
                go.GetComponent<SpriteRenderer>().color = Color.white;
            }
            
            while (!DirectionTrue(point, possible[indexDirection])) {    
                yield return new WaitForSeconds(3.0f);
                possible.RemoveAt(indexDirection);
                if (possible.Count == 0) {
                    go.GetComponent<SpriteRenderer>().color = Color.white;
                }
                indexDirection = Random.Range(0, possible.Count);   
            }                                              

            Destroy(g);
            g = CreatePoint(new GridState.Point() { x = point.x + possible[indexDirection][0], y = point.y + possible[indexDirection][1] }, Color.red);
            g.GetComponent<SpriteRenderer>().sortingOrder = 50;
            x += possible[indexDirection][0];
            y += possible[indexDirection][1];
            Debug.Log(gridState.Lenght);
        }
        Debug.Log("Finish");
        yield return null;
    }
    List<GameObject> gos = new List<GameObject>();

    private bool DirectionTrue(GridState.Point point, int[] dir) {
        foreach (GameObject g in gos) {
             Destroy(g);
        }
        GridState.Point last = gridState.Last();
        bool needBuildWaves = false;
        Debug.Log("check:" + (point.x) + ":" + (point.y));
        Debug.Log("check:" + (point.x + dir[0])+":" + (point.y + dir[1]));
        int indexCirclePoint = gridState.Lenght + 1;
        for (int x = -1; x <= 1; x++) {
            for (int y = -1; y <= 1; y++) {
                int nX = point.x + dir[0] + x;
                int nY = point.y + dir[1] + y;
                gos.Add(CreatePoint(new GridState.Point() { x = nX, y = nY }, new Color(1,1,0,0.5f)));
                gos.Last().GetComponent<SpriteRenderer>().sortingOrder = 100;
                gos.Last().transform.localScale = Vector3.one / 2;
                if ((x == 0 && y == 0)
                    || (nX == last.x && nY == last.y)
                    || ((nX == last.x && last.x != point.x + dir[0]) || (nY == last.y && last.y != +dir[1]))) {
                   
                } else {
                    gos.Last().GetComponent<SpriteRenderer>().color = Color.blue;

                    if (gridState.GetPoint(nX, nY) != null) {
                        needBuildWaves = true;
                        int index = gridState.GetPoint(nX, nY).index;
                        indexCirclePoint = Mathf.Min(indexCirclePoint, index);
                    }
                }
            }
        }             

        if (!needBuildWaves) {
            return true;
        }
        List<List<GridState.Point>> waves = new List<List<GridState.Point>>();
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

           /* foreach (GridState.Point p in newWave) {
                gos.Add(CreatePoint(p, color));
            }*/
           
            waves.Add(newWave);
        }

        return waves.Count >= gridState.Lenght/2;
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

        return possible;
    }

    GameObject CreatePoint(GridState.Point point, Color c) {
        GameObject go = Instantiate(sprite);
        go.GetComponent<SpriteRenderer>().color = c;
        go.name = point.x + "-" + point.y;
        go.transform.SetParent(transform);
        go.transform.position = new Vector2(point.x, point.y);
        return go;
    }
}
