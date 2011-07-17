using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Research.Kinect.Nui;

namespace KinAid_Attempt1
{
    public interface BodyPartOrientation
    {
        bool IsBodyPartInOrientation(SkeletonData bodyPartData);
    }
}
