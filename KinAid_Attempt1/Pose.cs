using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Research.Kinect.Nui;

namespace KinAid_Attempt1
{
    public class Pose
    {
        private Dictionary<SharedContent.BodyPartID, BodyPartOrientation> poseBodyPartOrientations = new Dictionary<SharedContent.BodyPartID, BodyPartOrientation>();

        public List<SharedContent.BodyPartID> bodyPartsOfInterest = new List<SharedContent.BodyPartID>();

        /// <summary>
        /// Constructor to define an arbitrary pose -- we use the idealized neutral orientations for null params
        /// </summary>
        /// <param name="poseBodyParts"></param>
        public Pose(Dictionary<SharedContent.BodyPartID, BodyPartOrientation> poseBodyParts)
        {
            if (poseBodyParts.Count == 0)
            {
                throw new Exception("No body part orientations specified");
            }
            foreach (SharedContent.BodyPartID bodyPartID in poseBodyParts.Keys)
            {
                BodyPartOrientation orientationToAdd;
                if (!poseBodyParts.TryGetValue(bodyPartID, out orientationToAdd)) throw new Exception("Should never happen");
                this.poseBodyPartOrientations.Add(bodyPartID, orientationToAdd);

                bodyPartsOfInterest.Add(bodyPartID);
            }
        }

        /// <summary>
        /// Constructor to create an instance of a pose from skeletal data -- only keeping track of the body parts of interest
        /// </summary>
        /// <param name="poseData">Skeletal data to create pose from</param>
        /// <param name="bodyPartsOfInterest">List of body parts that are of interest -- all body parts not included in this list will not be examined/tracked by this pose instance</param>
        public Pose(SkeletonData poseData, List<SharedContent.BodyPartID> bodyPartsOfInterest)
        {
            foreach (SharedContent.BodyPartID bodyPartID in bodyPartsOfInterest)
            {
                switch (bodyPartID)
                {
                    case SharedContent.BodyPartID.Head:
                        this.poseBodyPartOrientations.Add(bodyPartID, new HeadOrientation(poseData));
                        break;
                    case SharedContent.BodyPartID.Torso:
                        this.poseBodyPartOrientations.Add(bodyPartID, new TorsoOrientation(poseData));
                        break;
                    default:
                        this.poseBodyPartOrientations.Add(bodyPartID, new LimbOrientation(bodyPartID, poseData));
                        break;
                }
                bodyPartsOfInterest.Add(bodyPartID);
            }
        }

        /// <summary>
        /// Calibrates the body part orientations of interest defined in this pose based on their orientations in the skeletal data, where calibration means that the orientation of the body parts
        /// in the skeletal data will henceforth be regarded as equal to their definitions in this pose
        /// </summary>
        /// <param name="poseData"></param>
        public void CalibratePose(SkeletonData poseData)
        {
            foreach (SharedContent.BodyPartID bodyPartID in this.bodyPartsOfInterest)
            {
                BodyPartOrientation bodyPartToCalibrate;
                if (!this.poseBodyPartOrientations.TryGetValue(bodyPartID, out bodyPartToCalibrate)) throw new Exception("Should never happen");
                bodyPartToCalibrate.CalibrateOrientation(poseData);
            }
        }

        /// <summary>
        /// Determines whether this those is in between the initial and final poses provided, where it is assumed that a pose is in between two poses if and only if it occurs if
        /// the pose will occur while linearly transitioning from the first pose to the second pose (linearly = along the shortest path). Returns the average percent of the
        /// displacement between the initial and final poses that this pose has traveled
        /// </summary>
        /// <param name="initialPose"></param>
        /// <param name="finalPose"></param>
        /// <returns></returns>
        public UserPerformanceAnalysisInfo IsInBetweenPoses(Pose initialPose, Pose finalPose)
        {
            int maxChange = int.MinValue, minChange = int.MaxValue, percentSum = 0, numPercents = 0;
            SharedContent.BodyPartID maxChangeID = SharedContent.BodyPartID.Head, minChangeID = SharedContent.BodyPartID.Head;
            UserPerformanceAnalysisInfo result;

            foreach (SharedContent.BodyPartID bodyPartID in initialPose.bodyPartsOfInterest)
            {
                BodyPartOrientation bodyPartToCheck;
                this.poseBodyPartOrientations.TryGetValue(bodyPartID, out bodyPartToCheck);

                BodyPartOrientation initialBodyPart, finalBodyPart;
                initialPose.poseBodyPartOrientations.TryGetValue(bodyPartID, out initialBodyPart);
                finalPose.poseBodyPartOrientations.TryGetValue(bodyPartID, out finalBodyPart);

                result = bodyPartToCheck.IsInBetweenOrientations(initialBodyPart, finalBodyPart);
                if (result.failed)
                {
                    return result;
                }
                else if (!result.negligableAction)
                {
                    if (result.percentComplete < minChange)
                    {
                        minChange = result.percentComplete;
                        minChangeID = bodyPartID;
                    }
                    else if (result.percentComplete > maxChange)
                    {
                        maxChange = result.percentComplete;
                        maxChangeID = bodyPartID;
                    }
                    percentSum += result.percentComplete;
                    numPercents++;
                }
            }

            if (numPercents == 0)
            {
                throw new Exception("In determining if pose is between two others, found that no non-negligable changes exist between the starting and ending poses");
            }

            if (maxChange - minChange > SharedContent.AllowableDeviationInPercent)
            {
                result = new UserPerformanceAnalysisInfo(true, String.Format("The difference in the percentage completion of the movement for {0} and {1} exceeded the maximum allowable deviation of {2}",
                    Enum.GetName(typeof(SharedContent.BodyPartID), maxChangeID), Enum.GetName(typeof(SharedContent.BodyPartID), minChangeID), SharedContent.AllowableDeviationInPercent));
                return result;
            }
            else
            {
                result = new UserPerformanceAnalysisInfo(percentSum / numPercents);
                return result;
            }
        }

        /// <summary>
        /// Compares each of the body parts tracked by this posed (where a body part is tracked if it was designated as "of interest" at the instantiation of this pose object
        /// </summary>
        /// <param name="poseData"></param>
        /// <returns>True if the body parts of skeleton defined in the skeletal data are in the orientation defined in this pose</returns>
        public bool IsInPose(SkeletonData poseData)
        {
            foreach (SharedContent.BodyPartID bodyPartID in this.bodyPartsOfInterest)
            {
                BodyPartOrientation bodyPartToCheck;
                if (!this.poseBodyPartOrientations.TryGetValue(bodyPartID, out bodyPartToCheck)) throw new Exception("Should never happen");
                if (!bodyPartToCheck.IsBodyPartInOrientation(poseData))
                {
                    return false;
                }
            }
            return true;
        }
    }
}
