using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;

/// <summary>
/// Сводное описание для SubpurchasesWorkflow
/// </summary>
public class SubpurchasesWorkflow
{
    private Model.NedvijimostDBEntities _context;
	public SubpurchasesWorkflow()
	{
        _context = new Model.NedvijimostDBEntities();
	}

    public SubpurchasesWorkflow(Model.NedvijimostDBEntities context)
    {
        _context = context;
    }

    public Model.SubPurchase AddSubpurchasePhone(string phone, bool validated)
    {
        return this.AddSubpurchasePhone(phone, string.Empty, string.Empty, validated);
    }

    public Model.SubPurchase AddSubpurchasePhone(string phone, string name, string surname, bool validated)
    {
        string phoneLikeExpression = MakePhoneLikeExpression(phone);
        var selectPhone = _context.SubPurchasePhones
            .FirstOrDefault(p => System.Data.Linq.SqlClient.SqlMethods.Like(p.phone, phoneLikeExpression));

        if (selectPhone == null)
        {
            var addingSubPurchase = new Model.SubPurchase()
            {
                Id = Guid.NewGuid(),
                name = name,
                surname = surname,
                not_checked = !validated,
                createDate = Utils.GetUkranianDateTimeNow(),
                modifyDate = Utils.GetUkranianDateTimeNow()
            };
            _context.AddToSubPurchases(addingSubPurchase);
            _context.SaveChanges();

            this.AddSubpurchasePhone(phone, addingSubPurchase);

            return addingSubPurchase;
        }
        else
        {
            return selectPhone.SubPurchase;
        }
    }

    private string MakePhoneLikeExpression(string phone)
    {
        string temp = phone.Replace("+", "%").Replace(" ", "%").Replace("-", "%").Trim();
        string result = "%";
        for (int i = 0; i < temp.Length; i++)
        {
            if (temp[i] != '%')
                result += temp[i] + "%";
            else result += temp[i];
        }

        return result;
    }

    public Model.SubPurchase AddSubpurchasePhone(string phone, Model.SubPurchase subpurchase)
    {
        if (subpurchase == null)
            return null;

        List<string> formatedPhones = GetPhoneFormatsList(phone);
        foreach (var formatedPhone in formatedPhones)
        {
            var newSubpurchasePhone = new Model.SubPurchasePhone()
            {
                Id = Guid.NewGuid(),
                phone = formatedPhone,
                createDate = Utils.GetUkranianDateTimeNow(),
                SubPurchase = subpurchase
            };
            _context.AddToSubPurchasePhones(newSubpurchasePhone);
            _context.SaveChanges();
        }

        return subpurchase;
    }

    private List<string> GetPhoneFormatsList(string phone)
    {
        List<string> phones = new List<string>(); 

        string temp = phone.Replace("+", "").Replace(" ", "").Replace("-", "").Trim();
        //Regex.Replace(phone, @"(\w{4})(\w{4})(\w{4})", @"$1-$2-$3");

        return phones;
    }
}