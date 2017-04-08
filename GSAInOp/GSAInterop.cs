using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Interop.Gsa_8_7;
using Autodesk.DesignScript.Runtime;

namespace GSAInOp
{
    public class GSAInterop
    {
        //Hide node GSAInterop
        private GSAInterop()
        {

        }

        [MultiReturn(new[] {"GSA Object"})]
        public static Dictionary<string, ComAutoClass> OpenGsaModel(string filePath)
        {

            ComAuto GsaObj = new ComAuto();
            GsaObj.SetLocale(Locale.LOC_EN_GB);
            GsaObj.Open(filePath);
//            ComAutoClass gsaFile = GsaObj as ComAutoClass;

            return new Dictionary<string, ComAutoClass>
                {
//                    {"GSA File", gsaFile},
                    {"GSA Object", GsaObj as ComAutoClass}
                };
        }


        [MultiReturn(new[] { "NodeRefs" })]
        public static Dictionary<string, object> GetGsaNodeNumbers(ComAutoClass GsaObj, string List)
        {

            ComAuto GsaFile = GsaObj as ComAuto;

            int[] NodeRefs = null;

            short s = GsaFile.EntitiesInList(List, GsaEntity.NODE, out NodeRefs);
                       
            //short s = GsaObj.EntitiesInList(List, GsaEntity.NODE, out NodeRefs);

            Array.Sort(NodeRefs);

            return new Dictionary<string, object>
                {
                    { "Node Refs", NodeRefs }
                };
        }
        


    }
}
