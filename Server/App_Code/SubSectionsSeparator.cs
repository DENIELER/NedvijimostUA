using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

class SubSectionsSeparator
{
    private Model.DataModel _context;
    private Log _log;
    private string _sectionCode;

    public SubSectionsSeparator(string SectionCode, Model.DataModel Context, Log Log)
    {
        // TODO: Complete member initialization
        this._sectionCode = SectionCode;
        this._context = Context;
        this._log = Log;
    }

    public bool NeedToDivideIntoSubSections
    {
        get
        {
            return _context.AdvertismentSubSections
                .Where(s => s.AdvertismentSection.code == _sectionCode)
                .Any();
        }
    }

    public void DivideIntoSubSections(IList<ParsedAdvertisment> advertisments)
    {
        List<Model.AdvertismentSubSection> subSections = _context.AdvertismentSubSections
                .Where(s => s.AdvertismentSection.code == _sectionCode)
                .ToList();

        List<string> subSectionCodes = subSections.Select(s => s.code).ToList();

        Dictionary<string, List<string>> subSectionDeterminationWords =
            Settings.SubSectionDeterminationWordsSettings.Instance
            .getSubSectionsDeterminationWords(_sectionCode, subSectionCodes);

        foreach (ParsedAdvertisment advertisment in advertisments)
        {
            foreach (var subSectionWords in subSectionDeterminationWords)
            {
                foreach (string subSectionWord in subSectionWords.Value)
                {
                    if (advertisment.Text.ToLower().Contains(subSectionWord))
                    {
                        advertisment.SubSectionID = subSections.FirstOrDefault(s => s.code == subSectionWords.Key).Id;
                        break;
                    }
                }

                if (advertisment.SubSectionID != null)
                    break;
            }

            if (advertisment.SubSectionID == null)
                advertisment.SubSectionID = subSections.FirstOrDefault().Id;
        }
    }
}
