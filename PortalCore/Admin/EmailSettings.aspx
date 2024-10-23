<%@ Page Title="تنظیمات ایمیل" Language="C#" MasterPageFile="~/Admin/AdminMasterPage.Master" AutoEventWireup="true" CodeBehind="EmailSettings.aspx.cs" Inherits="PortalCore.Admin.EmailSettings" %>
<asp:Content ID="Content1" ContentPlaceHolderID="CpHeader" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="CpMain" runat="server">
    <div class="alert alert-info">
        پسام های سیستمی: <asp:Literal runat="server" ID="ltSettingsMessage"></asp:Literal>
        <br/>
        <asp:ValidationSummary ID="vsSettings" ValidationGroup="Settings" runat="server" />
    </div>
    <div class="col-sm-8">
        <section class="panel">
            <section class="panel-heading">
                تنظیمات وب سایت (برای راهنمایی روی علامت سوال <i class="fa fa-question"></i> جلوی هر ورودی کلیک کنید.)
            </section>
            <div class="panel-body">
                <div class="form-horizontal tasi-form">
                    <div class="form-group">
                        <label class="control-label col-sm-5">
                            ایمیل
                            <asp:RequiredFieldValidator runat="server" ID="rfvEmail" ForeColor="Red"
                                ErrorMessage="ایمیل خالی است" ControlToValidate="txtEmail"
                                ValidationGroup="Settings">*</asp:RequiredFieldValidator>
                        </label>
                        <div class="col-sm-7 input-group m-bot15">
                            <asp:TextBox runat="server" ID="txtEmail" 
                                CssClass="form-control input-group m-bot15"></asp:TextBox>
                            <a href="javascript:;" class="input-group-addon" data-toggle="modal"
                                data-target="#Email" title="توضیحات"><i class="fa fa-question"></i></a>
                        </div>
                    </div>
                    <div class="form-group">
                        <label class="control-label col-sm-5">
                            کلمه عبور
                            <asp:RequiredFieldValidator runat="server" ID="rfvPassword" ForeColor="Red"
                                ErrorMessage="کبمه عبور خالی است" ControlToValidate="txtPassword"
                                ValidationGroup="Settings">*</asp:RequiredFieldValidator>
                        </label>
                        <div class="col-sm-7 input-group m-bot15">
                            <asp:TextBox runat="server" ID="txtPassword" 
                                CssClass="form-control input-group m-bot15"></asp:TextBox>
                            <a href="javascript:;" class="input-group-addon" data-toggle="modal"
                                data-target="#Password" title="توضیحات"><i class="fa fa-question"></i></a>
                        </div>
                    </div>
                    <div class="form-group">
                        <label class="control-label col-sm-5">
                            آدرس SMTP سرور ایمیل
                            <asp:RequiredFieldValidator runat="server" ID="rfvSmtp" ForeColor="Red"
                                ErrorMessage="آدرس SMTP خالی است" ControlToValidate="txtSmtpServer"
                                ValidationGroup="Settings">*</asp:RequiredFieldValidator>
                        </label>
                        <div class="col-sm-7 input-group m-bot15">
                            <asp:TextBox runat="server" ID="txtSmtpServer" 
                                CssClass="form-control input-group m-bot15"></asp:TextBox>
                            <a href="javascript:;" class="input-group-addon" data-toggle="modal"
                                data-target="#Smtp" title="توضیحات"><i class="fa fa-question"></i></a>
                        </div>
                    </div>
                    <div class="form-group">
                        <label class="control-label col-sm-5">
                            پورت
                            <asp:RequiredFieldValidator runat="server" ID="rfvPort" ForeColor="Red"
                                ErrorMessage="پورت خالی است" ControlToValidate="txtPort"
                                ValidationGroup="Settings">*</asp:RequiredFieldValidator>
                        </label>
                        <div class="col-sm-7 input-group m-bot15">
                            <asp:TextBox runat="server" ID="txtPort" 
                                CssClass="form-control input-group m-bot15"></asp:TextBox>
                            <a href="javascript:;" class="input-group-addon" data-toggle="modal"
                                data-target="#Port" title="توضیحات"><i class="fa fa-question"></i></a>
                        </div>
                    </div>
                </div>
            </div>
        </section>
    </div>
    
    
    <div class="col-sm-4">
        <section class="panel">
            <section class="panel-heading">
                دستورات مدیریتی
            </section>
            <div class="panel-body">
                <asp:Button runat="server" ID="btnSubmitEmailSettings" CssClass="btn btn-block btn-shadow btn-success"
                    Text="ثبت تنظیمات ایمیل" ValidationGroup="Settings" OnClick="btnSubmitEmailSettings_OnClick"/>
            </div>
        </section>
    </div>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="cpJsFiles" runat="server">
    <div id="Email" class="modal fade" role="dialog">
            <div class="modal-dialog">
                <!-- Modal content-->
                <div class="modal-content">
                    <div class="modal-body">
                        <div class="row">
                            <section class="panel">
                                <section class="panel-heading">
                                    ایمیل وب سایت
                                </section>
                                <div class="panel-body">
                                    از این ایمیل برای ارسال کلمه عبور و خبرنامه به کاربران استفاده می شود.
                                    <br/>
                                    ایمیلی برای وب سایت بسازید و مشخصات Smpt آن را در فرم وارد کنید.
                                </div>
                            </section>
                        </div>
                    </div>
                    <div class="modal-footer">
                        <div class="pull-right">
                            
                        </div>
                        <button type="button" class="btn btn-default" data-dismiss="modal">برگشت</button>
                    </div>
                </div>
            </div>
        </div>
    
    <div id="Password" class="modal fade" role="dialog">
            <div class="modal-dialog">
                <!-- Modal content-->
                <div class="modal-content">
                    <div class="modal-body">
                        <div class="row">
                            <section class="panel">
                                <section class="panel-heading">
                                    کلمه عبور ایمیل
                                </section>
                                <div class="panel-body">
                                    کلمه عبور ایمیل خود را وارد کنید.
                                </div>
                            </section>
                        </div>
                    </div>
                    <div class="modal-footer">
                        <div class="pull-right">
                            
                        </div>
                        <button type="button" class="btn btn-default" data-dismiss="modal">برگشت</button>
                    </div>
                </div>
            </div>
        </div>
    
    <div id="Smtp" class="modal fade" role="dialog">
            <div class="modal-dialog">
                <!-- Modal content-->
                <div class="modal-content">
                    <div class="modal-body">
                        <div class="row">
                            <section class="panel">
                                <section class="panel-heading">
                                    آدرس Smtp
                                </section>
                                <div class="panel-body">
                                    برای گوگل : smtp.gmail.com 
                                    <br/>
                                    برای یاهو : smtp.mail.yahoo.com 
                                </div>
                            </section>
                        </div>
                    </div>
                    <div class="modal-footer">
                        <div class="pull-right">
                            
                        </div>
                        <button type="button" class="btn btn-default" data-dismiss="modal">برگشت</button>
                    </div>
                </div>
            </div>
        </div>
    
    <div id="Port" class="modal fade" role="dialog">
            <div class="modal-dialog">
                <!-- Modal content-->
                <div class="modal-content">
                    <div class="modal-body">
                        <div class="row">
                            <section class="panel">
                                <section class="panel-heading">
                                    شماره پورت ایمیل
                                </section>
                                <div class="panel-body">
                                    پورت گوگل : 465 و 587 و 25
                                    <br/>
                                    پورت یاهو : 465 و 25
                                </div>
                            </section>
                        </div>
                    </div>
                    <div class="modal-footer">
                        <div class="pull-right">
                            
                        </div>
                        <button type="button" class="btn btn-default" data-dismiss="modal">برگشت</button>
                    </div>
                </div>
            </div>
        </div>
</asp:Content>
