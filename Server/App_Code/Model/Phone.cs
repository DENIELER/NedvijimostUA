﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
/// Summary description for Phone
/// </summary>
public class Phone
{
	public Phone()
	{
	}

    public static string RemoveWrongSymbols(string phone)
    {
        if (phone.StartsWith("8-"))
            return phone.Remove(0, 2);

        if (phone.StartsWith("8"))
            return phone.Remove(0, 1);

        if (phone.StartsWith("+38-"))
            return phone.Remove(0, 4);

        if (phone.StartsWith("38-"))
            return phone.Remove(0, 3);

        if (phone.StartsWith("+38"))
            return phone.Remove(0, 3);

        if (phone.StartsWith("38"))
            return phone.Remove(0, 2);

        return phone;
    }
}