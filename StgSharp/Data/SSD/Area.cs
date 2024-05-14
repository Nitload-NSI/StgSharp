using StgSharp.Data.FileIO;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StgSharp.Data
{
    /// <summary>
    /// An independent storage area, used as the only folder type in root folder.
    /// Contains several <see cref="SSD"/> files reference as virtual disc.
    /// Data is actually stored in <see cref="SSD"/>s in certain RAID mode.
    /// </summary>
    public partial class Area:SSDFolder
    {
        private byte _id;
        private List<SSD> discList;

        public Area()
        {
             _id = StgSharp.NewArea();
        }

        public SSDSegment FindSegment(int id, byte SSD)
        {
            return discList[SSD].FindSegment(id);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="head"></param>
        /// <returns></returns>
        public static SSDraw AnalyzeHead(SSDrawHead head)
        {
            if (head._head.globalID == 0)
            {
                return null;
            }
            SSDraw ret = new SSDraw();
            foreach ((int, int) item in head)
            {

            }
            return ret;
        }

        public SSDraw FindFile(string route, SerializableTypeCode typeCode)
        {
            int id = 0;
            if (route.Contains('/'))
            {
                if (route.Contains(Serializer.GetNameTail(typeCode)))
                {
                    route += Serializer.GetNameTail(typeCode);
                }
                id = idIndex[route];
                return this.files[id];
            }
            if (route.Contains(Serializer.GetNameTail(typeCode)))
            {
                route += Serializer.GetNameTail(typeCode);
            }
            string[] folderRoute = route.Split('/');
            int index = 0;
            if (folderRoute[0] != ".")
            {
                index = 1;
            }
            SSDFolder temp = this;
            for (; index < folderRoute.Length - 1; index++)
            {
                id = temp.idIndex[folderRoute[index]];
                temp = temp.folders[id];
            }
            id = idIndex[folderRoute[folderRoute.Length-1]];
            return temp.files[id];
        }
    }
}
