using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;

/// <summary>
/// Сводное описание для SubpurchasesWorkflow
/// </summary>
public class SubPurchases
{
    private Model.DataModel _context;
	public SubPurchases()
	{
        _context = new Model.DataModel();
	}

    public SubPurchases(Model.DataModel context)
    {
        _context = context;
    }

    public Model.SubPurchase AddSubpurchasePhone(string phone, bool validated)
    {
        return this.AddSubpurchasePhone(phone, string.Empty, string.Empty, validated);
    }

    public Model.SubPurchase AddSubpurchasePhone(string phone, string name, string surname, bool validated)
    {
        //string phoneLikeExpression = MakePhoneLikeExpression(phone);
        var selectPhone = _context.SubPurchasePhones
            .FirstOrDefault(p => p.phone == phone);

        if (selectPhone == null)
        {
            var addingSubPurchase = new Model.SubPurchase()
            {
                id = Guid.NewGuid(),
                name = name,
                surname = surname,
                not_checked = !validated,
                createDate = Utils.GetUkranianDateTimeNow(),
                modifyDate = Utils.GetUkranianDateTimeNow()
            };
            _context.SubPurchases.InsertOnSubmit(addingSubPurchase);
            _context.SubmitChanges();

            this.AddSubpurchasePhone(phone, addingSubPurchase);

            return addingSubPurchase;
        }
        else
        {
            return selectPhone.SubPurchase;
        }
    }

    public static string MakePhoneLikeExpression(string phone)
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

        //List<string> formatedPhones = GetPhoneFormatsList(phone);
        //foreach (string formatedPhone in formatedPhones)
        //{
            var newSubpurchasePhone = new Model.SubPurchasePhone()
            {
                Id = Guid.NewGuid(),
                //phone = formatedPhone,
                phone = phone,
                createDate = Utils.GetUkranianDateTimeNow(),
                SubPurchaseId = subpurchase.id
            };
            _context.SubPurchasePhones.InsertOnSubmit(newSubpurchasePhone);
            _context.SubmitChanges();
        //}

        return subpurchase;
    }

    private List<string> GetPhoneFormatsList(string phone)
    {
        List<string> phones = new List<string>(); 

        string temp = phone.Replace("+", "").Replace(" ", "").Replace("-", "").Trim();
        if (temp.StartsWith("8"))
            temp = temp.Substring(1);
        else if (temp.StartsWith("38"))
            temp = temp.Substring(2);

        if (temp.Length == 7)
        {
            phones.Add(Regex.Replace(temp, @"(\d{3})(\d{2})(\d{2})", @"$1-$2-$3"));
            phones.Add(Regex.Replace(temp, @"(\d{2})(\d{2})(\d{3})", @"$1-$2-$3"));
            phones.Add(Regex.Replace(temp, @"(\d{3})(\d{1})(\d{3})", @"$1-$2-$3"));
            phones.Add(Regex.Replace(temp, @"(\d{1})(\d{3})(\d{3})", @"$1-$2-$3"));
            phones.Add(Regex.Replace(temp, @"(\d{2})(\d{3})(\d{2})", @"$1-$2-$3"));
        }
        else if (temp.Length == 10)
        {
            phones.Add(Regex.Replace(temp, @"(\d{3})(\d{3})(\d{2})(\d{2})", @"$1-$2-$3-$4"));
            phones.Add(Regex.Replace(temp, @"(\d{3})(\d{7})", @"$1-$2"));
            phones.Add(Regex.Replace(temp, @"(\d{3})(\d{5})(\d{2})", @"$1-$2-$3"));
            phones.Add(Regex.Replace(temp, @"(\d{3})(\d{3})(\d{4})", @"$1-$2-$3"));
            phones.Add(Regex.Replace(temp, @"(\d{3})(\d{4})(\d{3})", @"$1-$2-$3"));
            phones.Add(Regex.Replace(temp, @"(\d{3})(\d{1})(\d{3})(\d{3})", @"$1-$2-$3-$4"));
            phones.Add(Regex.Replace(temp, @"(\d{3})(\d{3})(\d{3})(\d{1})", @"$1-$2-$3-$4"));
            phones.Add(Regex.Replace(temp, @"(\d{3})(\d{3})(\d{1})(\d{3})", @"$1-$2-$3-$4"));
            phones.Add(Regex.Replace(temp, @"(\d{3})(\d{2})(\d{2})(\d{3})", @"$1-$2-$3-$4"));
            phones.Add(Regex.Replace(temp, @"(\d{3})(\d{2})(\d{4})(\d{1})", @"$1-$2-$3-$4"));
            phones.Add(Regex.Replace(temp, @"(\d{3})(\d{1})(\d{2})(\d{2})(\d{2})", @"$1-$2-$3-$4-$5"));
            phones.Add(Regex.Replace(temp, @"(\d{3})(\d{2})(\d{3})(\d{2})", @"$1-$2-$3-$4"));
            phones.Add(Regex.Replace(temp, @"(\d{3})(\d{3})(\d{1})(\d{3})", @"$1 $2 $3 $4"));
            phones.Add(Regex.Replace(temp, @"(\d{3})(\d{3})(\d{2})(\d{2})", @"$1 $2 $3 $4"));
            phones.Add(Regex.Replace(temp, @"(\d{3})(\d{2})(\d{2})(\d{3})", @"$1 $2 $3 $4"));
            phones.Add(Regex.Replace(temp, @"(\d{3})(\d{2})(\d{4})(\d{1})", @"$1-$2-$3-$4"));
            phones.Add(Regex.Replace(temp, @"(\d{3})(\d{3})(\d{4})", @"$1 $2 $3"));
            phones.Add(Regex.Replace(temp, @"(\d{10})", @"$1"));
            phones.Add(Regex.Replace(temp, @"(\d{10})", @"8$1"));
            phones.Add(Regex.Replace(temp, @"(\d{3})(\d{2})(\d{2})(\d{3})", @"8-$1-$2-$3-$4"));
            phones.Add(Regex.Replace(temp, @"(\d{3})(\d{3})(\d{2})(\d{2})", @"8-$1-$2-$3-$4"));
        }
        else
        {
            phones.Add(phone);
        }

        phones.ForEach(p =>
        {
            if (string.IsNullOrEmpty(p))
                phones.Remove(p);
        });
        return phones;
    }
}