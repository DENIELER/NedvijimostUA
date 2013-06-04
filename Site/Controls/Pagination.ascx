<%@ Control Language="C#" AutoEventWireup="true" CodeFile="Pagination.ascx.cs" Inherits="Controls_Pagination" %>

<div style="margin-top: 30px;" class="pagination">
    <ul>
        <%
            var pagesCount = ElementsCount % ElementsPerPage == 0
                                 ? (int) (ElementsCount / ElementsPerPage)
                                 : (int) Math.Round((decimal) ElementsCount/(decimal) ElementsPerPage) + 1;
                        
            %>
            <li <%= CurrentPageNum == 1 ? "class=\"disabled\"" : "class=\"active\""  %>>
                <a href="<%= CurrentPageNum == 1 ? "#" : GetCurrentUrlWithPageNum(CurrentPageNum - 1) %>">Prev</a>
            </li>
            <%
            
                if (pagesCount > 6)
                {
                    %>
                    <li class="disabled">
                        <a href="#">...</a>
                    </li>
                    <%

                    int startIndex = CurrentPageNum == 1 ? 1 : CurrentPageNum - 1;
                    for (int i = startIndex; i <= startIndex + 2; i++)
                    {
                        %>
                        <li <%= CurrentPageNum == i ? "class=\"disabled\"" : "class=\"active\""  %>>
                            <a href="<%= CurrentPageNum == i ? "#" : GetCurrentUrlWithPageNum(i) %>"><%= i %></a>
                        </li>
                        <%            
                    }
                            
                    %>
                    <li class="disabled">
                        <a href="#">...</a>
                    </li>
                    <%
                }
                else
                {
                    for (int i = 1; i <= pagesCount; i++)
                    {
                        %>
                        <li <%= CurrentPageNum == i ? "class=\"disabled\"" : "class=\"active\""  %>>
                            <a href="<%= CurrentPageNum == i ? "#" : GetCurrentUrlWithPageNum(i) %>"><%= i %></a>
                        </li>
                        <%
                    }
                }

%>
            <li <%= CurrentPageNum == pagesCount ? "class=\"disabled\"" : "class=\"active\""  %>>
                <a href="<%= CurrentPageNum == pagesCount ? "#" : GetCurrentUrlWithPageNum(CurrentPageNum + 1) %>">Next</a>
            </li>
            <%
        %>
    </ul>    
</div>