using System;
using System.Collections.Generic;
using Interop.gsa_8_7;
using Autodesk.DesignScript.Geometry;
using Autodesk.DesignScript.Runtime;

namespace DynaGsa
{


    public class ReadGsa
    {
        private ReadGsa()
        {

        }

        [MultiReturn(new[] { "GSA File" })]
        public static Dictionary<string, object> OpenModel(string filePath)
        {
            ComAuto GsaObj = new ComAuto();
            GsaObj.SetLocale(Locale.LOC_EN_GB);
            GsaObj.Open(filePath);

            return new Dictionary<string, object>
                {
                    {"GSA File", GsaObj as ComAuto}
                };
        }

        [MultiReturn(new[] { "Node.Refs" , "Nodes.Coordinates" })]
        public static Dictionary<string, object> NodesFromList(object GsaFile, string sList)
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
                    {"Node.Refs", NodeRefs as int[]},
                    {"Nodes.Coordinates", PtList as List<Point>}                   
                };

        }


        [MultiReturn(new[] { "Node.Refs", "Nodes.Coordinates" })]
        public static Dictionary<string, object> NodesFromRefs(object GsaFile, int[] NodeRefs)
        {

            ComAuto GsaObj = GsaFile as ComAuto;

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
                    {"Node.Refs", NodeRefs as int[]},
                    {"Nodes.Coordinates", PtList as List<Point>}
                };

        }

        [MultiReturn(new[] { "Elements.Refs", "Elements.EndPoints", "Elements.Properties", "Elements.Names" })]
        public static Dictionary<string, object> Elements(object GsaFile, string sList)
        {

            ComAuto GsaObj = GsaFile as ComAuto;
            
            int[] EleRefs = null;

            short s = GsaObj.EntitiesInList(sList, GsaEntity.ELEMENT, out EleRefs);

            Array.Sort(EleRefs);
            List<int[]> Pts = new List<int[]>();
            List<int> Properties = new List<int>();
            List<int> Names = new List<int>();
            //       List<Point> StPoint = new List<Point>();
            //       List<Point> EndPoint = new List<Point>();



            GsaElement[] GsaElements;

            //Get node coordinates
            short t = GsaObj.Elements(EleRefs, out GsaElements);

            foreach (GsaElement item in GsaElements)
            {

                Pts.Add(item.Topo);
                Properties.Add(item.Property);
                Names.Add(item.eType);
            }



            return new Dictionary<string, object>
                {            
                    {"Elements.Refs", EleRefs as int[]},
                    {"Elements.EndPoints", Pts as List<int[]>},
                    {"Elements.Properties", Properties as List<int>},
                    {"Elements.Names", Names as List<int> }
                };

        }


    }

    public class WriteGsa
    {

        [MultiReturn(new[] { "GsaNodes" })]
        public static Dictionary<string, double> CreateGSANodes(object GsaFile, Point StartPts, Point EndPts, double tolerance)
        {
            double StXcoord = StartPts.X;
            double StYcoord = StartPts.Y;
            double StZcoord = StartPts.Z;

            double EndXcoord = EndPts.X;
            double EndYcoord = EndPts.Y;
            double EndZcoord = EndPts.Z;

            double tol = (double)tolerance;

            ComAuto GsaObj = GsaFile as ComAuto;

            int StartNode = GsaObj.Gen_NodeAt(StXcoord, StYcoord, StZcoord, tol);
            int EndNode = GsaObj.Gen_NodeAt(EndXcoord, EndYcoord, EndZcoord, tol);


            SetElem1d(GsaObj, 1, 1 , "1", StartNode, EndNode);

            return new Dictionary<string, double>
                {              
                    {"GsaNodes", StartNode}
                };

        }



        public static int SetElem1d(object GsaFile, int iElem, int iProp, string sID, int StartNode, int EndNode)
        {
            ComAuto GsaObj = GsaFile as ComAuto;

            string sGwaCommand = "";

            //write beam element
            //EL_BEAM | num | prop | group | topo(2) | node | angle | dummy
            sGwaCommand = "EL_BEAM:";
            sGwaCommand += "{RVT:" + sID + "}";
            sGwaCommand += "," + iElem.ToString();                  //'number
            sGwaCommand += "," + iProp.ToString();                  //'property
            sGwaCommand += ",1";                                    //'group
            sGwaCommand += "," + StartNode.ToString();      //'topo 0
            sGwaCommand += "," + EndNode.ToString();      //'topo 1

            GsaObj.GwaCommand("EL_BEAM");

            return iElem;
        }



    }
  
}
