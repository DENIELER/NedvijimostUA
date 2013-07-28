using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

class SubSectionsSeparationWorkflow : BaseContextWorkflow
{
    private string sectionCode;

    public SubSectionsSeparationWorkflow(string sectionCode, Model.DataModel context)
        : base(context)
    {
        this.sectionCode = sectionCode;
    }

    public bool NeedToDivideIntoSubSections
    {
        get
        {
            return context.AdvertismentSubSections
                .Where(s => s.AdvertismentSection.code == sectionCode)
                .Any();
        }
    }

    public void DivideIntoSubSections(ref List<Server.Entities.Advertisment> advertisments)
    {
        List<Model.AdvertismentSubSection> subSections = context.AdvertismentSubSections
                .Where(s => s.AdvertismentSection.code == sectionCode)
                .ToList();

        List<string> subSectionCodes = subSections.Select(s => s.code).ToList();

        Dictionary<string, List<string>> subSectionDeterminationWords =
            Settings.SubSectionDeterminationWordsSettings.Instance
            .getSubSectionsDeterminationWords(sectionCode, subSectionCodes);

        foreach (Server.Entities.Advertisment advertisment in advertisments)
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
