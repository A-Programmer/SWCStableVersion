<%@ Page Title="مدیریت کاربران" Language="C#" ValidateRequest="false" MasterPageFile="~/Admin/AdminMasterPage.Master" AutoEventWireup="true" CodeBehind="ManageUsers.aspx.cs" Inherits="PortalCore.Admin.ManageUsers" %>

<asp:Content ID="Content1" ContentPlaceHolderID="CpHeader" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="CpMain" runat="server">
    <script src="//cdn.ckeditor.com/4.5.4/basic/ckeditor.js"></script>
    <asp:MultiView runat="server" ID="mvUsers" ActiveViewIndex="0">
        <asp:View runat="server" ID="vwUsers">
            <section class="panel">
                <header class="panel-heading">

                    <div class="row">
                        <div class="col-lg-6">
                            <asp:Button runat="server" ID="btnAdd" Text="افزودن کاربر جدید"
                                CausesValidation="False" CssClass="btn btn-shadow btn-success" OnClick="btnAdd_OnClick"/>
                            <asp:Literal runat="server" ID="ltUserMessage"></asp:Literal>
                        </div>
                        <div class="col-lg-6">
                            <div class="input-group input-group-lg">
                                <asp:TextBox runat="server" ID="txtSearchUser" CssClass="form-control"
                                    placeholder="نام، نام خانوادگی، نام کاربری یا ایمیل را وارد کنید"></asp:TextBox>
                                <span class="input-group-btn">
                                    <asp:Button runat="server" ID="btnSearch" Text="بیاب"
                                        CssClass="btn btn-default" OnClick="btnSearch_OnClick"/>
                                </span>
                            </div>
                        </div>
                    </div>

                </header>
                <div class="panel-body">
                    <div class="col-sm-12">
                        <asp:Panel runat="server" ID="pnlUsersStats">
                            تعداد کاربران : <asp:Literal runat="server" ID="ltUsersCount"></asp:Literal>
                        </asp:Panel>
                    </div>
                    <asp:GridView runat="server" ID="grdUsers" AutoGenerateColumns="False" GridLines="None"
                        ItemType="PortalCore.Models.User" PageSize="20" AllowPaging="True"
                        CssClass="table table-striped table-advance table-hover table-bordered table-responsive"
                        OnPageIndexChanging="grdUsers_OnPageIndexChanging" OnRowCommand="grdUsers_OnRowCommand">
                        <Columns>
                            <asp:BoundField DataField="UserName" HeaderText="نام کاربری" />
                            <asp:CheckBoxField DataField="IsActive" HeaderText="وضعیت فعال"/>
                            <%--<asp:CheckBoxField DataField="IsSuperAdmin" HeaderText="مدیرکل"/>--%>
                            <asp:TemplateField HeaderText="دستورات">
                                <ItemStyle Width="25%"></ItemStyle>
                                <ItemTemplate>
                                    <div class="btn-group btn-group-justified">
                                        <asp:LinkButton runat="server" ID="lbEdit" CommandArgument='<%#Eval("UserId") %>'
                                            CommandName="DoEdit" CssClass="btn btn-block btn-shadow btn-info"
                                            ToolTip="ویرایش"><i class="fa fa-edit"></i></asp:LinkButton>

                                        <asp:LinkButton runat="server" ID="lbDelete" CommandArgument='<%#Eval("UserId") %>'
                                            CommandName="DoDelete" CssClass="btn btn-block btn-shadow btn-danger"
                                            ToolTip="حذف"><i class="fa fa-remove"></i></asp:LinkButton>

                                        <asp:LinkButton runat="server" ID="lbLockUnlock" CommandArgument='<%#Eval("UserId") %>'
                                            CommandName="DoLockUnlock" CssClass="btn btn-block btn-shadow btn-default"
                                            ToolTip="قفل/باز کردن کاربر"><i class="fa fa-lock"></i></asp:LinkButton>

                                        <%--<asp:LinkButton runat="server" ID="lbSuperAdmin" CommandArgument='<%#Eval("UserId") %>'
                                            CommandName="DoSuperAdmin" CssClass="btn btn-block btn-shadow btn-success"
                                            Visible='<%# GetAccess("SetSuperAdmin") %>'
                                            ToolTip="کاربر ارشد"><i class="fa fa-bolt"></i></asp:LinkButton>--%>
                                        
                                        <a href="javascript:;" class="btn btn-block btn-shadow btn-primary" title="مشخصات"
                                            data-toggle="modal" data-target="#UserInfo-<%#Eval("UserId") %>">
                                            <i class="fa fa-info"></i>
                                        </a>
                                    </div>
                                    <div id="UserInfo-<%#Eval("UserId") %>" class="modal fade" role="dialog">
                                            <div class="modal-dialog">
                                                <!-- Modal content-->
                                                <div class="modal-content">
                                                    <div class="modal-body">
                                                        <div class="row">
                                                                <section class="panel">
                                                                <header class="panel-heading">
                                                                    مشخصات کاربر
                                                                </header>
                                                                <div class="panel-body">
                                                                    <div class="col-sm-3">
                                                                        <div style="width: 100%; height: 30%;">
                                                                            <asp:Image runat="server" ID="imgUserPic" 
                                                                                ImageUrl='<%#Eval("UserPicture") %>'
                                                                                Width="80px" Height="80px"/>
                                                                        </div>
                                                                
                                                                    </div>
                                                                    <div class="form-horizontal tasi-form col-lg-9">
                                                                        <div class="form-group">
                                                                            <label class="control-label col-sm-4">
                                                                                نام کاربری
                                                                            </label>
                                                                            <div class="col-sm-8 input-group m-bot15">
                                                                                <label class="form-control">
                                                                                    <%#Eval("UserName") %>
                                                                                </label>
                                                                            </div>
                                                                        </div>
                                                                        <div class="form-group">
                                                                            <label class="control-label col-sm-4">
                                                                                ایمیل
                                                                            </label>
                                                                            <div class="col-sm-8 input-group m-bot15">
                                                                                <label class="form-control">
                                                                                    <%#Eval("Email") %>
                                                                                </label>
                                                                            </div>
                                                                        </div>
                                                                        <div class="form-group">
                                                                            <label class="control-label col-sm-4">
                                                                                نام
                                                                            </label>
                                                                            <div class="col-sm-8 input-group m-bot15">
                                                                                <label class="form-control">
                                                                                    <%#Eval("FirstName") %>
                                                                                </label>
                                                                            </div>
                                                                        </div>
                                                                        <div class="form-group">
                                                                            <label class="control-label col-sm-4">
                                                                                نام خانوادگی
                                                                            </label>
                                                                            <div class="col-sm-8 input-group m-bot15">
                                                                                <label class="form-control">
                                                                                    <%#Eval("LastName") %>
                                                                                </label>
                                                                            </div>
                                                                        </div>
                                                                        <div class="form-group">
                                                                            <label class="control-label col-sm-4">
                                                                                شماره تماس
                                                                            </label>
                                                                            <div class="col-sm-8 input-group m-bot15">
                                                                                <label class="form-control">
                                                                                    <%#Eval("Phone") %>
                                                                                </label>
                                                                            </div>
                                                                        </div>
                                                                        <div class="form-group">
                                                                            <label class="control-label col-sm-4">
                                                                                توضیحات
                                                                            </label>
                                                                            <div class="col-sm-8 input-group m-bot15">
                                                                                <label class="form-control">
                                                                                    <%#Eval("UserInfo") %>
                                                                                </label>
                                                                            </div>
                                                                        </div>
                                                                    </div>
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

                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                    </asp:GridView>
                </div>
            </section>

        </asp:View>
        <asp:View runat="server" ID="vwEditUser">
            <div class="row">
                <div class="col-sm-8">
                    <section class="panel">
                        <header class="panel-heading">
                            <span>مشخصات کاربر</span> &nbsp;&nbsp;
                            <asp:Literal runat="server" ID="ltProfileMessage"></asp:Literal>
                        </header>
                        <div class="panel-body">
                            <div class="form-horizontal tasi-form">
                                <div class="form-group">
                                    <label class="control-label col-sm-3">
                                        نام کاربری
                                        <asp:RequiredFieldValidator runat="server" ID="rfvUserName"
                                            ErrorMessage="نام کاربری خالی است" ForeColor="Red"
                                    ControlToValidate="txtUserName"
                                    ValidationGroup="Register"></asp:RequiredFieldValidator>
                                    </label>
                                    <div class="col-sm-9 input-group m-bot15">
                                        <asp:TextBox runat="server" ID="txtUserName" CssClass="form-control input-group m-bot15"></asp:TextBox>
                                        <span class="input-group-addon"><i class="fa fa-user"></i></span>
                                    </div>
                                </div>
                                <div class="form-group">
                                    <label class="control-label col-sm-3">
                                        کلمه عبور
                                        <asp:RequiredFieldValidator runat="server" ID="rfvPassword"
                                            ErrorMessage="کلمه عبور خالی است" ForeColor="Red"
                                    ControlToValidate="txtPassword"
                                    ValidationGroup="Register"></asp:RequiredFieldValidator>
                                    </label>
                                    <div class="col-sm-9 input-group m-bot15">
                                        <asp:TextBox runat="server" ID="txtPassword" CssClass="form-control input-group m-bot15"></asp:TextBox>
                                        <span class="input-group-addon"><i class="fa fa-user-secret"></i></span>
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
                                    ValidationGroup="Register">*</asp:RequiredFieldValidator>
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
                            <span>دستورات مدیریتی</span>
                        </header>
                        <div class="panel-body">
                                <asp:Button runat="server" ID="btnCancel" Text="انصراف"
                                CssClass="btn btn-shadow btn-default" CausesValidation="False"
                                    OnClick="btnCancel_OnClick"/>

                            <asp:Button runat="server" ID="btnSubmitUser"
                                CssClass="btn btn-shadow btn-success"
                                Text="ثبت مشخصات" ValidationGroup="Register" OnClick="btnSubmitUser_OnClick"/>
                        </div>
                    </section>
                    <section class="panel">
                        <header class="panel-heading">
                            <span>گروه کاربری</span>
                        </header>
                        <div class="panel-body">
                            <div class="col-sm-12 input-group m-bot15">
                                <span class="input-group-addon"><i class="fa fa-group"></i></span>
                                <asp:DropDownList runat="server" ID="ddlRoles" ItemType="PortalCore.Models.Role"/>

                            </div>
                        </div>
                    </section>
                    <section class="panel">
                        <header class="panel-heading">
                            <span>انتخاب ها</span>
                        </header>
                        <div class="panel-body">
                            <%--<asp:CheckBox runat="server" CssClass="form-control" ID="chkSuperAdmin" Text="&nbsp;&nbsp; کاربر ارشد"/>
                            <br/>--%>
                            <asp:CheckBox runat="server" CssClass="form-control" Checked="True" ID="chkActive" Text="&nbsp;&nbsp; فعال؟"/>

                        </div>
                    </section>
                </div>
            </div>
            <div class="row">
                <div class="col-lg-12">
                    <section class="panel">
                        <header class="panel-heading">
                            <span>درباره کاربر</span>

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
        </asp:View>
    </asp:MultiView>
    <script>
        CKEDITOR.replace('<%= userinfo.ClientID %>', {
            language: 'fa',
            uiColor: '#9AB8F3',
            contentsLangDirection: 'rtl'
        });
    </script>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="cpJsFiles" runat="server">
</asp:Content>
