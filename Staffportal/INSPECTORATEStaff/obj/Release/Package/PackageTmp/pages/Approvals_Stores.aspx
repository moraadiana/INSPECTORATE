<%@ Page Language="C#" Title="Stores Approvals" MasterPageFile="~/pages/Main.Master" AutoEventWireup="true" CodeBehind="Approvals_Stores.aspx.cs" Inherits="INSPECTORATEStaff.pages.Approvals_Stores" %>

<asp:Content ID="Content1" ContentPlaceHolderID="Main1" runat="Server">
    <!-- Content Wrapper. Contains page content -->
    <div class="content-wrapper">
        <!-- Content Header (Page header) -->
        <section class="content-header">
            <h1>Stores Approvals
       
                <%--<small>Version 2.0</small>--%>
            </h1>
            <ol class="breadcrumb">
                <li><a href="Dashboard.aspx"><i class="fa fa-dashboard"></i>Home</a></li>
                <li class="active">Stores Approvals</li>
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
                                    <h3 class="box-title">Pending Stores Approvals</h3>
                                </div>
                                <div class="box-body">
                                    <asp:GridView ID="gvLeaveApprovals" runat="server" AllowPaging="true" AutoGenerateColumns="false"
                                        DataKeyNames="Document No_" EmptyDataText="You have no stores Approvals" ShowHeaderWhenEmpty="true"
                                        OnRowCommand="gvLeaveApprovals_RowCommand" CssClass="table table-striped table-bordered"
                                        BorderWidth="0" ShowFooter="true" PageSize="20">
                                        <Columns>
                                            <asp:TemplateField HeaderText="#">
                                                <ItemTemplate>
                                                    <%# Container.DataItemIndex + 1 %>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:BoundField DataField="No_" HeaderText="No" ItemStyle-CssClass="text-uppercase"
                                                ReadOnly="true" />
                                            <asp:BoundField DataField="Request date" HeaderText="Application date"
                                                DataFormatString="{0:dd/MM/yyyy}" ReadOnly="true" />
                                            <asp:BoundField DataField="Request Description" HeaderText="Comments" ItemStyle-CssClass="text-capitalize"
                                                ReadOnly="true" />
                                            <asp:BoundField DataField="EmployeeName" HeaderText="Employee Name" ItemStyle-CssClass="text-uppercase"
                                                ReadOnly="true" />
                                            <asp:TemplateField HeaderText="Action" HeaderStyle-CssClass="text-center" ItemStyle-CssClass="small text-center">
                                                <ItemTemplate>
                                                    <i class='fa fa-coffee text-primary' title='view details'></i>
                                                    <asp:LinkButton ID="btnDetails" runat="server" CssClass="text-info" CommandName="ViewDetails"
                                                        Style="padding-right: 5px;">View Details</asp:LinkButton>
                                                    <%-- 
                            &nbsp;|&nbsp;
                            <i class='fa fa-check text-success' title='approve booking'></i>
                            <asp:LinkButton ID="btnEditMealBooking" runat="server" CssClass="pointer txt-success" CommandName="Approve" style="padding-right:5px; display: none;">Approve</asp:LinkButton>
                            &nbsp;|&nbsp;
                            <i class='fa fa-close text-danger' title='reject booking'></i>
                            <asp:LinkButton ID="btnDeleteMealBooking" runat="server" modalID="conf-modal" CssClass="pointer txt-danger" OnClientClick="return getModal(event)"  CommandName="Reject" style="display: none;">Reject</asp:LinkButton>
                                                    --%>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                        </Columns>
                                        <EmptyDataRowStyle CssClass="small text-center" ForeColor="#ff3300" />
                                        <HeaderStyle CssClass="small" BorderWidth="0" ForeColor="#525252" />
                                        <RowStyle CssClass="small" BorderWidth="0" ForeColor="#337ab7" />
                                        <FooterStyle CssClass="small" Font-Bold="true" />
                                    </asp:GridView>
                                </div>
                            </div>
                        </asp:View>
                        <asp:View ID="View2" runat="server">
                            <div class="modal-dialog" role="document">
                                <div class="modal-content">
                                    <div class="modal-header padding-10">
                                        <button type="button" class="close" data-dismiss="modal" aria-label="Close"><span aria-hidden="true">&times;</span></button>
                                        <h5 class="modal-title">Stores requisition Details</h5>
                                    </div>
                                    <div class="modal-body padding-10">
                                        <div id="dvmealSummary" runat="server" class="row" style="display: none;">
                                            <div class="col-md-12">
                                                <p class="bg-primary padding-15" title="Click to Edit" onclick="$">
                                                    <asp:Label runat="server" ID="lblMeetingType"></asp:Label>
                                                    Booking (<asp:Label runat="server" ID="lblMeetingID"></asp:Label>)&nbsp;for&nbsp;<asp:Label runat="server" ID="lblMeetingPax"></asp:Label><br />
                                                    <asp:Label runat="server" ID="lblDuration"></asp:Label>&nbsp;<asp:Label runat="server" ID="lblMeetingName"></asp:Label>&nbsp;meeting at&nbsp;
                                    <asp:Label runat="server" ID="lblMeetingVenue"></asp:Label>&nbsp;on&nbsp;<asp:Label runat="server" ID="lblMeetingDate"></asp:Label>&nbsp;from&nbsp;
                                    <asp:Label runat="server" ID="lblMeetingStarttime"></asp:Label>. 
                                                </p>
                                            </div>
                                        </div>

                                        <asp:GridView ID="gvLines" AutoGenerateColumns="false" DataKeyNames="Requistion No" class="table table-responsive no-padding table-bordered table-hover" runat="server"
                                            AllowSorting="True" AllowPaging="true" ShowFooter="true" PageSize="25">
                                            <Columns>
                                                <asp:TemplateField HeaderStyle-HorizontalAlign="Left" HeaderText="#No" SortExpression="">
                                                    <HeaderStyle Width="30px" />
                                                    <ItemTemplate>
                                                        <%# string.Format("{0}",Container.DataItemIndex + 1 +".") %>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:BoundField DataField="Requistion No" HeaderText="Req No." />
                                                <%--<asp:BoundField DataField="Issuing Store" HeaderText="Store" />--%>
                                                <asp:BoundField DataField="Description" HeaderText="Description" />
                                                <asp:TemplateField HeaderText="Quantity">
                                                    <ItemTemplate>
                                                        <%# string.Format("{0:#,##0.00}", ((decimal)Eval("[Quantity]")))%>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Cost">
                                                    <ItemTemplate>
                                                        <%# string.Format("{0:#,##0.00}", ((decimal)Eval("[varAmount]")))%>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                            </Columns>
                                            <FooterStyle HorizontalAlign="Center" />
                                            <EmptyDataTemplate>
                                                <span style="color: red">No Recods</span>
                                            </EmptyDataTemplate>
                                        </asp:GridView>

                                        <div class="row">
                                            <div class="col-lg-12 col-md-8">
                                                <div class="form-group">
                                                    <asp:Label ID="Label1" runat="server" Text="Approver Comments"></asp:Label>
                                                    <asp:TextBox ID="txtApproverComments" runat="server" CssClass="form-control" TextMode="MultiLine"></asp:TextBox>
                                                </div>
                                            </div>
                                            <br />
                                            <div class="col-lg-12 col-md-6">
                                                <div runat="server" id="dvSaveBooking" class="col-md-12">
                                                    <a runat="server" id="btnApprove" onserverclick="btnApprovelBooking"><span class="btn btn-success">
                                                        <i class="fa fa-pencil"></i>Approve </span></a>

                                                    <%--<br />--%>

                                                    <a runat="server" id="btnReject" onserverclick="btnRejectlBooking"><span class="btn btn-danger">
                                                        <i class="fa fa-remove"></i>Reject </span></a>

                                                    <%--<br />--%>

                                                    <a runat="server" id="btnCancel" onserverclick="btnCancelBooking"><span
                                                        class="btn btn-info"><i class="fa fa-close"></i>Cancel </span></a>
                                                </div>
                                            </div>
                                        </div>


                                    </div>
                                    <%--                    <div class="modal-footer padding-5">
                    <button type="button" class="btn btn-sm btn-default" data-dismiss="modal">No, keep this Booking</button>
                    <button id="btnModalConfirm" type="button" class="btn btn-sm btn-danger">Yes, delete this Booking</button>
                    </div>--%>
                                </div>
                            </div>
                        </asp:View>
                    </asp:MultiView>
                </div>
            </div>
        </section>
    </div>
    <div class="modal fade msg-modal" tabindex="-1" role="dialog" aria-labelledby="MsgModalLabel">
        <div class="modal-dialog" role="document">
            <div runat="server" id="dvMdlContentAprConfirm" class="modal-content" visible="false">
                <div class="modal-header bg-success padding-10">
                    <button type="button" class="close" data-dismiss="modal" aria-label="Close">
                        <span aria-hidden="true">&times;</span></button>
                    <h5 class="modal-title">Success!</h5>
                </div>
                <div class="modal-body padding-10">
                    <p>
                        The Imprest Request has been successfully Approved.
                    </p>
                </div>
            </div>
            <div runat="server" id="dvMdlContentRejConfirm" class="modal-content" visible="false">
                <div class="modal-header bg-danger padding-10">
                    <button type="button" class="close" data-dismiss="modal" aria-label="Close">
                        <span aria-hidden="true">&times;</span></button>
                    <h5 class="modal-title">Success!</h5>
                </div>
                <div class="modal-body padding-10">
                    <p>
                        The Imprest Request has been successfully Rejected.
                    </p>
                </div>
            </div>
        </div>
    </div>
</asp:Content>
