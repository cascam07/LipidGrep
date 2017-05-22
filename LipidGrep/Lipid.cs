using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace LipidGrep
{
    class Lipid
    {
        
        public string LipidMapsId { get; set; }
        public string CommonName { get; set; }
        public string AdductFull { get; set; }
        public string Category { get; set; }
        public string MainClass { get; set; }
        public string SubClass { get; set; }
        public string Formula { get; set; }
        public string PubChemSid { get; set; }
        public string PubChemCid { get; set; }
        public string InchiKey { get; set; }
        public string KeggId { get; set; }
        public string HmdbId { get; set; }
        public int ChebiId { get; set; }
        public int LipidatId { get; set; }
        public string LipidBankId { get; set; }

        public string CleanName
        {
            get
            {
                StringBuilder stringBuilder = new StringBuilder();
                MatchCollection matchCollection = Regex.Matches(CommonName, "([mdtOP]-?)?\\d+:\\d+(\\(((\\d+)?(OH|\\(OH\\))|CHO|COOH)\\))?"); //"([mdtOP]-?)?\\d+:\\d+(\\((2OH|CHO|COOH)\\))?");
                Regex pattern = new Regex(@"-");
                var namesplit = CommonName.Split('(');
                var lipidclass = namesplit[0];
                lipidclass = pattern.Replace(lipidclass, "");
                var oxidation = namesplit.Last();
                namesplit = CommonName.Split(')');
                var suffix = namesplit.Last();

                var chains = (from object match in matchCollection select match.ToString()).ToList();
                if (chains.Any())
                {
                    stringBuilder.Append(lipidclass + "(");

                    for (int i = 0; i < chains.Count(); i++)
                    {
                        var acylChain = chains[i];
                        stringBuilder.Append(acylChain);
                        if (i < chains.Count - 1) stringBuilder.Append("/");
                    }
                    stringBuilder.Append(")");
                    if (oxidation.Contains("OH")) stringBuilder.Append("(" + oxidation);
                    //stringBuilder.Append(suffix);
                    
                }

                return stringBuilder.ToString();
            }
        }

        public string LipidMapsUrl
        {
            get
            {
                if (LipidMapsId != "") return "http://www.lipidmaps.org/data/LMSDRecord.php?LMID=" + LipidMapsId;
                else return "";
            } 
        }


    }


}
