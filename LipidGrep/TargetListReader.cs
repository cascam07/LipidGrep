using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace LipidGrep
{
    internal class TargetListReader
    {
        private const string LM_ID = "LM_ID";
        private const string COMMON_NAME = "COMMON_NAME";
        private const string ADDUCT = "ADDUCT";
        private const string CATEGORY = "CATEGORY";
        private const string MAIN_CLASS = "MAIN_CLASS";
        private const string SUB_CLASS = "SUB_CLASS";
        private const string FORMULA = "FORMULA";
        private const string PUBCHEM_SID = "PUBCHEM_SID";
        private const string PUBCHEM_CID = "PUBCHEM_CID";
        private const string INCHI_KEY = "INCHI_KEY";
        private const string KEGG_ID = "KEGG_ID";
        private const string HMDBID = "HMDBID";
        private const string CHEBI_ID = "CHEBI_ID";
        private const string LIPIDAT_ID = "LIPIDAT_ID";
        private const string LIPIDBANK_ID = "LIPIDBANK_ID";

        /// <summary>
        /// Creates a mapping of column titles to their indices.
        /// </summary>
        /// <param name="columnString">The line containing the headers.</param>
        /// <returns>A Dictionary mapping column titles to their indices.</returns>
        public Dictionary<string, int> CreateColumnMapping(String columnString)
        {
            var columnMap = new Dictionary<string, int>();
            string[] columnTitles = columnString.Split('\t', '\n');

            for (int i = 0; i < columnTitles.Count(); i++)
            {
                String columnTitle = columnTitles[i].ToUpper();

                switch (columnTitle)
                {
                    case LM_ID:
                        columnMap.Add(LM_ID, i);
                        break;
                    case COMMON_NAME:
                        columnMap.Add(COMMON_NAME, i);
                        break;
                    case ADDUCT:
                        columnMap.Add(ADDUCT, i);
                        break;
                    case FORMULA:
                        columnMap.Add(FORMULA, i);
                        break;
                    case PUBCHEM_SID:
                        columnMap.Add(PUBCHEM_SID, i);
                        break;
                    case PUBCHEM_CID:
                        columnMap.Add(PUBCHEM_CID, i);
                        break;
                    case CATEGORY:
                        columnMap.Add(CATEGORY, i);
                        break;
                    case MAIN_CLASS:
                        columnMap.Add(MAIN_CLASS, i);
                        break;
                    case SUB_CLASS:
                        columnMap.Add(SUB_CLASS, i);
                        break;
                    case INCHI_KEY:
                        columnMap.Add(INCHI_KEY, i);
                        break;
                    case KEGG_ID:
                        columnMap.Add(KEGG_ID, i);
                        break;
                    case HMDBID:
                        columnMap.Add(HMDBID, i);
                        break;
                    case CHEBI_ID:
                        columnMap.Add(CHEBI_ID, i);
                        break;
                    case LIPIDAT_ID:
                        columnMap.Add(LIPIDAT_ID, i);
                        break;
                    case LIPIDBANK_ID:
                        columnMap.Add(LIPIDBANK_ID, i);
                        break;
                }
            }

            return columnMap;
        }

        /// <summary>
        /// Parses a line to create a LipidMapsEntry object.
        /// </summary>
        /// <param name="line">A line containing data representing a LipidMapsEntry object.</param>
        /// <param name="columnMapping">The mapping of column titles to their indices.</param>
        /// <returns>A LipidMapsEntry object.</returns>
        public Lipid ParseLine(String line, IDictionary<string, int> columnMapping)
        {
            string[] columns = line.Split('\t', '\n');

            var lipidEntry = new Lipid();

            if (columnMapping.ContainsKey(COMMON_NAME)) lipidEntry.CommonName = columns[columnMapping[COMMON_NAME]];
            else throw new SystemException("Common name is required for lipid import.");

            if (columnMapping.ContainsKey(ADDUCT)) lipidEntry.AdductFull = columns[columnMapping[ADDUCT]];
            else throw new SystemException("Adduct is required for lipid import.");

            if (columnMapping.ContainsKey(LM_ID)) lipidEntry.LipidMapsId = columns[columnMapping[LM_ID]];
            if (columnMapping.ContainsKey(FORMULA)) lipidEntry.Formula = columns[columnMapping[FORMULA]];
            if (columnMapping.ContainsKey(PUBCHEM_SID)) lipidEntry.PubChemSid = columns[columnMapping[PUBCHEM_SID]];
            if (columnMapping.ContainsKey(PUBCHEM_CID)) lipidEntry.PubChemCid = columns[columnMapping[PUBCHEM_CID]];
            if (columnMapping.ContainsKey(CATEGORY)) lipidEntry.Category = columns[columnMapping[CATEGORY]];
            if (columnMapping.ContainsKey(MAIN_CLASS)) lipidEntry.MainClass = columns[columnMapping[MAIN_CLASS]];
            if (columnMapping.ContainsKey(SUB_CLASS)) lipidEntry.SubClass = columns[columnMapping[SUB_CLASS]];
            if (columnMapping.ContainsKey(INCHI_KEY)) lipidEntry.InchiKey = columns[columnMapping[INCHI_KEY]];
            if (columnMapping.ContainsKey(KEGG_ID)) lipidEntry.KeggId = columns[columnMapping[KEGG_ID]];
            if (columnMapping.ContainsKey(HMDBID)) lipidEntry.HmdbId = columns[columnMapping[HMDBID]];
            if (columnMapping.ContainsKey(CHEBI_ID))
            {
                string value = columns[columnMapping[CHEBI_ID]];
                int chebi;
                int.TryParse(value, out chebi);
                lipidEntry.ChebiId = chebi;
            }
            if (columnMapping.ContainsKey(LIPIDAT_ID))
            {
                string value = columns[columnMapping[LIPIDAT_ID]];
                if (!value.Equals("")) lipidEntry.LipidatId = int.Parse(value);
            }
            if (columnMapping.ContainsKey(LIPIDBANK_ID)) lipidEntry.LipidBankId = columns[columnMapping[LIPIDBANK_ID]];

            return lipidEntry;
        }

        public List<Lipid> ReadTargets(FileInfo fileInfo)
        {
            var lipidTargets = new List<Lipid>();
            using (TextReader textReader = new StreamReader(fileInfo.FullName))
            {
                string columnHeaders = textReader.ReadLine();
                Dictionary<string, int> columnMapping = CreateColumnMapping(columnHeaders);

                string line;
                while ((line = textReader.ReadLine()) != null)
                {
                    var createdObject = ParseLine(line, columnMapping);
                    if (createdObject != null) lipidTargets.Add(createdObject);
                }
                return lipidTargets;
            }
        }
        /// <summary>
        /// Dictionary<OriginalName, CleanName>
        /// </summary>
        /// <param name="fileInfo"></param>
        /// <returns></returns>
        public List<Tuple<string,string>> ReadNames(FileInfo fileInfo)
        {
            var lipidNames = new List<Tuple<string, string>>();
            using (TextReader textReader = new StreamReader(fileInfo.FullName))
            {
                char[] delimeters = new[] {'\r', '\n', ',','\t',';'};
                Regex pattern = new Regex(@"(_)(\w{1,3})");
                var names = textReader.ReadToEnd();
                var lines = names.Split(delimeters, StringSplitOptions.RemoveEmptyEntries);
                foreach (var line in lines)
                {
                    var trimline = line.Trim();
                    Match match = Regex.Match(trimline, @"(_)(\w{1,3})");
                    if (match.Success)
                    {
                        var newline = pattern.Replace(trimline, "");
                        lipidNames.Add(new Tuple<string, string>(trimline, newline));
                        continue;
                    }
                    lipidNames.Add(new Tuple<string, string>(trimline, trimline));
                }
                return lipidNames;
            }
            
        } 
    }
}
