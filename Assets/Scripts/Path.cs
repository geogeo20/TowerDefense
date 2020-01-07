using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Path
{
   /// <summary>
   /// Script to help me generate a bezier curve that enemies would follow
   /// </summary>
   
   
   [SerializeField, HideInInspector] private List<Vector2> points;
   [SerializeField, HideInInspector] private bool isClosed;
   [SerializeField, HideInInspector] private bool setControl;
   public Path(Vector2 centre)
   {
      points = new List<Vector2>
      {
         centre + Vector2.left,
         centre + (Vector2.left + Vector2.up) * .5f,
         centre + (Vector2.right + Vector2.down) * .5f,
         centre + Vector2.right

      };

   }

   public Vector2 this[int i]
   {
      get { return points[i]; }
   }

   public int NumPoints
   {
      get { return points.Count; }
   }
   
   public int NumSegments
   {
      get { return points.Count / 3; }
   }

   public bool SetControl
   {
      get { return setControl;}
      set
      {
         if (setControl != value)
         {
            setControl = value;
            if (setControl)
            {
               SetAllAnchors();
            }
         }
      }
   }

   public void AddSegment(Vector2 anchorPos)
   {
      points.Add(points[points.Count-1]*2 - points[points.Count-2]);
      points.Add((points[points.Count-1] + anchorPos ) *.5f);
      points.Add(anchorPos);

      if (setControl)
      {
         SetAllAffected(points.Count-1);
      }
      
      
   }

   public Vector2[] GetPointsSegment(int i)
   {
      return new Vector2[]{points[i*3],points[i*3+1],points[i*3+2],points[LoopIndex(i*3+3)]};
   }

   public void MovePoint(int i, Vector2 pos)
   {
      Vector2 deltaMove = pos - points[i];
      if (i % 3 == 0 || !setControl)
      {
         points[i] = pos;

         if (setControl)
         {
            SetAllAffected(i);
         }
         else
         {
            {
               if (i % 3 == 0)
               {
                  if (i + 1 < points.Count || isClosed)
                  {
                     points[LoopIndex(i+1)] += deltaMove;
                  }

                  if (i - 1 >= 0 || isClosed)
                  {
                     points[LoopIndex(i-1)] += deltaMove;
                  }
         
               }
               else
               {
                  bool nextPointIsAnchor = (i + 1) % 3 == 0;
                  int correspondingControlIndex = (nextPointIsAnchor) ? i + 2 : i - 2;
                  int anchorIndex = (nextPointIsAnchor) ? i + 1 : i - 1;

                  if (correspondingControlIndex >= 0 && correspondingControlIndex < points.Count || isClosed)
                  { 
                     float distance = (points[LoopIndex(anchorIndex)] - points[LoopIndex(correspondingControlIndex)]).magnitude;
                     Vector2 dir = (points[LoopIndex(anchorIndex)] - pos).normalized;
                     points[LoopIndex(correspondingControlIndex)] = points[LoopIndex(anchorIndex)] + dir * distance;
                  }
         
               }
            }
         }
      }
     
      
      
   }

   public void ToogleClosed()
   {
      isClosed = !isClosed;

      if (isClosed)
      {
         points.Add(points[points.Count-1]*2 - points[points.Count-2]);
         points.Add(points[0]*2 - points[1]);
         if (setControl)
         {
            SetAnchorControl(0);
            SetAnchorControl(points.Count-3);
         }
      }
      else
      {
         points.RemoveRange(points.Count-2, 2);
         if (setControl)
         {
            SetStartEnd();
         }
      }
      
   }

   void SetAllAffected(int updatedIndex)
   {
      for (int i = updatedIndex -3; i <= updatedIndex + 3; i+=3)
      {
         if (i >= 0 && i < points.Count || isClosed)
         {
            SetAnchorControl(LoopIndex(i));  
         }
      }
      
      SetStartEnd();
      
   }

   void SetAllAnchors()
   {
      for (int i = 0; i < points.Count; i++)
      {
         SetAnchorControl(i);
         
      }
      SetStartEnd();
   }
   

   void SetAnchorControl(int anchorIndex)
   {
      Vector2 anchorPos = points[anchorIndex];
      Vector2 direction = Vector2.zero;
      float[] neighbourDistance = new float[2];

      if (anchorIndex - 3 >= 0 || isClosed)
      {
         Vector2 offset = points[LoopIndex(anchorIndex-3)] -anchorPos;
         direction += offset.normalized;
         neighbourDistance[0] = offset.magnitude;
      }
      
      if (anchorIndex + 3 >= 0 || isClosed)
      {
         Vector2 offset = points[LoopIndex(anchorIndex+3)] -anchorPos;
         direction -= offset.normalized;
         neighbourDistance[1] = -offset.magnitude;
      }

      direction.Normalize();
      
      for (int i = 0; i < 2; i++)
      {
         int controlIndex = anchorIndex + i * 2 - 1;
         if (controlIndex >= 0 && controlIndex < points.Count || isClosed)
            points[LoopIndex(controlIndex)] = anchorPos + direction * neighbourDistance[i] * .5f;
      }
      
   }

   void SetStartEnd()
   {
      if (!isClosed)
      {
         points[1] = (points[0] + points[2]) * .5f;
         points[points.Count - 2] = (points[points.Count - 1] * points[points.Count - 3]) *.5f;
      }
      
   }
   
   int LoopIndex(int i)
   {
      return (i + points.Count) % points.Count;
   }
   
}
