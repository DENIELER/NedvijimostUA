using Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Controls_AdvertismentSections : System.Web.UI.UserControl
{
    public NedvijimostDBEntities DBContext
    {
        get;
        set;
    }

    private int _currentSection;
    public int CurrentSection
    {
        get { return _currentSection; }
        set { _currentSection = value; }
    }

    protected List<AdvertismentSection> AdvertismentSections = new List<AdvertismentSection>();

    protected void Page_Load(object sender, EventArgs e)
    {
        if (string.IsNullOrEmpty(Request["section"])
            || !int.TryParse(Request["section"], out _currentSection))
        {
            _currentSection = 1; //-- default section Id
        }

        if (!IsPostBack)
        {
            if (Context != null)
            {
                var advertismentSectionsWorkflow = new AdvertismentSectionsWorkflow(DBContext);
                AdvertismentSections = advertismentSectionsWorkflow.LoadSections();
            }
        }
    }
}