<%@ Page Language="C#" AutoEventWireup="true" CodeFile="AuthForm.aspx.cs" Inherits="Auth" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>

    <script type="text/javascript">
        function VkontakteOpen() {
            window.open('<%= VkontakteHref %>', 'Вконтакте авторизация', 'menubar=0,resizable=0,width=800,height=600');
        }

        /*function FacebookOpen() {
            window.open('<%= FacebookHref %>', 'Facebook авторизация', 'width=800,height=600');
        }*/

        function GoogleOpen() {
            window.open('<%= GoogleHref %>', 'Google авторизация', 'width=800,height=600');
        }
    </script>
</head>
<body>
    <form id="MainAuthorizationForm" runat="server">
        <div style="float:left;">
            <a href="#" onclick="javastript: VkontakteOpen();" ID="lnkVkontakte">
                <img src="../Images/Social_media/vkontakte-32.png" width="32" height="32" alt="Vkontakte authorization"/>
            </a>

            <a <%= "href=\""+FacebookHref+"\"" %> ID="lnkFacebook">
                <img src="../Images/Social_media/facebook-32.png" width="32" height="32" alt="Facebook authorization"/>
            </a>

            <a href="#" onclick="javastript: GoogleOpen();" ID="lnkGoogle">
                <img src="../Images/Social_media/google-32.png" width="32" height="32" alt="Goolge authorization"/>
            </a>

            <a <%= "href=\""+TwitterHref+"\"" %> target="_blank" ID="lnkTwitter">
                <img src="../Images/Social_media/twitter-32.png" width="32" height="32" alt="Twitter authorization"/>
            </a>
        </div>
    </form>
</body>
</html>
