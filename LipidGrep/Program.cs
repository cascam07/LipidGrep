using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Mime;
using System.Text;
using System.Threading.Tasks;

namespace LipidGrep
{
    class Program
    {
        static void Main(string[] args)
        {
            if (args.Length != 2)
            {
                System.Console.Write(
                    "Method signature is:\nLipidGrep [Lipid Names File Path] [Global Target List File Path]");
            }
            else
            {
                var namePath = args[0];
                var targetsPath = args[1];

                var targetReader = new TargetListReader();
                var fileInfo = new FileInfo(targetsPath);
                var lipidTargets = targetReader.ReadTargets(fileInfo);
                fileInfo = new FileInfo(namePath);
                var outputLocation = fileInfo.FullName.Replace(".txt","").Replace(".csv","") + "_IdentifierInfo.txt";
                var names = targetReader.ReadNames(fileInfo);

                using (TextWriter textWriter = new StreamWriter(outputLocation))
                {
                    textWriter.WriteLine("Lipid Annotation\tClean Name\tGlobal Target Match\tCategory\tMain Class\tSub Class\tFormula\tLM_ID\tLipidMaps Entry\tPUBCHEM_SID\tPUBCHEM_CID\tINCHI_KEY\tKEGG_ID\tHMDBID\tCHEBI_ID\tLIPIDAT_ID\tLIPIDBANK_ID");
                    foreach (var name in names)
                    {                
                        var matches = from match in lipidTargets where match.CleanName.Equals(name.Item2) select match;
                  
                        foreach (var match in matches)
                        {

                            StringBuilder line = new StringBuilder();

                            line.Append(name.Item1 + "\t");
                            line.Append(match.CleanName + "\t");
                            line.Append(match.CommonName + "\t");
                            line.Append(match.Category + "\t");
                            line.Append(match.MainClass + "\t");
                            line.Append(match.SubClass + "\t");
                            line.Append(match.Formula + "\t");
                            line.Append(match.LipidMapsId + "\t");
                            line.Append(match.LipidMapsUrl + "\t");
                            line.Append(match.PubChemSid + "\t");
                            line.Append(match.PubChemCid + "\t");
                            line.Append(match.InchiKey + "\t");
                            line.Append(match.KeggId + "\t");
                            line.Append(match.HmdbId + "\t");
                            line.Append(match.ChebiId + "\t");
                            line.Append(match.LipidatId + "\t");
                            line.Append(match.LipidBankId + "\t");

                            textWriter.WriteLine(line.ToString());
                        }
                    }
                }

            }


        }
    }
}
