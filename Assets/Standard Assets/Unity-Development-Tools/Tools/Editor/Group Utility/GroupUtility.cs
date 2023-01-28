
/*---------------- Creation Date: 15-Dec-17 -----------------//
//------------ Last Modification Date: 15-Dec-17 ------------//
//----------- Luis Arzola: http://heisarzola.com ------------*/

/*----------------------------- OVERVIEW -------------------------------//
 *   <<< NAME >>>
 *       -- Group Utility.
 *       
 *   <<< DESCRIPTION >>>
 *       -- Utility in charge of grouping the selected scene objects.
 *
 *   <<< LIMITATIONS >>>
 *       -- None.
 *
 *   <<< DEPENDENCIES >>>
 *       -- Plugins: None.
 *       -- Module:
 *          -- GameObject
//----------------------------------------------------------------------*/

/*------------------------------- NOTES --------------------------------//
 *   <<< TO-DO LIST >>>
 *       -- <<< EMPTY >>>
 *
 *   <<< POSSIBLES >>>
 *       -- <<< EMPTY >>>
 *
 *   <<< SOURCES >>>
 *       -- [1] Unedited whole tool : http://www.reptoidgames.com/goodies/GroupUtility.cs
//---------------------------------------------------------------------*/

/*---------------------------- CHANGELOG -------------------------------//
 *   <<< V.1.0.0 -- 15-Dec-17 >>>
 *       -- Unedited method recovered from source [1].
//----------------------------------------------------------------------*/

using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

[ExecuteInEditMode]
public class GroupUtility : Editor
{
    //------------------------------------------------------------------------------------//
    //---------------------------------- METHODS -----------------------------------------//
    //------------------------------------------------------------------------------------//

    [MenuItem("Edit/Group %g", false)]
    public static void Group()
    {
        if (Selection.transforms.Length > 0)
        {
            GameObject group = new GameObject("Group");

            // set pivot to average of selection
            Vector3 pivotPosition = Vector3.zero;
            foreach (Transform g in Selection.transforms)
            {
                pivotPosition += g.transform.position;
            }
            pivotPosition /= Selection.transforms.Length;
            group.transform.position = pivotPosition;

            // register undo as we parent objects into the group
            Undo.RegisterCreatedObjectUndo(group, "Group");
            List<GameObject> objs = Selection.gameObjects.ToList();

            group.transform.parent = objs[0].transform.parent;
            group.transform.SetSiblingIndex(objs[0].transform.GetSiblingIndex());
            group.name = objs[0].name + " - Group";
            objs.Sort((x, y) => String.Compare(x.name, y.name, StringComparison.Ordinal));
            foreach (GameObject s in objs)
            {
                Undo.SetTransformParent(s.transform, group.transform, "Group");
            }

            Selection.activeGameObject = group;
        }
        else
        {
            Debug.LogWarning("You must select one or more objects.");
        }
    }
}