﻿<%@ Control Language="C#" AutoEventWireup="true" CodeFile="SearchResultsChart.ascx.cs" Inherits="Controls_SearchResultsChart" %>

<script type="text/javascript" src="Scripts/tufte-graph/raphael.js"></script>
<script type="text/javascript" src="Scripts/tufte-graph/jquery.enumerable.js"></script>
<script type="text/javascript" src="Scripts/tufte-graph/jquery.tufte-graph.js"></script>

<script type="text/javascript">
    $(document).ready(function () {
        $('#statistics-graph').tufteBar({
            data: [
                <%
                if (SearchResults != null)
                {
                    foreach (var result in SearchResults)
                    {
                        %>
                        [[<%= result.fullCount %>, <%= result.withoutSubPurchaseCount %>], { label: '<%= result.date.ToShortDateString() %>' }],
                        <%
                    }
                }
                %>
            ],
            barWidth: 0.5,
            barLabel: function (index) { return this[0][0] + '/' + this[0][1] },
            axisLabel: function (index) { return this[1].label },
            colors: ['#f47a20', '#82293B']
            //color: function (index) { return ['#E57536', '#82293B'][index % 2] },
            //legend: {
            //    data: ["SubPurchase", "NotSubPurchase"]
            //}
        });
    });
</script>

<div id="statistics-graph" class="graph" style="height: 150px"></div>
<table class="legend">
    <tr>
        <td><div style="width: 14px; height: 10px; background-color: #f78d1d;"></div></td>
        <td><small>Общее кол-во объявлений</small></td>
    </tr>
    <tr>
        <td><div style="width: 14px; height: 10px; background-color: #82293B;"></div></td>
        <td><small>Объявлений без посредников</small></td>
    </tr>
</table>