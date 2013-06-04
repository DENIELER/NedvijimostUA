using System;
using System.Collections.Generic;
using System.Linq;
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
        var selectPhone = _context.SubPurchasePhones.FirstOrDefault(p => p.phone == phone);

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

    public Model.SubPurchase AddSubpurchasePhone(string phone, Model.SubPurchase subpurchase)
    {
        if (subpurchase == null)
            return null;

        var newSubpurchasePhone = new Model.SubPurchasePhone()
        {
            Id = Guid.NewGuid(),
            phone = phone,
            createDate = Utils.GetUkranianDateTimeNow(),
            SubPurchase = subpurchase
        };
        _context.AddToSubPurchasePhones(newSubpurchasePhone);
        _context.SaveChanges();

        return subpurchase;
    }
}