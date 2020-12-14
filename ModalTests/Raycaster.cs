using Stride.Engine;
using Stride.Physics;
using Stride.Core.Mathematics;
using Stride.Core.Collections;

namespace ModalTests
{
  public static class Raycaster
  {
    public static bool CheckForHits(Vector2 screenOrMousePos, CameraComponent camera, Simulation simulation, out ClickResult clickResult)
    {
      var invViewProj = Matrix.Invert(camera.ViewProjectionMatrix);
      var sPos = new Vector3(screenOrMousePos.X * 2f - 1f, 1f - screenOrMousePos.Y * 2f, 0);
      var vectorNear = Vector3.Transform(sPos, invViewProj);
      vectorNear /= vectorNear.W;

      sPos.Z = 1f;

      var vectorFar = Vector3.Transform(sPos, invViewProj);
      vectorFar /= vectorFar.W;

      clickResult = new ClickResult();
      clickResult.ClickedEntity = null;
      clickResult.WorldPosition = Vector3.Zero;

      var minDistance = float.PositiveInfinity;

      var result = new FastList<HitResult>();
      simulation.RaycastPenetrating(vectorNear.XYZ(), vectorFar.XYZ(), result);
      foreach (var hitResult in result)
      {

        if (hitResult.Collider is StaticColliderComponent staticBody)
        {
          var distance = (vectorNear.XYZ() - hitResult.Point).LengthSquared();

          if (distance < minDistance)
          {
            minDistance = distance;
            clickResult.WorldPosition = hitResult.Point;
            clickResult.ClickedEntity = hitResult.Collider.Entity;
          }

        }
      }

      return (clickResult.ClickedEntity != null);
    }

    public class ClickResult
    {
      private bool isEmpty;
      public static readonly ClickResult Empty = new ClickResult(true);

      ClickResult(bool empty)
      {
        isEmpty = empty;
      }

      public ClickResult() : this(false)
      {
      }

      /// <summary>
      /// The world-space position of the click, where the raycast hits the collision body
      /// </summary>
      public Vector3 WorldPosition;

      /// <summary>
      /// The Entity containing the collision body which was hit
      /// </summary>
      public Entity ClickedEntity;

      public bool IsEmpty => this == Empty;

      public static bool operator ==(ClickResult a, ClickResult b)
      {
        if (a is null || b is null)
        {
          return false;
        }

        if (a.isEmpty && b.isEmpty)
        {
          return true;
        }

        return a.WorldPosition == b.WorldPosition && a.ClickedEntity == b.ClickedEntity;
      }

      public static bool operator !=(ClickResult a, ClickResult b)
      {
        return !(a == b);
      }

      public override int GetHashCode()
      {
        return WorldPosition.GetHashCode() + (ClickedEntity?.GetHashCode() ?? 0);
      }

      public override bool Equals(object obj)
      {
        return Equals(obj as ClickResult);
      }

      public bool Equals(ClickResult other)
      {
        return other is ClickResult result &&
         WorldPosition.Equals(result.WorldPosition) &&
         ClickedEntity == result.ClickedEntity;
      }
    }
  }
}