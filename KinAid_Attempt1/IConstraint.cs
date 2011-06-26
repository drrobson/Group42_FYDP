using System;
using System.Collections.Generic;
using System.Text;

using Microsoft.Research.Kinect.Nui;

namespace KinAid_Attempt1
{
    /// <summary>
    /// The basic interface for a constraint object.
    /// </summary>
    public interface IConstraint
    {
        SharedContent.Progression verify(SkeletonData currData, SkeletonData newData = null);
    }
}
