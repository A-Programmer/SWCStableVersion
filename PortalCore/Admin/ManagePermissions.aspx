<%@ Page Title="مدیریت دسترسی ها" Language="C#" MasterPageFile="~/Admin/AdminMasterPage.Master" AutoEventWireup="true" CodeBehind="ManagePermissions.aspx.cs" Inherits="PortalCore.Admin.ManagePermissions" %>
<asp:Content ID="Content1" ContentPlaceHolderID="CpHeader" runat="server">
    <style>
        .tvPermissions input[type="checkbox"] {
            margin-left: 5px;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="CpMain" runat="server">
    
    <asp:MultiView runat="server" ID="mvPremissions" ActiveViewIndex="0">
        <asp:View runat="server" ID="vwRoles">
            <div class="alert alert-danger">
                برای ویرایش و یا نمایش دسترسی ها روی ویرایش کلیک کنید.
            </div>
                <section class="panel">
                    <header class="panel-heading">
                        
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
                                <asp:TemplateField HeaderText="ویرایش دسترسی ها">
                                    <ItemStyle Width="15%"></ItemStyle>
                                    <ItemTemplate>
                                        <div class="btn-group btn-group-justified">
                                                <asp:LinkButton runat="server" ID="lbEdit"
                                                    CommandArgument='<%#Eval("RoleId") %>'
                                                    CommandName="DoEdit" CssClass="btn btn-block btn-shadow btn-info"
                                                    Visible='<%# GetAccess("EditRolePermissions") %>'
                                                    ToolTip="ویرایش"><i class="fa fa-edit"></i>&nbsp;&nbsp;ویرایش</asp:LinkButton>
                                            </div>
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                        </asp:GridView>
                    </div>
                </section>
        </asp:View>
        <asp:View runat="server" ID="vwEdirPermissions">
            <div class="col-sm-8">
                <section class="panel">
                    <header class="panel-heading">
                        دسترسی های موجود
                    </header>
                    <div class="panel-body">
                        <asp:TreeView runat="server" ID="tvPermissionGroups" ShowCheckBoxes="Leaf"
                            CssClass="tvPermissions">
                        </asp:TreeView>
                    </div>
                </section>
            </div>
            <div class="col-sm-4">
                <section class="panel">
                    <header class="panel-heading">
                        دستورات مدیریتی
                        <asp:Literal runat="server" ID="ltSubmitPermissionMessage"></asp:Literal>
                    </header>
                    <div class="panel-body">
                        <asp:Button runat="server" ID="btnCancel" Text="انصراف"
                                CssClass="btn btn-shadow btn-default" CausesValidation="False"
                                    OnClick="btnCancel_OnClick"/>

                            <asp:Button runat="server" ID="btnSubmitPermissions"
                                CssClass="btn btn-shadow btn-success"
                                Text="ثبت دسترسی ها"
                                OnClick="btnSubmitPermissions_OnClick"/>
                    </div>
                </section>
            </div>
        </asp:View>
    </asp:MultiView>
    

    
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="cpJsFiles" runat="server">
</asp:Content>
