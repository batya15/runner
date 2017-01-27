using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class DrawRectangle : MonoBehaviour
{
    LineRenderer line;
    [SerializeField]
    Transform charcter;
    [SerializeField]
    Transform point;
    [SerializeField]
    GameObject red;

    IEnumerator Start()
    {
        line = transform.GetComponent<LineRenderer>();
        line.SetVertexCount(0);
        line.SetWidth(0.1f, 0.1f);
        line.material = new Material(Shader.Find("Particles/Additive"));
        line.SetColors(Color.green, Color.green);
        line.useWorldSpace = true;

        int x = 0;
        int y = 0;

        List<string> list = new List<string>() { "0-0" };

        for (int i = 0; i < 100; i++) {
            line.SetVertexCount(i + 1);
            line.SetPosition(i, new Vector3(x , y, 0));

            int oldX = x;
            int oldY = y;
            int t = 0;
            r:
            x = oldX;
            y = oldY;
            t++;
            int n = Random.Range(0, 4);
            //Debug.Log(n);
            switch (n) {
                case 0: 
                    y += 1;
                    break;
                case 1:
                    x += 1;
                    break;
                case 2:
                    y -= 1;
                    break;
                default:
                    x -= 1;
                    break;
            }

            if (list.Where(s => s.Equals(x + "-" + y)).Count() > 0)
            {
                x = oldX;
                y = oldY;
                if (t < 500)
                {
                    goto r;
                }
                else
                {
                    Debug.Log("Break" +t);
                    break;
                }
                //break;
            }


            point.position = new Vector3(x, y, 0);
            // yield return new WaitForSeconds(1);
            // НУЖНО ПРОВЕРЯТЬ 8 ТОЧЕК + АЛГОРИТМ ПОИСКА В ШИРИНУ

            for (int e = -1; e <= 1; e++) {
                for (int r = -1; r <= 1; r++) {
                    int nX = x + e;
                    int nY = y + r;
                    /*GameObject go = Instantiate(red);
                    go.name = nX + "-" + nY;
                    go.transform.position = new Vector3(nX, nY);
                    yield return new WaitForSeconds(1);*/
                    if ((e == 0 && r == 0) || (nX == oldX && nY == oldY) 
                        || ((nX == oldX && oldX != x) || (nY == oldY && oldY != y))) {
                       // go.GetComponent<SpriteRenderer>().color = Color.yellow;
                    } else {
                        if (list.Where(s => s.Equals(nX + "-" + nY)).Count() > 0) {
                            if (hasPath(nX, nY, list))
                            {
                                // int index = list.FindIndex(s => s.Equals((x + 1) + "-" + y));
                                if (t < 500)
                                { goto r; }
                            }
                        }
                    }

                }
            }
           

            list.Add(x + "-" + y);
            yield return new WaitForFixedUpdate();
        }
        Debug.Log("Finish");
    }

    bool hasPath(int xU, int yU, List<string> list) {
        List<string> used = new List<string>();
        Queue<int[]> q = new Queue<int[]>();
        q.Enqueue(new int[] { xU, yU });
        int step = 0;
        List<GameObject> g = new List<GameObject>();
        while (step < list.Count() + 1 / 2 && q.Count() > 0) {
            step++;
            int x = q.Peek()[0];
            int y = q.Peek()[1];
            q.Dequeue();

            GameObject go = Instantiate(red);
            go.name = x + "-" + y;
            go.transform.position = new Vector3(x, y);
            g.Add(go);

            used.Add(x + "-" + y);
            for (int e = -1; e <= 1; e++) {
                for (int r = -1; r <= 1; r++)
                {
                    int nX = x + e;
                    int nY = y + r;
                    if (e == 0 && r == 0)
                    {
                        // go.GetComponent<SpriteRenderer>().color = Color.yellow;
                    } else {
                        if (list.Where(s => s.Equals(nX + "-" + nY)).Count() == 0
                        && used.Where(s => s.Equals(nX + "-" + nY)).Count() == 0)
                        {
                            q.Enqueue(new int[] { nX, nY });
                        }
                    }

                }
            }

        }
        if (!(step < list.Count() + 1 / 2)) {
            foreach (GameObject gg in g) {
                Destroy(gg);
            }
        }

        return step < list.Count() + 1 / 2;
    }


}