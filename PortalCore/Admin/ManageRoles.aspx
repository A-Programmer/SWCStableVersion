<%@ Page Title="مدیریت گروه ها" Language="C#" MasterPageFile="~/Admin/AdminMasterPage.Master" AutoEventWireup="true" CodeBehind="ManageRoles.aspx.cs" Inherits="PortalCore.Admin.ManageRoles" %>
<asp:Content ID="Content1" ContentPlaceHolderID="CpHeader" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="CpMain" runat="server">
    <asp:MultiView runat="server" ID="mvRoles" ActiveViewIndex="0">
        <asp:View runat="server" ID="vwRoles">
            <div class="alert alert-danger">
                توجه داشته باشید با حذف گروه کاربری، تمامی دسترسی های گروه حذف خواهد شد و کاربران آن به گروه پیش فرض منتقل میشوند.
            </div>
                <section class="panel">
                    <header class="panel-heading">
                        <asp:Button runat="server" ID="btnAdd" Text="افزودن گروه کاربری جدید"
                            CausesValidation="False" CssClass="btn btn-shadow btn-success" 
                            OnClick="btnAdd_OnClick"/>
                        <asp:Literal runat="server" ID="ltRolesMessage"></asp:Literal>
                    </header>
                    <div class="panel-body">
                        <asp:GridView runat="server" ID="grdRoles" AutoGenerateColumns="False" GridLines="None"
                            ItemType="PortalCore.Models.Role" PageSize="20" AllowPaging="True"
                        CssClass="table table-striped table-advance table-hover table-bordered table-responsive"
                            OnPageIndexChanging="grdRoles_OnPageIndexChanging"
                            OnRowCommand="grdRoles_OnRowCommand">
                            <Columns>
                                <asp:BoundField DataField="RoleTitle" HeaderText="نام لاتین"/>
                                <asp:BoundField DataField="RoleName" HeaderText="نام فارسی"/>
                                <asp:TemplateField HeaderText="دستورات">
                                    <ItemStyle Width="15%"></ItemStyle>
                                    <ItemTemplate>
                                        <div class="btn-group btn-group-justified">
                                                <asp:LinkButton runat="server" ID="lbEdit"
                                                    CommandArgument='<%#Eval("RoleId") %>'
                                                    CommandName="DoEdit" CssClass="btn btn-block btn-shadow btn-info"
                                                    ToolTip="ویرایش"><i class="fa fa-edit"></i></asp:LinkButton>

                                                <asp:LinkButton runat="server" ID="lbDelete"
                                                    CommandArgument='<%#Eval("RoleId") %>'
                                                    CommandName="DoDelete" CssClass="btn btn-block btn-shadow btn-danger"
                                                    Visible='<%# !IsDefaultRole(Convert.ToInt32(Eval("RoleId"))) %>'
                                                    ToolTip="حذف"><i class="fa fa-remove"></i></asp:LinkButton>
                                            </div>
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                        </asp:GridView>
                    </div>
                </section>
        </asp:View>
        <asp:View runat="server" ID="vwEditRole">
            <div class="row">
                <div class="col-sm-8">
                    <section class="panel">
                        <header class="panel-heading">
                            <span>اطلاعات گروه</span>
                            &nbsp;&nbsp;&nbsp;
                            <asp:Literal runat="server" ID="ltRoleMessage"></asp:Literal>
                        </header>
                        <div class="panel-body">
                            <div class="form-horizontal tasi-form">
                                <div class="form-group">
                                    <label class="control-label col-sm-3">
                                        نام لاتین
                                        <asp:RequiredFieldValidator runat="server" ID="rfvRoleTitle"
                                            ErrorMessage="نام لاتین گروه خالی است" ForeColor="Red"
                                    ControlToValidate="txtRoleTitle"
                                    ValidationGroup="Roles"></asp:RequiredFieldValidator>
                                    </label>
                                    <div class="col-sm-9 input-group m-bot15">
                                        <asp:TextBox runat="server" ID="txtRoleTitle"
                                            CssClass="form-control input-group m-bot15"></asp:TextBox>
                                        <span class="input-group-addon"><i class="fa fa-users"></i></span>
                                    </div>
                                </div>
                                <div class="form-group">
                                    <label class="control-label col-sm-3">
                                        نام فارسی
                                        <asp:RequiredFieldValidator runat="server" ID="rfvRoleName"
                                            ErrorMessage="نام فارسی گروه خالی است" ForeColor="Red"
                                    ControlToValidate="txtRoleName"
                                    ValidationGroup="Roles"></asp:RequiredFieldValidator>
                                    </label>
                                    <div class="col-sm-9 input-group m-bot15">
                                        <asp:TextBox runat="server" ID="txtRoleName"
                                            CssClass="form-control input-group m-bot15"></asp:TextBox>
                                        <span class="input-group-addon"><i class="fa fa-users"></i></span>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </section>
                </div>
                <div class="col-sm-4">
                    <section class="panel">
                        <header class="panel-heading">
                            <span>دستورات مدیریتی</span>
                        </header>
                        <div class="panel-body">
                                <asp:Button runat="server" ID="btnCancel" Text="انصراف"
                                CssClass="btn btn-shadow btn-default" CausesValidation="False"
                                    OnClick="btnCancel_OnClick"/>

                            <asp:Button runat="server" ID="btnSubmitRole"
                                CssClass="btn btn-shadow btn-success"
                                Text="ثبت گروه کاربری" ValidationGroup="Roles"
                                OnClick="btnSubmitRole_OnClick"/>

                        </div>
                    </section>
                </div>
            </div>
        </asp:View>
    </asp:MultiView>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="cpJsFiles" runat="server">
</asp:Content>
