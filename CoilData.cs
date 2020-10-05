using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Bot.Connector;

namespace MRBuddy
{
    public static class CoilData
    {
        private static  Dictionary<string, List<string>> _coilcombinationlist;
        private static List<CoilModel> GetCoilList()
        {
            List<CoilModel> lstCoil = new List<CoilModel>()
            {

                new CoilModel() {CoilName="T/R Head", CoilType = "Transmit/Receive", Design="Volume coil",
                 Applications = "Brain",                 
                 ImagePath ="https://images.philips.com/is/image/philipsconsumer/2a64c50faabb41549efaa92100cb578c?$jpglarge$&bgc=ffffff&wid=150"
                 },

                new CoilModel() {CoilName="Sense body", CoilType = "Receive-only", Design="Flexible volume coil consisting of an upper (anterior) and lower (posterior) coil part. Each part contains two phased-array coil elements",
                 Applications = "Abdomen, Thorax, Pelvis, Abdominal, Angiography",                 
                 ImagePath ="https://cdn.dotmed.com/images/listingpics/2267927.jpg"
                 },

                new CoilModel() { CoilName="SENSE Pediatric Head Spine", CoilType = "Receive-only", Design="Rigid volume coil",
                 Applications ="Pediatric head and spine",                 
                 ImagePath ="data:image/jpeg;base64,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"
                 },

                new CoilModel() { CoilName="SENSE T/R Knee", CoilType = "Transmit/Receive", Design="Rigid volume coil consisting of a coil base and an anterior coil part plus auxiliaries.",
                 Applications ="Knee imaging, feet-first examinations",
                 ImagePath ="data:image/jpeg;base64,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"
                 },
            };

            return lstCoil;
        }

        public static CoilModel GetCoilDatails(string coilName)
        {
            List<string> conditionList = new List<string>();

            List<CoilModel> lstCoil = GetCoilList();

            var items = lstCoil.SingleOrDefault(x => x.CoilName.ToLower() == coilName.ToLower());            

            return items;
        }

        public static string GetCoilType(string coilName)
        {
            CoilModel coil = GetCoilDatails(coilName);
            if(coil!=null)
            {
                return coilName + " is a " + coil.CoilType;
            }
            else
            {
                return coilName + " not found!";
            }
        }
        public static string isCoilCombinationValid(string coilname1, string coilname2)
        {
            PopulateCoilCombintions();
            string _retval = "Coil Combination is not recommended";
            coilname1 = coilname1.Replace(" ", "");
            if (_coilcombinationlist.ContainsKey(coilname1.ToLower()))
            {
                bool combine = SearchList(_coilcombinationlist[coilname1], coilname2);
                if (combine) _retval = "Recommended coil combination: Dual or multi coil imaging can be performed.";
            }
            else if (_coilcombinationlist.ContainsKey(coilname2.ToLower()))
            {
                bool combine = SearchList(_coilcombinationlist[coilname2], coilname1);
                if (combine) _retval = "Recommended coil combination: Dual or multi coil imaging can be performed.";
            }

            return _retval;
        }
        private static void PopulateCoilCombintions()
        {
            _coilcombinationlist = new Dictionary<string, List<string>>();

            _coilcombinationlist["senseheadspine8"] = new List<string>() { "Sense Flex L", "Sense Flex M", "Sense Flex S", "Sense Torso 16", "GP Flex L coil" };
            _coilcombinationlist["sensehst"] = new List<string>() { "SENSE GP Flex L" };

        }
        private static bool SearchList(List<string> coillist, string keyword)
        {
            bool _retval = false;
            foreach(var coil in coillist)
            {
                if(String.Compare(coil,keyword, CultureInfo.CurrentCulture, CompareOptions.IgnoreCase | CompareOptions.IgnoreSymbols) == 0)
                {
                    _retval = true;
                }
            }
            return _retval;
        }
    }
}
