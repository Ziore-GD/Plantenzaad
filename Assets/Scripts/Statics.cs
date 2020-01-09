using UnityEngine;
public static class Statics {
    private static CreateNodesFromTilemaps _grid;

    public static bool GetTurn (float x) {
        if (x > 0.1f) {
            return true;
        } else if (x < -0.1f) {
            return false;
        }
        return true;
    }
    public static Vector3 GetRandomSpawnPos (Vector3 pos, float radius) {
        if (_grid == null) _grid = CreateNodesFromTilemaps.Instance;

        for (int i = 0; i < 30; i++) {
            Vector3 randomPoint = pos + Random.insideUnitSphere * radius;
            Node targetNode = _grid.GetNode (new Vector2 ((int) randomPoint.x, (int) randomPoint.y));
            if (targetNode == null) {
                continue;
            }
            if (targetNode.walkable) {
                return targetNode.vectorPos;
            }
        }
        return pos;
    }
    public static int ConvertToEven (int a) {
        int Result = Mathf.RoundToInt (Mathf.Ceil ((float) a / 2) * 2);
        return Result;
    }
    public static Vector3 GetAttackDir (Vector3 charpos) {
        Vector3 dir = Statics.GetMouseDir(charpos);
        if (dir == Vector3.zero) {
            dir = new Vector3 (1, 0);
        }
        return dir.normalized;
    }
    public static Vector3 GetMouseDir (Vector3 charPos) {
        Vector3 pos = Input.mousePosition; // use new input system.
        pos.z = 10;
        Vector3 mouseDir = Camera.main.ScreenToWorldPoint (pos) - charPos;
        return mouseDir;
    }
}