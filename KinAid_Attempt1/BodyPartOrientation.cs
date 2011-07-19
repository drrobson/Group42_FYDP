using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Media.Media3D;

using Microsoft.Research.Kinect.Nui;

namespace KinAid_Attempt1
{
    public abstract class BodyPartOrientation
    {
        public abstract void CalibrateOrientation(SkeletonData bodyPartData);
        public abstract bool IsBodyPartInOrientation(SkeletonData bodyPartData);
        public abstract UserPerformanceAnalysisInfo IsInBetweenOrientations(BodyPartOrientation initialOrientation, BodyPartOrientation finalOrientation);

        /// <summary>
        /// Checks whether: 1) the vector to check is approximately in the plane created by the other vectors, where it is approximately in the plane if
        /// the angle between it and its projection in the plane is less than or equal to the allowable deviation in degrees, and 2) the vector to check's
        /// projection in the plane yields a vector that segments the interior angle of the angle between the other vectors. Returns the percentage of the angular
        /// distance between the first plane-defining vector and the second plane-defining vector that the vector to check has traveled
        /// </summary>
        /// <param name="vectorToCheck"></param>
        /// <param name="firstVector"></param>
        /// <param name="secondVector"></param>
        /// <returns>-1 if vector does satisfy requirements. -2 if there is a negligable (less than allowable deviation) angle between the first plane vector
        /// and the second plane vector</returns>
        public static UserPerformanceAnalysisInfo IsVectorOnPathInPlane(Vector3D vectorToCheck, Vector3D firstPlaneVector, Vector3D secondPlaneVector)
        {
            UserPerformanceAnalysisInfo result;
            double totalDisplacementToTravel = Vector3D.AngleBetween(firstPlaneVector, secondPlaneVector);
            if (totalDisplacementToTravel <= SharedContent.AllowableDeviationInDegrees)
            {
                result = new UserPerformanceAnalysisInfo(true);
            }

            Vector3D normalToPlane = Vector3D.CrossProduct(firstPlaneVector, secondPlaneVector);
            normalToPlane.Normalize();
            Vector3D projectionOfInclIntoNormal = Vector3D.DotProduct(vectorToCheck, normalToPlane) * normalToPlane;
            Vector3D projectionOfInclIntoPlane = vectorToCheck - projectionOfInclIntoNormal;

            //Verifies that the deviation from the plane defined by the initial and final orientations is less than the allowable deviation
            if (Vector3D.AngleBetween(projectionOfInclIntoPlane, vectorToCheck) > SharedContent.AllowableDeviationInDegrees)
            {
                result = new UserPerformanceAnalysisInfo(true, String.Format("The deviation between the vector to check and the path between the two plane-defining vectors exceeds {0} degrees",
                    SharedContent.AllowableDeviationInDegrees));
                return result;
            }

            //Verifies that this orientation instance falls between the initial and final orientations in the interior angle give or take the allowable deviation
            if ((Vector3D.AngleBetween(projectionOfInclIntoPlane, secondPlaneVector) > Vector3D.AngleBetween(secondPlaneVector, firstPlaneVector) && 
                Vector3D.AngleBetween(projectionOfInclIntoPlane,firstPlaneVector) > SharedContent.AllowableDeviationInDegrees) ||
                (Vector3D.AngleBetween(projectionOfInclIntoPlane, firstPlaneVector) > Vector3D.AngleBetween(secondPlaneVector, firstPlaneVector) &&
                Vector3D.AngleBetween(projectionOfInclIntoPlane, secondPlaneVector) > SharedContent.AllowableDeviationInDegrees))
            {
                result = new UserPerformanceAnalysisInfo(true, "The vector is not in the shortest path between the two plane-defining vectors");
                return result;
            }

            double currentDisplacementTraveled = Vector3D.AngleBetween(projectionOfInclIntoPlane, firstPlaneVector);

            return new UserPerformanceAnalysisInfo((int)(100 * (currentDisplacementTraveled / totalDisplacementToTravel)));
        }
    }

    public enum BodyPartOrientationAnalysisInfo
    {

    }
}
