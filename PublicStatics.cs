using System.Collections;
using System.Collections.Generic;
using UnityEngine;

internal static class PublicStatics {

    internal static RaycastHit[] RaycastAll(Vector3 Position, Vector3 Direction, float Distance, Color TheColor, float Duration, bool DrawRay = false) {
        //Check to draw ray
        if (DrawRay) {
            //Show
            Debug.DrawLine(Position, Position + (Direction * Distance), TheColor, Duration);
        }
        //Return
        return Physics.RaycastAll(Position, Direction, Distance);
    }

    internal static RaycastHit[] RaycastAll(Vector3 Position, Vector3 Direction, float Distance, Color TheColor, float Duration, int TheLayerMask, bool DrawRay = false) {
        //Check to draw ray
        if (DrawRay) {
            //Show
            Debug.DrawLine(Position, Position + (Direction * Distance), TheColor, Duration);
        }
        //Return
        return Physics.RaycastAll(Position, Direction, Distance, TheLayerMask);
    }

    internal static RaycastHit[] RaycastAll(Ray TheRay, float Distance, Color TheColor, float Duration, bool DrawRay = false) {
        //Check to draw ray
        if (DrawRay) {
            //Show
            Debug.DrawLine(TheRay.origin, TheRay.origin + (TheRay.direction * Distance), TheColor, Duration);
        }
        //Return
        return Physics.RaycastAll(TheRay, Distance);
    }

    internal static RaycastHit[] RaycastAll(Ray TheRay, float Distance, Color TheColor, float Duration, int TheLayerMask, bool DrawRay = false) {
        //Check to draw ray
        if (DrawRay) {
            //Show
            Debug.DrawLine(TheRay.origin, TheRay.origin + (TheRay.direction * Distance), TheColor, Duration);
        }
        //Return
        return Physics.RaycastAll(TheRay, Distance, TheLayerMask);
    }

}