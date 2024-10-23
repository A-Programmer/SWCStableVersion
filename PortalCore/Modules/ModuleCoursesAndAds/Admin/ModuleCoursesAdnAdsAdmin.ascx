<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ModuleCoursesAdnAdsAdmin.ascx.cs" Inherits="Module_CoursesAndAds.Modules.ModuleCoursesAndAds.Admin.ModuleCoursesAdnAdsAdmin" %>
<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<%@ Register TagPrefix="CKEditor" Namespace="CKEditor.NET" Assembly="CKEditor.NET, Version=3.6.4.0, Culture=neutral, PublicKeyToken=e379cdf2f8354999" %>

<asp:MultiView runat="server" ID="mvCommers" ActiveViewIndex="0">

    <asp:View runat="server" ID="veCommersList">
        <telerik:RadGrid ID="grdCommersList" runat="server" 
            AllowFilteringByColumn="True" AllowPaging="True" AllowSorting="True" 
            AutoGenerateColumns="False" CellSpacing="0" GridLines="None" 
            ShowGroupPanel="True" Skin="Outlook" Culture="fa-IR" 
            onitemcommand="grdCommersList_ItemCommand">
            <ClientSettings AllowDragToGroup="True">
            </ClientSettings>
            <MasterTableView CommandItemDisplay="TopAndBottom" Dir="RTL">
                <CommandItemSettings ExportToPdfText="Export to PDF" />
                <RowIndicatorColumn FilterControlAltText="Filter RowIndicator column">
                    <HeaderStyle Width="20px" />
                </RowIndicatorColumn>
                <ExpandCollapseColumn FilterControlAltText="Filter ExpandColumn column">
                    <HeaderStyle Width="20px" />
                </ExpandCollapseColumn>
                <Columns>
                    <telerik:GridBoundColumn DataField="CmTitle" 
                        FilterControlAltText="Filter CmTitle column" HeaderText="عنوان" 
                        SortExpression="CmTitle" UniqueName="CmTitle">
                    </telerik:GridBoundColumn>
                    <telerik:GridBoundColumn AllowFiltering="False" AllowSorting="False" 
                        DataField="CMSendDate" FilterControlAltText="Filter CMSendDate column" 
                        HeaderText="تاریخ ارسال" ShowSortIcon="False" SortExpression="CMSendDate" 
                        UniqueName="CMSendDate">
                    </telerik:GridBoundColumn>
                    <telerik:GridTemplateColumn AllowFiltering="False" 
                        FilterControlAltText="Filter Commands column" HeaderText="دستورات" 
                        UniqueName="Commands">
                        <ItemTemplate>
                            <asp:LinkButton ID="lbEdit" CommandName="DoEdit" CommandArgument='<%#Eval("CMID") %>' CausesValidation="false" Visible='<%# PortalCommon.PermissionControl.CheckPermission("EditCommers") %>' runat="server">ویرایش</asp:LinkButton>
                     &nbsp;<asp:LinkButton ID="lbDelete" CommandName="DoDelete" CommandArgument='<%#Eval("CMID") %>' CausesValidation="false" Visible='<%# PortalCommon.PermissionControl.CheckPermission("DeleteCommers") %>' runat="server">حذف</asp:LinkButton>
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>
                </Columns>
                <EditFormSettings>
                    <EditColumn FilterControlAltText="Filter EditCommandColumn column">
                    </EditColumn>
                </EditFormSettings>
            </MasterTableView>
            <FilterMenu EnableImageSprites="False">
            </FilterMenu>
            <HeaderContextMenu CssClass="GridContextMenu GridContextMenu_Default">
            </HeaderContextMenu>
        </telerik:RadGrid>
    </asp:View>

    <asp:View runat="server" ID="veCommersEdit">
        <table>
            <tr>
                <td>عنوان</td>
                <td>
                    <asp:TextBox runat="server" ID="txtTitle"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td>متن کوتاه</td>
                <td>
                <asp:TextBox runat="server" ID="txtDes" Height="98px" TextMode="MultiLine" 
                        Width="227px"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td>توضیحات</td>
                <td>
                    <CKEditor:CKEditorControl ID="txtContent" BasePath="../../../ckeditor/" runat="server" Skin="kama" Toolbar="Full"></CKEditor:CKEditorControl>
                </td>
            </tr>
            <tr>
                <td>نوع</td>
                <td>
                    <asp:DropDownList runat="server" ID="ddlKinds">
                    
                        <asp:ListItem Value="1">تبلیغات</asp:ListItem>
                        <asp:ListItem Value="2">دوره ها</asp:ListItem>
                        <asp:ListItem Value="3">فروشگاه</asp:ListItem>
                    
                    </asp:DropDownList>
                </td>
            </tr>
            <tr>
                <td>روز - ماه</td>
                <td>
                    <asp:TextBox runat="server" ID="txtDay" Width="70px"></asp:TextBox> - 
                    <asp:TextBox runat="server" ID="txtMonth" Width="70px"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td>قیمت</td>
                <td>
                    <asp:TextBox runat="server" ID="txtPrice" Width="70px"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td>برچسب ها</td>
                <td>
                    <asp:TextBox runat="server" ID="txtTags"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td></td>
                <td>
                    <asp:Button runat="server" ID="btnSubmitChanges" Text="ثبت تغییرات" 
                        onclick="btnSubmitChanges_Click"/>
                    <asp:Button runat="server" ID="btnCancel" Text="انصراف" 
                        onclick="btnCancel_Click"/>
                </td>
            </tr>
        </table>
    </asp:View>

</asp:MultiView>