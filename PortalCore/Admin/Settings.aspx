<%@ Page Title="تنظیمات عمومی" Language="C#" MasterPageFile="~/Admin/AdminMasterPage.Master" AutoEventWireup="true" CodeBehind="Settings.aspx.cs" Inherits="PortalCore.Admin.Settings" %>
<asp:Content ID="Content1" ContentPlaceHolderID="CpHeader" runat="server">
    <style>
        .modal-body {
            line-height: 30px;
        }
         .spinner {
             width: 100px;
         }
        .spinner input {
            text-align: right;
        }
        .input-group-btn-vertical {
            position: relative;
            white-space: nowrap;
            width: 1%;
            vertical-align: middle;
            display: table-cell;
        }
        .input-group-btn-vertical > .btn {
            display: block;
            float: none;
            width: 100%;
            max-width: 100%;
            padding: 8px;
            margin-left: -1px;
            position: relative;
            border-radius: 0;
        }
        .input-group-btn-vertical > .btn:first-child {
            border-top-left-radius: 4px;
        }
        .input-group-btn-vertical > .btn:last-child {
            margin-top: -2px;
            border-bottom-left-radius: 4px;
        }
        .input-group-btn-vertical i{
            position: absolute;
            top: 0;
            left: 4px;
        }
    </style>
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
                            عنوان سایت
                            <asp:RequiredFieldValidator runat="server" ID="rfvSiteTitle" ForeColor="Red"
                                ErrorMessage="عنوان سایت خالی است" ControlToValidate="txtTitle"
                                ValidationGroup="Settings">*</asp:RequiredFieldValidator>
                        </label>
                        <div class="col-sm-7 input-group m-bot15">
                            <asp:TextBox runat="server" ID="txtTitle" 
                                CssClass="form-control input-group m-bot15"></asp:TextBox>
                            <a href="javascript:;" class="input-group-addon" data-toggle="modal"
                                data-target="#SiteTitle" title="توضیحات"><i class="fa fa-question"></i></a>
                        </div>
                    </div>
                    <div class="form-group">
                        <label class="control-label col-sm-5">
                            وضعیت
                        </label>
                        <div class="col-sm-7 input-group m-bot15">
                            <asp:CheckBox runat="server" ID="chkStatus"
                                CssClass="form-control input-group m-bot15" Text="&nbsp;&nbsp;فعال"/>
                            <a href="javascript:;" class="input-group-addon" data-toggle="modal"
                                data-target="#SiteStatus" title="توضیحات"><i class="fa fa-question"></i></a>
                        </div>
                    </div>
                    <div class="form-group">
                        <label class="control-label col-sm-5">
                            مدت تعلیق سایت
                        </label>
                        <div class="col-sm-7 input-group m-bot15">
                            <div class="input-group spinner">
                                <asp:TextBox runat="server" ID="txtPendingDays"
                                CssClass="form-control" Text="0"></asp:TextBox>
                                <div class="input-group-btn-vertical">
                                  <button class="btn btn-default" type="button"><i class="fa fa-caret-up"></i></button>
                                  <button class="btn btn-default" type="button"><i class="fa fa-caret-down"></i></button>
                                </div>
                              </div>
                            <a href="javascript:;" class="input-group-addon" data-toggle="modal"
                                data-target="#PendingDays" title="توضیحات"><i class="fa fa-question"></i></a>
                        </div>
                    </div>
                    <div class="form-group">
                        <label class="control-label col-sm-5">
                            ثبت نام
                        </label>
                        <div class="col-sm-7 input-group m-bot15">
                            <asp:CheckBox runat="server" ID="chkEnableRegistration"
                                CssClass="form-control input-group m-bot15" Text="&nbsp;&nbsp;فعال"/>
                            <a href="javascript:;" class="input-group-addon" data-toggle="modal"
                                data-target="#EnableRegistration" title="توضیحات"><i class="fa fa-question"></i></a>
                        </div>
                    </div>
                    <div class="form-group">
                        <label class="control-label col-sm-5">
                            ثبت نام بدون تایید
                        </label>
                        <div class="col-sm-7 input-group m-bot15">
                            <asp:CheckBox runat="server" ID="chkUnlockRegistration"
                                CssClass="form-control input-group m-bot15" Text="&nbsp;&nbsp;فعال"/>
                            <a href="javascript:;" class="input-group-addon" data-toggle="modal"
                                data-target="#EnableUnlockRegistration" title="توضیحات"><i class="fa fa-question"></i></a>
                        </div>
                    </div>
                    <%--<div class="form-group">
                        <label class="control-label col-sm-5">
                            ورود با حساب گوگل و فیس بوک
                        </label>
                        <div class="col-sm-7 input-group m-bot15">
                            <asp:CheckBox runat="server" ID="chkEnableExternalLogin"
                                CssClass="form-control input-group m-bot15" Text="&nbsp;&nbsp;فعال"/>
                            <a href="javascript:;" class="input-group-addon" data-toggle="modal"
                                data-target="#ExternalLogin" title="توضیحات"><i class="fa fa-question"></i></a>
                        </div>
                    </div>--%>
                    <div class="form-group">
                        <label class="control-label col-sm-5">
                            بازنشانی کلمه عبور
                        </label>
                        <div class="col-sm-7 input-group m-bot15">
                            <asp:CheckBox runat="server" ID="chkEnablePasswordRecovery"
                                CssClass="form-control input-group m-bot15" Text="&nbsp;&nbsp;فعال"/>
                            <a href="javascript:;" class="input-group-addon" data-toggle="modal"
                                data-target="#PasswordRecovery" title="توضیحات"><i class="fa fa-question"></i></a>
                        </div>
                    </div>
                    <div class="form-group">
                        <label class="control-label col-sm-5">
                            گروه پیش فرض
                        </label>
                        <div class="col-sm-7 input-group m-bot15">
                            <asp:DropDownList runat="server" ID="ddlRoles"
                                CssClass="form-control input-group m-bot15" ItemType="PortalCore.Models.Role"/>
                            <a href="javascript:;" class="input-group-addon" data-toggle="modal"
                                data-target="#DefaultRole" title="توضیحات"><i class="fa fa-question"></i></a>
                        </div>
                    </div>
                    <div class="form-group">
                        <label class="control-label col-sm-5">
                            پوسته فعال
                        </label>
                        <div class="col-sm-7 input-group m-bot15">
                            <asp:DropDownList runat="server" ID="ddlTemplates"
                                CssClass="form-control input-group m-bot15" ItemType="PortalCore.Models.Template"/>
                            <a href="javascript:;" class="input-group-addon" data-toggle="modal"
                                data-target="#ActiveTemplate" title="توضیحات"><i class="fa fa-question"></i></a>
                        </div>
                    </div>
                    <div class="form-group">
                        <label class="control-label col-sm-5">
                            برچسب ها
                            <asp:RequiredFieldValidator runat="server" ID="rfvTags" ForeColor="Red"
                                ErrorMessage="برچسب های سایت خالی است" ControlToValidate="txtTags"
                                ValidationGroup="Settings">*</asp:RequiredFieldValidator>
                        </label>
                        <div class="col-sm-7 input-group m-bot15">
                            <asp:TextBox runat="server" ID="txtTags" 
                                CssClass="form-control input-group m-bot15"></asp:TextBox>
                            <a href="javascript:;" class="input-group-addon" data-toggle="modal"
                                data-target="#Tags" title="توضیحات"><i class="fa fa-question"></i></a>
                        </div>
                    </div>
                    <div class="form-group">
                        <label class="control-label col-sm-5">
                            توضیحات
                            <asp:RequiredFieldValidator runat="server" ID="RequiredFieldValidator2" ForeColor="Red"
                                ErrorMessage="توضیحات سایت خالی است" ControlToValidate="txtDescription"
                                ValidationGroup="Settings">*</asp:RequiredFieldValidator>
                        </label>
                        <div class="col-sm-7 input-group m-bot15">
                            <asp:TextBox runat="server" ID="txtDescription" 
                                CssClass="form-control input-group m-bot15" TextMode="MultiLine" Rows="4"></asp:TextBox>
                            <a href="javascript:;" class="input-group-addon" data-toggle="modal"
                                data-target="#Description" title="توضیحات"><i class="fa fa-question"></i></a>
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
                <asp:Button runat="server" ID="btnSubmitSettings" CssClass="btn btn-block btn-shadow btn-success"
                    OnClick="btnSubmitSettings_OnClick" Text="ثبت تنظیمات عمومی" ValidationGroup="Settings"/>
            </div>
        </section>
    </div>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="cpJsFiles" runat="server">
    <div id="SiteTitle" class="modal fade" role="dialog">
            <div class="modal-dialog">
                <!-- Modal content-->
                <div class="modal-content">
                    <div class="modal-body">
                        <div class="row">
                            <section class="panel">
                                <section class="panel-heading">
                                    عنوان وب سایت
                                </section>
                                <div class="panel-body">
                                    عنوان وب سایت شما.
                                    <br/>
                                    مثال : وب سایت شخصی جان دو
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
    
    <div id="SiteStatus" class="modal fade" role="dialog">
            <div class="modal-dialog">
                <!-- Modal content-->
                <div class="modal-content">
                    <div class="modal-body">
                        <div class="row">
                            <section class="panel">
                                <section class="panel-heading">
                                    وضعیت فعال بودن وب سایت
                                </section>
                                <div class="panel-body">
                                    با غیر فعال کردن وضعیت وب سایت کاربران شما به صفحه "در دست ساخت" هدایت می شوند.
                                    <br/>
                                    اگر این گزینه را از حالت انتخاب خارج میکنید باید در قسمت بعد تعداد روزهایی که وب سایت به حالت تعلیق در می آید را وارد کنید، به عنوان مثال اگر وب سایت شما قرار است 10 روز دیگر راه اندازی شود این گزینه را از حالت انتخاب خارج کنید و در قسمت "مدت تعلیق سایت" عدد 10 را وارد کنید.
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
    
    <div id="PendingDays" class="modal fade" role="dialog">
            <div class="modal-dialog">
                <!-- Modal content-->
                <div class="modal-content">
                    <div class="modal-body">
                        <div class="row">
                            <section class="panel">
                                <section class="panel-heading">
                                    مدت زمان تعلیق وب سایت
                                </section>
                                <div class="panel-body">
                                    اگر در قسمت قبلی گزینه فعال بودن وب سایت را از حالت انتخاب خارج کرده اید در این قسمت تعداد روزهایی که باید وب سایت در حالت تعلیق باشد را وارد کنید.
                                    <br/>
                                    مثال : اگر قرار است وب سایت 10 روز در حالت تعلیق باشید عدد 10 را وارد کنید.
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
    
    <div id="EnableRegistration" class="modal fade" role="dialog">
            <div class="modal-dialog">
                <!-- Modal content-->
                <div class="modal-content">
                    <div class="modal-body">
                        <div class="row">
                            <section class="panel">
                                <section class="panel-heading">
                                    فعال کردن ثبت نام برای کاربران
                                </section>
                                <div class="panel-body">
                                    اگر این گزینه را فعال کنید فرم ثبت نام برای کاربران نمایش داده میشود و می توانند ثبت نام کنند.
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
    
    <div id="EnableUnlockRegistration" class="modal fade" role="dialog">
            <div class="modal-dialog">
                <!-- Modal content-->
                <div class="modal-content">
                    <div class="modal-body">
                        <div class="row">
                            <section class="panel">
                                <section class="panel-heading">
                                    ثبت نام کاربران بدون تایید مدیر
                                </section>
                                <div class="panel-body">
                                    با فعال کردن این گزینه کاربرانی که ثبت نام میکنند نیازی به تایید توسط مدیریت ندارند و بعد از ثبت نام می توانند وارد حساب کاربری خود شوند.
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

    <%--<div id="ExternalLogin" class="modal fade" role="dialog">
        <div class="modal-dialog">
            <!-- Modal content-->
            <div class="modal-content">
                <div class="modal-body">
                    <div class="row">
                        <section class="panel">
                            <section class="panel-heading">
                                ورود با حساب گوگل و فیس بوک
                            </section>
                            <div class="panel-body">
                                با فعال کردن این گزینه کاربران می توانند با نام کاربری و کلمه عبور خود در سایت های گوگل و فیس بوک وارد حساب کاربری خود شوند.
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
    </div>--%>
    
    <div id="PasswordRecovery" class="modal fade" role="dialog">
            <div class="modal-dialog">
                <!-- Modal content-->
                <div class="modal-content">
                    <div class="modal-body">
                        <div class="row">
                            <section class="panel">
                                <section class="panel-heading">
                                    فعال کردن قسمت فراموش کردن کلمه عبود
                                </section>
                                <div class="panel-body">
                                    اگر این گزینه را فعال کنید کاربران در صورت فراموشی کلمه عبور خود می توانند با وارد کردن ایمیل خود کلمه عبود خود را دریافت کنند.
                                    <br/>
                                    به خاطر داشته باشید اگر این گزینه را فعال کردید باید تنظیمات ایمیل را نیز انجام دهید.
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
    
    <div id="DefaultRole" class="modal fade" role="dialog">
            <div class="modal-dialog">
                <!-- Modal content-->
                <div class="modal-content">
                    <div class="modal-body">
                        <div class="row">
                            <section class="panel">
                                <section class="panel-heading">
                                    گروه چیش فرض کاربران جدید
                                </section>
                                <div class="panel-body">
                                    اگر ثبت نام در سایت عمومی است و به کاربران اجازه ثبت نام داده اید از این قسمت گروه پیش فرض را انتخاب کنید.
                                    <br/>
                                    کاربرانی که از فرم ثبت نام استفاده می کنند در این گروه قرار می گیرند.
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
    
    <div id="ActiveTemplate" class="modal fade" role="dialog">
            <div class="modal-dialog">
                <!-- Modal content-->
                <div class="modal-content">
                    <div class="modal-body">
                        <div class="row">
                            <section class="panel">
                                <section class="panel-heading">
                                    قالب پیش فرض وب سایت
                                </section>
                                <div class="panel-body">
                                    انتخاب قالب پیش فرض وب سایت.
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
    
    <div id="Tags" class="modal fade" role="dialog">
            <div class="modal-dialog">
                <!-- Modal content-->
                <div class="modal-content">
                    <div class="modal-body">
                        <div class="row">
                            <section class="panel">
                                <section class="panel-heading">
                                    برچسب ها
                                </section>
                                <div class="panel-body">
                                    برچسب ها کلیدواژه هایی درباره وب سایت شما هستند که به موتورهای جست و جو گر کمک می کنند تا وب سایت شما را بهتر بشناسند.
                                    <br/>
                                    لطفا برجسب ها را با "," یا همان ویرگول لاتین (کاما) از هم جدا کنید.
                                    <br/>
                                    مثال : سایت ساز,طراحی سایت,برنامه نویسی
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
    
    <div id="Description" class="modal fade" role="dialog">
            <div class="modal-dialog">
                <!-- Modal content-->
                <div class="modal-content">
                    <div class="modal-body">
                        <div class="row">
                            <section class="panel">
                                <section class="panel-heading">
                                    توضیحی درباره وب سایت
                                </section>
                                <div class="panel-body">
                                    این گزینه نیز برای معرفی بهتر وب سایت شما به موتورهای جست و جو است، در این قسمت توضیح کوتاهی درباره وب سایت خود قرار دهید این توضیح باید حداکثر 160 کاراکتر باشد.
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

    <script type="text/javascript">
        (function ($) {
            $('.spinner .btn:first-of-type').on('click', function () {
                $(this).parent().parent().find('input[type=text]').val(parseInt($(this).parent().parent().find('input[type=text]').val(), 10) + 1);
            });
            $('.spinner .btn:last-of-type').on('click', function () {
                $(this).parent().parent().find('input[type=text]').val(parseInt($(this).parent().parent().find('input[type=text]').val(), 10) - 1);
            });
        })(jQuery);
    </script>
</asp:Content>
