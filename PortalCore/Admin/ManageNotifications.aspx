<%@ Page Title="مدیریت اعلان ها" Language="C#" MasterPageFile="~/Admin/AdminMasterPage.Master" AutoEventWireup="true" CodeBehind="ManageNotifications.aspx.cs" Inherits="PortalCore.Admin.ManageNotifications" %>
<asp:Content ID="Content1" ContentPlaceHolderID="CpHeader" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="CpMain" runat="server">
    <div class="alert alert-info">
        پیام های سیستمی : <asp:Literal runat="server" ID="ltNotificationMessages"></asp:Literal>
    </div>
    <div class="col-sm-12">
        <section class="panel">
            <section class="panel-heading">
                لیست تمام اعلان های شما
            </section>
            <div class="panel-body">
                <asp:GridView runat="server" ID="grdNotifications" AutoGenerateColumns="False" GridLines="None"
                    ItemType="PortalCore.Models.Notification" PageSize="20" AllowPaging="True"
                    CssClass="table table-striped table-advance table-hover table-bordered table-responsive"
                    OnPageIndexChanging="grdNotifications_OnPageIndexChanging"
                    OnRowCommand="grdNotifications_OnRowCommand">
                    <Columns>
                        <asp:TemplateField HeaderText="عنوان">
                            <ItemTemplate>
                                <a href="javascript:;" data-toggle="modal"
                                    style='font-weight: <%# GetFontWeightStyle(Convert.ToInt32(Eval("Id")))%>;'
                                    data-target="#ManageNotification-<%#Eval("Id") %>">
                                    <%#Eval("Title") %>
                                </a>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:CheckBoxField DataField="Status" HeaderText="وضعیت"/>
                        <asp:TemplateField HeaderText="تاریخ">
                            <ItemTemplate>
                                <%# Persia.Calendar.ConvertToPersian(Convert.ToDateTime(Eval("SentDate"))).ToRelativeDateString("p,4,60,22") %>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="دستورات">
                            <ItemStyle Width="40%"></ItemStyle>
                            <ItemTemplate>
                                <asp:LinkButton runat="server" ID="lbChangeStatus"
                                     CssClass="btn btn-shadow btn-warning"
                                    CommandArgument='<%#Eval("Id") %>' CommandName="ChangeStatus">
                                    تغییر وضعیت
                                </asp:LinkButton>
                                &nbsp;
                                <asp:LinkButton runat="server" ID="lbDelete" 
                                    CssClass="btn btn-shadow btn-danger"
                                    CommandArgument='<%#Eval("Id") %>' CommandName="DoDelete">
                                    حذف
                                </asp:LinkButton>
                                <div id="ManageNotification-<%#Eval("Id") %>" class="modal fade" role="dialog">
                                    <div class="modal-dialog">
                                        <!-- Modal content-->
                                        <div class="modal-content">
                                            <div class="modal-body">
                                                <div class="row">
                                                    <div class="panel">
                                                        <header class="panel-heading">
                                                            <%#Eval("Title") %>
                                                        </header>
                                                        <div class="panel-body">
                                                            <%#Eval("Message") %>
                                                        </div>
                                                    </div>
                                                </div>
                                            </div>
                                            <div class="modal-footer">
                                                <div class="pull-right">
                                                    <%# Persia.Calendar.ConvertToPersian(Convert.ToDateTime(Eval("SentDate"))).ToRelativeDateString("p,4,60,22") %>
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
    </div>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="cpJsFiles" runat="server">
</asp:Content>
