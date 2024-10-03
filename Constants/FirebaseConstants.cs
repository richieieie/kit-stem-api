using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace kit_stem_api.Constants
{
    public static class FirebaseConstants
    {
        public static string BucketPrivate = "swp391kitstemhubprivate";
        public static string BucketPublic = "swp391kitstemhub.appspot.com";
        public static string LabsFolder = "Labs";
        public static string ImagesKitsFolder = "Images/Kits";

        public static string GetCurrentKitFolder(int kitId)
        {
            return $"{ImagesKitsFolder}/{kitId}";
        }
    }
}