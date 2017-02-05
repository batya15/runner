using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

static public class UserPrefs {

    const string KEY = "key";

    static public int[] key {
        get {
            string k = PlayerPrefs.GetString(KEY, "");
            int[] result = null;
            if (k.Equals("")) {
                int count = Random.Range(12, 20);
                result = new int[count];
                for (int i = 0; i < count; i++) {
                    result[i] = Random.Range(0, 30);
                }     
                PlayerPrefs.SetString(KEY, string.Join(",", result.Select(x => x.ToString()).ToArray()));
            } else {
                result = k.Split(',').Select(s => int.Parse(s)).ToArray(); 
            }
            return result;
        }
    }
     
}
