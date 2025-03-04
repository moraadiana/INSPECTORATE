<%@ Page Language="C#" Title="Transport Requisition" MasterPageFile="~/pages/Main.Master" AutoEventWireup="true" CodeBehind="TransportListing.aspx.cs" Inherits="INSPECTORATEStaff.pages.TransportListing" %>

<asp:Content ID="Content1" ContentPlaceHolderID="Main1" runat="Server">
    <!-- Content Wrapper. Contains page content -->
    <div class="content-wrapper">
        <!-- Content Header (Page header) -->
        <section class="content-header">
            <h1>Transport Requests
       
                <%--<small>Version 2.0</small>--%>
            </h1>
            <ol class="breadcrumb">
                <li><a href="Dashboard.aspx"><i class="fa fa-dashboard"></i>Home</a></li>
                <li class="active">Transport Requests</li>
            </ol>
        </section>

        <!-- Main content -->
        <section class="content">
            <div class="row">
                <div class="col-md-12">
                    <div class="box box-warning">
                        <div class="box-header with-border">
                            <h3 class="box-title">Transport Requests List</h3>

                            <div class="box-tools pull-right">
                                <button type="button" class="btn btn-box-tool" data-widget="collapse">
                                    <i class="fa fa-minus"></i>
                                </button>
                                <button type="button" class="btn btn-box-tool" data-widget="remove"><i class="fa fa-times"></i></button>
                            </div>
                        </div>
                        <!-- /.box-header -->
                        <div class="box-body">
                             <p class="text-center"><a id="btnTravelRequest" runat="server" onserverclick="btnTravelRequest_ServerClick"><span class="btn btn-pill btn-warning u-posRelative"><i class="fa fa-car"></i>Request for a Vehicle</span></a></p>
                            <%=Session["ResponseMsg"] %>
                            <br />
                            <div class="table-responsive">
                                <table id="example1" class="table no-margin">
                                    <thead>
                                        <tr>
                                            <th class="small">Ref No</th>
                                            <th class="small">Commencement</th>
                                            <th class="small">Destination</th>
                                            <th class="small">Vehicle Allocated</th>
                                            <th class="small">Driver Allocated</th>
                                            <th class="small">Date of Request</th>
                                            <th class="small">Date of Trip</th>
                                            <th class="small">No of Days Requested</th>
                                            <%--<th class="small">No of Passengers</th>--%>
                                            <th class="small">Status</th>
                                            <th class="small">Action</th>
                                        </tr>
                                    </thead>
                                    <tbody>
                                        <%=Jobs()%>
                                    </tbody>
                                </table>
                            </div>
                            <!-- /.table-responsive -->
                        </div>
                        <!-- /.box-body -->
                        <%--  <div class="box-footer clearfix">
                            <a href="javascript:void(0)" class="btn btn-sm btn-info btn-flat pull-left">Place New Order</a>
                            <a href="javascript:void(0)" class="btn btn-sm btn-default btn-flat pull-right">View All Orders</a>
                        </div>--%>
                        <!-- /.box-footer -->
                    </div>
                </div>
                <!-- /.col -->
            </div>
        </section>
        <!-- /.content -->
    </div>
    <!-- /.content-wrapper -->
     <div class="modal fade msg-modal" tabindex="-1" role="dialog" aria-labelledby="delModalLabel">
        <div class="modal-dialog" role="document">
            <div class="modal-content" visible="false">
                <div class="modal-header bg-success padding-10">
                    <button type="button" class="close" data-dismiss="modal" aria-label="Close"><span aria-hidden="true">&times;</span></button>
                    <h5 class="modal-title">Success!</h5>
                </div>
                <div class="modal-body padding-10">
                    <p>Your Travel Request has been successfully sent for Approval.</p>
                </div>
            </div>
            <div class="modal-content" visible="false">
                <div class="modal-header bg-danger padding-10">
                    <button type="button" class="close" data-dismiss="modal" aria-label="Close"><span aria-hidden="true">&times;</span></button>
                    <h5 class="modal-title" id="hTitle" runat="server">Failed!</h5>
                </div>
                <div class="modal-body padding-10">
                   
                </div>
            </div>
        </div>
    </div>
    <!-- BEGIN: modal -->
    <div class="modal fade" id="eventModal" tabindex="-1" role="dialog" aria-labelledby="eventModalLabel">
        <div class="modal-dialog" role="document">
            <div class="modal-content bg-emerland"  runat="server" id="dvMdlContentPass"  visible="false">
                <div class="modal-header">
                    <button type="button" class="close" data-dismiss="modal" aria-label="Close">
                        <span aria-hidden="true">&times;</span>
                    </button>
                    <h4 class="modal-title" id="eventModalLabel">Create New Event</h4>
                </div>
                <div class="modal-body">
                    <div>
                         <p>Your Travel Request has been successfully sent for Approval.</p>
                    </div>
                </div>
            </div>
              <div class="modal-content bg-alizarin text-white"  runat="server" id="dvMdlContentFail" visible="false">
                <div class="modal-header">
                    <button type="button" class="close" data-dismiss="modal" aria-label="Close">
                        <span aria-hidden="true">&times;</span>
                    </button>
                    <h4 class="modal-title" id="eventModalLabelf">Error!</h4>
                </div>
                <div class="modal-body">
                    <div>
                         <p id="pMsg" runat="server">You have a Pending Travel Application.</p>
                    </div>
                </div>
            </div>
        </div>
    </div>
    <!-- END: modal -->
</asp:Content>

