<%@ Page Title="نمایه" Language="C#" ValidateRequest="false" MasterPageFile="~/Admin/AdminMasterPage.Master" AutoEventWireup="true" CodeBehind="Profile.aspx.cs" Inherits="PortalCore.Admin.Profile" %>

<asp:Content ID="Content1" ContentPlaceHolderID="CpHeader" runat="server">
    
    
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="CpMain" runat="server">
    <script src="//cdn.ckeditor.com/4.5.4/basic/ckeditor.js"></script>
    <div class="row">
        <div class="col-sm-8">
            <section class="panel">
                <header class="panel-heading">
                    <span>مشخصات من</span>
                    <asp:Literal runat="server" ID="ltProfileMessage"></asp:Literal>
                </header>
                <div class="panel-body">
                    <div class="form-horizontal tasi-form">
                        <div class="form-group">
                            <label class="control-label col-sm-3">
                                نام کاربری
                            </label>
                            <div class="col-sm-9">
                                <asp:Literal runat="server" ID="ltUserName"></asp:Literal>
                            </div>
                        </div>
                        <div class="form-group">
                            <label class="control-label col-sm-3">
                                نام
                            </label>
                            <div class="col-sm-9 input-group m-bot15">
                                <asp:TextBox runat="server" ID="txtFirstName" CssClass="form-control input-group m-bot15"></asp:TextBox>
                                <span class="input-group-addon"><i class="fa fa-user"></i></span>
                            </div>
                        </div>
                        <div class="form-group">
                            <label class="control-label col-sm-3">
                                نام خانوادگی
                            </label>
                            <div class="col-sm-9 input-group m-bot15">
                                <asp:TextBox runat="server" ID="txtLastName" CssClass="form-control input-group m-bot15"></asp:TextBox>
                                <span class="input-group-addon"><i class="fa fa-user"></i></span>
                            </div>
                        </div>
                        <div class="form-group">
                            <label class="control-label col-sm-3">
                                ایمیل
                                <asp:RequiredFieldValidator runat="server" ID="rfvEmail"
                                            ErrorMessage="ایمیل خالی است" ForeColor="Red"
                                    ControlToValidate="txtEmail"
                                            ValidationGroup="Profile">*</asp:RequiredFieldValidator>
                            </label>
                            <div class="col-sm-9 input-group m-bot15">
                                <asp:TextBox runat="server" ID="txtEmail" CssClass="form-control input-group m-bot15"></asp:TextBox>
                                <span class="input-group-addon"><i class="fa fa-envelope"></i></span>
                            </div>
                        </div>
                        <div class="form-group">
                            <label class="control-label col-sm-3">
                                شماره تماس
                            </label>
                            <div class="col-sm-9 input-group m-bot15">
                                <asp:TextBox runat="server" ID="txtPhone" CssClass="form-control input-group m-bot15"></asp:TextBox>
                                <span class="input-group-addon"><i class="fa fa-phone"></i></span>
                            </div>
                        </div>
                    </div>
                </div>
            </section>
        </div>
        <div class="col-lg-4">
            <section class="panel">
                <header class="panel-heading">
                    <span>گروه کاربری شما</span>
                </header>
                <div class="panel-body">
                    <div class="col-sm-12 input-group m-bot15">
                        <span class="input-group-addon"><i class="fa fa-group"></i></span>
                        <asp:TextBox runat="server" ID="txtUserRole" CssClass="form-control input-group m-bot15"
                            Enabled="False"></asp:TextBox>
                        
                    </div>
                </div>
            </section>
            <section class="panel">
                <header class="panel-heading">
                    <span>انتخاب عکس پروفایل</span>
                </header>
                <div class="panel-body">
                    <asp:Image runat="server" ID="imgProfile" Width="50" Height="50"/>
                    &nbsp;&nbsp;
                    عکس فعلی شما
                    <br/>
                    <asp:FileUpload runat="server" ID="fuProfileImage"/>
                </div>
            </section>
            <section class="panel">
                <header class="panel-heading">
                    <span>دستورات مدیریتی</span>
                </header>
                <div class="panel-body">
                    <asp:Button runat="server" ID="btnSubmitProfile" CssClass="btn btn-shadow btn-success"
                         Text="ثبت تغییرات" ValidationGroup="Profile" OnClick="btnSubmitProfile_OnClick"/>
                    
                </div>
            </section>
        </div>
    </div>
    <div class="row">
        <div class="col-lg-12">
            <section class="panel">
                <header class="panel-heading">
                    <span>درباره من</span>
                    
                </header>
                <div class="panel-body">
                    <div class="form-horizontal tasi-form">
                        <div class="form-group">

                            <div class="col-sm-12">
                                <textarea runat="server" id="userinfo"></textarea>
                            </div>
                        </div>
                    </div>
                </div>
            </section>
        </div>
    </div>
    <script>
        CKEDITOR.replace('<%= userinfo.ClientID %>', {
            language: 'fa',
            uiColor: '#9AB8F3',
            contentsLangDirection : 'rtl'
        });
    </script>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="cpJsFiles" runat="server">
    
</asp:Content>
