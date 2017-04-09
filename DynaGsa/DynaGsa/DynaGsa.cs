using System;
using System.Collections.Generic;
using Interop.gsa_8_7;
using Autodesk.DesignScript.Geometry;
using Autodesk.DesignScript.Runtime;

namespace DynaGsa
{


    public class DynaGsa
    {
        private DynaGsa()
        {

        }

        [MultiReturn(new[] { "GSA File" })]
        public static Dictionary<string, ComAuto> OpenModel(string filePath)
        {
            ComAuto GsaObj = new ComAuto();
            GsaObj.SetLocale(Locale.LOC_EN_GB);
            GsaObj.Open(filePath);

            return new Dictionary<string, ComAuto>
                {
                    {"GSA File", GsaObj as ComAuto}
                };
        }

        [MultiReturn(new[] { "Nodes.Coordinates", "Node.Refs" })]
        public static Dictionary<string, object> NodeCoordinates(object GsaFile, string sList)
        {

            ComAuto GsaObj = GsaFile as ComAuto;

            int[] NodeRefs = null;

            short s = GsaObj.EntitiesInList(sList, GsaEntity.NODE, out NodeRefs);

            Array.Sort(NodeRefs);

            List<Point> PtList = new List<Point>();


            GsaNode[] Nodes;


            //Get node coordinates
            short t = GsaObj.Nodes(NodeRefs, out Nodes);

            foreach (GsaNode item in Nodes)
            {

                PtList.Add(Point.ByCoordinates(item.Coor[0], item.Coor[1], item.Coor[2]));
            }

            return new Dictionary<string, object>
                {
//                
                    {"Nodes.Coordinates", PtList as List<Point>},
                    {"Node.Refs", NodeRefs as int[]}
                };

        }

    }

    public class Commands

    {
            }
}
