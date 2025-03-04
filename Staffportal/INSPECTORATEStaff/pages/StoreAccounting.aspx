<%@ Page Language="C#" Title="Store Accounting" MasterPageFile="~/pages/Main.Master" AutoEventWireup="true" CodeBehind="StoreAccounting.aspx.cs" Inherits="INSPECTORATEStaff.pages.StoreqAccounting" %>

<asp:Content ID="Content1" ContentPlaceHolderID="Main1" runat="Server">
    <div class="content-wrapper">
    <div class="panel panel-info">
        <div class="panel-heading">
            <h3 class="panel-title">Check Out Store requisiton(s)</h3>
        </div>
        <div class="panel-body">
            <div class="row">
                <div class="col-md-12">
                    <div class="form-group">
                        <label>Posted Store Requisitions</label>
                        <asp:DropDownList ID="ddlStoreqs" class="form-control select2" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddlStoreqs_SelectedIndexChanged"></asp:DropDownList>
                    </div>
                </div>
            </div>
                <div class="col-sm-12">
                    <div class="form-group">
                        <label for="atext3">Requisition details</label>
                        <div class="icon-after-input">
                            <asp:GridView ID="gvLines" AutoGenerateColumns="false" DataKeyNames="No" class="table table-responsive no-padding table-bordered table-hover" runat="server"
                                AllowSorting="True" AllowPaging="true" ShowFooter="true" PageSize="5" OnRowDataBound="gvLines_RowDataBound">
                                <Columns>
                                    <asp:TemplateField HeaderStyle-HorizontalAlign="Left" HeaderText="#No" SortExpression="">
                                        <HeaderStyle Width="30px" />
                                        <ItemTemplate>
                                            <%# string.Format("{0}",Container.DataItemIndex + 1 +".") %>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:BoundField DataField="Account No:" HeaderText="Account No" />
                                    <asp:BoundField DataField="Account Name" HeaderText="Account Name" />
                                    <asp:BoundField DataField="varAmount" HeaderText="Amount" />
                                    <asp:TemplateField HeaderText="Receipt No">
                                        <ItemTemplate>
                                            <asp:DropDownList ID="ddlReceipts" CssClass="form-control" OnSelectedIndexChanged="ddlReceipts_SelectedIndexChanged" AutoPostBack="true" runat="server">
                                            </asp:DropDownList>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Actual Number given" SortExpression="">
                                        <ItemTemplate>
                                            <asp:TextBox ID="txtActual" runat="server" Text="" BorderColor="Blue" BorderStyle="Ridge" BorderWidth="2px" ForeColor="Blue" Width="100px" />
                                        </ItemTemplate>
                                        <ItemStyle Font-Bold="True" ForeColor="Green"></ItemStyle>
                                    </asp:TemplateField>
                  
                                    <%--  <asp:BoundField DataField="Ac" HeaderText="Actual Amount" />
                                    <asp:BoundField DataField="" HeaderText="Cash Amount" />--%>
                                    <%--<asp:BoundField DataField="NumOfDays" HeaderText="Days" />--%>
                                </Columns>
                                <FooterStyle HorizontalAlign="Center" />
                                <EmptyDataTemplate>
                                    <span style="color: red">No Recods</span>
                                </EmptyDataTemplate>
                            </asp:GridView>
                        </div>
                    </div>
                </div>
            </div>
            <!-- /.box-body -->
            <div class="box-footer clearfix">
                <asp:LinkButton ID="lbtnApply" class="btn btn-sm btn-success btn-flat pull-right" runat="server" OnClick="lbtnApply_Click"> <i class="fa fa-send-o"></i> Submit</asp:LinkButton>
                <asp:LinkButton ID="lbtnBack" class="btn btn-sm btn-warning btn-flat pull-left" runat="server" OnClick="lbtnBack_Click"> <i class="fa fa-backward"></i> Back</asp:LinkButton>
                <%-- <a href="javascript:void(0)">Place New Order</a>
                            <a href="javascript:void(0)">View All Orders</a>--%>
            </div>
        </div>
    </div>
</asp:Content>