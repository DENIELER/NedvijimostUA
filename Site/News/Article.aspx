<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Article.aspx.cs" Inherits="News_Article" MasterPageFile="~/AdvertismentMaster.master" %>

<asp:Content runat="server" ContentPlaceHolderID="Head">
    <title><%= Article_Title %> - Недвижимость-UA</title>

    <meta name="description" content="<%= Utils.StripTagsRegex(Article_Description) %>" />
    <meta name="keywords" content="<%= Article_Keywords %>" />
</asp:Content>

<asp:Content runat="server" ContentPlaceHolderID="Body">
    <% if(Article_Detected) { %>
    <div runat="server" id="pnlSites">
        <h1><%= Article_Header %></h1>
        
        <%= Article_Text %>

        <div style="margin: 10px 0;">
            <small><%= Article_CreateDate.ToShortDateString() %></small> | 
            <script type="text/javascript" src="//yandex.st/share/share.js" charset="utf-8"></script>
            <div style="display:inline;" class="yashare-auto-init" data-yashareL10n="ru" data-yashareType="none" data-yashareQuickServices="vkontakte,facebook,twitter,gplus,pinterest"></div> 
        </div>

        <div>
            <div id="disqus_thread"></div>
            <script type="text/javascript">
                /* * * CONFIGURATION VARIABLES: EDIT BEFORE PASTING INTO YOUR WEBPAGE * * */
                var disqus_shortname = 'nedvijimostua'; // required: replace example with your forum shortname

                /* * * DON'T EDIT BELOW THIS LINE * * */
                (function () {
                    var dsq = document.createElement('script'); dsq.type = 'text/javascript'; dsq.async = true;
                    dsq.src = 'http://' + disqus_shortname + '.disqus.com/embed.js';
                    (document.getElementsByTagName('head')[0] || document.getElementsByTagName('body')[0]).appendChild(dsq);
                })();
            </script>
            <noscript>Please enable JavaScript to view the <a href="http://disqus.com/?ref_noscript">comments powered by Disqus.</a></noscript>
            <a href="http://disqus.com" class="dsq-brlink">comments powered by <span class="logo-disqus">Disqus</span></a>
        </div>
    </div>
    <% }else{ %>
    <h2>Статья не найдена</h2>
    <p>
        Извините, статья не была найдена. Пожалуйста, обратитесь к администратору сайта.
    </p>
    <% } %>

</asp:Content>