using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace kit_stem_api.Constants
{
    public static class FirebaseConstants
    {
        public const string BucketPrivate = "swp391kitstemhubprivate";
        public const string BucketPublic = "swp391kitstemhub.appspot.com";
        public const string LabsFolder = "Labs";
        public const string ImagesKitsFolder = "Images/Kits";

        public static string GetCurrentKitFolder(int kitId)
        {
            return $"{ImagesKitsFolder}/{kitId}";
        }
    }
}