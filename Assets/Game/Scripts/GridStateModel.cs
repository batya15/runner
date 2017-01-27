using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridStateModel : IEnumerable<CellModel> {

    const int GRID_SIZE = 7;

    private CellModel[,] state = new CellModel[GRID_SIZE, GRID_SIZE];

    public GridStateModel() {
        
    }
    
    public IEnumerator<CellModel> GetEnumerator()
    {
        throw new System.NotImplementedException();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return this.GetEnumerator();
    }

    public CellModel this[int i, int j]
    {
        get {
            if (state == null || i < 0 || i >= GRID_SIZE || j < 0 || j >= GRID_SIZE) {
                return null;
            }
            return state[i, j];
        }
        set { state[i, j] = value; }
    }

}
