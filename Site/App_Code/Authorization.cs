using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Security;

/// <summary>
/// Сводное описание для Authorization
/// </summary>
public class Authorization
{
    public static string Vk_uid_SessionKey = "VK_Uid";
    public static string Vk_FirstName_SessionKey = "Vk_FirstName";
    public static string Vk_LastName_SessionKey = "Vk_LastName";
    public static string Vk_Photo_SessionKey = "Vk_Photo";
    public static string Vk_PhotoRec_SessionKey = "Vk_PhotoRec";
    public static string Vk_Hash_SessionKey = "Vk_Hash";

	public Authorization()
	{
		
	}

    public bool AuthorizeVkontakteUser(string uid, string first_name, string last_name, string photo, string photo_small, string hash)
    {
        HttpContext.Current.Session[Vk_uid_SessionKey] = uid;
        HttpContext.Current.Session[Vk_FirstName_SessionKey] = first_name;
        HttpContext.Current.Session[Vk_LastName_SessionKey] = last_name;
        HttpContext.Current.Session[Vk_Photo_SessionKey] = photo;
        HttpContext.Current.Session[Vk_PhotoRec_SessionKey] = photo_small;
        HttpContext.Current.Session[Vk_Hash_SessionKey] = hash;

        string cookiestr;
        HttpCookie ck;
        var tkt = new FormsAuthenticationTicket(1, uid, DateTime.Now,
        DateTime.Now.AddHours(7), true, first_name + "|" + last_name + "|" + photo + "|" + photo_small);
        cookiestr = FormsAuthentication.Encrypt(tkt);
        ck = new HttpCookie(FormsAuthentication.FormsCookieName, cookiestr);
        ck.Expires = tkt.Expiration;
        ck.Path = FormsAuthentication.FormsCookiePath;
        HttpContext.Current.Response.Cookies.Add(ck);

        return true;
    }
    public bool LogoffVkontakteUser()
    {
        HttpContext.Current.Session.Remove(Vk_uid_SessionKey);
        HttpContext.Current.Session.Remove(Vk_FirstName_SessionKey);
        HttpContext.Current.Session.Remove(Vk_LastName_SessionKey);
        HttpContext.Current.Session.Remove(Vk_Photo_SessionKey);
        HttpContext.Current.Session.Remove(Vk_PhotoRec_SessionKey);
        HttpContext.Current.Session.Remove(Vk_Hash_SessionKey);
        FormsAuthentication.SignOut();

        return true;
    }

    public static bool IsUserVkontakteAuthorized()
    {
        string user_uid = HttpContext.Current.Session[Vk_uid_SessionKey] as string;
        string user_hash = HttpContext.Current.Session[Vk_Hash_SessionKey] as string;

        return !string.IsNullOrEmpty(user_uid)
            && !string.IsNullOrEmpty(user_hash)
           && user_hash == CalculateMD5Hash("3111027" + user_uid + "FSApuxGl1jU3CpgG1Fra");
    }
    public static bool IsAdmin(string uid)
    {
        return uid == "106154673"; // DENIELER account
    }

    public static string GetVkontakteUserName()
    {
        if(IsUserVkontakteAuthorized())
        {
            string user_firstname = HttpContext.Current.Session[Vk_FirstName_SessionKey] as string;
            string user_lastname = HttpContext.Current.Session[Vk_LastName_SessionKey] as string;

            return user_firstname + " " + user_lastname;
        }else return null;
    }
    public static string GetVkontakteUserSmallPhoto()
    {
        if (IsUserVkontakteAuthorized())
        {
            string user_photo = HttpContext.Current.Session[Vk_PhotoRec_SessionKey] as string;
            
            return user_photo;
        }
        else return null;
    }
    public static string GetVkontakteUserUid()
    {
        if (IsUserVkontakteAuthorized())
        {
            string user_uid = HttpContext.Current.Session[Vk_uid_SessionKey] as string;

            return user_uid;
        }
        else return null;
    }

    protected static string CalculateMD5Hash(string input)
    {
        // step 1, calculate MD5 hash from input
        var md5 = System.Security.Cryptography.MD5.Create();
        byte[] inputBytes = System.Text.Encoding.UTF8.GetBytes(input);
        byte[] hash = md5.ComputeHash(inputBytes);

        // step 2, convert byte array to hex string
        var sb = new StringBuilder();
        for (int i = 0; i < hash.Length; i++)
        {
            sb.Append(hash[i].ToString("X2"));
        }
        return sb.ToString().ToLower();
    }
}