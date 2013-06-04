<%@ Page Language="C#" AutoEventWireup="true" CodeFile="SubPurchase.aspx.cs" Inherits="Kharkiv_SubPurchase" MasterPageFile="~/AdvertismentMaster.master" %>

<asp:Content runat="server" ContentPlaceHolderID="Head">
    <title><%= SubPurchasePhone %> - телефон посредника, агента недвижимости, Харьков - Недвижимость - UA</title>

    <meta name="description" content="Телефонные номера агенств недвижимости и посредников Харькова. Единый список посредников города Харькова при продаже или аренде недвижимости, квартир." />
    <meta name="keywords" content="Телефон, телефоны, номер, номера, посредник, агенство, агенство недвижимости, Харьков, город, город Харьков" />

</asp:Content>

<asp:Content runat="server" ContentPlaceHolderID="Body">

    <form id="MainForm" runat="server">
        <h2><%= SubPurchasePhone %></h2>
        <h4>Посредник. Харьков.</h4>

        <table style="margin: 50px 0;">
            <tr>
                <td><strong>Имя\Название организации:</strong></td> <td><%= SubPurchaseFullName %></td>
            </tr>
            <tr>
                <td><strong>Найден:</strong></td> <td><%= SubPurchaseCreateDate %></td>
            </tr>
        </table>
        <p>
            Если вы сталкивались с данным посредником или каким-либо образом сотрудничали с ним, пожалуйста, 
            оставьте о нем комментарий:
        </p>
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
    </form>
    
</asp:Content>