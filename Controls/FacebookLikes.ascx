<%@ Control Language="C#" AutoEventWireup="true" CodeFile="FacebookLikes.ascx.cs" Inherits="Controls_FacebookLikes" %>

    <div id="fb-root"></div>

    <script>
        (function (d, s, id)
        {
            var js, fjs = d.getElementsByTagName(s)[0];
            if (d.getElementById(id)) return;
            js = d.createElement(s); js.id = id;
            js.src = "//connect.facebook.net/en_US/sdk.js#xfbml=1&version=v2.0";
            fjs.parentNode.insertBefore(js, fjs);
        }
        (document, 'script', 'facebook-jssdk'));

    </script>

    <div class="FacebookWrapper text-center">
        <div class="fb-like" data-href="https://www.facebook.com/EveryDoorDirectMail" data-layout="button_count" data-action="like" data-show-faces="false" data-share="true" data-colorscheme="dark"></div>
    </div>
