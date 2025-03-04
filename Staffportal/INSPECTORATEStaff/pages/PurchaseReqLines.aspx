<%@ Page Language="C#" Title="Purchase Requisition" MasterPageFile="~/pages/Main.Master" AutoEventWireup="true" CodeBehind="PurchaseReqLines.aspx.cs" Inherits="INSPECTORATEStaff.pages.PurchaseReqLines" %>

<asp:Content ID="Content1" ContentPlaceHolderID="Main1" runat="Server">
    <!-- Content Wrapper. Contains page content -->
    <div class="content-wrapper">
        <!-- Content Header (Page header) -->
        <section class="content-header">
            <h1>Purchase Requisition
       
                <%--<small>Version 2.0</small>--%>
            </h1>
            <ol class="breadcrumb">
                <li><a href="Dashboard.aspx"><i class="fa fa-dashboard"></i>Home</a></li>
                <li class="active">Purchase Requisition</li>
            </ol>
        </section>

        <!-- Main content -->
        <section class="content">
            <div class="row">
                <div class="col-md-12">
                    <asp:MultiView ID="MultiView1" runat="server">
                        <asp:View ID="View1" runat="server">
                            <div class="box box-success box-solid">
                                <div class="box-header with-border">
                                    <h3 class="box-title">New Purchase Requisition</h3>
                                </div>
                                <div class="box-body">
                                    <table class="table">
                                        <tbody>
                                            <tr>
                                                <th>No:</th>
                                                <td>
                                                    <asp:Label ID="lblNo" runat="server"></asp:Label>
                                                </td>
                                                <th>Main:</th>
                                                <td>
                                                    <asp:DropDownList ID="ddlBizCode" runat="server" CssClass="form-control select2"></asp:DropDownList>
                                                </td>
                                            </tr>
                                            <tr>
                                                <th>Department Code:</th>
                                                <td>
                                                    <asp:DropDownList ID="ddlDepart" runat="server" CssClass="form-control select2"></asp:DropDownList></td>
                                                <th>Responsibility Centre:</th>
                                                <td>
                                                    <asp:DropDownList ID="ddlRCentre" CssClass="form-control select2" runat="server"></asp:DropDownList></td>
                                            </tr>
                                            <tr>
                                                <th>Requestor or User Id:</th>
                                                <td>
                                                    <asp:Label ID="lbluserId" runat="server" Text="User Id" CssClass="text-info"></asp:Label></td>
                                                <th>Status:</th>
                                                <td>
                                                    <asp:Label ID="lblstatus" runat="server" Text="Status" CssClass="text-success"></asp:Label></td>
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
                                                <td>
                                                    <asp:Button ID="btnSubmit" class="btn btn-primary pull-right" runat="server" Text="Submit" OnClick="btnSubmit_Click" /></td>
                                            </tr>
                                        </tbody>
                                    </table>
                                </div>
                            </div>
                        </asp:View>
                        <asp:View ID="View2" runat="server">
                            <div class="panel panel-info">
                                <div class="panel-heading">
                                    <h3 class="panel-title">Purchase Lines -
                                        <asp:Label ID="lblLNo" runat="server"></asp:Label></h3>
                                </div>
                                <div class="panel-body">
                                    <div id="newLines" runat="server" visible="false">
                                        <div class="row">
                                            <asp:LinkButton ID="lbnClose" ToolTip="Close Lines" class="pull-right text-danger" runat="server" OnClick="lbnClose_Click"><i class="fa fa-minus-circle"></i> Close lines</asp:LinkButton>
                                        </div>
                                        <div class="row">
                                            <div class="col-md-2">
                                                <div class="form-group">
                                                    <label for="exampleInputEmail1">Type</label>
                                                    <asp:DropDownList ID="ddlType" runat="server" CssClass="form-control select2" OnSelectedIndexChanged="ddlType_SelectedIndexChanged" AutoPostBack="true">
                                                        <asp:ListItem Value="" Text="--Select--"></asp:ListItem>
                                                        <asp:ListItem Value="0" Text=" "></asp:ListItem>
                                                        <asp:ListItem Value="1" Text="G/L Account"></asp:ListItem>
                                                        <asp:ListItem Value="2" Text="Item"></asp:ListItem>
                                                        <asp:ListItem Value="3" Text="Fixed Asset"></asp:ListItem>
                                                        <asp:ListItem Value="4" Text="Charge(Item)"></asp:ListItem>
                                                    </asp:DropDownList>
                                                </div>
                                            </div>
                                            <div class="col-md-4">
                                                <div class="form-group">
                                                    <label for="exampleInputPassword1">Functions</label>
                                                    <asp:DropDownList ID="ddlItems" CssClass="form-control select2" runat="server" ></asp:DropDownList>
                                                </div>
                                            </div>
                                            <div class="col-md-2">
                                                <div class="form-group">
                                                    <label for="exampleInputFile">Location Code:</label>
                                                    <asp:DropDownList ID="ddlLoc" CssClass="form-control select2" runat="server"></asp:DropDownList>
                                                </div>
                                            </div>
                                            <div class="col-md-2">
                                                <div class="form-group">
                                                    <label for="exampleInputFile">Unit Of Measure:</label>
                                                    <asp:DropDownList ID="ddlUnits" CssClass="form-control select2" runat="server"></asp:DropDownList>
                                                </div>
                                            </div>
                                            <div class="col-md-2">
                                                <div class="form-group">
                                                    <label for="exampleInputFile">Quantity:</label>
                                                    <asp:TextBox ID="txtQty" CssClass="form-control" runat="server"></asp:TextBox>
                                                </div>
                                            </div>
                                            <div class="box-footer">
                                                <button id="btnLinez" runat="server" onserverclick="btnLine_Click" type="submit" class="btn btn-primary pull-right">Add New Line</button>
                                            </div>
                                        </div>
                                    </div>
                                    <asp:LinkButton ID="lbnAddLine" ToolTip="Add New Lines" class="pull-right text-info" runat="server" OnClick="lbnAddLine_Click"><i class="fa fa-plus-circle "></i> Add New item</asp:LinkButton>
                                    <asp:GridView ID="gvLines" AutoGenerateColumns="false" DataKeyNames="Document No_" class="table table-responsive no-padding table-bordered table-hover" runat="server"
                                        AllowSorting="True" AllowPaging="true" ShowFooter="true" PageSize="15">
                                         <Columns>
                                            <asp:TemplateField HeaderStyle-HorizontalAlign="Left" HeaderText="#No" SortExpression="">
                                                <HeaderStyle Width="30px" />
                                                <ItemTemplate>
                                                    <%# string.Format("{0}",Container.DataItemIndex + 1 +".") %>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:BoundField DataField="Document No_" HeaderText="Number" />
                                            <asp:BoundField DataField="Description" HeaderText="Description"/>
                                            <asp:BoundField DataField="Unit of Measure" HeaderText="Unit of Measure"/>
                                            <asp:TemplateField HeaderText="Quantity Requested">
                                                <ItemTemplate>
                                                    <%# string.Format("{0:#,##0.00}", ((decimal)Eval("[Quantity]")))%>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                             <asp:TemplateField HeaderText="Action" SortExpression="" HeaderStyle-HorizontalAlign="Left">
                                                <ItemStyle Width="110px" HorizontalAlign="Left" />
                                                <ItemTemplate>
                                                     <asp:LinkButton ID="lbtnCancll" CssClass="label label-danger" runat="server" ToolTip="Click to Remove line" OnClick="cancelz" CommandArgument='<%# Eval("Line No_") %>'><i class="fa fa-remove"></i> Remove</asp:LinkButton>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                        </Columns>
                                        <FooterStyle HorizontalAlign="Center" />
                                        <EmptyDataTemplate>
                                            <span style="color: red">No Records</span>
                                        </EmptyDataTemplate>
                                    </asp:GridView>
                                    <table class="table table-hover">
                                        <thead>
                                            <tr>
                                                <th></th>
                                                <th></th>
                                                <th></th>
                                                <th></th>
                                                <th>&nbsp;</th>
                                                <th>
                                                    <asp:Button ID="btnApproval" CssClass="btn btn-success pull-right" runat="server" Text="Send For Approval" OnClick="btnApproval_Click" />&nbsp;
                                        <asp:Button ID="btncancel" runat="server" CssClass="btn btn-danger pull-right" Text="Cancel Approval Request" OnClick="btncancel_Click" />
                                                </th>
                                            </tr>
                                        </thead>
                                        <tbody>
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


