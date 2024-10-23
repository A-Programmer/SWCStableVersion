<%@ Page Title="مدیریت صفحات" Language="C#" MasterPageFile="~/Admin/AdminMasterPage.Master" AutoEventWireup="true" CodeBehind="ManagePages.aspx.cs" Inherits="PortalCore.Admin.ManagePages" %>
<asp:Content ID="Content1" ContentPlaceHolderID="CpHeader" runat="server">
    <style>
        .tvPermissions input[type="checkbox"] {
            margin-left: 5px;
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
        پیام های سیستمی : <asp:Literal runat="server" ID="ltPagesMessage"></asp:Literal>
    </div>
    <asp:MultiView runat="server" ID="mvPagesAdmin" ActiveViewIndex="0">
        <asp:View runat="server" ID="vwPagesList">
            <section class="panel">
                <header class="panel-heading">
                    <asp:Button runat="server" ID="btnAddCorePage" Text="افزودن صفحه هسته"
                        CausesValidation="False" CssClass="btn btn-shadow btn-success" 
                        OnClick="btnAddCorePage_OnClick"/>
                    &nbsp;&nbsp;
                    <asp:Button runat="server" ID="btnAddModulePage" Text="افزودن صفحه ماژول"
                        CausesValidation="False" CssClass="btn btn-shadow btn-success" 
                        OnClick="btnAddModulePage_OnClick"/>
                </header>
                <div class="panel-body">
                    <asp:GridView runat="server" ID="grdPages" AutoGenerateColumns="False" GridLines="None"
                        ItemType="PortalCore.Models.Page" PageSize="20" AllowPaging="True"
                    CssClass="table table-striped table-advance table-hover table-bordered table-responsive"
                        OnPageIndexChanging="grdPages_OnPageIndexChanging" OnRowCommand="grdPages_OnRowCommand">
                        <Columns>
                            <asp:BoundField DataField="Name" HeaderText="نام لاتین"/>
                            <asp:BoundField DataField="Title" HeaderText="عنوان"/>
                            <asp:TemplateField HeaderText="نوع صفحه">
                                <ItemTemplate>
                                    <%# GetPageType(Convert.ToInt32(Eval("ModuleId"))) %>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="دستورات">
                                <ItemStyle Width="40%"></ItemStyle>
                                <ItemTemplate>
                                    <div class="btn-group btn-group-justified">
                                        <asp:LinkButton runat="server" ID="lbEdit"
                                            CommandArgument='<%#Eval("PageId") %>'
                                            CommandName="DoEdit" CssClass="btn btn-block btn-shadow btn-warning"
                                            ToolTip="ویرایش"><i class="fa fa-edit"></i></asp:LinkButton>

                                        <asp:LinkButton runat="server" ID="lbEditLayout"
                                            CommandArgument='<%#Eval("PageId") %>'
                                            CommandName="DoEditLayout" CssClass="btn btn-block btn-shadow btn-info"
                                            ToolTip="ویرایش چینش"><i class="fa fa-pencil"></i></asp:LinkButton>

                                        <asp:LinkButton runat="server" ID="lbDelete"
                                            CommandArgument='<%#Eval("PageId") %>'
                                            CommandName="DoDelete" CssClass="btn btn-block btn-shadow btn-danger"
                                            ToolTip="حذف"><i class="fa fa-remove"></i></asp:LinkButton>
                                        </div>
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                        <EmptyDataTemplate>
                            صفحه ای موجود نیست.
                        </EmptyDataTemplate>
                    </asp:GridView>
                </div>
            </section>
        </asp:View>
        
        <asp:View runat="server" ID="vwEditPage">
            <div class="col-sm-8">
                <section class="panel">
                    <header class="panel-heading">
                        <span>مشخصات صفحه</span>
                    </header>
                    <div class="panel-body">
                        <div class="form-horizontal tasi-form">
                            <div class="form-group" runat="server" id="fgModules">
                                <label class="control-label col-sm-3">
                                    ماژول مربوطه
                                </label>
                                <div class="col-sm-9 input-group m-bot15">
                                    <asp:DropDownList runat="server" ID="ddlModules"
                                        CssClass="form-control input-group m-bot15"/>
                                </div>
                            </div>
                            <div class="form-group" runat="server" id="fgPageName">
                                <label class="control-label col-sm-3">
                                    نام لاتین صفحه
                                    <asp:RequiredFieldValidator runat="server" ID="rfvPageName"
                                            ErrorMessage="نام لاتین صفحه خالی است" ForeColor="Red"
                                    ControlToValidate="txtPageName"
                                    ValidationGroup="Pages"></asp:RequiredFieldValidator>
                                </label>
                                <div class="col-sm-9 input-group m-bot15">
                                    <asp:TextBox runat="server" ID="txtPageName"
                                        CssClass="form-control input-group m-bot15"></asp:TextBox>
                                    <span class="input-group-addon"><i class="fa fa-tag"></i></span>
                                </div>
                            </div>
                            <div class="form-group" runat="server" id="fgPageTitle">
                                <label class="control-label col-sm-3">
                                    عنوان فارسی صفحه
                                    <asp:RequiredFieldValidator runat="server" ID="rfvPageTitle"
                                            ErrorMessage="نام لاتین صفحه خالی است" ForeColor="Red"
                                    ControlToValidate="txtPageTitle"
                                    ValidationGroup="Pages"></asp:RequiredFieldValidator>
                                </label>
                                <div class="col-sm-9 input-group m-bot15">
                                    <asp:TextBox runat="server" ID="txtPageTitle"
                                        CssClass="form-control input-group m-bot15"></asp:TextBox>
                                    <span class="input-group-addon"><i class="fa fa-tag"></i></span>
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

                        <asp:Button runat="server" ID="btnSubmitPage"
                            CssClass="btn btn-shadow btn-success"
                            Text="ثبت مشخصات" ValidationGroup="Pages"
                            OnClick="btnSubmitPage_OnClick"/>

                    </div>
                </section>
            </div>
        </asp:View>
        
        <asp:View runat="server" ID="vwPageDesigner">
            
         <div class="row">
            <div class="col-sm-5">
                <section class="panel">
                    <section class="panel-heading">
                        انتخاب بلاک
                    </section>
                    <div class="panel-body">
                        <asp:TreeView runat="server" ID="tvModules" NodeIndent="15" ShowCheckBoxes="Leaf"
                            CssClass="tvPermissions">
                            <NodeStyle Font-Names="Tahoma" Font-Size="8pt" ForeColor="Black" HorizontalPadding="2px"
                                NodeSpacing="0px" VerticalPadding="2px"></NodeStyle>
                            <ParentNodeStyle Font-Bold="False" />
                            <NodeStyle CssClass="draggable"></NodeStyle>
                        </asp:TreeView>
                    </div>
                </section>
                        
            </div>
            <div class="col-sm-4">
                    <section class="panel">
                        <section class="panel-heading">
                            انتخاب منطقه نمایش
                        </section>
                        <div class="panel-body">
                            <asp:CheckBoxList runat="server" ID="chkZones" CssClass="tvPermissions"/>
                        </div>
                    </section>
                </div>
            <div class="col-sm-3">
                <section class="panel">
                    <section class="panel-heading">
                        دستورات
                    </section>
                    <div class="panel-body">
                        <asp:Button runat="server" ID="btnCancelPageLayout" Text="انصراف"
                            CssClass="btn btn-shadow btn-default" CausesValidation="False"
                                OnClick="btnCancel_OnClick"/>

                        <asp:Button runat="server" ID="btnSubmitPageLayout"
                            CssClass="btn btn-shadow btn-success"
                            Text="ثبت چینش" ValidationGroup="Pages"
                            OnClick="btnSubmitPageLayout_OnClick"/>
                    </div>
                </section>   
            </div>
         </div>

        <div class="row">
            <div class="col-sm-12">
                <asp:GridView runat="server" ID="grdPageLayouts" AutoGenerateColumns="False" GridLines="None"
                    PageSize="20" AllowPaging="True"
                    CssClass="table table-striped table-advance table-hover table-bordered table-responsive"
                    OnPageIndexChanging="grdPageLayouts_OnPageIndexChanging" OnRowCommand="grdPageLayouts_OnRowCommand">
                <Columns>
                    <asp:TemplateField HeaderText="منطقه">
                        <ItemTemplate>
                            <%# GetZoneTitle(Eval("ZoneName").ToString()) %>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:BoundField DataField="Title" HeaderText="بلاک"/>
                    <asp:TemplateField HeaderText="ترتیب نمایش">
                        <HeaderTemplate>
                            ترتیب نمایش
                            <asp:LinkButton runat="server" ID="lbSubmitAppearanceOrer"
                                CssClass="btn btn-shadow btn-success" CommandName="SubmitAppearanceOrder"
                                >
                                ثبت ترتیب نمایش &nbsp;&nbsp;
                                <i class="fa fa-sort-alpha-asc"></i>
                            </asp:LinkButton>
                        </HeaderTemplate>
                        <ItemTemplate>
                            <div class="input-group spinner">
                                <asp:TextBox runat="server" ID="txtAppearanceOrder"
                                CssClass="form-control" Text='<%#Eval("AppearanceOrder").ToString() %>'></asp:TextBox>
                                <div class="input-group-btn-vertical">
                                  <button class="btn btn-default" type="button"><i class="fa fa-caret-up"></i></button>
                                  <button class="btn btn-default" type="button"><i class="fa fa-caret-down"></i></button>
                                </div>
                              </div>
                            
                            <div style="display: none;">
                                <asp:Literal runat="server" ID="lblPageLayoutId" Text='<%# Eval("PageLayoutId") %>'>
                                </asp:Literal>
                            </div>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="دستورات">
                        <ItemTemplate>
                            <asp:LinkButton runat="server" ID="lbDelete"
                                CommandArgument='<%#Eval("PageLayoutId") %>'
                                CommandName="DoDelete" CssClass="btn btn-block btn-shadow btn-danger"
                                ToolTip="حذف"><i class="fa fa-remove"></i></asp:LinkButton>
                        </ItemTemplate>
                    </asp:TemplateField>
                </Columns>
                </asp:GridView>
            </div>
        </div>
        </asp:View>
    </asp:MultiView>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="cpJsFiles" runat="server">
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
