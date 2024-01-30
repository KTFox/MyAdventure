using UnityEditor;

[CustomEditor(typeof(HitBox))]
public class HitBoxEditor : Editor
{

    #region SerializedProperties
    SerializedProperty type;
    SerializedProperty damage;
    SerializedProperty _flyHitbox;
    SerializedProperty timeToDestroy;
    #endregion

    private void OnEnable() {
        type = serializedObject.FindProperty("type");
        damage = serializedObject.FindProperty("damage");
        _flyHitbox = serializedObject.FindProperty("_flyHitbox");
        timeToDestroy = serializedObject.FindProperty("timeToDestroy");
    }

    public override void OnInspectorGUI() {
        HitBox customInspector = (HitBox)target;

        serializedObject.Update();

        EditorGUILayout.PropertyField(type);
        EditorGUILayout.PropertyField(damage);

        EditorGUILayout.PropertyField(_flyHitbox);
        if (customInspector.FlyHitbox) {
            EditorGUILayout.PropertyField(timeToDestroy);
        }

        serializedObject.ApplyModifiedProperties();
    }


}
