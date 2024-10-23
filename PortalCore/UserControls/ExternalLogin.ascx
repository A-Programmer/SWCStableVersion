<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ExternalLogin.ascx.cs" Inherits="PortalCore.UserControls.ExternalLogin" %>

<p>یا توسط یکی از حسابهای شبکه اجتماعی خود وارد شوید</p>
<div class="login-social-link">
    
    <asp:LinkButton runat="server" ID="lbFaceBookLogin" CssClass="btn btn-shadow btn-lg btn-block facebook">
        <i class="fa fa-facebook"></i>
        فیسبوک
    </asp:LinkButton>
    
    <asp:LinkButton runat="server" ID="lbGoogleLogin" CssClass="btn btn-shadow btn-lg btn-block google">
        <i class="fa fa-google-plus"></i>
        گوگل
    </asp:LinkButton>
</div>
