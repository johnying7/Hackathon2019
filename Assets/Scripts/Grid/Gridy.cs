using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gridy : MonoBehaviour
{
#region Private Variables & Properties
    [SerializeField]
    private float size = 1f;
    //[SerializeField]
    //private float grid_limit = 20f;
#endregion

#region  Public Variables & Properties
    public float Size { get { return size; } }
#endregion

    public Vector3 GetGridPoint(Vector3 position) {
        position -= transform.position;

        int x_count = Mathf.RoundToInt(position.x / size);
        int y_count = Mathf.RoundToInt(position.y / size);
        int z_count = Mathf.RoundToInt(position.z / size);

        Vector3 result = new Vector3(
            (float)x_count * size,
            (float)y_count * size,
            (float)z_count * size);

        result += transform.position;
        return result;
    }

    private void OnDrawGizmos() {
        Gizmos.color = Color.green;
        // for (float x = -grid_limit; x < grid_limit; x += size) {
        //     for (float z = -grid_limit; z < grid_limit; z += size) {
        //         var point = GetGridPoint(new Vector3(x, 0f, z));
        //         Gizmos.DrawSphere(point, 0.1f);
        //     }
        // }
    }
}
