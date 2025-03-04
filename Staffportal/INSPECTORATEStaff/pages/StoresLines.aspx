<%@ Page Language="C#" Title="Stores Requisition" MasterPageFile="~/pages/Main.Master" AutoEventWireup="true" CodeBehind="StoresLines.aspx.cs" Inherits="INSPECTORATEStaff.pages.StoresLines" %>

<asp:Content ID="Content1" ContentPlaceHolderID="Main1" runat="Server">
    <!-- Content Wrapper. Contains page content -->
    <div class="content-wrapper">
        <!-- Content Header (Page header) -->
        <section class="content-header">
            <h1>Stores Requisition
       
                <%--<small>Version 2.0</small>--%>
            </h1>
            <ol class="breadcrumb">
                <li><a href="Dashboard.aspx"><i class="fa fa-dashboard"></i>Home</a></li>
                <li class="active">Stores Requisition</li>

            </ol>
        </section>

        <!-- Main content -->
        <section class="content">
            <div class="row">
                <div class="col-md-12">
                    <asp:MultiView ID="MultiView1" runat="server">
                        <asp:View ID="View1" runat="server">
                            <div class="box box-warning box-solid">
                                <div class="box-header with-border">
                                    <h3 class="box-title">New Stores Requisition</h3>
                                </div>
                                <div class="box-body">
                                    <table class="table">
                                        <tbody>
                                            <tr>
                                                <th>No:</th>
                                                <td>
                                                    <asp:Label ID="lblNo" runat="server"></asp:Label>
                                                </td>
                                                <th>Required Date:</th>
                                                <td>
                                                    <asp:TextBox ID="dtRequiredDate" CssClass="form-control" runat="server" Width="350px"></asp:TextBox>
                                                    <script>
                                                        $j('#Main1_dtRequiredDate').Zebra_DatePicker({
                                                            // remember that the way you write down dates
                                                            // depends on the value of the "format" property!
                                                           // direction: [1, false],
                                                            //disabled_dates: ['* * * 0,6'] 
                                                        });</script>
                                                </td>
                                            </tr>
                                            <tr>
                                                
                                                <%--<th>Requisition Type</th>
                                                <td>
                                                    <asp:DropDownList ID="ddlRType" runat="server" CssClass="form-control select2">
                                                        <asp:ListItem Value="0">Stationery</asp:ListItem>
                                                        <asp:ListItem Value="1">Others</asp:ListItem>
                                                        <asp:ListItem Value="2">Food-Stuff</asp:ListItem>
                                                        <asp:ListItem Value="3">Grocery</asp:ListItem>
                                                        <asp:ListItem Value="4">Cereals</asp:ListItem>
                                                        <asp:ListItem Value="5">Hardware Materials</asp:ListItem>
                                                        <asp:ListItem Value="6">Building Materials</asp:ListItem>
                                                        <asp:ListItem Value="7">Drugs</asp:ListItem>
                                                        <asp:ListItem Value="8">Non-Pharmaceuticals</asp:ListItem>
                                                        <asp:ListItem Value="9">Teaching Materials</asp:ListItem>
                                                        <asp:ListItem Value="10">Minor Assets</asp:ListItem>
                                                        <asp:ListItem Value="11">Assets</asp:ListItem>
                                                    </asp:DropDownList></td>--%>
                                                <th>Status:</th>
                                                <td>
                                                    <asp:Label ID="lblstatus" runat="server" Text="Status" CssClass="text-success"></asp:Label></td>
                                  0             
                                            </tr>
                                            <tr>
                                                <th>Responsibility Centre:</th>
                                                <td>
                                                    <asp:DropDownList ID="ddlresponibilitycentres" runat="server" CssClass="form-control select2"></asp:DropDownList>
                                                </td>
                                                <th>Requestor or User Id:</th>
                                                <td>
                                                    <asp:Label ID="lbluserId" runat="server" Text="User Id" CssClass="text-info"></asp:Label></td>
                                            </tr>
                                            <tr>
                                                <%--<th>Main: </th>
                                                <td>
                                                    <asp:DropDownList ID="ddlBranch" runat="server" CssClass="form-control select2"></asp:DropDownList>
                                                </td>--%>
                                                <th>Department Code:</th>
                                                <td>
                                                    <asp:DropDownList ID="ddlregions" runat="server" CssClass="form-control select2"></asp:DropDownList>
                                                </td>
                                            </tr>
                                            <tr>
                                                <th>Request Description: </th>
                                                <td colspan="3">
                                                    <asp:TextBox ID="txtHdescription" runat="server" CssClass="form-control" TextMode="MultiLine"></asp:TextBox></td>
                                                <th></th>
                                                <td></td>
                                            </tr>
                                            <tr>
                                                <th></th>
                                                <td></td>
                                                <td></td>
                                                <div class="row">
                                 <div class="col-sm-12">
                              <div class="form-group">


























                                           Attach Document:
                              <asp:FileUpload ID="FileUpload1" runat="server" />
                              </div>
                            </div>
                                  </div>
                                                <td>
                                                    <asp:Button ID="btnSubmit" class="btn btn-primary pull-right" runat="server" Text="Send for Approval" OnClick="btnSubmit_Click" /></td>&nbsp;
                                            </tr>
                                        </tbody>
                                    </table>
                                </div>
                            </div>
                        </asp:View>
                    </asp:MultiView>
                </div>
            </div>
        </section>
    </div>
</asp:Content>

